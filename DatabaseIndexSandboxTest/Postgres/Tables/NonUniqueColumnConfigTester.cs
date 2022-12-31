using DatabaseIndexSandbox.Abstract.DB.Tables;
using DatabaseIndexSandbox.Generic.Tables;
using DatabaseIndexSandboxTest.Config;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace DatabaseIndexSandboxTest.Postgres.Tables
{
    public class NonUniqueColumnConfigTester
    {
        // Create a ConfigHelper to read from the test config file.
        private ConfigHelper config = new ConfigHelper("Config/config.json");
        private IList<IColumnConfig> columns;



        public NonUniqueColumnConfigTester()
        {
            columns = config.Tables[config.UsersTableName];
        }



        [Fact]
        public void ColumnConfigIsConstructedCorrectly()
        {
            // Set up the column config.
            BaseNonUniqueColumnConfig column = (BaseNonUniqueColumnConfig)columns.First();
            BaseNonUniqueColumnConfig columnConfig = new NonUniqueColumnConfig(
                column.Name,
                column.ColumnType,
                column.Values
            );

            // Confirm that the properties in the column config match the arguments provided.
            using (new AssertionScope())
            {
                columnConfig.Name.Should().Be(columns.First().Name);
                for (int i = 0; i < columnConfig.Values.Count; i++)
                {
                    column.Values[i].Should().Be(columnConfig.Values[i]);
                }
            }
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("Hello", "World")]
        [InlineData(0, 1, 3, 6, 10)]
        [InlineData(0.4321, "1", 77, "Something", -15)]
        public void NextValueIsExpected(params object[] values)
        {
            // Set up the column config.
            BaseNonUniqueColumnConfig column = (BaseNonUniqueColumnConfig)columns.First();
            BaseNonUniqueColumnConfig columnConfig = new NonUniqueColumnConfig(column.Name, column.ColumnType, values);

            // Generate a series of values and confirm that they are as expected.
            using (new AssertionScope())
            {
                int numTestLoops = 10;
                for (int i = 0; i < numTestLoops; ++i)
                {
                    // Get the next value.
                    object nextValue = columnConfig.GetNextValue();

                    // The new value should be one of the ones provided to the constructor.
                    values.Should().Contain(nextValue);
                }
            }
        }
    }
}
