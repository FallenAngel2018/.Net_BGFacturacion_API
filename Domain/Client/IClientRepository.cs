using BgTiendaFacturacionAPI.Domain.Product;
using Microsoft.AspNetCore.Mvc;

namespace BgTiendaFacturacionAPI.Domain.Client
{
    public interface IClientRepository
    {
        public Task<ActionResult<IList<ClientModel>>> ObtenerClientes(ClientModel cliente);
        public Task<ActionResult<ClientResponseModel>> AgregarCliente(ClientModel cliente);

    }
}
