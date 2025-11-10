using Microsoft.Data.SqlClient;
using System.Data;

namespace NoteApi.Services
{
    public class DbContext(IConfiguration configuration)
    {
        public IDbConnection CreateConnection()
        => new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }
}
