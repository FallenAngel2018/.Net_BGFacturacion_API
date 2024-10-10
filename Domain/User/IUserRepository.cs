using BgTiendaFacturacionAPI.Domain.Product;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace BgTiendaFacturacionAPI.Domain.User
{
    public interface IUserRepository
    {
        public Task<ActionResult> LogIn(UserModel usuario);
        //public Task<ActionResult<IList<UserModel>>> ObtenerUsuarios(UserModel usuario);
        public Task<ActionResult<UserResponseModel>> ObtenerUsuarios(UserModel usuario);
        public Task<ActionResult<UserResponseModel>> AgregarUsuario(UserModel usuario);
        public Task<ActionResult<UserResponseModel>> ActualizarUsuario(UserModel usuario);
    }
}
