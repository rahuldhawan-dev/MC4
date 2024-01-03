namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues
{
    public class ColumnIssueBase : IssueBase
    {
        #region Properties

        public string ColumnName { get; }

        #endregion

        #region Constructors

        public ColumnIssueBase(string tableName, string columnName) : base(tableName)
        {
            ColumnName = columnName;
        }

        #endregion
    }
}
