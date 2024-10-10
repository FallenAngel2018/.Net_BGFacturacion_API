using Microsoft.AspNetCore.Mvc;

namespace BgTiendaFacturacionAPI.Domain.Invoice
{
    public interface IInvoiceRepository
    {
        public Task<ActionResult<IList<InvoiceModel>>> ObtenerFacturas(InvoiceModel factura);
        public Task<ActionResult<InvoiceResponseModel>> AgregarFactura(InvoiceModel factura);
    }
}
