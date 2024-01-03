using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.SAP.Controllers;
using MapCallMVC.Areas.SAP.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.SAP.Controllers
{
    [TestClass]
    public class SAPFunctionalLocationControllerTest : MapCallMvcControllerTestBase<SAPFunctionalLocationController,Equipment>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/SAP/SAPFunctionalLocation/Index");
                a.RequiresLoggedInUserOnly("~/SAP/SAPFunctionalLocation/Find");
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // override because not ISearchSet.
            var sapRepo = new Mock<ISAPFunctionalLocationRepository>();
            _container.Inject(sapRepo.Object);
            var results = new SAPFunctionalLocationCollection();
            results.Items.Add(new SAPFunctionalLocation { SAPErrorCode = "Successful" });
            sapRepo.Setup(x => x.Search(It.IsAny<MapCall.Common.Model.ViewModels.SearchSapFunctionalLocation>())).Returns(results);
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var planningPlant = GetEntityFactory<PlanningPlant>().Create(new { OperatingCenter = operatingCenter, Code = "P101" });
            operatingCenter.PlanningPlants.Add(planningPlant);
            Session.Flush();
            var search = new SearchSapFunctionalLocation() { OperatingCenter = operatingCenter.Id };
            InitializeControllerAndRequest($"~/SAP/SAPFunctionalLocation/Index.json");

            var result = _target.Index(search) as JsonResult;

            Assert.IsNotNull(result);
        }

        #endregion

        #region Find

        [TestMethod]
        public void TestFindReturnsPartialViewWithSearch()
        {
            var result = _target.Find(null, null);

            MvcAssert.IsViewNamed(result, "_Find");
        }

        [TestMethod]
        public void TestFindFindsByOperatingCenter()
        {
            var thisOC = GetFactory<UniqueOperatingCenterFactory>().Create();
            var thatOC = GetFactory<UniqueOperatingCenterFactory>().Create();
            var expected = GetEntityFactory<PlanningPlant>().Create(new {OperatingCenter = thisOC});
            var unexpected = GetEntityFactory<PlanningPlant>().Create(new {OperatingCenter = thatOC});

            var result = _target.Find(thisOC.Id, null);

            var items = (IEnumerable<SelectListItem>)_target.ViewData["PlanningPlant"];

            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(expected.Code, items.Single().Value);
        }

        #endregion
    }
}