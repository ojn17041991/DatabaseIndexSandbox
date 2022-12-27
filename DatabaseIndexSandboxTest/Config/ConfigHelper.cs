using Microsoft.Extensions.Configuration;

namespace DatabaseIndexSandboxTest.Config
{
    public class ConfigHelper
    {
        public string ConfigLocation { get; } = string.Empty;

        public string ConnectionString { get; } = string.Empty;
        public string HostName { get; } = string.Empty;
        public string DatabaseName { get; } = string.Empty;
        public int PortNumber { get; } = default(int);
        public string UserName { get; } = string.Empty;
        public string Password { get; } = string.Empty;

        public string TableName { get; } = string.Empty;
        public string[] ColumnNames { get; } = new string[0];

        public ConfigHelper(string configLocation)
        {
            ConfigLocation = configLocation;
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile(ConfigLocation);
            IConfigurationRoot configuration = configurationBuilder.Build();

            ConnectionString = configuration.GetSection("ConnectionStrings:Test:ConnectionString")?.Value ?? string.Empty;
            HostName = configuration.GetSection("ConnectionStrings:Test:Components:HostName")?.Value ?? string.Empty;
            DatabaseName = configuration.GetSection("ConnectionStrings:Test:Components:DatabaseName")?.Value ?? string.Empty;
            PortNumber = Convert.ToInt32(configuration.GetSection("ConnectionStrings:Test:Components:PortNumber")?.Value ?? string.Empty);
            UserName = configuration.GetSection("ConnectionStrings:Test:Components:UserName")?.Value ?? string.Empty;
            Password = configuration.GetSection("ConnectionStrings:Test:Components:Password")?.Value ?? string.Empty;

            TableName = configuration.GetSection("SchemaInfo:Tables:Users:Name")?.Value ?? string.Empty;
            ColumnNames = configuration.GetSection("SchemaInfo:Tables:Users:Columns")?.GetChildren().Select(c => c.Value ?? string.Empty).ToArray() ?? new string[0];
        }
    }
}
