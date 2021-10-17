using ExchangeRates.Common.Repos;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace ExchangeRates.Importer.Repos
{
    public interface IDBConnectionProvider
    {
        Task<IDbConnection> CreateConnection();
    }

    public class PGDBConnectionProvider : IDBConnectionProvider
    {
        private string _connectionString;

        public PGDBConnectionProvider(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionString = connectionStrings.Value.PGConnectionString;
        }
        public async Task<IDbConnection> CreateConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            return connection;
        }
    }
}