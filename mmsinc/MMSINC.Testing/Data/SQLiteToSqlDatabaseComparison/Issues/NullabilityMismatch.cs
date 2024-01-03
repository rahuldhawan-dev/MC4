namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues
{
    public class NullabilityMismatch : BooleanAttributeMismatchBase
    {
        #region Properties

        public override string Title => "Nullability";
        public override string Description => "nullable";

        #endregion

        #region Constructors

        public NullabilityMismatch(string tableName, string columnName, bool realColumnStatus, bool fakeColumnStatus) :
            base(tableName, columnName, realColumnStatus, fakeColumnStatus) { }

        #endregion
    }
}
