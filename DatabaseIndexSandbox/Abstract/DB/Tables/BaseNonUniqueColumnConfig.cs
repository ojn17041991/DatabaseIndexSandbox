namespace DatabaseIndexSandbox.Abstract.DB.Tables
{
    /// <summary>
    /// Specifically for columns that don't abide by a PRIMARY KEY / UNIQUE constraint.
    /// </summary>
    public abstract class BaseNonUniqueColumnConfig : IColumnConfig
    {
        public BaseNonUniqueColumnConfig(string name, Type columnType, IList<object> values)
        {
            random = new Random();
            Name = name;
            ColumnType = columnType;
            Values = values;
        }

        private Random random;

        public string Name { get; }

        public Type ColumnType { get; }

        public IList<object> Values { get; }

        public object GetNextValue()
        {
            return Values[random.Next(Values.Count)];
        }
    }
}
