using Xunit;
using DatabaseIndexSandbox.Abstract.DB.Helper;
using DatabaseIndexSandbox.Postgres.Helper;
using FluentAssertions;
using DatabaseIndexSandboxTest.Utils.Database;
using DatabaseIndexSandboxTest.Config;
using FluentAssertions.Execution;

namespace DatabaseIndexSandboxTest.Postgres.Helper
{
    public class PostgresObjectHelperTester
    {
        // Create a ConfigHelper to read from the test config file.
        private ConfigHelper config = new ConfigHelper("Config/config.json");

        // Create an object to compare connection strings.
        private ConnectionStringTestHelper connectionStringTestHelper = new ConnectionStringTestHelper();



        [Fact]
        public void ComponentsAreStoredCorrectly()
        {
            // Create the PostgresObjectHelpe with the config data.
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper(
                config.HostName,
                config.DatabaseName,
                config.PortNumber,
                config.UserName,
                config.Password
            );

            // Each component should match those defined in the config data.
            using (new AssertionScope())
            {
                postgresObjectHelper.HostName.Should().Be(config.HostName);
                postgresObjectHelper.DatabaseName.Should().Be(config.DatabaseName);
                postgresObjectHelper.PortNumber.Should().Be(config.PortNumber);
                postgresObjectHelper.UserName.Should().Be(config.UserName);
                postgresObjectHelper.Password.Should().Be(config.Password);
            }
        }

        [Fact]
        public void ConnectionStringIsStoredCorrectly()
        {
            // Create the PostgresObjectHelpe with the config data.
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper(config.ConnectionString);

            // The connection string should match the one defined in the config data.
            postgresObjectHelper.ConnectionString.Should().Be(config.ConnectionString);
        }

        [Theory]
        [MemberData(nameof(connectionStringTestArgs))]
        public void ConnectionStringIsGeneratedCorrectlyFromComponents(
            string hostName, 
            string databaseName,
            int portNumber,
            string userName, 
            string password, 
            string connectionString)
        {
            // Create the PostgresObjectHelper with all properties required for the connection string.
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper(
                hostName,
                databaseName,
                portNumber,
                userName,
                password
            );

            // Assert that the connection string generated is the same as the one expected.            
            connectionStringTestHelper.ConnectionStringsAreEqual(
                postgresObjectHelper.ConnectionString,
                connectionString
            ).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(connectionStringTestArgs))]
        public void ComponentsAreGeneratedCorrectlyFromConnectionString(
            string hostName, 
            string databaseName,
            int portNumber,
            string userName, 
            string password, 
            string connectionString)
        {
            // Create the PostgresObjectHelper with all properties required for the connection string.
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper(connectionString);

            // Each component should match the ones in the connection string.
            using (new AssertionScope())
            {
                postgresObjectHelper.HostName.Should().Be(hostName);
                postgresObjectHelper.DatabaseName.Should().Be(databaseName);
                postgresObjectHelper.PortNumber.Should().Be(portNumber);
                postgresObjectHelper.UserName.Should().Be(userName);
                postgresObjectHelper.Password.Should().Be(password);
            }
        }

        [Fact]
        public void ConnectionIsGeneratedCorrectly()
        {
            // Create the PostgresObjectHelper with a valid connection string.
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper(config.ConnectionString);

            // Open the connection as an action and confirm that the action does not throw an Exception.
            Action action = () => postgresObjectHelper.Connection.Open();
            action.Should().NotThrow<Exception>();
        }



        public static IEnumerable<object[]> connectionStringTestArgs()
        {
            // Return re-usable test data for comparing connection strings against individual connection string components.
            yield return new object[] {
                "localhost",
                "postgres",
                5432,
                "admin",
                "pass123!",
                "Username=admin;Password=pass123!;Host=localhost;Port=5432;Database=postgres;Connection Lifetime=0"
            };
            yield return new object[] {
                "127.0.0.1",
                "my_database",
                2448,
                "sysadmin",
                "m0nkey2",
                "Username=sysadmin;Password=m0nkey2;Host=127.0.0.1;Port=2448;Database=my_database;Connection Lifetime=0"
            };
            yield return new object[] {
                "\\\\network\\server\\instance",
                "production",
                65535,
                "ro_user",
                "HUNTER_11#",
                "Username=ro_user;Password=HUNTER_11#;Host=\\\\network\\server\\instance;Port=65535;Database=production;Connection Lifetime=0"
            };
        }
    }
}