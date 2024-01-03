using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.WaterQuality.Models.ViewModels
{
    [TestClass]
    public class CreateSampleSiteTest : SampleSiteViewModelTest<CreateSampleSite>
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();

            _vmTester.CanMapBothWays(x => x.Street);
            _vmTester.CanMapBothWays(x => x.CrossStreet);
        }

        [TestMethod]
        public void TestSetDefaultsSetsStatusToPending()
        {
            _viewModel.SetDefaults();

            Assert.IsNotNull(_viewModel.Status);
            Assert.AreEqual(SampleSiteStatus.Indices.PENDING, _viewModel.Status.Value);
        }

        #endregion
    }
}
