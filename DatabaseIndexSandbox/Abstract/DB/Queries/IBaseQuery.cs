using System.Data.Common;

namespace DatabaseIndexSandbox.Abstract.DB.Queries
{
    public interface IBaseQuery
    {
        DbConnection Connection { get; set; }
        string CommandText { get; set; }
        IDictionary<string, object> Parameters { get; set; }
    }
}
