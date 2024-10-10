using Newtonsoft.Json;

namespace BgTiendaFacturacionAPI.Domain.Product
{
    public class ProductModel
    {
        public ProductModel() { }

        public ProductModel(long? IdProducto, string? CodigoProducto)
        {
            this.IdProducto = IdProducto;
            this.CodigoProducto = CodigoProducto;
        }

        public ProductModel(long? IdProducto, string? CodigoProducto, string? NombreProducto, decimal? PrecioProducto)
        {
            this.IdProducto = IdProducto;
            this.CodigoProducto = CodigoProducto;
            this.NombreProducto = NombreProducto;
            this.PrecioProducto = PrecioProducto;
        }

        public long? IdProducto { get; set; }
        
        public string? CodigoProducto { get; set; }

        public string? NombreProducto { get; set; }

        public decimal? PrecioProducto { get; set; }


    }
}
