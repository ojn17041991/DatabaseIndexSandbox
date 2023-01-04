using DatabaseIndexSandbox.Abstract.DB.Queries;
using System.Data.Common;
using System.Text;

namespace DatabaseIndexSandbox.Abstract.DB.Factories.NonQueries.Inserts
{
    public abstract class GenericBatchInsertFactory : IBatchInsertFactory
    {
        public GenericBatchInsertFactory(DbConnection connection)
        {
            Connection = connection;
        }



        public DbConnection Connection { get; }
        public string TableName { get; set; } = string.Empty;
        public IList<string> Parameters { get; set; } = new List<string>();
        public IList<IList<object>> Arguments { get; set; } = new List<IList<object>>();
        public int MaxBatchSize { get; protected set; } = 10000;
        public int BatchPosition { get; protected set; }



        protected string prepareCommandText(string tableName, IList<string> parameters)
        {
            StringBuilder commandText = new StringBuilder();
            commandText.AppendLine($"INSERT INTO {tableName} (");
            commandText.AppendLine(string.Join(',', parameters.Select(p => p)));
            commandText.AppendLine(") VALUES (");
            commandText.AppendLine(string.Join(',', parameters.Select(p => '@' + p)));
            commandText.AppendLine(")");
            return commandText.ToString();
        }

        protected IDictionary<string, object> combineParametersAndArguments(IList<object> arguments)
        {
            IDictionary<string, object> combinedParameters = new Dictionary<string, object>();
            for (int i = 0; i < Parameters.Count; ++i)
            {
                combinedParameters.Add(Parameters[i], arguments[i]);
            }
            return combinedParameters;
        }



        public void CreateBatch(string tableName, IList<string> parameters)
        {
            TableName = tableName;
            Parameters = parameters;
            Arguments = new List<IList<object>>();
        }

        public void AddArgumentsToBatch(IList<object> arguments)
        {
            Arguments.Add(arguments);
        }



        public abstract INonQuery Generate(string tableName, IDictionary<string, object> parameters);
        public abstract IList<INonQuery> GenerateBatch();
    }
}
