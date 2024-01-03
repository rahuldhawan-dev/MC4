using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class VehicleRepositoryTest : InMemoryDatabaseTest<Vehicle, VehicleRepository>
    {
        #region Tests

        [TestMethod]
        public void TestGetByOperatingCenterIdReturnsFacilitiesWithOperatingCenterId()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var valid = GetFactory<VehicleFactory>().Create(new {OperatingCenter = opc});
            var invalid = GetFactory<VehicleFactory>()
               .Create(new {OperatingCenter = typeof(UniqueOperatingCenterFactory)});

            var result = Repository.GetByOperatingCenterId(opc.Id).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(valid));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestGetByOperatingCenterDoesntCryOverNullOperatingCenters()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var valid = GetFactory<VehicleFactory>().Create(new {OperatingCenter = opc});
            var invalid = GetFactory<VehicleFactory>().Create();
            Assert.IsNull(invalid.OperatingCenter, "Sanity check.");

            Session.Evict(valid);
            Session.Evict(invalid);
            var result = Repository.GetByOperatingCenterId(opc.Id).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(x => x.Id == valid.Id));
            Assert.IsFalse(result.Any(x => x.Id == invalid.Id));
        }

        [TestMethod]
        public void TestReportThing()
        {
            var vehicle = GetFactory<VehicleFactory>().Create();
            var result = Repository.SearchVehicleUtilization(new TestSearchVehicleUtilizationReport()).ToList();
        }

        #endregion

        #region Test classes

        private class TestSearchVehicleUtilizationReport : SearchSet<VehicleUtilizationReportItem>,
            ISearchVehicleUtilizationReport
        {
            public int? OperatingCenter { get; set; }
            public int? Department { get; set; }
            public int? Status { get; set; }
            public int? AssignmentStatus { get; set; }
            public int? AssignmentCategory { get; set; }
            public int? AssignmentJustification { get; set; }
            public bool? PoolUse { get; set; }
            public int? AccountingRequirement { get; set; }
            public bool? Flag { get; set; }
        }

        #endregion
    }
}
