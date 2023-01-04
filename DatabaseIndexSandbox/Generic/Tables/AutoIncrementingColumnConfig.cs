using DatabaseIndexSandbox.Abstract.DB.Tables;

namespace DatabaseIndexSandbox.Generic.Tables
{
    public class AutoIncrementingColumnConfig : BaseAutoIncrementingColumnConfig
    {
        public AutoIncrementingColumnConfig(string name, Type columnType, int startValue) : base(name, columnType, startValue)
        {
            // Stub.
        }
    }
}
