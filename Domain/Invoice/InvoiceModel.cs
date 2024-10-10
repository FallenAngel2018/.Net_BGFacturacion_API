using BgTiendaFacturacionAPI.Domain.DetailInvoice;

namespace BgTiendaFacturacionAPI.Domain.Invoice
{
    public class InvoiceModel
    {
        public long? IdFactura { get; set; }

        public long? IdCliente { get; set; }

        public long? IdUsuarioVendedor { get; set; }

        public DateTime? FacturaFecha { get; set; }

        public int? FacturaFormaPago { get; set; }

        public decimal? FacturaTotal { get; set; }

        public IList<DetailInvoiceModel>? Detalles { get; set; }
        //public DetailInvoiceModel[]? Detalles { get; set; }


        public InvoiceModel() { }

        public InvoiceModel(long? idFactura, long? idUsuarioVendedor)
        {
            IdFactura = idFactura;
            IdUsuarioVendedor = idUsuarioVendedor;
        }

        public InvoiceModel(long? idFactura, long? idCliente, long? idUsuarioVendedor)
        {
            IdFactura = idFactura;
            IdCliente = idCliente;
            IdUsuarioVendedor = idUsuarioVendedor;
        }

        public InvoiceModel(long? idFactura, long? idCliente, long? idUsuarioVendedor,
            DateTime? facturaFecha, int? facturaFormaPago, decimal? facturaTotal)
        {
            IdFactura = idFactura;
            IdCliente = idCliente;
            IdUsuarioVendedor = idUsuarioVendedor;
            FacturaFecha = facturaFecha;
            FacturaFormaPago = facturaFormaPago;
            FacturaTotal = facturaTotal;
        }

        public InvoiceModel(long? idFactura, long? idCliente, long? idUsuarioVendedor,
            DateTime? facturaFecha, int? facturaFormaPago, decimal? facturaTotal, DetailInvoiceModel[]? detalles)
        {
            IdFactura = idFactura;
            IdCliente = idCliente;
            IdUsuarioVendedor = idUsuarioVendedor;
            FacturaFecha = facturaFecha;
            FacturaFormaPago = facturaFormaPago;
            FacturaTotal = facturaTotal;
            Detalles = detalles;
        }
    }
}
