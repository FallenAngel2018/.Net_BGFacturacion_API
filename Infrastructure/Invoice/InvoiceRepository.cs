using BgTiendaFacturacionAPI.Domain.Connection;
using BgTiendaFacturacionAPI.Domain.DetailInvoice;
using BgTiendaFacturacionAPI.Domain.Invoice;
using BgTiendaFacturacionAPI.Infrastructure.Connection;
using BgTiendaFacturacionAPI.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;

namespace BgTiendaFacturacionAPI.Infrastructure.Invoice
{
    public class InvoiceRepository : ControllerBase, IInvoiceRepository
    {
        private SqlConnection? connection;
        private SqlConnection? connectionSecret;
        private readonly IConnectionRepository _connectionRepository;

        public InvoiceRepository(IConnectionRepository connRepository)
        {
            _connectionRepository = connRepository;
        }


        public async Task<ActionResult<IList<InvoiceModel>>> ObtenerFacturas(InvoiceModel factura)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable(); // Tabla que se obtiene de un select
            dt.Columns.Add("IdFactura", typeof(long));
            dt.Columns.Add("IdUsuarioVendedor", typeof(long));
            dt.Columns.Add("MensajeError", typeof(string));

            List<InvoiceModel> facturasResponse = new List<InvoiceModel>();

            try
            {
                InvoiceModel obj = new InvoiceModel();

                connection = ConnectionRepository.GetStaticConnection();
                connectionSecret = _connectionRepository.GetBgFacturacionConnection();

                await using (var command = new SqlCommand("BgFact_Factura_ObtenerFacturas", connectionSecret) // connection
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add("@idFactura", SqlDbType.BigInt).Value = factura.IdFactura != null ? factura.IdFactura : 0;
                    command.Parameters.Add("@facturaFecha", SqlDbType.DateTime).Value = factura.FacturaFecha;
                    command.Parameters.Add(new SqlParameter
                    {
                        // Nombre del parámetro
                        ParameterName = "@facturaTotal",

                        // Tipo del parámetro
                        SqlDbType = SqlDbType.Decimal,

                        // Precisión total de 10 dígitos
                        Precision = 10,

                        // Escala de 2 dígitos decimales
                        Scale = 2,

                    }).Value = factura.FacturaTotal != null ? factura.FacturaTotal : 0;

                    connection.Open();

                    da.SelectCommand = command;
                    da.Fill(dt);
                }

                connection.Close();

                if (dt.Rows.Count > 0)
                {
                    long IdFactura = (long)dt.Rows[0]["IdFactura"];

                    foreach (DataRow row in dt.Rows)
                    {
                        InvoiceModel invoice = new InvoiceModel
                        {
                            IdFactura = Convert.ToInt64(row["IdFactura"]),
                            IdCliente = Convert.ToInt64(row["IdCliente"]),
                            IdUsuarioVendedor = Convert.ToInt64(row["IdUsuarioVendedor"]),
                            FacturaFecha = (DateTime) row["FacturaFecha"],
                            FacturaFormaPago = Convert.ToInt32(row["FacturaFormaPago"]),
                            FacturaTotal = (decimal)row["FacturaTotal"],
                        };

                        facturasResponse.Add(invoice);
                    }
                }


                return Ok(facturasResponse);
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

        public async Task<ActionResult<InvoiceResponseModel>> AgregarFactura(InvoiceModel factura)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable(); // Tabla que se obtiene de un select
            dt.Columns.Add("ResponseCode", typeof(string));
            dt.Columns.Add("Message", typeof(string));
            dt.Columns.Add("IdFactura", typeof(long));
            dt.Columns.Add("IdUsuarioVendedor", typeof(long));

            DataTable detallesDataTable = new DataTable();
            detallesDataTable.Columns.Add("IdDetalleFactura", typeof(long));
            detallesDataTable.Columns.Add("IdFactura", typeof(long));
            detallesDataTable.Columns.Add("IdProducto", typeof(long));
            detallesDataTable.Columns.Add("Cantidad", typeof(int));
            detallesDataTable.Columns.Add("PrecioUnitario", typeof(decimal));
            detallesDataTable.Columns.Add("Total", typeof(decimal));


            InvoiceResponseModel invoiceResponse = new InvoiceResponseModel();

            try
            {
                //detallesDataTable = InvoiceMapper.ToDataTable(factura.Detalles!); // Este método funciona mal, solo esta para pruebas
                detallesDataTable = InvoiceMapper.ConvertListToDataTable(factura.Detalles!);

                //detallesDataTable = ToDataTable<IList<DetailInvoiceModel>>(factura.Detalles);


                connectionSecret = _connectionRepository.GetBgFacturacionConnection();

                await using (var command = new SqlCommand("BgFact_Factura_AgregarFactura", connectionSecret)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    //command.Parameters.Add("@idFactura", SqlDbType.BigInt).Value = factura.IdFactura;
                    command.Parameters.Add("@idCliente", SqlDbType.BigInt).Value = factura.IdCliente;
                    command.Parameters.Add("@idUsuarioVendedor", SqlDbType.BigInt).Value = factura.IdUsuarioVendedor;
                    //command.Parameters.Add("@facturaFecha", SqlDbType.DateTime).Value = factura.FacturaFecha;
                    command.Parameters.Add("@facturaFormaPago", SqlDbType.Int).Value = factura.FacturaFormaPago;
                    command.Parameters.Add("@detalles", SqlDbType.Structured).Value = detallesDataTable;
                    command.Parameters.Add(new SqlParameter
                    {
                        // Nombre del parámetro
                        ParameterName = "@facturaTotal",

                        // Tipo del parámetro
                        SqlDbType = SqlDbType.Decimal,

                        // Precisión total de 10 dígitos
                        Precision = 10,

                        // Escala de 2 dígitos decimales
                        Scale = 2,

                    }).Value = factura.FacturaTotal;

                    connectionSecret.Open();

                    da.SelectCommand = command;
                    // Aqui se genera la factura
                    da.Fill(dt);
                }

                long IdFactura = (long)dt.Rows[0]["IdFactura"];
                long IdUsuarioVendedor = (long)dt.Rows[0]["IdUsuarioVendedor"];

                invoiceResponse.ResponseCode = (string)dt.Rows[0]["ResponseCode"];
                invoiceResponse.Message = (string)dt.Rows[0]["Message"];
                invoiceResponse.Invoice = new InvoiceModel(IdFactura, IdUsuarioVendedor);

                connectionSecret.Close();

                return Ok(invoiceResponse);
            }
            catch (Exception e)
            {
                invoiceResponse.ResponseCode = "100";
                invoiceResponse.Message = e.Message;

                return ResponseBadRequest<InvoiceResponseModel>(invoiceResponse);
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
