using DatabaseIndexSandbox.Abstract.DB.Tables;

namespace DatabaseIndexSandbox.Generic.Tables
{
    /// <summary>
    /// Basic configuration of a non-unique column.
    /// </summary>
    public class NonUniqueColumnConfig : INonUniqueColumnConfig
    {
        public NonUniqueColumnConfig(string name, IList<object> values)
        {
            Name = name;
            Values = values;
        }

        public string Name { get; }

        public IList<object> Values { get; }
    }
}
