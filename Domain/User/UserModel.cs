using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BgTiendaFacturacionAPI.Domain.User
{
    public class UserModel
    {
        public UserModel() { }
        public UserModel(string? nombreUsuario, string? claveUsuario) {
            this.NombreUsuario = nombreUsuario;
            this.ClaveUsuario = claveUsuario;
        }
        public UserModel(long idUsuario, string? nombre, string? nombreUsuario, string? claveUsuario, int? tipoUsuario)
        {
            IdUsuario = idUsuario;
            Nombre = nombre;
            NombreUsuario = nombreUsuario;
            ClaveUsuario = claveUsuario;
            TipoUsuario = tipoUsuario;
        }

        //[JsonProperty("idUsuario")]
        public long? IdUsuario { get; set; }
        
        //[JsonProperty("nombre")]
        public string? Nombre { get; set; }

        //[JsonProperty("nombreUsuario")]
        public string? NombreUsuario { get; set; }

        //[JsonProperty("claveUsuario")]
        public string? ClaveUsuario { get; set; }

        //[JsonProperty("tipoUsuario")]
        public int? TipoUsuario { get; set; }

    }
}
