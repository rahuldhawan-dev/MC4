using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison;
using MMSINC.Testing.Data.SQLiteToSqlDatabaseComparison.Issues;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class AllMappingsTest : InMemoryDatabaseTest<ABCIndicator>
    {
        private SqlConnection _realDbConn;

        // set to the type of a child of IssueBase to only see issues of that type
        public Type Only => null;

        // add types of children of IssueBase to ignore issues of those types
        public Type[] Ignore => new Type[] {typeof(NullabilityMismatch)};

        // TODO: automatically generate this
        public Dictionary<Type, string> IssueTypes => new Dictionary<Type, string> {
            {typeof(MissingRealTable), "missing tables"},
            {typeof(MissingRealColumn), "missing columns"},
            {typeof(PrimaryKeyMismatch), "primary key mismatches"},
            {typeof(DataTypeMismatch), "data type mismatches"},
            {typeof(NullabilityMismatch), "nullability mismatches"},
            //            {typeof(UniquenessMismatch), "uniqueness mismatches"},
            {typeof(ForeignKeyMismatch), "foreign key mismatches"}
        };

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _realDbConn =
                new SqlConnection("Data Source=localhost;Initial Catalog=MapCallDEV;Integrated Security=sspi;");
            _realDbConn.Open();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _realDbConn.Close();
            _realDbConn.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestGeneratedTestSchemaMatchesRealDBSchema()
        {
            var failures = new DatabaseComparer((SQLiteConnection)_container.GetInstance<IDbConnection>(), _realDbConn)
               .Compare();

            if (!failures.Any())
            {
                return;
            }

            var typesToCheck =
                (Only != null ? IssueTypes.Where(t => t.Key == Only) : IssueTypes.Where(t => !Ignore.Contains(t.Key)))
               .ToDictionary(t => t.Key, t => t.Value);
            failures = failures.Where(f => typesToCheck.ContainsKey(f.GetType()));
            var sb = new StringBuilder($"Found {failures.Count()} issues:{Environment.NewLine}");

            foreach (var type in typesToCheck.Take(typesToCheck.Count - 1))
            {
                sb.Append($"{failures.Count(f => f.GetType() == type.Key)} {type.Value}, ");
            }

            var last = typesToCheck.Last();
            sb.AppendLine($"{failures.Count(f => f.GetType() == last.Key)} {last.Value}");
            sb.AppendLine("======================================================================================");

            foreach (var failure in failures)
            {
                sb.AppendLine("  " + failure);
            }

            Assert.Fail(sb.ToString());
        }

        private Dictionary<Type, string> FilterTypesToCheck()
        {
            var typesToCheck = Only != null
                ? IssueTypes.Where(t => t.Key == Only)
                : IssueTypes.Where(t => !Ignore.Contains(t.Key));
            var ret = new Dictionary<Type, string>();

            foreach (var t in typesToCheck)
            {
                ret[t.Key] = t.Value;
            }

            return ret;
        }
    }
}
