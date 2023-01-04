using DatabaseIndexSandbox.Abstract.DB.Queries;
using Npgsql;
using System.Data.Common;

namespace DatabaseIndexSandbox.Postgres.Inserts
{
    public class PostgresInsert : INonQuery
    {
        public PostgresInsert(NpgsqlConnection connection, string commandText, IDictionary<string, object> parameters)
        {
            Connection = connection;
            CommandText = commandText;
            Parameters = parameters;
        }

        public DbConnection Connection { get; set; }
        public string CommandText { get; set; }
        public IDictionary<string, object> Parameters { get; set; }

        public void Execute()
        {
            using (NpgsqlCommand command = new NpgsqlCommand(CommandText, (NpgsqlConnection)Connection))
            {
                foreach (KeyValuePair<string, object> parameter in Parameters)
                {
                    command.Parameters.AddWithValue('@' + parameter.Key, parameter.Value);
                }
                command.ExecuteNonQuery();
            }
        }
    }
}
