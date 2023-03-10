using DatabaseIndexSandbox.Abstract.DB.Queries;

namespace DatabaseIndexSandbox.Abstract.DB.Factories.NonQueries
{
    internal interface INonQueryFactory : IBaseQueryFactory
    {
        INonQuery Generate(string tableName, IDictionary<string, object> parameters);
    }
}
