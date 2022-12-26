using Microsoft.Extensions.Configuration;

namespace DatabaseIndexSandboxTest.Postgres.Config
{
    public class ConfigHelper
    {
        private string configLocation { get; } = "Postgres/Config/config.json";
        public string ConnectionString { get; } = string.Empty;
        public string TableName { get; } = string.Empty;
        public string[] ColumnNames { get; } = new string[0];

        public ConfigHelper()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile(configLocation);
            IConfigurationRoot configuration = configurationBuilder.Build();
            ConnectionString = configuration.GetSection("ConnectionStrings:Test")?.Value ?? string.Empty;
            TableName = configuration.GetSection("SchemaInfo:Tables:Users:Name")?.Value ?? string.Empty;
            ColumnNames = configuration.GetSection("SchemaInfo:Tables:Users:Columns")?.GetChildren().Select(c => c.Value ?? string.Empty).ToArray() ?? new string[0];
        }
    }
}
