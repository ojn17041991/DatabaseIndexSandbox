using DatabaseIndexSandbox.Abstract.DB.Helper;
using Npgsql;
using System.Data.Common;

namespace DatabaseIndexSandbox.Postgres.Helper
{
    public class PostgresObjectHelper : GenericDatabaseObjectHelper
    {
        public PostgresObjectHelper(string hostName, string databaseName, int portNumber, string userName, string password) :
                               base(hostName, databaseName, portNumber, userName, password)
        {
        }

        internal override DbConnection getConnection()
        {
            return new NpgsqlConnection(getConnectionString());
        }

        internal override string getConnectionString()
        {
            return $"Username={UserName};Password={Password};Host={HostName};Port={PortNumber};Database={DatabaseName};Connection Lifetime=0";
        }
    }
}
