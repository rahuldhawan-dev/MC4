using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;
using MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues;

namespace MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison
{
    public class ForeignKeyComparer : SchemaComparerBase
    {
        #region Constructors

        public ForeignKeyComparer(SQLiteConnection fakeDbConn, SqlConnection realDbConn) :
            base(fakeDbConn, realDbConn) { }

        #endregion

        #region Private Methods

        private IEnumerable<(string Column, string ForeignTable)> GatherRealFKConstraints(string tableName)
        {
            var fkeys = SQLQueryDataTable($"exec sp_fkeys @fktable_name = '{tableName}'");

            foreach (DataRow row in fkeys.Rows)
            {
                yield return (row["FKCOLUMN_NAME"].ToString(), row["PKTABLE_NAME"].ToString());
            }
        }

        private IEnumerable<(string Column, string ForeignTable)> GatherFakeFKConstraints(string tableName)
        {
            var sql = SQLiteQueryScalar<string>($@"
SELECT sql
  FROM sqlite_master
 WHERE tbl_name = '{tableName}'
   AND type != 'meta'
   AND sql NOTNULL
   AND name NOT LIKE 'sqlite_%'
   AND sql LIKE '%REFERENCES%'
 ORDER BY substr(type, 2, 1), name");

            var rgx = new Regex(@"constraint [^ ]+ foreign key \(([^)]+)\) references ([^), ]+),?");
            Match match;

            while (sql != null && (match = rgx.Match(sql)).Success)
            {
                sql = sql.Replace(match.Groups[0].Value, "");
                yield return (match.Groups[1].Value, match.Groups[2].Value);
            }
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<IssueBase> Compare(string tableName)
        {
            var fakeFKConstraints = GatherFakeFKConstraints(tableName).ToList();
            var realFKConstraints = GatherRealFKConstraints(tableName).ToList();

            if (fakeFKConstraints.Count == realFKConstraints.Count && realFKConstraints.Count == 0)
            {
                yield break;
            }

            foreach (var constraint in fakeFKConstraints)
            {
                var realConstraint = realFKConstraints.Where(c => c.Column.ToLower() == constraint.Column.ToLower());

                if (!realConstraint.Any())
                {
                    yield return new ForeignKeyMismatch(
                        $"Real table '{tableName}' has no foreign key constraint on column '{constraint.Column}', but fake table does");
                }
                else if (realConstraint.Count() > 1)
                {
                    yield return new ForeignKeyMismatch(
                        $"Real table '{tableName}' has more than one foreign key constraint on column '{constraint.Column}'");
                }
                else if (realConstraint.Single().ForeignTable.ToLower() != constraint.ForeignTable.ToLower())
                {
                    yield return new ForeignKeyMismatch(
                        $"Real table '{tableName}' has foreign key for column '{constraint.Column}' referencing table '{realConstraint.Single().ForeignTable}', but fake column references '{constraint.ForeignTable}'");
                }
            }

            foreach (var constraint in realFKConstraints)
            {
                var fakeConstraint = fakeFKConstraints.Where(c => c.Column.ToLower() == constraint.Column.ToLower());

                if (!fakeConstraint.Any())
                {
                    yield return new ForeignKeyMismatch(
                        $"Fake table '{tableName}' has no foreign key constraint on column '{constraint.Column}', but real table does");
                }
                else if (fakeConstraint.Count() > 1)
                {
                    yield return new ForeignKeyMismatch(
                        $"Fake table '{tableName}' has more than one foreign key constraint on column '{constraint.Column}'");
                }
                else if (fakeConstraint.Single().ForeignTable.ToLower() != constraint.ForeignTable.ToLower())
                {
                    yield return new ForeignKeyMismatch(
                        $"Fake table '{tableName}' has foreign key for column '{constraint.Column}' referencing table '{fakeConstraint.Single().ForeignTable}', but real column references '{constraint.ForeignTable}'");
                }
            }
        }

        #endregion
    }
}
