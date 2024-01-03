namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues
{
    public abstract class IssueBase
    {
        public string TableName { get; }

        public IssueBase(string realTableName)
        {
            TableName = realTableName;
        }
    }
}
