using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.SAP.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.SAP.Controllers
{
    [TestClass]
    public class WBSElementControllerTest : MapCallMvcControllerTestBase<WBSElementController, WorkOrder>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/SAP/WBSElement/Index", role, RoleActions.Read);
                a.RequiresRole("~/SAP/WBSElement/Find", role, RoleActions.Read);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // overridden because not ISearchSet
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because not ISearchSet
            var sapRepo = new Mock<ISAPWBSElementRepository>();
            _container.Inject(sapRepo.Object);
            var results = new SAPWBSElementCollection();
            results.Items.Add(new SAPWBSElement { SAPErrorCode = "Successful" });
            sapRepo.Setup(x => x.Search(It.IsAny<SAPWBSElement>())).Returns(results);
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var planningPlant = GetEntityFactory<PlanningPlant>().Create(new { OperatingCenter = operatingCenter, Code = "D101" });
            operatingCenter.PlanningPlants.Add(planningPlant);

            var search = new SearchWBSElement { OperatingCenter = operatingCenter.Id };
            InitializeControllerAndRequest($"~/SAP/WBSElement/Index.json");

            var result = _target.Index(search) as JsonResult;

            Assert.IsNotNull(result);
        }

        #endregion

        #region Find

        [TestMethod]
        public void TestFindReturnsPartialViewWithSearch()
        {
            var search = new SearchWBSElement();

            var result = _target.Find(666);

            MvcAssert.IsViewNamed(result, "_Find");
        }

        #endregion
    }
}