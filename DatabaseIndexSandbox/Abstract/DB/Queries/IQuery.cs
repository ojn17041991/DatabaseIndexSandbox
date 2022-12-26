namespace DatabaseIndexSandbox.Abstract.DB.Queries
{
    public interface IQuery : IBaseQuery
    {
        object[] Execute(); // OJN: This type needs more consideration.
    }
}
