using BgTiendaFacturacionAPI.Domain.Client;
using BgTiendaFacturacionAPI.Domain.Connection;
using BgTiendaFacturacionAPI.Domain.Invoice;
using BgTiendaFacturacionAPI.Domain.Product;
using BgTiendaFacturacionAPI.Infrastructure.Connection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BgTiendaFacturacionAPI.Infrastructure.Client
{

    public class ClientRepository : ControllerBase, IClientRepository
    {
        private SqlConnection? connectionSecret;
        private readonly IConnectionRepository _connectionRepository;

        public ClientRepository(IConnectionRepository connRepository)
        {
            _connectionRepository = connRepository;
        }


        public async Task<ActionResult<IList<ClientModel>>> ObtenerClientes(ClientModel cliente)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable(); // Tabla que se obtiene de un select
            dt.Columns.Add("IdCliente", typeof(long));
            dt.Columns.Add("NumIdentificacion", typeof(string));
            dt.Columns.Add("MensajeError", typeof(string));

            List<ClientModel> clientesResponse = new List<ClientModel>();

            try
            {
                ClientModel obj = new ClientModel();

                connectionSecret = _connectionRepository.GetBgFacturacionConnection();

                await using (var command = new SqlCommand("BgFact_Cliente_ObtenerClientes", connectionSecret)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add("@clienteNumIdentificacion", SqlDbType.VarChar).Value = cliente.NumIdentificacion != null ? cliente.NumIdentificacion : string.Empty;
                    command.Parameters.Add("@clienteNombre", SqlDbType.VarChar).Value = cliente.NombreCliente != null ? cliente.NombreCliente : string.Empty;

                    connectionSecret.Open();

                    da.SelectCommand = command;
                    da.Fill(dt);
                }

                connectionSecret.Close();

                if (dt.Rows.Count > 0)
                {
                    long IdProducto = (long)dt.Rows[0]["IdCliente"];
                    string CodigoProducto = (string)dt.Rows[0]["NumIdentificacion"];
                    ClientModel obj1 = new ClientModel();

                    foreach (DataRow row in dt.Rows)
                    {
                        ClientModel client = new ClientModel
                        {
                            IdCliente = Convert.ToInt64(row["IdCliente"]),
                            NumIdentificacion = row["NumIdentificacion"].ToString(),
                            NombreCliente = row["NombreCliente"].ToString(),
                            NumTelefono = row["NumTelefono"].ToString(),
                            Correo = row["Correo"].ToString(),
                        };

                        clientesResponse.Add(client);
                    }
                }


                return Ok(clientesResponse);
            }
            catch (Exception e)
            {
                //dt.Rows[0]["MensajeError"] = e.Message;
                return ResponseFault(e);
            }
            finally
            {
                DisposeService();
            }


        }

        public async Task<ActionResult<ClientResponseModel>> AgregarCliente(ClientModel producto)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable(); // Tabla que se obtiene de un select
            dt.Columns.Add("ResponseCode", typeof(string));
            dt.Columns.Add("Message", typeof(string));
            dt.Columns.Add("IdCliente", typeof(long));
            dt.Columns.Add("NumIdentificacion", typeof(string));

            ClientResponseModel clientResponse = new ClientResponseModel();

            try
            {
                ClientModel obj = new ClientModel();

                connectionSecret = _connectionRepository.GetBgFacturacionConnection();

                await using (var command = new SqlCommand("BgFact_Cliente_AgregarCliente", connectionSecret)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add("@clienteNumIdentificacion", SqlDbType.VarChar).Value = producto.NumIdentificacion;
                    command.Parameters.Add("@clienteNombre", SqlDbType.VarChar).Value = producto.NombreCliente;
                    command.Parameters.Add("@clienteTelefono", SqlDbType.VarChar).Value = producto.NumTelefono;
                    command.Parameters.Add("@clienteCorreo", SqlDbType.VarChar).Value = producto.Correo;

                    connectionSecret.Open();

                    da.SelectCommand = command;
                    da.Fill(dt);
                }

                connectionSecret.Close();

                long IdCliente = (long)dt.Rows[0]["IdCliente"];
                string NumIdentificacion = (string)dt.Rows[0]["NumIdentificacion"];
                //ClientModel obj1 = new ClientModel(IdCliente, NumIdentificacion);

                clientResponse.ResponseCode = (string)dt.Rows[0]["ResponseCode"];
                clientResponse.Message = (string)dt.Rows[0]["Message"];
                clientResponse.Client = new ClientModel(IdCliente, NumIdentificacion);

                //return Ok(obj1);
                return Ok(clientResponse);
            }
            catch (Exception e)
            {
                clientResponse.ResponseCode = "100";
                clientResponse.Message = e.Message;

                return ResponseBadRequest<ClientResponseModel>(clientResponse);
                //return ResponseFault(e);
            }
            finally
            {
                DisposeService();
            }


        }


        #region Métodos de devolución de errores

        private ObjectResult ResponseBadRequest<T>(T objectResponse)
        {
            var statusCode = StatusCodes.Status400BadRequest;

            return StatusCode(statusCode, objectResponse);
        }

        private StatusCodeResult ResponseBadRequest(string code, string description)
        {
            var statusCode = StatusCodes.Status400BadRequest;

            return StatusCode(statusCode);
        }

        private ObjectResult ResponseFault(Exception e)
        {
            var statusCode = StatusCodes.Status500InternalServerError;

            //return StatusCode(statusCode, e.Message);
            return new ObjectResult(e.Message) { StatusCode = statusCode };
        }

        #endregion


        #region Métodos de liberación de recursos
        private void DisposeService()
        {
            GC.RemoveMemoryPressure(1000);
            GC.Collect();
        }

        #endregion
    }
}
