namespace DatabaseIndexSandboxTest.Config.Enums
{
    [Flags]
    public enum ColumnAttribute
    {
        Column = 0,
        PrimaryKey = 1,
        ForeignKey = 2,
        AutoIncrementing = 4
    }
}
