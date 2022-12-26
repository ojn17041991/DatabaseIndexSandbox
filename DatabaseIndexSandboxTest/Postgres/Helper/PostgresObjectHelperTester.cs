using Xunit;
using DatabaseIndexSandbox.Abstract.DB.Helper;
using DatabaseIndexSandbox.Postgres.Helper;
using System.Collections.Generic;
using DatabaseIndexSandboxTest.Postgres.Utils;

namespace DatabaseIndexSandboxTest.Postgres.Helper
{
    public class PostgresObjectHelperTester
    {
        private ConnectionStringTestHelper connectionStringTestHelper = new ConnectionStringTestHelper();

        [Fact]
        public void HostNameIsSetCorrectly()
        {
            string testHostName = "TestHostName";
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper(testHostName, "", 0, "", "");
            Assert.Equal(postgresObjectHelper.HostName, testHostName);
        }

        [Fact]
        public void DatabaseNameIsSetCorrectly()
        {
            string databaseName = "TestDatabaseName";
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper("", databaseName, 0, "", "");
            Assert.Equal(postgresObjectHelper.DatabaseName, databaseName);
        }

        [Fact]
        public void PortNumberIsSetCorrectly()
        {
            int portNumber = 4444;
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper("", "", portNumber, "", "");
            Assert.Equal(postgresObjectHelper.PortNumber, portNumber);
        }

        [Fact]
        public void UserNameIsSetCorrectly()
        {
            string testUserName = "TestUserName";
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper("", "", 0, testUserName, "");
            Assert.Equal(postgresObjectHelper.UserName, testUserName);
        }

        [Fact]
        public void PasswordIsSetCorrectly()
        {
            string passwordName = "PasswordName";
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper("", "", 0, "", passwordName);
            Assert.Equal(postgresObjectHelper.Password, passwordName);
        }

        [Theory]
        [InlineData("localhost", "postgres", 5432, "admin", "pass123!", "Username=admin;Password=pass123!;Host=localhost;Port=5432;Database=postgres;Connection Lifetime=0")]
        [InlineData("127.0.0.1", "my_database", 2448, "sysadmin", "m0nkey2", "Username=sysadmin;Password=m0nkey2;Host=127.0.0.1;Port=2448;Database=my_database;Connection Lifetime=0")]
        [InlineData("\\\\network\\server\\instance", "production", 65535, "ro_user", "HUNTER_11#", "Username=ro_user;Password=HUNTER_11#;Host=\\\\network\\server\\instance;Port=65535;Database=production;Connection Lifetime=0")]
        public void ConnectionStringIsGeneratedCorrectly(string hostName, string databaseName, int portNumber, string userName, string password, string expected)
        {
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper(hostName, databaseName, portNumber, userName, password);
            Assert.True(connectionStringTestHelper.ConnectionStringsAreEqual(postgresObjectHelper.ConnectionString, expected));
        }

        [Fact]
        public void ConnectionIsGeneratedCorrectly()
        {
            bool exceptionTriggered = false;
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper("localhost", "index_sandbox", 5432, "postgres", "Passw0rd#");
            try
            {
                postgresObjectHelper.Connection.Open();
            }
            catch (Exception)
            {
                exceptionTriggered = true;
            }
            Assert.False(exceptionTriggered);
        }

        [Theory]
        [InlineData("", "postgres", 5432, "admin", "pass123!")]
        [InlineData("localhost", "", 5432, "admin", "pass123!")]
        [InlineData("localhost", "postgres", 0, "admin", "pass123!")]
        [InlineData("localhost", "postgres", 5432, "", "pass123!")]
        [InlineData("localhost", "postgres", 5432, "admin", "")]
        [InlineData("", "", 0, "", "")]
        public void CantGenerateConnectionWithInvalidConnectionString(string hostName, string databaseName, int portNumber, string userName, string password)
        {
            bool exceptionTriggered = false;
            IDatabaseObjectHelper postgresObjectHelper = new PostgresObjectHelper(hostName, databaseName, portNumber, userName, password);
            try
            {
                postgresObjectHelper.Connection.Open();
            }
            catch (Exception)
            {
                exceptionTriggered = true;
            }
            Assert.True(exceptionTriggered);
        }
    }
}