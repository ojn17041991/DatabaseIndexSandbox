namespace DatabaseIndexSandbox.Abstract.DB.Tables
{
    /// <summary>
    /// Specifically for columns that don't abide by a PRIMARY KEY / UNIQUE constraint.
    /// </summary>
    public interface INonUniqueColumnConfig : IColumnConfig
    {
    }
}
