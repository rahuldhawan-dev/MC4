using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues;

namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison
{
    public class TableComparer : SchemaComparerBase
    {
        #region Private Members

        private readonly ColumnComparer _columnComparer;

        #endregion

        #region Constructors

        public TableComparer(SQLiteConnection fakeDbConn, SqlConnection realDbConn) : base(fakeDbConn, realDbConn)
        {
            _columnComparer = new ColumnComparer(_fakeDbConn, _realDbConn);
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<IssueBase> Compare(string table)
        {
            var failures = new List<IssueBase>();
            var columns = SQLiteQueryDataTable($"pragma table_info({table})");

            foreach (DataRow column in columns.Rows)
            {
                foreach (var failure in _columnComparer.Compare(table, column))
                {
                    failures.Add(failure);
                }
            }

            return failures;
        }

        #endregion
    }
}
