using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Permits.Data.Client.Repositories;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class StreetOpeningPermitReportControllerTest : MapCallMvcControllerTestBase<StreetOpeningPermitReportController, OperatingCenter, OperatingCenterRepository>
    {
        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IPermitsRepositoryFactory>().Use(() => new Mock<IPermitsRepositoryFactory>().Object); 
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/StreetOpeningPermitReport/Search", StreetOpeningPermitReportController.ROLE);
                a.RequiresRole("~/Reports/StreetOpeningPermitReport/Index", StreetOpeningPermitReportController.ROLE);
            });
        }

        [TestMethod]
        public void TestIndexRedirectsBackToSearchIfModelIsInvalid()
        {
            var search = new SearchStreetOpeningPermit();

            _target.ModelState.AddModelError("", "Error");
            var result = _target.Index(search) as RedirectToRouteResult;

            Assert.AreEqual("Search", result.RouteValues["action"]);
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            Assert.Inconclusive("Tests weren't written for this and the automatic testing will not work here.");
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me. This doesn't use ISearchSet");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Test me. This doesn't use ISearchSet");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            Assert.Inconclusive("Test me. This doesn't use ISearchSet");
        }
    }
}