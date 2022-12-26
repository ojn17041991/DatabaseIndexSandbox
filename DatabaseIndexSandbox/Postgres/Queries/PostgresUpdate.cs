using DatabaseIndexSandbox.Abstract.DB.Queries;
using System.Data.Common;

namespace DatabaseIndexSandbox.Postgres.Queries
{
    internal class PostgresUpdate : INonQuery
    {
        public DbConnection Connection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string CommandText { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDictionary<string, object> Parameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
