using BgTiendaFacturacionAPI.Domain.Invoice;

namespace BgTiendaFacturacionAPI.Domain.Invoice
{
    public class InvoiceResponseModel
    {
        public string? ResponseCode { get; set; }
        public string? Message { get; set; }
        public InvoiceModel? Invoice { get; set; }
        public List<InvoiceModel>? Invoices { get; set; }
    }
}
