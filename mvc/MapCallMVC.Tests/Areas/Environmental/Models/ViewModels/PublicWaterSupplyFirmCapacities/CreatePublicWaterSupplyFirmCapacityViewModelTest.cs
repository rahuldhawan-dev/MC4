using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Models.ViewModels.PublicWaterSupplyFirmCapacities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels.PublicWaterSupplyFirmCapacities
{
    [TestClass]
    public class CreatePublicWaterSupplyFirmCapacityViewModelTest : PublicWaterSupplyFirmCapacityViewModelTestBase<CreatePublicWaterSupplyFirmCapacityViewModel>
    {
        #region Tests

        #region Mapping
        [TestMethod]
        public void TestMapSetsCurrentPublicWaterSupplyFirmCapacity()
        {
            _viewModel.PublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create().Id;

            _vmTester.MapToEntity();

            Assert.IsNotNull(_entity.PublicWaterSupply.CurrentPublicWaterSupplyFirmCapacity);
            Assert.AreEqual(_entity.PublicWaterSupply.CurrentPublicWaterSupplyFirmCapacity.Id, _viewModel.Id);
        }

        #endregion

        #endregion
    }
}