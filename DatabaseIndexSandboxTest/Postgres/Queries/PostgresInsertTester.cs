using DatabaseIndexSandbox.Abstract.DB.Queries;
using DatabaseIndexSandbox.Abstract.DB.Tables;
using DatabaseIndexSandbox.Postgres.Inserts;
using DatabaseIndexSandboxTest.Config;
using DatabaseIndexSandboxTest.Utils.Database;
using FluentAssertions;
using FluentAssertions.Execution;
using Npgsql;
using Xunit;

namespace DatabaseIndexSandboxTest.Postgres.Query
{
    public class PostgresInsertTester
    {
        // Create a ConfigHelper to read from the test config file.
        private ConfigHelper config = new ConfigHelper("Config/config.json");
        private NpgsqlConnection connection;
        private IList<IColumnConfig> columns;

        // Create an object to compare connection strings.
        private ConnectionStringTestHelper connectionStringTestHelper = new ConnectionStringTestHelper();



        public PostgresInsertTester()
        {
            // Set up a connection once in the constructor to save on memory usage.
            connection = new NpgsqlConnection(config.ConnectionString);
            connection.Open();
            columns = config.Tables[config.UsersTableName];
        }



        [Fact]
        public void InsertIsConstructedCorrectly()
        {
            // Set up the command text.
            string commandText = "INSERT INTO table (column) VALUES ('dummy')";

            // Set up the parameters.
            string columnName = "@MyColumn";
            string columnValue = "ColumnValue";
            IDictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { columnName, columnValue }
            };

            // Create the PostgresInsert object.
            INonQuery insert = new PostgresInsert(connection, commandText, parameters);

            // Assert that the connection, command text and parameters all match the original values. 
            using (new AssertionScope())
            {
                // Connection string should match config.
                connectionStringTestHelper.ConnectionStringsAreEqual(
                    insert.Connection.ConnectionString, config.ConnectionString
                ).Should().BeTrue();

                // Command text should match original.
                insert.CommandText.Should().Be(commandText);

                // Paramters should match originals.
                insert.Parameters[columnName].Should().Be(columnValue);
            }
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
            string commandParameters = String.Join(',', columns.Select(c => c.Name));
            string commandArguments = String.Join(',', columns.Select(c => '@' + c.Name));
            string commandText = $"INSERT INTO {config.UsersTableName} ({commandParameters}) VALUES ({commandArguments})";

            // Set up the parameters.
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            for (int i = 0; i < columns.Count; i++)
            {
                parameters.Add(columns[i].Name, arguments[i]);
            }

            // Create the PostgresInsert object.
            INonQuery insert = new PostgresInsert(connection, commandText, parameters);
            Action action = () => insert.Execute();

            // The insert execution should not throw an Exception.
            action.Should().NotThrow<Exception>();
        }



        ~PostgresInsertTester()
        {
            // We're done testing the inserts, so close the connection.
            connection.Close();
        }
    }
}
