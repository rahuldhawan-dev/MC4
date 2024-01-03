using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class UserRoleControllerTest : MapCallMvcControllerTestBase<UserRoleController, Role>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/Reports/UserRole/Index");
                a.RequiresSiteAdminUser("~/Reports/UserRole/Search");
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }

        #endregion
    }
}
