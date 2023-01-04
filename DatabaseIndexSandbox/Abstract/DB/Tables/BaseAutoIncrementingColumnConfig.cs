namespace DatabaseIndexSandbox.Abstract.DB.Tables
{
    public abstract class BaseAutoIncrementingColumnConfig : IColumnConfig
    {
        public BaseAutoIncrementingColumnConfig(string name, Type columnType, int startValue)
        {
            Name = name;
            ColumnType = columnType;
            StartValue = startValue;
            currentValue = startValue;
        }

        private int currentValue;

        public string Name { get; }

        public Type ColumnType { get; }

        public int StartValue { get; }

        public object GetNextValue()
        {
            return currentValue++;
        }
    }
}
