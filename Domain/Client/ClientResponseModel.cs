using BgTiendaFacturacionAPI.Domain.Invoice;

namespace BgTiendaFacturacionAPI.Domain.Client
{
    public class ClientResponseModel
    {
        public string? ResponseCode { get; set; }
        public string? Message { get; set; }
        public ClientModel? Client { get; set; }
        public List<ClientModel>? Clients { get; set; }
    }
}
