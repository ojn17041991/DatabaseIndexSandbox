using DatabaseIndexSandbox.Abstract.DB.Tables;
using DatabaseIndexSandbox.Generic.Tables;
using DatabaseIndexSandboxTest.Config;
using FluentAssertions.Execution;
using FluentAssertions;
using Xunit;

namespace DatabaseIndexSandboxTest.Postgres.Tables
{
    public class AutoIncrementingColumnConfigTester
    {
        // Create a ConfigHelper to read from the test config file.
        private ConfigHelper config = new ConfigHelper("Config/config.json");
        private IList<IColumnConfig> columns;



        public AutoIncrementingColumnConfigTester()
        {
            columns = config.Tables[config.CounterTableName];
        }



        [Fact]
        public void ColumnConfigIsConstructedCorrectly()
        {
            // Set up the column config.
            int startValue = 1;
            BaseAutoIncrementingColumnConfig column = (BaseAutoIncrementingColumnConfig)columns.First();
            BaseAutoIncrementingColumnConfig columnConfig = new AutoIncrementingColumnConfig(
                column.Name,
                column.ColumnType,
                startValue
            );

            // Confirm that the properties in the column config match the arguments provided.
            using (new AssertionScope())
            {
                columnConfig.Name.Should().Be(column.Name);
                columnConfig.StartValue.Should().Be(startValue);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(207)]
        [InlineData(1098440116)]
        [InlineData(0)]
        [InlineData(-55)]
        public void NextValueIsExpected(int startValue)
        {
            // Set up the column config.
            BaseAutoIncrementingColumnConfig column = (BaseAutoIncrementingColumnConfig)columns.First();
            BaseAutoIncrementingColumnConfig columnConfig = new AutoIncrementingColumnConfig(
                column.Name,
                column.ColumnType,
                startValue
            );

            // Generate a series of values and confirm that they are as expected.
            using (new AssertionScope())
            {
                int numTestLoops = 10;
                int previousValue = startValue - 1;
                for (int i = 0; i < numTestLoops; ++i)
                {
                    // Get the next value.
                    object nextValue = columnConfig.GetNextValue();

                    // The new value should be the previous value incremented
                    nextValue.Should().Be(previousValue + 1);

                    // Update the previous value.
                    previousValue = Convert.ToInt32(nextValue);
                }
            }
        }
    }
}
