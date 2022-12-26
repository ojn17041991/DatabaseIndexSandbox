using System.Data.Common;

namespace DatabaseIndexSandbox.Abstract.DB.Tables
{
    public interface ITablePopulater
    {
        DbConnection Connection { get; }
        string TableName { get; }
        IList<IColumnConfig> Parameters { get; }
        int NumberOfOperations { get; }

        void AddParameters(IList<IColumnConfig> parameters);
        void Populate();
    }
}
