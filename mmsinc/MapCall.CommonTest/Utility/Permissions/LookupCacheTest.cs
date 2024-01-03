using System;
using System.Collections.Generic;
using System.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.CommonTest.Utility.Permissions
{
    [TestClass]
    public class LookupCacheTest
    {
        #region Fields

        public const string CONNECTION_STRING =
            "data source=Jerk;Initial Catalog=Jerk;persist security info=true;user id=Jerk;password=Jerk";

        private Mock<IDbConnection> _conn;

        #endregion

        #region Initializinating

        private TestLookupCache InitializeBuilder()
        {
            var tlc = new TestLookupCache();
            _conn = new Mock<IDbConnection>();
            tlc.MockConnection = _conn.Object;
            return tlc;
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorThrowsForNullOrWhiteSpaceConnectionStringParameter()
        {
            MyAssert.Throws<ArgumentNullException>(() => new LookupCache(null));
            MyAssert.Throws<ArgumentNullException>(() => new LookupCache(string.Empty));
            MyAssert.Throws<ArgumentNullException>(() => new LookupCache("    "));
        }

        [TestMethod]
        public void TestConstructorSetsConnectionString()
        {
            var expected = CONNECTION_STRING;
            var target = new LookupCache(expected);
            Assert.AreEqual(expected, target.ConnectionString);
        }

        #endregion

        #region GetConnection

        [TestMethod]
        public void TestGetConnectionReturnsConnectionWithConnectionStringSet()
        {
            var target = new TestLookupCache();

            using (var result = target.TestGetConnection())
            {
                Assert.AreEqual(CONNECTION_STRING, result.ConnectionString);
            }
        }

        #endregion

        [TestMethod]
        public void TestInitializeCallsAllReadAndSetMethods()
        {
            var target = InitializeBuilder();
            target.UseTestReadAndSetOverrides = true;

            var cmd = new Mock<IDbCommand>();
            _conn.Setup(x => x.CreateCommand()).Returns(cmd.Object);
            var reader = new Mock<IDataReader>();
            cmd.Setup(x => x.ExecuteReader()).Returns(reader.Object);

            // Do something to initialize
            var stuff = target.OperatingCenters;

            reader.Verify(x => x.NextResult());
            _conn.Verify(x => x.Open());
            Assert.IsTrue(target.ReadAndSetActionsCalled);
            Assert.IsTrue(target.ReadAndSetApplicationsCalled);
            Assert.IsTrue(target.ReadAndSetModulesCalled);
            Assert.IsTrue(target.ReadAndSetOperatingCentersCalled);
        }

        [TestMethod]
        public void TestReadAndSetActionsSetsActionsProperty()
        {
            var reader = new Mock<IDataReader>();
            var canRead = true;
            reader.Setup(x => x.Read())
                  .Returns(() => canRead)
                  .Callback(() => canRead = false);
            reader.Setup(x => x.GetOrdinal("ActionID")).Returns(0);
            reader.Setup(x => x.GetOrdinal("Name")).Returns(1);
            reader.Setup(x => x.GetInt32(0)).Returns(10);
            reader.Setup(x => x.GetString(1)).Returns("ActionName");

            var target = InitializeBuilder();
            target.TestReadAndSetActions(reader.Object);

            target.UseTestInitializeMethod = true;
            Assert.IsTrue(target.Actions.ContainsKey(10));

            var byId = target.Actions[10];
            Assert.AreEqual(10, byId.ActionId);
            Assert.AreEqual("ActionName", byId.Name);
        }

        [TestMethod]
        public void TestReadAndSetApplicationsSetsApplicationsProperty()
        {
            var reader = new Mock<IDataReader>();
            var canRead = true;
            reader.Setup(x => x.Read())
                  .Returns(() => canRead)
                  .Callback(() => canRead = false);
            reader.Setup(x => x.GetOrdinal("ApplicationID")).Returns(0);
            reader.Setup(x => x.GetOrdinal("Name")).Returns(1);
            reader.Setup(x => x.GetInt32(0)).Returns(10);
            reader.Setup(x => x.GetString(1)).Returns("ApplicationName");

            var target = InitializeBuilder();
            target.TestReadAndSetApplications(reader.Object);

            target.UseTestInitializeMethod = true;
            Assert.IsTrue(target.Applications.ContainsKey(10));
            Assert.IsTrue(target.ApplicationsByName.ContainsKey("ApplicationName"));

            var cheating = (Dictionary<string, IRoleApplication>)target.ApplicationsByName;
            Assert.AreSame(StringComparer.InvariantCultureIgnoreCase, cheating.Comparer);

            var byId = target.Applications[10];
            var byName = target.ApplicationsByName["ApplicationName"];
            Assert.AreSame(byId, byName);
            Assert.IsNotNull(byId);

            Assert.AreEqual(10, byId.ApplicationId);
            Assert.AreEqual("ApplicationName", byId.Name);
        }

        [TestMethod]
        public void TestReadAndSetModulesSetsModulesProperty()
        {
            var reader = new Mock<IDataReader>();
            var canRead = true;
            reader.Setup(x => x.Read())
                  .Returns(() => canRead)
                  .Callback(() => canRead = false);
            reader.Setup(x => x.GetOrdinal("ModuleID")).Returns(0);
            reader.Setup(x => x.GetOrdinal("Name")).Returns(1);
            reader.Setup(x => x.GetInt32(0)).Returns(10);
            reader.Setup(x => x.GetString(1)).Returns("ModuleName");

            var target = InitializeBuilder();
            target.TestReadAndSetModules(reader.Object);

            target.UseTestInitializeMethod = true;
            Assert.IsTrue(target.Modules.ContainsKey(10));

            var byId = target.Modules[10];

            Assert.AreEqual(10, byId.ModuleId);
            Assert.AreEqual("ModuleName", byId.Name);
        }

        [TestMethod]
        public void TestReadAndSetOperatingCentersSetsOperatingCentersProperty()
        {
            var reader = new Mock<IDataReader>();
            var canRead = true;
            reader.Setup(x => x.Read())
                  .Returns(() => canRead)
                  .Callback(() => canRead = false);
            reader.Setup(x => x.GetOrdinal("OperatingCenterId")).Returns(0);
            reader.Setup(x => x.GetOrdinal("OperatingCenterCode")).Returns(1);
            reader.Setup(x => x.GetInt32(0)).Returns(10);
            reader.Setup(x => x.GetString(1)).Returns("OPC");

            var target = InitializeBuilder();
            target.TestReadAndSetOperatingCenters(reader.Object);

            target.UseTestInitializeMethod = true;
            Assert.IsTrue(target.OperatingCenters.ContainsKey(10));
            Assert.IsTrue(target.OperatingCentersByName.ContainsKey("OPC"));

            var cheating = (Dictionary<string, IOperatingCenter>)target.OperatingCentersByName;
            Assert.AreSame(StringComparer.InvariantCultureIgnoreCase, cheating.Comparer);

            var byId = target.OperatingCenters[10];
            var byName = target.OperatingCentersByName["OPC"];
            Assert.AreSame(byId, byName);
            Assert.IsNotNull(byId);

            Assert.AreEqual(10, byId.OperatingCenterId);
            Assert.AreEqual("OPC", byId.OperatingCenterCode);
        }

        #endregion
    }

    public class TestLookupCache : LookupCache
    {
        #region Properties

        public bool UseTestInitializeMethod { get; set; }
        public bool UseTestReadAndSetOverrides { get; set; }
        public bool ReadAndSetActionsCalled { get; set; }
        public bool ReadAndSetApplicationsCalled { get; set; }
        public bool ReadAndSetModulesCalled { get; set; }
        public bool ReadAndSetOperatingCentersCalled { get; set; }

        public IDbConnection MockConnection { get; set; }

        public IDbConnection TestGetConnection()
        {
            return GetConnection();
        }

        #endregion

        #region Constructors

        public TestLookupCache()
            : base(LookupCacheTest.CONNECTION_STRING) { }

        #endregion

        #region private Methods

        protected override IDbConnection GetConnection()
        {
            if (MockConnection != null)
            {
                return MockConnection;
            }

            return base.GetConnection();
        }

        protected override void Initialize()
        {
            if (UseTestInitializeMethod)
            {
                return;
            }

            base.Initialize();
        }

        protected override void ReadAndSetActions(IDataReader reader)
        {
            if (UseTestReadAndSetOverrides)
            {
                ReadAndSetActionsCalled = true;
            }
            else
            {
                base.ReadAndSetActions(reader);
            }
        }

        protected override void ReadAndSetApplications(IDataReader reader)
        {
            if (UseTestReadAndSetOverrides)
            {
                ReadAndSetApplicationsCalled = true;
            }
            else
            {
                base.ReadAndSetApplications(reader);
            }
        }

        protected override void ReadAndSetModules(IDataReader reader)
        {
            if (UseTestReadAndSetOverrides)
            {
                ReadAndSetModulesCalled = true;
            }
            else
            {
                base.ReadAndSetModules(reader);
            }
        }

        protected override void ReadAndSetOperatingCenters(IDataReader reader)
        {
            if (UseTestReadAndSetOverrides)
            {
                ReadAndSetOperatingCentersCalled = true;
            }
            else
            {
                base.ReadAndSetOperatingCenters(reader);
            }
        }

        #endregion

        #region Public Methods

        public void TestReadAndSetActions(IDataReader reader)
        {
            ReadAndSetActions(reader);
        }

        public void TestReadAndSetApplications(IDataReader reader)
        {
            ReadAndSetApplications(reader);
        }

        public void TestReadAndSetModules(IDataReader reader)
        {
            ReadAndSetModules(reader);
        }

        public void TestReadAndSetOperatingCenters(IDataReader reader)
        {
            ReadAndSetOperatingCenters(reader);
        }

        #endregion
    }
}
