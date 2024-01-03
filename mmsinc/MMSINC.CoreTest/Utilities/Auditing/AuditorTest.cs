using System.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Auditing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINC.CoreTest.Utilities.Auditing
{
    /// <summary>
    /// Summary description for AuditingTest
    /// </summary>
    [TestClass]
    public class AuditorTest
    {
        #region Private Fields

        private MockRepository _mocks;
        private string _someConnectionString;
        private IDbConnection _mockedDbConnection;
        private IDbCommand _mockedDbCommand;
        private IDataParameterCollection _mockedCommandParameters;

        #endregion

        #region Initialization/Cleanup

        [TestInitialize]
        public void DataPageBaseTestInitialize()
        {
            _mocks = new MockRepository();
            _someConnectionString =
                "data source=0.0.0.0;Initial Catalog=meow;persist security info=true;user id=ronald;password=mcdonald";
        }

        [TestCleanup]
        public void DataPageBaseTestCleanup()
        {
            _mocks.VerifyAll();
        }

        private TestAuditor BuildTarget()
        {
            var target = new TestAuditor();

            _mocks.DynamicMock(out _mockedDbConnection)
                  .DynamicMock(out _mockedDbCommand)
                  .DynamicMock(out _mockedCommandParameters);

            target.SqlConnectionTest = _mockedDbConnection;
            target.SqlConnectionString = _someConnectionString;
            return target;
        }

        #endregion

        [TestMethod]
        public void TestAuditorInsertWorksWhenGivenValidParameters()
        {
            var target = BuildTarget();

            using (_mocks.Record())
            {
                SetupResult.For(_mockedDbConnection.CreateCommand()).Return(_mockedDbCommand);
                SetupResult.For(_mockedDbCommand.Parameters).Return(_mockedCommandParameters);
            }

            using (_mocks.Playback())
            {
                target.Insert(AuditCategory.DataView, "Me", "You're cool");
            }
        }

        [TestMethod]
        public void TestAuditorInsertThrowsWithNullOrEmptyCreatedByParameter()
        {
            var target = BuildTarget();

            using (_mocks.Record())
            {
                SetupResult.For(_mockedDbConnection.CreateCommand()).Return(_mockedDbCommand);
                SetupResult.For(_mockedDbCommand.Parameters).Return(_mockedCommandParameters);
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws(() => target.Insert(AuditCategory.DataView, null, "You're cool"));
                MyAssert.Throws(() => target.Insert(AuditCategory.DataView, string.Empty, "You're cool"));
            }
        }

        [TestMethod]
        public void TestAuditorInsertThrowsWithNullOrEmptyDetailsParameter()
        {
            var target = BuildTarget();

            using (_mocks.Record())
            {
                SetupResult.For(_mockedDbConnection.CreateCommand()).Return(_mockedDbCommand);
                SetupResult.For(_mockedDbCommand.Parameters).Return(_mockedCommandParameters);
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws(() => target.Insert(AuditCategory.DataView, "Me", null));
                MyAssert.Throws(() => target.Insert(AuditCategory.DataView, "Me", string.Empty));
            }
        }

        [TestMethod]
        public void TestAuditorCreatesSqlConnectionWithGivenConnectionString()
        {
            var target = BuildTarget();

            using (_mocks.Record()) { }

            using (_mocks.Playback())
            {
                var conn = target.CreateBaseConnectionTest(_someConnectionString);
                Assert.AreEqual(conn.ConnectionString, _someConnectionString);
            }
        }
    }

    internal class TestAuditor : Auditor
    {
        public IDbConnection SqlConnectionTest { get; set; }

        protected override IDbConnection CreateConnection()
        {
            SqlConnectionTest.ConnectionString = SqlConnectionString;
            return SqlConnectionTest;
        }

        public IDbConnection CreateBaseConnectionTest(string sqlConnectionString)
        {
            return base.CreateConnection();
        }
    }
}
