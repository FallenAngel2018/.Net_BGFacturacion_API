using Microsoft.Data.SqlClient;
using BgTiendaFacturacionAPI.Domain.Connection;


namespace BgTiendaFacturacionAPI.Infrastructure.Connection
{
    public class ConnectionRepository : IConnectionRepository
    {
        public IConfiguration _config;
        public ConnectionRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        private string GetBgFacturacionConnectionString()
        {
            return _config.GetSection("ConnectionStrings:BgFacturacionConnectionString").Value;
        }

        public SqlConnection GetBgFacturacionConnection()
        {
            return new SqlConnection(GetBgFacturacionConnectionString());
        }


        public static SqlConnection GetStaticConnection()
        {
            // Fuente: https://stackoverflow.com/questions/15631602/how-can-i-set-an-sql-server-connection-string
            const String connectionString =
                    "Data Source=DESKTOP-R3SPBDJ;" + // Nombre servidor
                    "Initial Catalog=BG_FacturacionPrueba;" + // Nombre base de datos
                    "User id=root_dba;" + // usuario
                    "Password=TilusMorph1974;" + // Contraseña
                    "TrustServerCertificate=true;" + // Certificado SSL
                    "Integrated Security=SSPI"; // 
            // Data Source=sql.bsite.net\\MSSQL2016;Initial Catalog=fallenage_CerveceriaOptimizacion;User id=fallenage_CerveceriaOptimizacion;Password=Sqlserverpassword2022;Integrated Security=SSPI
            var conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
