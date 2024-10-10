using BgTiendaFacturacionAPI.Domain.Connection;
using BgTiendaFacturacionAPI.Domain.Product;
using BgTiendaFacturacionAPI.Infrastructure.Invoice;
using BgTiendaFacturacionAPI.Infrastructure.Product;
using Microsoft.AspNetCore.Mvc;

namespace BgTiendaFacturacionAPI.Application.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IConnectionRepository connRepository)
        {
            productRepository = new ProductRepository(connRepository);
        }


        // POST api/<ProductController>
        [HttpPost("get_products")]
        public async Task<ActionResult<IList<ProductModel>>> GetProducts([FromBody] ProductModel producto)
            => await productRepository.ObtenerProductos(producto);

        // POST api/<ProductController>
        [HttpPost("add_product")]
        public async Task<ActionResult<ProductModel>> AddProduct([FromBody] ProductModel producto)
            => await productRepository.AgregarProducto(producto);



    }
}
