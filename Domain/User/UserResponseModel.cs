namespace BgTiendaFacturacionAPI.Domain.User
{
    public class UserResponseModel
    {
        public string? ResponseCode { get; set; }
        public string? Message { get; set; }
        public UserModel? User { get; set; }
        public List<UserModel>? Users { get; set; }

    }
}
