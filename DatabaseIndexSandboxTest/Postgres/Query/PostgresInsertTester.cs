using DatabaseIndexSandbox.Abstract.DB.Queries;
using DatabaseIndexSandbox.Postgres.Inserts;
using DatabaseIndexSandboxTest.Postgres.Config;
using DatabaseIndexSandboxTest.Postgres.Utils;
using Npgsql;
using System.Reflection;
using Xunit;

namespace DatabaseIndexSandboxTest.Postgres.Query
{
    public class PostgresInsertTester
    {
        private ConfigHelper config = new ConfigHelper();
        private string connectionString = string.Empty;
        private string tableName = string.Empty;
        private string[] columnNames = new string[0];
        private NpgsqlConnection connection;

        private ConnectionStringTestHelper connectionStringTestHelper = new ConnectionStringTestHelper();

        public PostgresInsertTester()
        {
            connectionString = config.ConnectionString;
            tableName = config.TableName;
            columnNames = config.ColumnNames;
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }

        [Fact]
        public void ConnectionIsSetCorrectly()
        {
            // Set up the PostgresInsert args.
            string commandText = string.Empty;
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            // Create the PostgresInsert object.
            INonQuery insert = new PostgresInsert(connection, commandText, parameters);

            // Assert that the values in the PostgresInsert Connection property matche those in the one provided.
            Assert.True(connectionStringTestHelper.ConnectionStringsAreEqual(insert.Connection.ConnectionString, connectionString));
        }

        [Fact]
        public void CommandTextIsSetCorrectly()
        {
            // Set up the Postgres Insert args.
            string commandText = "SELECT * FROM MyTable";
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            // Create the PostgresInsert object.
            INonQuery insert = new PostgresInsert(connection, commandText, parameters);

            // Assert that the value in the PostgresInsert CommandText property matches the one provided.
            Assert.True(insert.CommandText == commandText);
        }

        [Fact]
        public void ParametersAreSetCorrectly()
        {
            // Set up the Postgres Insert args.
            string commandText = string.Empty;
            string columnName = "@MyColumn";
            string columnValue = "ColumnValue";
            IDictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { columnName, columnValue }
            };

            // Create the PostgresInsert object.
            INonQuery insert = new PostgresInsert(connection, commandText, parameters);

            // Assert that the values in the PostgresInsert Parameters property matches the one provided.
            Assert.True(
                insert.Parameters.First().Key == columnName &&
                insert.Parameters.First().Value.ToString() == columnValue
            );
        }

        [Theory]
        [InlineData(1, "Oliver", "Nicholls")]
        [InlineData(2, "Joe", "Bloggs")]
        [InlineData(3, "Jane", "Doe")]
        [InlineData(4, "Mick", "Jagger")]
        [InlineData(5, "Paul", "McCartney")]
        public void InsertsWithoutError(params object[] arguments)
        {
            // Set up the command text.
            string commandParameters = String.Join(',', columnNames);
            string commandArguments = String.Join(',', columnNames.Select(c => '@' + c));
            string commandText = $"INSERT INTO {tableName} ({commandParameters}) VALUES ({commandArguments})";

            // Set up the parameters.
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            for (int i = 0 ; i < columnNames.Length ; i++)
            {
                parameters.Add('@' + columnNames[i], arguments[i]);
            }

            // Create the PostgresInsert object.
            INonQuery insert = new PostgresInsert(connection, commandText, parameters);

            // Execute the INSERT command and check for errors.
            bool exceptionOccurred = false;
            try
            {
                insert.Execute();
            }
            catch (Exception)
            {
                exceptionOccurred = true;
            }

            // Assert that no errors occurred.
            Assert.False(exceptionOccurred);
        }

        ~PostgresInsertTester()
        {
            connection.Close();
        }
    }
}
