namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues
{
    public class MissingRealColumn : ColumnIssueBase
    {
        #region Constructors

        public MissingRealColumn(string tableName, string columnName) : base(tableName, columnName) { }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return $"Could not find column '{ColumnName}' in real table '{TableName}'";
        }

        #endregion
    }
}
