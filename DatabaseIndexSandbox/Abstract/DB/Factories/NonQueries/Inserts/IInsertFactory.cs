using DatabaseIndexSandbox.Abstract.DB.Queries;

namespace DatabaseIndexSandbox.Abstract.DB.Factories.NonQueries.Inserts
{
    internal interface IInsertFactory : INonQueryFactory
    {
        IList<INonQuery> GenerateBatch();
    }
}
