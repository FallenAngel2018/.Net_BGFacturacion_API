using Microsoft.Data.SqlClient;

namespace BgTiendaFacturacionAPI.Domain.Connection
{
    public interface IConnectionRepository
    {
        SqlConnection GetBgFacturacionConnection();
    }
}
