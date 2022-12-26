namespace DatabaseIndexSandbox.Abstract.DB.Utilities
{
    public interface IBatchable
    {
        string TableName { get; set; }
        IList<string> Parameters { get; set; }
        IList<IList<object>> Arguments { get; set; }

        void CreateBatch(string tableName, IList<string> parameters);
        void AddArgumentsToBatch(IList<object> arguments);
    }
}
