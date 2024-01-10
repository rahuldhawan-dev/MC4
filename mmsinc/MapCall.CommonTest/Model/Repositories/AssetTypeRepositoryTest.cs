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

        [TestMethod]
        public void TestGetByStateIdReturnsByStateId()
        {
            var assetType1 = GetFactory<FacilityAssetTypeFactory>().Create();
            var state = GetFactory<StateFactory>().Create();
            var opc1 = GetFactory<OperatingCenterFactory>().Create(new { State = state });
            var opcAssetType1 = GetEntityFactory<OperatingCenterAssetType>()
               .Create(new { OperatingCenter = opc1, AssetType = assetType1 });
            Session.Save(opc1);
            var assetType2 = GetFactory<EquipmentAssetTypeFactory>().Create();
            var opc2 = GetFactory<OperatingCenterFactory>().Create(new { State = state });
            var opcAssetType2 = GetEntityFactory<OperatingCenterAssetType>()
               .Create(new { OperatingCenter = opc2, AssetType = assetType2 });
            Session.Save(opc2);
            Session.Flush();

            var result = Repository.GetByStateId(state.Id).ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.IsTrue(result.Contains(assetType1));
            Assert.IsTrue(result.Contains(assetType2));
        }
    }
}
