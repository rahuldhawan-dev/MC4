using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues;

namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison
{
    public class ColumnComparer : SchemaComparerBase
    {
        #region Constructors

        public ColumnComparer(SQLiteConnection fakeDbConn, SqlConnection realDbConn) : base(fakeDbConn, realDbConn) { }

        #endregion

        #region Private Methods

        private IssueBase CompareColumnDataTypes(DataRow realColumn, DataRow column, string columnName,
            string tableName)
        {
            string realType;
            string expectedType = realType = realColumn["DataType"].ToString().ToLower();
            var actualType = column["type"].ToString().ToLower();

            DataTypeMismatch Result(params string[] expectedTypes)
            {
                return new DataTypeMismatch(tableName, columnName, realType, actualType, expectedTypes);
            }

            switch (realType)
            {
                case "tinyint":
                case "smallint":
                case "int":
                    if (!new[] {"int", "integer"}.Contains(actualType))
                    {
                        return Result("int", "integer");
                    }

                    return null;
                case "bigint":
                    expectedType = "bigint";
                    break;
                case "varchar":
                case "nvarchar":
                case "ntext":
                case "char":
                case "nchar":
                    expectedType = "text";
                    break;
                case "real":
                case "float":
                    if (!new[] {"real", "double"}.Contains(actualType))
                    {
                        return Result("real", "double");
                    }

                    return null;
                case "decimal":
                case "money":
                    expectedType = "numeric";
                    break;
                case "bit":
                    expectedType = "bool";
                    break;
                case "date":
                case "smalldatetime":
                    expectedType = "datetime";
                    break;
            }

            return expectedType != actualType ? Result(expectedType) : null;
        }

        private DataTable GetRealColumn(string tableName, string columnName)
        {
            return SQLQueryDataTable($@"
SELECT 
    t.Name 'DataType',
    c.max_length 'MaxLength',
    c.Precision ,
    c.Scale ,
    c.is_nullable 'IsNullable',
    ISNULL(i.is_primary_key, 0) 'PrimaryKey'
FROM    
    sys.columns c
INNER JOIN 
    sys.types t ON c.user_type_id = t.user_type_id
LEFT OUTER JOIN 
    sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
LEFT OUTER JOIN 
    sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
WHERE
    c.object_id = OBJECT_ID('{tableName}')
AND
    c.Name = '{columnName}'");
        }

        private IssueBase CompareColumnNullability(DataRow realColumn, DataRow column, string columnName,
            string tableName)
        {
            var realColumnNullable = (bool)realColumn["IsNullable"];
            var fakeColumnNullable = ((long)column["notnull"]) == 0;

            return realColumnNullable != fakeColumnNullable
                ? new NullabilityMismatch(tableName, columnName, realColumnNullable, fakeColumnNullable)
                : null;
        }

        private IssueBase ComparePk(DataRow realColumn, DataRow column, string columnName, string tableName)
        {
            var realColumnPk = (bool)realColumn["PrimaryKey"];
            var fakeColumnPk = ((long)column["pk"]) == 1;

            return realColumnPk != fakeColumnPk
                ? new PrimaryKeyMismatch(tableName, columnName, realColumnPk, fakeColumnPk)
                : null;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<IssueBase> Compare(string tableName, DataRow column)
        {
            var columnName = column["name"].ToString();
            var realColumnInTable = GetRealColumn(tableName, columnName);

            if (realColumnInTable.Rows.Count < 1)
            {
                return new[] {new MissingRealColumn(tableName, columnName)};
            }

            var realColumn = realColumnInTable.Rows[0];
            var failures = new List<IssueBase>();

            MaybeAddResult(failures, CompareColumnDataTypes(realColumn, column, columnName, tableName));
            MaybeAddResult(failures, CompareColumnNullability(realColumn, column, columnName, tableName));
            MaybeAddResult(failures, ComparePk(realColumn, column, columnName, tableName));
            //MaybeAddResult(failures, CompareUniqueness(realColumn, column, columnName, tableName));

            return failures;
        }

        #endregion

        //private IssueBase CompareUniqueness(DataRow realColumn, DataRow column, string columnName, string tableName)
        //{
        //    var sqliteDt = SQLiteQueryDataTable($"pragma index_list({tableName});");

        //    foreach (DataRow row in sqliteDt.Rows)
        //    {
        //        if (!row.ItemArray[1].ToString().Contains("autoindex"))
        //        {
        //            return null;
        //        }
        //    }

        //    return null;
        //}
    }
}
