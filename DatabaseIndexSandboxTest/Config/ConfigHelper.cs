using DatabaseIndexSandbox.Abstract.DB.Tables;
using DatabaseIndexSandbox.Generic.Tables;
using DatabaseIndexSandboxTest.Config.Templates;
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

        public IDictionary<string, IList<IColumnConfig>> Tables { get; } = new Dictionary<string, IList<IColumnConfig>>();
        public string UsersTableName { get; } = string.Empty;
        public string CounterTableName { get; } = string.Empty;

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

            // Read each table defined in the config.

            foreach (IConfigurationSection tableSection in configuration.GetSection("SchemaInfo:Tables").GetChildren())
            {
                string tableName = configuration.GetSection($"{tableSection.Path}:Name")?.Value ?? string.Empty;
                IList<ColumnConfigTemplate> columnTemplates = new List<ColumnConfigTemplate>();

                // Read the columns from the config, store them in a template, convert to IColumnConfig, and store.
                foreach (IConfigurationSection columnSection in configuration.GetSection($"SchemaInfo:Tables:{tableSection.Key}:Columns").GetChildren())
                {
                    ColumnConfigTemplate column = new ColumnConfigTemplate();
                    configuration.GetSection(columnSection.Path).Bind(column);
                    columnTemplates.Add(column);
                }

                // We need to get all templated first, then order them, then convert to IColumnConfig, and append to the table.
                IList<IColumnConfig> columns = columnTemplates.OrderBy(c => c.Id).Select(c => c.ToColumnConfig()).ToList();
                Tables.Add(tableName, columns);
            }

            // Get the table names.
            UsersTableName = configuration.GetSection($"SchemaInfo:Tables:Users:Name")?.Value ?? string.Empty;
            CounterTableName = configuration.GetSection($"SchemaInfo:Tables:Counter:Name")?.Value ?? string.Empty;
        }
    }
}
