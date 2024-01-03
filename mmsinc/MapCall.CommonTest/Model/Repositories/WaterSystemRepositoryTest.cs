using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class WaterSystemRepositoryTest : InMemoryDatabaseTest<WaterSystem, WaterSystemRepository>
    {
        [TestMethod]
        public void TestGetByOperatingCenterIdReturnsByOperatingCenterId()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var waterSystem = GetEntityFactory<WaterSystem>().Create();
            var invalid = GetEntityFactory<WaterSystem>().Create();
            opc.WaterSystems.Add(waterSystem);
            Session.Save(opc);
            Session.Flush();

            var result = Repository.GetByOperatingCenterId(opc.Id).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsFalse(result.Contains(invalid));
        }
    }
}
