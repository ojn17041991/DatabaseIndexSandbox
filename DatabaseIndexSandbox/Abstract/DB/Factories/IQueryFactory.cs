using DatabaseIndexSandbox.Abstract.DB.Queries;

namespace DatabaseIndexSandbox.Abstract.DB.Factories
{
    internal interface IQueryFactory : IBaseQueryFactory
    {
        IQuery Generate(string tableName, IDictionary<string, object> parameters);
    }
}
