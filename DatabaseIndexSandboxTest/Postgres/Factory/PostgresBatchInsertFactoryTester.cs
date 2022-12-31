using DatabaseIndexSandbox.Abstract.DB.Factories.NonQueries.Inserts;
using DatabaseIndexSandbox.Abstract.DB.Queries;
using DatabaseIndexSandbox.Abstract.DB.Tables;
using DatabaseIndexSandbox.Postgres.Factories;
using DatabaseIndexSandboxTest.Config;
using DatabaseIndexSandboxTest.Utils.Database;
using FluentAssertions;
using FluentAssertions.Execution;
using Npgsql;
using Xunit;

namespace DatabaseIndexSandboxTest.Postgres.Factory
{
    public class PostgresBatchInsertFactoryTester
    {
        // Create a ConfigHelper to read from the test config file.
        private ConfigHelper config = new ConfigHelper("Config/config.json");
        private NpgsqlConnection connection;
        private IList<IColumnConfig> columns;

        // Create an object to compare connection strings.
        private ConnectionStringTestHelper connectionStringTestHelper = new ConnectionStringTestHelper();

        // Create some dummy arguments.
        IList<IList<object>> arguments = new List<IList<object>>()
        {
            new List<object>() { 101, "Test1", "User1" },
            new List<object>() { 102, "Test2", "User2" },
            new List<object>() { 103, "Test3", "User3" },
            new List<object>() { 104, "Test4", "User4" },
            new List<object>() { 105, "Test5", "User5" }
        };

        public PostgresBatchInsertFactoryTester()
        {
            // Set up a connection once in the constructor to save on memory usage.
            connection = new NpgsqlConnection(config.ConnectionString);
            connection.Open();
            columns = config.Tables[config.UsersTableName];
        }



        [Fact]
        public void FactoryIsConstructedCorrectly()
        {
            // Create the factory.
            factoryOption options = factoryOption.CreateFactory;
            GenericBatchInsertFactory factory = getFactory(options);

            // Assert that the connection strings match between the factory and config.
            connectionStringTestHelper.ConnectionStringsAreEqual(
                factory.Connection.ConnectionString,
                config.ConnectionString
            ).Should().BeTrue();
        }

        [Fact]
        public void BatchIsCreatedCorrectly()
        {
            // Create the factory with the batch.
            factoryOption options = 
                factoryOption.CreateFactory | 
                factoryOption.CreateBatch;
            GenericBatchInsertFactory factory = getFactory(options);

            // Assert that the table name and parameters match between the factory and config.
            using (new AssertionScope())
            {
                factory.TableName.Should().Be(config.UsersTableName);

                for (int i = 0; i < columns.Count; ++i)
                {
                    factory.Parameters[i].Should().Be('@' + columns[i].Name);
                }
            }
        }

        [Fact]
        public void ArgumentsAreAddedCorrectly()
        {
            // Create the factory with the batch and arguments appended.
            factoryOption options = 
                factoryOption.CreateFactory | 
                factoryOption.CreateBatch | 
                factoryOption.AddArguments;
            GenericBatchInsertFactory factory = getFactory(options);

            // Assert that each of the arguments matches the pre-defined list.
            using (new AssertionScope())
            {
                for (int i = 0; i < arguments.Count; ++i)
                {
                    for (int j = 0; j < arguments[i].Count; ++j)
                    {
                        factory.Arguments[i][j].Should().Be(arguments[i][j]);
                    }
                }
            }
        }

        [Fact]
        public void BatchIsGeneratedCorrectly()
        {
            // Create the factory with the batch and arguments appended.
            factoryOption options = 
                factoryOption.CreateFactory | 
                factoryOption.CreateBatch | 
                factoryOption.AddArguments;
            GenericBatchInsertFactory factory = getFactory(options);

            // Get the batch from the factory.
            IList<INonQuery> inserts = factory.GenerateBatch();

            // Assert that we have the right number of inserts and the insert properties are correct.
            using (new AssertionScope())
            {
                // Confirm the number of inserts in the batch.
                arguments.Count.Should().Be(inserts.Count);

                for (int i = 0; i < inserts.Count; ++i)
                {
                    // Confirm each insert has the right connection.
                    connectionStringTestHelper.ConnectionStringsAreEqual(
                        inserts[i].Connection.ConnectionString,
                        config.ConnectionString
                    ).Should().BeTrue();

                    // Confirm that each insert has all required parameters.
                    foreach (IColumnConfig column in columns)
                    {
                        Assert.True(inserts[i].Parameters.ContainsKey('@' + column.Name));
                    }

                    // Confirm that each insert has the correct arguments.
                    for (int j = 0; j < arguments[i].Count; ++j)
                    {
                        inserts[i].Parameters['@' + columns[j].Name].Should().Be(arguments[i][j]);
                    }
                }
            }
        }

        [Fact]
        public void MaxBatchSizeIsApplied()
        {
            // Create the factory with the batch created and batch arguments appended.
            factoryOption options =
                factoryOption.CreateFactory |
                factoryOption.CreateBatch |
                factoryOption.AddArguments |
                factoryOption.AddArgumentsBatch;
            GenericBatchInsertFactory factory = getFactory(options);

            // Get the batch from the factory.
            IList<INonQuery> batch1 = factory.GenerateBatch();
            IList<INonQuery> batch2 = factory.GenerateBatch();

            // The inserts should have been split due to exceeding the max batch size in the factory.
            using (new AssertionScope())
            {
                batch1.Count.Should().Be(factory.MaxBatchSize);
                batch2.Count.Should().Be(arguments.Count);
            }
        }



        [Flags]
        private enum factoryOption
        {
            CreateFactory = 1,
            CreateBatch = 2,
            AddArguments = 4,
            AddArgumentsBatch = 8
        }
        private GenericBatchInsertFactory getFactory(factoryOption options)
        {
            // Create the factory.
            GenericBatchInsertFactory factory = new PostgresBatchInsertFactory(connection);

            if (options.HasFlag(factoryOption.CreateFactory))
            {
                if (options.HasFlag(factoryOption.CreateBatch))
                {
                    // Add the table name and parameters to create the batch.
                    IList<string> parameters = columns.Select(c => '@' + c.Name).ToList();
                    factory.CreateBatch(config.UsersTableName, parameters);

                    // Check if we're adding arguments in bulk. If so we need to add more than the factory max batch size.
                    int numberOfArgumentLoops = 1;
                    if (options.HasFlag(factoryOption.AddArgumentsBatch))
                    {
                        numberOfArgumentLoops = (factory.MaxBatchSize / arguments.Count) + 1;
                    }

                    // Add in a list of pre-defined arguments.
                    if (options.HasFlag(factoryOption.AddArguments))
                    {
                        for (int i = 0; i < numberOfArgumentLoops; ++i)
                        {
                            foreach (IList<object> argumentSet in arguments)
                            {
                                factory.AddArgumentsToBatch(argumentSet);
                            }
                        }
                    }
                }
            }

            // Return the factory with the requested setup.
            return factory;
        }

        ~PostgresBatchInsertFactoryTester()
        {
            // We're done testing the inserts, so close the connection.
            connection.Close();
        }
    }
}
