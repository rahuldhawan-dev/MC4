using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Controllers {
    [TestClass]
    public class AssetTypeControllerTest : MapCallMvcControllerTestBase<AssetTypeController, AssetType>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/AssetType/ByOperatingCenterId/");
            });
        }

        #region ByOperatingCenterId

        [TestMethod]
        public void TestByOperatingCenterIdReturnsAssetTypesForOperatingCenter()
        {
            var opcntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opcntr2 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ4" });
            var validAssetType1 = GetFactory<FacilityAssetTypeFactory>().Create();
            var validAssetType2 = GetFactory<EquipmentAssetTypeFactory>().Create();
            var invalid = GetFactory<HydrantAssetTypeFactory>().Create();
            validAssetType1.OperatingCenterAssetTypes.Add(new OperatingCenterAssetType { OperatingCenter = opcntr1, AssetType = validAssetType1 });
            validAssetType2.OperatingCenterAssetTypes.Add(new OperatingCenterAssetType { OperatingCenter = opcntr1, AssetType = validAssetType2 });
            invalid.OperatingCenterAssetTypes.Add(new OperatingCenterAssetType { OperatingCenter = opcntr2, AssetType = invalid });

            Session.Flush();

            var result = (CascadingActionResult)_target.ByOperatingCenterId(opcntr1.Id);

            var actual = result.GetSelectListItems();

            Assert.AreEqual(2, actual.Count() - 1); // --select here--
            foreach (var selectListItem in actual)
            {
                Assert.AreNotEqual(invalid.Id.ToString(), selectListItem.Value);
            }
        }

        #endregion
    }
}