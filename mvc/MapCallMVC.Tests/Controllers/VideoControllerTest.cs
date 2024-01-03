using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class VideoControllerTest : MapCallMvcControllerTestBase<VideoController, Video, VideoRepository>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            // This needs to exist.
            GetFactory<ActiveAssetStatusFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Create tests can't run because they require a mocked repository. Or the
            // parts of VideoRepository that deal with the Sprout API should be moved to
            // a separate mockable class.
            options.CreateRedirectsToReferrerOnSuccess = true;
            options.DestroyRedirectsToReferrerOnSuccess = true;
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Video/Index");
                a.RequiresLoggedInUserOnly("~/Video/Show");
                a.RequiresLoggedInUserOnly("~/Video/GetAllTags");
                a.RequiresLoggedInUserOnly("~/Video/Create");
                a.RequiresLoggedInUserOnly("~/Video/Destroy");
            });
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            Assert.Inconclusive("Test me. I need a mock repo.");
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            Assert.Inconclusive("Test me. I need a mock repo.");
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            Assert.Inconclusive("Test me. I need a mock repo");
        }

        #endregion
    }
}
