using BgTiendaFacturacionAPI.Domain.User;
using BgTiendaFacturacionAPI.Infrastructure.Connection;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using System.Net;
using BgTiendaFacturacionAPI.Domain.Connection;
using BgTiendaFacturacionAPI.Domain.Login;
using BgTiendaFacturacionAPI.Domain.Product;

namespace BgTiendaFacturacionAPI.Infrastructure.User
{
    public class UserRepository : ControllerBase, IUserRepository
    {
        private SqlConnection? connection;
        private SqlConnection? connectionSecret;
        private readonly IConnectionRepository _connectionRepository;

        public UserRepository(IConnectionRepository connRepository)
        {
            _connectionRepository = connRepository;
        }


        public async Task<ActionResult> LogIn(UserModel usuario)
        {
            LoginResponseModel loginResponse = new LoginResponseModel();

            try
            {
                connection = ConnectionRepository.GetStaticConnection();
                connectionSecret = _connectionRepository.GetBgFacturacionConnection();

                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable(); // Tabla que se obtiene de un select
                dt.Columns.Add("ResponseCode", typeof(string));
                dt.Columns.Add("Message", typeof(string));
                dt.Columns.Add("LoginExitoso", typeof(bool));

                await using (var command = new SqlCommand("BgFact_Usuario_Login", connectionSecret) // connection
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add("@usuarioNombre", SqlDbType.VarChar).Value = usuario.NombreUsuario;
                    command.Parameters.Add("@usuarioClave", SqlDbType.VarChar).Value = usuario.ClaveUsuario;

                    connection.Open();

                    da.SelectCommand = command;
                    da.Fill(dt);
                }


                bool loginExitoso = (bool)dt.Rows[0]["LoginExitoso"];

                loginResponse.ResponseCode = (string)dt.Rows[0]["ResponseCode"];
                loginResponse.Message = (string)dt.Rows[0]["Message"];
                loginResponse.LoginExitoso = (bool)dt.Rows[0]["LoginExitoso"];


                connection.Close();

                return Ok(loginResponse);
            }
            catch (Exception e)
            {
                return ResponseFault(e);

            }
            finally
            {
                DisposeService();
            }
            
        }


        //public async Task<ActionResult<IList<UserModel>>> ObtenerUsuarios(UserModel usuario)
        public async Task<ActionResult<UserResponseModel>> ObtenerUsuarios(UserModel usuario)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable(); // Tabla que se obtiene de un select
            //dt.Columns.Add("ResponseCode", typeof(string));
            //dt.Columns.Add("Message", typeof(string));
            //dt.Columns.Add("IdUsuario", typeof(long));
            //dt.Columns.Add("TipoUsuario", typeof(int));

            List<UserModel> usuariosResponse = new List<UserModel>();
            UserResponseModel userResponse = new UserResponseModel();

            try
            {
                UserModel obj = new UserModel();

                connection = ConnectionRepository.GetStaticConnection();
                connectionSecret = _connectionRepository.GetBgFacturacionConnection();

                await using (var command = new SqlCommand("BgFact_Usuario_ObtenerUsuarios", connectionSecret) // connection
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add("@usuarioNombre", SqlDbType.VarChar).Value = usuario.NombreUsuario != null ? usuario.NombreUsuario : string.Empty;
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = usuario.Nombre != null ? usuario.Nombre : string.Empty;
                    command.Parameters.AddWithValue("@tipoUsuario", SqlDbType.Int).Value = usuario.TipoUsuario != null ? usuario.TipoUsuario : null;

                    connectionSecret.Open();

                    da.SelectCommand = command;
                    da.Fill(dt);
                }

                connectionSecret.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        UserModel user = new UserModel
                        {
                            IdUsuario = Convert.ToInt64(row["IdUsuario"]),
                            Nombre = row["Nombre"].ToString(),
                            NombreUsuario = row["NombreUsuario"].ToString(),
                            ClaveUsuario = row["ClaveUsuario"].ToString(),
                            TipoUsuario = (int)row["TipoUsuario"],
                        };

                        usuariosResponse.Add(user);
                    }
                }

                userResponse.Users = usuariosResponse!;

