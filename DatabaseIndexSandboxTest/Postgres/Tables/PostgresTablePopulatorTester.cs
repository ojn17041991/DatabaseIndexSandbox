using DatabaseIndexSandbox.Abstract.DB.Tables;
using DatabaseIndexSandbox.Postgres.Tables;
using DatabaseIndexSandboxTest.Config;
using DatabaseIndexSandboxTest.Utils.Database;
using Npgsql;
using Xunit;

namespace DatabaseIndexSandboxTest.Postgres.Tables
{
    public class PostgresTablePopulatorTester
    {
        // Create a ConfigHelper to read from the test config file.
        private ConfigHelper config = new ConfigHelper("Config/config.json");
        private NpgsqlConnection connection;

        // Create an object to compare connection strings.
        private ConnectionStringTestHelper connectionStringTestHelper = new ConnectionStringTestHelper();

        public PostgresTablePopulatorTester()
        {
            // Set up a connection once in the constructor to save on memory usage.
            connection = new NpgsqlConnection(config.ConnectionString);
            connection.Open();
        }



        [Fact]
        public void PopulatorIsConstructedCorrectly()
        {
            // Create the populator.
            ITablePopulater populator = new PostgresTablePopulater(connection, config.TableName, 1);
        }
    }
}
