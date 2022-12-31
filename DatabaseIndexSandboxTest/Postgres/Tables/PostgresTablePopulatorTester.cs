using DatabaseIndexSandbox.Abstract.DB.Factories.NonQueries.Inserts;
using DatabaseIndexSandbox.Abstract.DB.Tables;
using DatabaseIndexSandbox.Generic.Tables;
using DatabaseIndexSandbox.Postgres.Factories;
using DatabaseIndexSandbox.Postgres.Tables;
using DatabaseIndexSandboxTest.Config;
using DatabaseIndexSandboxTest.Utils.Database;
using FluentAssertions;
using FluentAssertions.Execution;
using Npgsql;
using System.Configuration.Internal;
using Xunit;

namespace DatabaseIndexSandboxTest.Postgres.Tables
{
    public class PostgresTablePopulatorTester
    {
        // Create a ConfigHelper to read from the test config file.
        private ConfigHelper config = new ConfigHelper("Config/config.json");
        private NpgsqlConnection connection;
        private IList<IColumnConfig> columns;

        // Create an object to compare connection strings.
        private ConnectionStringTestHelper connectionStringTestHelper = new ConnectionStringTestHelper();

        // Create some dummy arguments.
        IList<object> idArguments = new List<object>()
        {
            101,
            102,
            103
        };
        IList<object> firstNameArguments = new List<object>()
        {
            "Test1",
            "Test2",
            "Test3"
        };
        IList<object> lastNameArguments = new List<object>()
        {
            "User1",
            "User2",
            "User3"
        };



        public PostgresTablePopulatorTester()
        {
            // Set up a connection once in the constructor to save on memory usage.
            connection = new NpgsqlConnection(config.ConnectionString);
            connection.Open();
            columns = config.Tables[config.UsersTableName];
        }



        [Fact]
        public void PopulatorIsConstructedCorrectly()
        {
            // Create the populator.
            int numberOfOperations = 1;
            ITablePopulator populator = new PostgresTablePopulater(connection, config.UsersTableName, numberOfOperations);

            // Confirm the arguments provided match the ones stored.
            using (new AssertionScope())
            {
                // Connection strings should match.
                connectionStringTestHelper.ConnectionStringsAreEqual(
                    populator.Connection.ConnectionString,
                    config.ConnectionString
                ).Should().BeTrue();

                // Table names should match.
                populator.TableName.Should().Be(config.UsersTableName);

                // Number of operations should match.
                populator.NumberOfOperations.Should().Be(numberOfOperations);
            }
        }

        [Fact]
        public void PopulatorParametersAreAddedCorrectly()
        {
            // Create the populator.
            int numberOfOperations = 1;
            ITablePopulator populator = new PostgresTablePopulater(connection, config.UsersTableName, numberOfOperations);

            // Add the parameters.
            populator.AddParameters(columns);

            // Confirm the paramters provided match the ones stored.
            using (new AssertionScope())
            {
                for (int i = 0; i < columns.Count; ++i)
                {
                    // Convert the column configs to make Values accessible for the test.
                    BaseNonUniqueColumnConfig expectedColumnConfig = (NonUniqueColumnConfig)columns[i];
                    BaseNonUniqueColumnConfig actualColumnConfig = (NonUniqueColumnConfig)populator.Parameters[i];

                    // The column names should match.
                    actualColumnConfig.Name.Should().Be(expectedColumnConfig.Name);

                    // The 
                    for (int j = 0; j < expectedColumnConfig.Values.Count; ++j)
                    {
                        actualColumnConfig.Values[j].Should().Be(expectedColumnConfig.Values[j]);
                    }
                }
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(10000)]
        public void PopulatorPopulatesWithoutError(int numberOfOperations)
        {
            // Create the populator.
            ITablePopulator populator = new PostgresTablePopulater(connection, config.UsersTableName, numberOfOperations);

            // Add the parameters.
            populator.AddParameters(columns);

            // Populate the table. There should be no error.
            Action action = new Action(() => populator.Populate());
            action.Should().NotThrow<Exception>();
        }
    }
}
