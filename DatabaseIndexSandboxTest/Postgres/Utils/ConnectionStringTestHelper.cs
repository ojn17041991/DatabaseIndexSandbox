using Xunit;

namespace DatabaseIndexSandboxTest.Postgres.Utils
{
    public class ConnectionStringTestHelper
    {
        public bool ConnectionStringsAreEqual(string primaryConnectionString, string secondaryConnectionString)
        {
            // Connection string properties are re-ordered, and passwords are removed, when a connection opens.
            // We need to compare the settings of the connection strings individually to test equality.

            // Split the connection strings up into their individual components.
            string[] primarySettings = primaryConnectionString.Split(';');
            string[] secondarySettings = secondaryConnectionString.Split(';');

            // Now, convert the 2 component arrays into dictionaries.
            IDictionary<string, string> primaryLookup = new Dictionary<string, string>();
            foreach (string setting in primarySettings)
            {
                string[] components = setting.Split("=");
                primaryLookup.Add(components[0], components[1]);
            }
            IDictionary<string, string> secondaryLookup = new Dictionary<string, string>();
            foreach (string setting in secondarySettings)
            {
                string[] components = setting.Split("=");
                secondaryLookup.Add(components[0], components[1]);
            }

            // Check if all primary components exist in the secondary connection string, and that the values are the same.
            return primaryLookup.All(p => secondaryLookup.ContainsKey(p.Key) && secondaryLookup[p.Key] == p.Value);
        }
    }
}
