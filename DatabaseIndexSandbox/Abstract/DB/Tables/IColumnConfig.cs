namespace DatabaseIndexSandbox.Abstract.DB.Tables
{
    /// <summary>
    /// Contains a column name with a list of possible values to be used for insertion.
    /// </summary>
    public interface IColumnConfig
    {
        string Name { get; }

        Type ColumnType { get; }

        object GetNextValue();
    }
}
