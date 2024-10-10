using BgTiendaFacturacionAPI.Domain.Client;
using BgTiendaFacturacionAPI.Domain.Connection;
using BgTiendaFacturacionAPI.Domain.Product;
using BgTiendaFacturacionAPI.Infrastructure.Client;
using BgTiendaFacturacionAPI.Infrastructure.Product;
using Microsoft.AspNetCore.Mvc;

namespace BgTiendaFacturacionAPI.Application.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository clientRepository;

        public ClientController(IConnectionRepository connRepository)
        {
            clientRepository = new ClientRepository(connRepository);
        }


        // POST api/<ProductController>
        [HttpPost("get_clients")]
        public async Task<ActionResult<IList<ClientModel>>> GetProducts([FromBody] ClientModel cliente)
            => await clientRepository.ObtenerClientes(cliente);

        // POST api/<ProductController>
        [HttpPost("add_client")]
        public async Task<ActionResult<ClientResponseModel>> AddProduct([FromBody] ClientModel cliente)
            => await clientRepository.AgregarCliente(cliente);



    }
}
