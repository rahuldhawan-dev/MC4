using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.WaterQuality.Controllers;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class SampleSiteLeadCopperCertificationControllerTest : MapCallMvcControllerTestBase<SampleSiteLeadCopperCertificationController, SampleSite>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = SampleSiteController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/WaterQuality/SampleSiteLeadCopperCertification/New/", role);
                a.RequiresRole("~/WaterQuality/SampleSiteLeadCopperCertification/Show/", role);
            });
        }

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override needed because of New param.
            Assert.Inconclusive("Test me");
        }

        #endregion

        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // override needed due to model.
            Assert.Inconclusive("Test me. I'm a pdf and I do things with Viewdata.");
        }

        [TestMethod]
        public override void TestShowReturnsNotFoundIfRecordCanNotBeFound()
        {
            // override due to model
            MvcAssert.IsNotFound(_target.Show(_viewModelFactory.Build<CreateSampleSiteLeadCopperCertification>()));
        }

        #endregion

        #endregion
    }
}
