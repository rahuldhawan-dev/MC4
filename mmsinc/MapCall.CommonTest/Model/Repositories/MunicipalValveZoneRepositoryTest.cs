using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    public class
        MunicipalValveZoneRepositoryTest : InMemoryDatabaseTest<MunicipalValveZone, MunicipalValveZoneRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        #endregion

        #region Reports

        [TestMethod]
        public void TestGetMunicipalValveZoneReportItemsReturnsCorrectCountsByGroup()
        {
            var args = new TestMunicipalValveZoneReport();
            var results = Repository.GetMunicipalValveZoneReportItems(args).ToList();

            Assert.AreEqual(0, results.Count);

            var factory = GetFactory<ValveFactory>();
            var billing1 = GetFactory<PublicValveBillingFactory>().Create();
            var billing2 = GetFactory<MunicipalValveBillingFactory>().Create();
            var activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            var smallSize = GetEntityFactory<ValveSize>().Create(new {Size = 2.0m});
            var largeSize = GetEntityFactory<ValveSize>().Create(new {Size = 12.0m});
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCenters.Add(opc1);
            Session.Clear();
            var valveZone1 = GetEntityFactory<ValveZone>().Create(new {Description = "1"});
            var valveZone2 = GetEntityFactory<ValveZone>().Create(new {Description = "2"});

            var val1 = factory.Create(new {
                ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1,
                Town = town, ValveZone = valveZone1
            });
            var val2 = factory.Create(new {
                ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1,
                Town = town, ValveZone = valveZone1
            });
            var val3 = factory.Create(new {
                ValveBilling = billing1, ValveSize = smallSize, Status = activeStatus, OperatingCenter = opc1,
                Town = town, ValveZone = valveZone1
            });

            var muniValZone1 = GetEntityFactory<MunicipalValveZone>().Create(new
                {OperatingCenter = opc1, Town = town, SmallValveZone = valveZone1, LargeValveZone = valveZone2});

            results = Repository.GetMunicipalValveZoneReportItems(args).ToList();

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(opc1.OperatingCenterCode, results[0].OperatingCenter);
            Assert.AreEqual(town.ShortName, results[0].Town);
            //Assert.AreEqual(3, results[0].SmallValves);
            //Assert.AreEqual(0, results[0].LargeValves);
            Assert.AreEqual("1", results[0].SmallValveZone);
            Assert.AreEqual("2", results[0].LargeValveZone);
            Assert.AreEqual(3, results[0].SmallValves);
            Assert.AreEqual(0, results[0].LargeValves);
        }

        private class TestMunicipalValveZoneReport : SearchSet<MunicipalValveZoneReportItem>
        {
            [SearchAlias("OperatingCenter", "opc", "Id")]
            public int? OperatingCenter { get; set; }

            public int? Town { get; set; }
        }

        #endregion
    }
}
