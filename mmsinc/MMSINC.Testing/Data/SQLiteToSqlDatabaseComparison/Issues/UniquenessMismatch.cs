namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues
{
    public class UniquenessMismatch : BooleanAttributeMismatchBase
    {
        #region Properties

        public override string Title => "Uniqueness";
        public override string Description => "unique";

        #endregion

        #region Constructors

        public UniquenessMismatch(string tableName, string columnName, bool realColumnStatus, bool fakeColumnStatus) :
            base(tableName, columnName, realColumnStatus, fakeColumnStatus) { }

        #endregion
    }

    public class ForeignKeyMismatch : ColumnIssueBase
    {
        public string Message { get; }

        public ForeignKeyMismatch(string message) : base(null, null)
        {
            Message = message;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
