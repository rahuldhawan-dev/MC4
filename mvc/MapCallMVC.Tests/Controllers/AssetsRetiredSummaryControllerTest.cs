using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class AssetsRetiredSummaryControllerTest : MapCallMvcControllerTestBase<AssetsRetiredSummaryController, EstimatingProject, EstimatingProjectRepository>
    {
        #region Constants

        public const RoleModules ROLE = AssetsRetiredSummaryController.ROLE;

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/AssetsRetiredSummary/Show/", ROLE);
            });
        }

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            Assert.Inconclusive("Test me. I return a view model instead of the entity.");
        }
    }
}