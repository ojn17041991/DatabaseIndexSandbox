using DatabaseIndexSandbox.Abstract.DB.Factories;
using DatabaseIndexSandbox.Abstract.DB.Queries;
using DatabaseIndexSandbox.Abstract.DB.Tables;
using DatabaseIndexSandbox.Postgres.Factories;
using Npgsql;
using System.Data.Common;

namespace DatabaseIndexSandbox.Postgres.Tables
{
    public class PostgresTablePopulater : ITablePopulater
    {
        public PostgresTablePopulater(DbConnection connection, string tableName, int numberOfOperations)
        {
            Connection = connection;
            TableName = tableName;
            insertFactory = new PostgresBatchInsertFactory(connection);
            NumberOfOperations = numberOfOperations;
        }

        public DbConnection Connection { get; }
        public string TableName { get; }
        public IList<IColumnConfig> Parameters { get; } = new List<IColumnConfig>();
        public int NumberOfOperations { get; }

        private GenericBatchInsertFactory insertFactory;
        private Random random = new Random();

        public void AddParameters(IList<IColumnConfig> parameters)
        {
            foreach (IColumnConfig parameter in parameters)
            {
                Parameters.Add(parameter);
            }
        }

        public void Populate()
        {
            insertFactory.CreateBatch(TableName, Parameters.Select(p => p.Name).ToList());

            for (int i = 0; i < NumberOfOperations; ++i)
            {
                insertFactory.AddArgumentsToBatch(Parameters.Select(p => p.Values[random.Next(p.Values.Count)]).ToList());
            }

            do
            {
                IList<INonQuery> inserts = insertFactory.GenerateBatch();
                foreach (INonQuery insert in inserts)
                {
                    insert.Execute();
                }
            } while (insertFactory.BatchPosition < NumberOfOperations);
        }
    }
}

