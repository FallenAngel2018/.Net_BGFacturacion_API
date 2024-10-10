namespace BgTiendaFacturacionAPI.Domain.Client
{
    public class ClientModel
    {
        public long IdCliente { get; set; }

        public string? NumIdentificacion { get; set; }

        public string? NombreCliente { get; set; }

        public string? NumTelefono { get; set; }

        public string? Correo { get; set; }


        public ClientModel() { }

        public ClientModel(long idCliente, string? numIdentificacion)
        {
            IdCliente = idCliente;
            NumIdentificacion = numIdentificacion;
        }

        public ClientModel(long idCliente, string? numIdentificacion, string? nombreCliente, string? numTelefono, string? correo)
        {
            IdCliente = idCliente;
            NumIdentificacion = numIdentificacion;
            NombreCliente = nombreCliente;
            NumTelefono = numTelefono;
            Correo = correo;
        }
    }
}
