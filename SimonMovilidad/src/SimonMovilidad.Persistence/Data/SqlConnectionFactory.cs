using Npgsql;
using SimonMovilidad.Application.Abstractions.Data;
using System.Data;

namespace SimonMovilidad.Persistence.Data
{
    internal sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            return connection;
        }
    }
}
