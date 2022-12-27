using DatabaseIndexSandbox.Abstract.DB.Helper;
using Npgsql;
using System.Data.Common;

namespace DatabaseIndexSandbox.Postgres.Helper
{
    public class PostgresObjectHelper : GenericDatabaseObjectHelper
    {
        private const string hostNameIdentifier = "Host";
        private const string databaseNameIdentifier = "Database";
        private const string portNumberIdentifier = "Port";
        private const string userNameIdentifier = "Username";
        private const string passwordIdentifier = "Password";

        public PostgresObjectHelper(string hostName, string databaseName, int portNumber, string userName, string password) :
                               base(hostName, databaseName, portNumber, userName, password)
        {
            // Stub to call the constructor in the abstract class.
        }

        public PostgresObjectHelper(string connectionString) :
                               base(connectionString)
        {
            // Stub to call the constructor in the abstract class.
        }

        protected override DbConnection getConnection()
        {
            return new NpgsqlConnection(getConnectionString());
        }

        protected override string getConnectionString()
        {
            return $"Username={UserName};Password={Password};Host={HostName};Port={PortNumber};Database={DatabaseName};Connection Lifetime=0";
        }

        protected override void decomposeConnectionString(string connectionString)
        {
            string[] settings = connectionString.Split(';');
            foreach (string setting in settings)
            {
                string[] components = setting.Split('=');
                switch (components[0])
                {
                    case hostNameIdentifier:
                        HostName = components[1];
                        break;
                    case databaseNameIdentifier:
                        DatabaseName = components[1];
                        break;
                    case portNumberIdentifier:
                        PortNumber = Convert.ToInt32(components[1]);
                        break;
                    case userNameIdentifier:
                        UserName = components[1];
                        break;
                    case passwordIdentifier:
                        Password = components[1];
                        break;
                }
            }
        }
    }
}
