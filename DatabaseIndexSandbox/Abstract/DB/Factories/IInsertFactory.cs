using DatabaseIndexSandbox.Abstract.DB.Queries;

namespace DatabaseIndexSandbox.Abstract.DB.Factories
{
    internal interface IInsertFactory : INonQueryFactory
    {
        IList<INonQuery> GenerateBatch();
    }
}
