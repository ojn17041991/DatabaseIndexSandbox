using DatabaseIndexSandbox.Abstract.DB.Tables;

namespace DatabaseIndexSandbox.Generic.Tables
{
    /// <summary>
    /// Basic configuration of a non-unique column.
    /// </summary>
    public class NonUniqueColumnConfig : BaseNonUniqueColumnConfig
    {
        public NonUniqueColumnConfig(string name, Type columnType, IList<object> values) : base(name, columnType, values)
        {
            // Stub.
        }
    }
}
