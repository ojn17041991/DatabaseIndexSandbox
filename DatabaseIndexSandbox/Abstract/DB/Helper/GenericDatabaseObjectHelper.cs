using System.Data.Common;

namespace DatabaseIndexSandbox.Abstract.DB.Helper
{
    public abstract class GenericDatabaseObjectHelper : IDatabaseObjectHelper
    {
        public GenericDatabaseObjectHelper(string hostName, string databaseName, int portNumber, string userName, string password)
        {
            HostName = hostName;
            DatabaseName = databaseName;
            PortNumber = portNumber;
            UserName = userName;
            Password = password;
            ConnectionString = getConnectionString();
        }

        public GenericDatabaseObjectHelper(string connectionString)
        {
            ConnectionString = connectionString;
            decomposeConnectionString(ConnectionString);
        }

        public string HostName { get; protected set; } = string.Empty;

        public string DatabaseName { get; protected set; } = string.Empty;

        public int PortNumber { get; protected set; } = default(int);

        public string UserName { get; protected set; } = string.Empty;

        public string Password { get; protected set; } = string.Empty;

        public string ConnectionString { get; protected set; } = string.Empty;

        public DbConnection Connection { get { return getConnection(); } }

        protected abstract string getConnectionString();

        protected abstract DbConnection getConnection();

        protected abstract void decomposeConnectionString(string connectionString);
    }
}
