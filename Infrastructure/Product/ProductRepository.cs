using BgTiendaFacturacionAPI.Domain.Product;
using BgTiendaFacturacionAPI.Infrastructure.Connection;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using System.Net;
using Azure;
using Newtonsoft.Json;
using System;
using BgTiendaFacturacionAPI.Domain.Connection;

namespace BgTiendaFacturacionAPI.Infrastructure.Product
{
    public class ProductRepository : ControllerBase, IProductRepository
    {
        private SqlConnection? connection;
        private SqlConnection? connectionSecret;
        private readonly IConnectionRepository _connectionRepository;

        public ProductRepository(IConnectionRepository connRepository)
        {
            _connectionRepository = connRepository;
        }


        public async Task<ActionResult<IList<ProductModel>>> ObtenerProductos(ProductModel producto)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable(); // Tabla que se obtiene de un select
            dt.Columns.Add("IdProducto", typeof(long));
            dt.Columns.Add("CodigoProducto", typeof(string));
            dt.Columns.Add("MensajeError", typeof(string));

            List<ProductModel> productosResponse = new List<ProductModel>();

            try
            {
                ProductModel obj = new ProductModel();

                connectionSecret = _connectionRepository.GetBgFacturacionConnection();

                await using (var command = new SqlCommand("BgFact_Producto_ObtenerProductos", connectionSecret)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add("@productoCodigo", SqlDbType.VarChar).Value = producto.CodigoProducto != null ? producto.CodigoProducto : string.Empty;
                    command.Parameters.Add("@productoNombre", SqlDbType.VarChar).Value = producto.NombreProducto != null ? producto.NombreProducto : string.Empty;

                    connectionSecret.Open();

                    da.SelectCommand = command;
                    da.Fill(dt);
                }

                connectionSecret.Close();

                if(dt.Rows.Count > 0)
                {
                    long IdProducto = (long)dt.Rows[0]["IdProducto"];
                    string CodigoProducto = (string)dt.Rows[0]["ProductoCodigo"];
                    ProductModel obj1 = new ProductModel();

                    foreach (DataRow row in dt.Rows)
                    {
                        ProductModel product = new ProductModel
                        {
                            IdProducto = Convert.ToInt64(row["IdProducto"]),
                            CodigoProducto = row["ProductoCodigo"].ToString(),
                            NombreProducto = row["ProductoNombre"].ToString(),
                            PrecioProducto = (decimal)row["ProductoPrecio"],
                            //PrecioProducto = Convert.ToDecimal(row["ProductoPrecio"]),
                        };

                        productosResponse.Add(product);
                    }
                }
                

                return Ok(productosResponse);
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

        public async Task<ActionResult<ProductModel>> AgregarProducto(ProductModel producto)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable(); // Tabla que se obtiene de un select
            dt.Columns.Add("IdProducto", typeof(long));
            dt.Columns.Add("CodigoProducto", typeof(string));
            dt.Columns.Add("MensajeError", typeof(string));

            try
            {
                ProductModel obj = new ProductModel();

                connectionSecret = _connectionRepository.GetBgFacturacionConnection();

                await using (var command = new SqlCommand("BgFact_Producto_AgregarProducto", connectionSecret)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.Add("@productoCodigo", SqlDbType.VarChar).Value = producto.CodigoProducto;
                    command.Parameters.Add("@productoNombre", SqlDbType.VarChar).Value = producto.NombreProducto;
                    command.Parameters.Add(new SqlParameter
                    {
                        // Nombre del parámetro
                        ParameterName = "@productoPrecio",

                        // Tipo del parámetro
                        SqlDbType = SqlDbType.Decimal,

                        // Precisión total de 10 dígitos
                        Precision = 10,

                        // Escala de 2 dígitos decimales
                        Scale = 2,

                    }).Value = producto.PrecioProducto;

                    connectionSecret.Open();

                    da.SelectCommand = command;
                    da.Fill(dt);
                }

                connectionSecret.Close();

                long IdProducto = (long)dt.Rows[0]["IdProducto"];
                string CodigoProducto = (string)dt.Rows[0]["CodigoProducto"];
                ProductModel obj1 = new ProductModel(IdProducto, CodigoProducto);

                return Ok(obj1);
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
