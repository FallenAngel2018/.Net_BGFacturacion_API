using BgTiendaFacturacionAPI.Domain.Product;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace BgTiendaFacturacionAPI.Domain.Product
{
    public interface IProductRepository
    {
        public Task<ActionResult<IList<ProductModel>>> ObtenerProductos(ProductModel producto);
        public Task<ActionResult<ProductModel>> AgregarProducto(ProductModel producto);
    }
}
