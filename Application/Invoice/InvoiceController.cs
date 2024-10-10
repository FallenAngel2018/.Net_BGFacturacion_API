using BgTiendaFacturacionAPI.Domain.Connection;
using BgTiendaFacturacionAPI.Domain.Invoice;
using BgTiendaFacturacionAPI.Infrastructure.Client;
using BgTiendaFacturacionAPI.Infrastructure.Invoice;
using Microsoft.AspNetCore.Mvc;

namespace BgTiendaFacturacionAPI.Application.Invoice
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceRepository invoiceRepository;

        public InvoiceController(IConnectionRepository connRepository)
        {
            invoiceRepository = new InvoiceRepository(connRepository);
        }


        // POST api/<ProductController>
        [HttpPost("get_invoices")]
        public async Task<ActionResult<IList<InvoiceModel>>> GetInvoices([FromBody] InvoiceModel factura)
            => await invoiceRepository.ObtenerFacturas(factura);

        // POST api/<ProductController>
        [HttpPost("add_invoice")]
        public async Task<ActionResult<InvoiceResponseModel>> AddInvoice([FromBody] InvoiceModel factura)
            => await invoiceRepository.AgregarFactura(factura);



    }
}
