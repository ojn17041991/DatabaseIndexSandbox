using DatabaseIndexSandbox.Abstract.API.Content.Parser;
using DatabaseIndexSandbox.Abstract.DB.Helper;
using DatabaseIndexSandbox.Abstract.DB.Tables;
using DatabaseIndexSandbox.API;
using DatabaseIndexSandbox.API.Content.Parser;
using DatabaseIndexSandbox.API.Content.Response;
using DatabaseIndexSandbox.Generic.Tables;
using DatabaseIndexSandbox.Postgres.Helper;
using DatabaseIndexSandbox.Postgres.Tables;
using System.Data.Common;


// Get the first and last names from the API.
FirstLastNamesGenerator firstLastNamesGenerator = new FirstLastNamesGenerator();
Task<string> firstNames = firstLastNamesGenerator.GetFirstNamesAsync();
Task<string> lastNames = firstLastNamesGenerator.GetLastNamesAsync();
await Task.WhenAll(firstNames, lastNames);


// Deserialise the JSON object.
IContentParser<FunGeneratorsResponse> nameParser = new JsonContentParser<FunGeneratorsResponse>();
FunGeneratorsResponse firstNamesResponse = nameParser.Parse(firstNames.Result) ?? new FunGeneratorsResponse();
FunGeneratorsResponse lastNamesResponse = nameParser.Parse(lastNames.Result) ?? new FunGeneratorsResponse();


// Connect to the Postgres database.
string hostName = "localhost";
string databaseName = "index_sandbox";
int portNumber = 5432;
string userName = "postgres";
string password = "Passw0rd#";
IDatabaseObjectHelper databaseObjectHelper = new PostgresObjectHelper(hostName, databaseName, portNumber, userName, password);
using (DbConnection connection = databaseObjectHelper.Connection)
{
    connection.Open();

    // Build up a bulk INSERT statement and run it against the database in batches.
    ITablePopulator populater = new PostgresTablePopulater(connection, "person", 100000000);
    populater.AddParameters(
        new List<IColumnConfig>() {
            new NonUniqueColumnConfig("FirstName", typeof(string), firstNamesResponse.contents.names.Select(n => (object)n).ToList()),
            new NonUniqueColumnConfig("LastName", typeof(string), lastNamesResponse.contents.names.Select(n => (object)n).ToList())
        }
    );
    populater.Populate();

    connection.Close();
}