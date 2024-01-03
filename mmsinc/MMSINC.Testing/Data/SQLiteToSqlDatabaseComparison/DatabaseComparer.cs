using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues;

namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison
{
    public class DatabaseComparer : SchemaComparerBase
    {
        private readonly TableComparer _tableComparer;
        private readonly ForeignKeyComparer _foreignKeyComparer;

        #region Constructors

        public DatabaseComparer(SQLiteConnection fakeDbConn, SqlConnection realDbConn) : base(fakeDbConn, realDbConn)
        {
            _tableComparer = new TableComparer(_fakeDbConn, _realDbConn);
            _foreignKeyComparer = new ForeignKeyComparer(_fakeDbConn, _realDbConn);
        }

        #endregion

        #region Private Methods

        protected bool RealTableExists(string table)
        {
            return SQLQueryScalar<int>(
                       $"SELECT CASE WHEN EXISTS ((SELECT 1 FROM information_schema.tables WHERE table_name = '{table}')) then 1 else 0 end") ==
                   1;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<IssueBase> Compare()
        {
            var failures = new List<IssueBase>();
            var tables =
                SQLiteQueryList<string>(
                    "SELECT name FROM main.sqlite_master WHERE type = 'table' AND name <> 'sqlite_sequence'");

            foreach (var table in tables)
            {
                if (!RealTableExists(table))
                {
                    failures.Add(new MissingRealTable(table));
                    continue;
                }

                foreach (var failure in _tableComparer.Compare(table)
                                                      .Union(_foreignKeyComparer.Compare(table)))
                {
                    failures.Add(failure);
                }
            }

            return failures;
        }

        #endregion
    }
}
