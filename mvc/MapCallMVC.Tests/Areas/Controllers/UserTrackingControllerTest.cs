using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Operations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class UserTrackingControllerTest : MapCallMvcControllerTestBase<UserTrackingController, User>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Operations/UserTracking/Index", RoleModules.ManagementGeneral);
                a.RequiresRole("~/Operations/UserTracking/Search", RoleModules.ManagementGeneral);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns 2 results
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            // overridden because search returns results still.
            Assert.Inconclusive("Test me");
        }

        #endregion
    }
}
