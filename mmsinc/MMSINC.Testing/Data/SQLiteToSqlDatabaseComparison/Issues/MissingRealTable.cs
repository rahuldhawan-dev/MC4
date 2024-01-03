namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues
{
    public class MissingRealTable : IssueBase
    {
        #region Constructors

        public MissingRealTable(string realTableName) : base(realTableName) { }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return $"Could not find real table '{TableName}'";
        }

        #endregion
    }
}
