using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class JobSiteSafetyAnalysisControllerTest : MapCallMvcControllerTestBase<JobSiteSafetyAnalysisController, JobSiteSafetyAnalysis>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/JobSiteSafetyAnalysis/Search/", JobSiteCheckListController.ROLE_MODULE);
                a.RequiresRole("~/JobSiteSafetyAnalysis/Index/", JobSiteCheckListController.ROLE_MODULE);
                a.RequiresRole("~/JobSiteSafetyAnalysis/Show/", JobSiteCheckListController.ROLE_MODULE);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me. Look at the test below because it is not testing what it claims to be testing.");
        }

        [TestMethod]
        public void TestIndexReturnsIndexViewWithArrayOfJobSiteSafetyAnalysiss()
        {
            // This is not testing for results like the test name claims.
            // It is testing that no results are found and it redirects back to the search page.
            Assert.Inconclusive("Wrong. This test is wrong.");
            var eq1 = GetEntityFactory<JobSiteSafetyAnalysis>().Create();
            var eq2 = GetEntityFactory<JobSiteSafetyAnalysis>().Create();
            var search = new SearchJobSiteSafetyAnalysis();
            _target.ControllerContext = new ControllerContext();

            var result = _target.Index(search) as RedirectToRouteResult;

            Assert.AreEqual("Search", result.RouteValues["action"]);
        }

        #endregion
    }
}
