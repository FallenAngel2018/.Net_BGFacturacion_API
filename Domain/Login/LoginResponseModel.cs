namespace BgTiendaFacturacionAPI.Domain.Login
{
    public class LoginResponseModel
    {
        public string? ResponseCode { get; set; }
        public string? Message { get; set; }
        public bool? LoginExitoso { get; set; } // Devolver modelo principal del dominio

    }
}
