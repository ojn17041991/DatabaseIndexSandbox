using DatabaseIndexSandbox.Abstract.DB.Tables;
using DatabaseIndexSandbox.Generic.Tables;
using DatabaseIndexSandboxTest.Config.Enums;

namespace DatabaseIndexSandboxTest.Config.Templates
{
    internal class ColumnConfigTemplate
    {
        public int Id { get; set; } = default(int);
        public string Name { get; set; } = string.Empty;
        public string ColumnType { get; set; } = string.Empty;
        public object[] Values { get; set; } = new object[0];
        public ColumnAttribute Attribute { get; set; }

        public IColumnConfig ToColumnConfig()
        {
            if (Attribute.HasFlag(ColumnAttribute.AutoIncrementing))
            {
                return new AutoIncrementingColumnConfig(Name, Type.GetType(ColumnType) ?? typeof(object), default(int));
            }
            else
            {
                return new NonUniqueColumnConfig(Name, Type.GetType(ColumnType) ?? typeof(object), Values);
            }
        }
    }
}
