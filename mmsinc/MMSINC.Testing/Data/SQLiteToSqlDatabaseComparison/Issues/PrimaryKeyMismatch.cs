namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues
{
    public class PrimaryKeyMismatch : BooleanAttributeMismatchBase
    {
        #region Properties

        public override string Title => "Primary Key";
        public override string Description => "PK";

        #endregion

        #region Constructors

        public PrimaryKeyMismatch(string tableName, string columnName, bool realColumnStatus, bool fakeColumnStatus) :
            base(tableName, columnName, realColumnStatus, fakeColumnStatus) { }

        #endregion
    }
}
