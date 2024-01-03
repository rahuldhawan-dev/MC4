namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues
{
    public abstract class BooleanAttributeMismatchBase : ColumnIssueBase
    {
        #region Properties

        public bool FakeColumnStatus { get; }

        public bool RealColumnStatus { get; }

        #endregion

        #region Abstract Properties

        public abstract string Title { get; }
        public abstract string Description { get; }

        #endregion

        #region Constructors

        public BooleanAttributeMismatchBase(string tableName, string columnName, bool realColumnStatus,
            bool fakeColumnStatus) : base(tableName, columnName)
        {
            RealColumnStatus = realColumnStatus;
            FakeColumnStatus = fakeColumnStatus;
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return
                $"{Title} mismatch on column '{ColumnName}' in table '{TableName}': real column is{(RealColumnStatus ? "" : " not")} {Description}, fake column is{(FakeColumnStatus ? "" : " not")} {Description}";
        }

        #endregion
    }
}
