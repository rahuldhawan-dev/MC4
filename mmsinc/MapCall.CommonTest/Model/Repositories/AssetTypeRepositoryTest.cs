using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class AssetTypeRepositoryTest : InMemoryDatabaseTest<AssetType, AssetTypeRepository>
    {
        [TestMethod]
        public void TestByOperatingCenterIdReturnsByOperatingCenterId()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var assetType = GetFactory<FacilityAssetTypeFactory>().Create();
            var invalid = GetFactory<EquipmentAssetTypeFactory>().Create();
            var opcAssetType = GetEntityFactory<OperatingCenterAssetType>()
               .Create(new {OperatingCenter = opc, AssetType = assetType});
            Session.Save(opc);
            Session.Flush();

            var result = Repository.GetByOperatingCenterId(opc.Id).ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.IsFalse(result.Contains(invalid));
            Assert.IsTrue(result.Contains(assetType));
        }
    }
}
