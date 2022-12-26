using DatabaseIndexSandbox.Abstract.DB.Factories;
using DatabaseIndexSandbox.Abstract.DB.Queries;
using DatabaseIndexSandbox.Postgres.Inserts;
using Npgsql;
using System.Data.Common;

namespace DatabaseIndexSandbox.Postgres.Factories
{
    public class PostgresBatchInsertFactory : GenericBatchInsertFactory
    {
        public PostgresBatchInsertFactory(DbConnection connection) : base(connection)
        {
            MaxBatchSize = 10000;
        }

        protected override int MaxBatchSize { get; set; }

        public override INonQuery Generate(string tableName, IDictionary<string, object> parameters)
        {
            string commandText = prepareCommandText(tableName, parameters.Select(p => p.Key).ToList());
            return new PostgresInsert((NpgsqlConnection)Connection, commandText, parameters);
        }

        public override IList<INonQuery> GenerateBatch()
        {
            // This will loop indefinitely, on repeat.
            // Outside this function, you will need to use a while loop
            //   comparing BatchPosition to the total number of inserts required.
            IList<INonQuery> queries = new List<INonQuery>();

            // Get the SQL command text.
            string commandText = prepareCommandText(TableName, Parameters);

            // Calculate the start and end indexes of this batch.
            int totalNumberOfIterations = Arguments.Count;
            int remainingNumberOfIterations = totalNumberOfIterations - BatchPosition;
            int numberOfIterations = Math.Min(remainingNumberOfIterations, MaxBatchSize);
            int startIndex = BatchPosition;
            int endIndex = startIndex + numberOfIterations;

            // Start building up a list of queries ready for execution.
            for (int i = startIndex; i < endIndex; ++i)
            {
                // Get the parameter:argument dictionary for the SQL command.
                IDictionary<string, object> combinedParameters = combineParametersAndArguments(Arguments[i]);

                // Generate the Insert object and add it to the list of queries.
                queries.Add(new PostgresInsert((NpgsqlConnection)Connection, commandText, combinedParameters));
            }

            // Increase the batch position to the end of the current batch.
            if (endIndex <= totalNumberOfIterations)
            {
                BatchPosition += MaxBatchSize;
            }
            else
            {
                BatchPosition = 0;
            }

            // Return this batch of inserts.
            return queries;
        }
    }
}
