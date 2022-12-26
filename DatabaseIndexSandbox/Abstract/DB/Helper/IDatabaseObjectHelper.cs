using System.Data.Common;

namespace DatabaseIndexSandbox.Abstract.DB.Helper
{
    public interface IDatabaseObjectHelper
    {
        string HostName { get; }
        string DatabaseName { get; }
        int PortNumber { get; }
        string UserName { get; }
        string Password { get; }
        string ConnectionString { get; }
        DbConnection Connection { get; }
    }
}
