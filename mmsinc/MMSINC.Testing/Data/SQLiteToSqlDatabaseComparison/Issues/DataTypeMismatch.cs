using System;

namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues
{
    public class DataTypeMismatch : ColumnIssueBase
    {
        #region Properties

        public string[] ExpectedTypes { get; }

        public string ActualType { get; }

        public string RealColumnType { get; }

        #endregion

        #region Constructors

        public DataTypeMismatch(string tableName, string columName, string realColumnType, string actualType,
            params string[] expectedTypes) : base(tableName, columName)
        {
            RealColumnType = realColumnType;
            ActualType = actualType;
            ExpectedTypes = expectedTypes;
        }

        #endregion

        #region Private Methods

        private string ListExpectedTypes()
        {
            switch (ExpectedTypes.Length)
            {
                case 1:
                    return ExpectedTypes[0];
                case 0:
                    throw new InvalidOperationException("Need to expect at least one type");
                default:
                    return $"'{string.Join("', '", ExpectedTypes)}'";
            }
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return
                $"Data type mismatch on '{RealColumnType}' column '{ColumnName}', table '{TableName}': actual '{ActualType}', expected {ListExpectedTypes()}";
        }

        #endregion
    }
}
