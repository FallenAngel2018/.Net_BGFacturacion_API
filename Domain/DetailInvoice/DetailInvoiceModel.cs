namespace BgTiendaFacturacionAPI.Domain.DetailInvoice
{
    public class DetailInvoiceModel
    {
        public long? IdDetalleFactura { get; set; }

        public long? IdFactura { get; set; }

        public long? IdProducto { get; set; }

        public int? Cantidad { get; set; }

        public decimal? PrecioUnitario { get; set; }
        public decimal? Total { get; set; }


        public DetailInvoiceModel() { }

        public DetailInvoiceModel(long? idDetalleFactura, long? idFactura, long? idProducto, int? cantidad, decimal? precioUnitario)
        {
            IdDetalleFactura = idDetalleFactura;
            IdFactura = idFactura;
            IdProducto = idProducto;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }

        public DetailInvoiceModel(long? idDetalleFactura, long? idFactura, long? idProducto,
            int? cantidad, decimal? precioUnitario, decimal? total)
        {
            IdDetalleFactura = idDetalleFactura;
            IdFactura = idFactura;
            IdProducto = idProducto;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            Total = total;
        }



    }
}