                return Ok(usuariosResponse);
            }
            catch (Exception e)
            {
                //dt.Rows[0]["MensajeError"] = e.Message;
                userResponse.ResponseCode = "100";
                userResponse.Message = e.Message;
                return ResponseFault(e);
            }
            finally
            {
                DisposeService();
            }


        }


        public async Task<ActionResult<UserResponseModel>> AgregarUsuario(UserModel usuario)
        {
            UserResponseModel userResponse = new UserResponseModel();
           
            try
            {
                connection = ConnectionRepository.GetStaticConnection();
                connectionSecret = _connectionRepository.GetBgFacturacionConnection();

                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable(); // Tabla que se obtiene de un select
                dt.Columns.Add("ResponseCode", typeof(string));
                dt.Columns.Add("Message", typeof(string));
                dt.Columns.Add("IdUsuario", typeof(long));
                dt.Columns.Add("TipoUsuario", typeof(int));

                await using (var command = new SqlCommand("BgFact_Usuario_AgregarUsuario", connectionSecret)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add("@usuarioNombre", SqlDbType.VarChar).Value = usuario.NombreUsuario;
                    command.Parameters.Add("@usuarioClave", SqlDbType.VarChar).Value = usuario.ClaveUsuario;
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = usuario.Nombre;
                    command.Parameters.Add("@usuarioTipo", SqlDbType.Int).Value = usuario.TipoUsuario;

                    connectionSecret.Open();

                    da.SelectCommand = command;
                    da.Fill(dt);
                }

                connectionSecret.Close();


                // TODO - PENDIENTE AGREGAR ESTO EN PROCEDURE
                userResponse.ResponseCode = (string)dt.Rows[0]["ResponseCode"];
                userResponse.Message = (string)dt.Rows[0]["Message"];
                userResponse.User = new UserModel();
                userResponse.User!.IdUsuario = (long)dt.Rows[0]["IdUsuario"];
                userResponse.User!.TipoUsuario = (int)dt.Rows[0]["TipoUsuario"];

                return Ok(userResponse);
            }
            catch (Exception e)
            {
                return ResponseFault(e);
            }
            finally
            {
                DisposeService();
            }
            

        }


        public async Task<ActionResult<UserResponseModel>> ActualizarUsuario(UserModel usuario)
        {
            UserResponseModel userResponse = new UserResponseModel();

            try
            {
                UserModel obj = new UserModel(usuario.NombreUsuario, usuario.ClaveUsuario);


                connection = ConnectionRepository.GetStaticConnection();

                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable(); // Tabla que se obtiene de un select
                dt.Columns.Add("ResponseCode", typeof(string));
                dt.Columns.Add("Message", typeof(string));
                dt.Columns.Add("IdUsuario", typeof(long));
                dt.Columns.Add("TipoUsuario", typeof(int));


                await using (var command = new SqlCommand("BgFact_Usuario_ActualizarUsuario", connection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add("@idUsuario", SqlDbType.BigInt).Value = usuario.IdUsuario;
                    command.Parameters.Add("@usuarioNombre", SqlDbType.VarChar).Value = usuario.NombreUsuario;
                    command.Parameters.Add("@usuarioClave", SqlDbType.VarChar).Value = usuario.ClaveUsuario;
                    command.Parameters.Add("@nombre", SqlDbType.VarChar).Value = usuario.Nombre;
                    command.Parameters.Add("@usuarioTipo", SqlDbType.Int).Value = usuario.TipoUsuario;

                    connection.Open();

                    da.SelectCommand = command;
                    da.Fill(dt);
                }


                userResponse.ResponseCode = (string)dt.Rows[0]["ResponseCode"];
                userResponse.Message = (string)dt.Rows[0]["Message"];
                userResponse.User = new UserModel();
                userResponse.User.IdUsuario = (long)dt.Rows[0]["IdUsuario"];
                userResponse.User.TipoUsuario = (int)dt.Rows[0]["TipoUsuario"];


                connection.Close();

                return Ok(userResponse);
            }
            catch (Exception e)
            {
                return ResponseFault(e);
            }
            finally
            {
                DisposeService();
            }


        }


        #region Métodos de devolución de errores

        private StatusCodeResult ResponseBadRequest(string code, string description)
        {
            var statusCode = StatusCodes.Status400BadRequest;

            return StatusCode(statusCode);
        }

        private ObjectResult ResponseFault(Exception e)
        {
            var statusCode = StatusCodes.Status500InternalServerError;

            //return StatusCode(statusCode, e.Message);
            return new ObjectResult(e.Message) { StatusCode = statusCode};
        }

        private object ResponseFault(UserResponseModel userResponse)
        {
            return userResponse;
            
            //var statusCode = StatusCodes.Status500InternalServerError;
            //return StatusCode(statusCode, e.Message);
            //return new ObjectResult(userResponse.Message) { StatusCode = statusCode };
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
