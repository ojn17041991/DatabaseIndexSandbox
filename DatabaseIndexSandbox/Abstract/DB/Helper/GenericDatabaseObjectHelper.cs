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

        public string HostName { get; }

        public string DatabaseName { get; }

        public int PortNumber { get; }

        public string UserName { get; }

        public string Password { get; }

        public string ConnectionString { get; }

        public DbConnection Connection { get { return getConnection(); } }

        internal abstract string getConnectionString();

        internal abstract DbConnection getConnection();
    }
}
