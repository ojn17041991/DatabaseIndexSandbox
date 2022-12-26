using System.Data.Common;

namespace DatabaseIndexSandbox.Abstract.DB.Factories
{
    internal interface IBaseQueryFactory
    {
        DbConnection Connection { get; }
    }
}
