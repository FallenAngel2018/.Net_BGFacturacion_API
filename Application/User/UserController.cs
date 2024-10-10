using BgTiendaFacturacionAPI.Domain.Connection;
using BgTiendaFacturacionAPI.Domain.Product;
using BgTiendaFacturacionAPI.Domain.User;
using BgTiendaFacturacionAPI.Infrastructure.Invoice;
using BgTiendaFacturacionAPI.Infrastructure.Product;
using BgTiendaFacturacionAPI.Infrastructure.User;
using Microsoft.AspNetCore.Mvc;

namespace BgTiendaFacturacionAPI.Application.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IConnectionRepository connRepository)
        {
            userRepository = new UserRepository(connRepository);
        }


        // POST api/<UserController>
        [HttpPost("request_login")]
        public async Task<ActionResult> RequestLogIn([FromBody] UserModel usuario)
            => await userRepository.LogIn(usuario);

        // POST api/<ProductController>
        [HttpPost("get_users")]
        public async Task<ActionResult<UserResponseModel>> GetUsers([FromBody] UserModel usuario)
        //public async Task<ActionResult<IList<UserModel>>> GetUsers([FromBody] UserModel usuario)
            => await userRepository.ObtenerUsuarios(usuario);

        // POST api/<UserController>
        [HttpPost("add_user")]
        public async Task<ActionResult<UserResponseModel>> AddUser([FromBody] UserModel usuario)
            => await userRepository.AgregarUsuario(usuario);

        // PUT api/<UserController>
        [HttpPut("update_user")]
        public async Task<ActionResult<UserResponseModel>> UpdateUser([FromBody] UserModel usuario)
            => await userRepository.ActualizarUsuario(usuario);



    }
}
