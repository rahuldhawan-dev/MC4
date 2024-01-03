using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class RoadwayImprovementNotificationStreetControllerTest : MapCallMvcControllerTestBase<RoadwayImprovementNotificationStreetController, RoadwayImprovementNotificationStreet>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var role = RoleModules.FieldServicesProjects;
                a.RequiresRole("~/ProjectManagement/RoadwayImprovementNotificationStreet/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/RoadwayImprovementNotificationStreet/Update", role, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/RoadwayImprovementNotificationStreet/Show", role);
                a.RequiresRole("~/ProjectManagement/RoadwayImprovementNotificationStreet/Search", role);
                a.RequiresRole("~/ProjectManagement/RoadwayImprovementNotificationStreet/Index", role);
            });
        }

        #region Show

        private RoadwayImprovementNotificationStreet CreateEntity()
        {
            // TODO: Shouldn't the factories all be setting this up already?
            var street = GetEntityFactory<Street>().Create();
            var rin = GetEntityFactory<RoadwayImprovementNotification>().Create();
            var status = GetEntityFactory<RoadwayImprovementNotificationStreetStatus>().Create();

            var entity = GetEntityFactory<RoadwayImprovementNotificationStreet>().Create(new {
                RoadwayImprovementNotification = rin,
                RoadwayImprovementNotificationStreetStatus = status,
                Street = street
            });
            return entity;
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var street = GetEntityFactory<Street>().Create();
            var rin = GetEntityFactory<RoadwayImprovementNotification>().Create();
            var status = GetEntityFactory<RoadwayImprovementNotificationStreetStatus>().Create();

            var eq = GetEntityFactory<RoadwayImprovementNotificationStreet>().Create(new
            {
                RoadwayImprovementNotification = rin,
                RoadwayImprovementNotificationStreetStatus = status,
                Street = street
            });
            var expected = 3;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditRoadwayImprovementNotificationStreet, RoadwayImprovementNotificationStreet>(eq, new {
                MainBreakActivity = expected
            }));

            Assert.AreEqual(expected, Session.Get<RoadwayImprovementNotificationStreet>(eq.Id).MainBreakActivity);
        }
       
        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = CreateEntity();
            var entity1 = CreateEntity();
            var search = new SearchRoadwayImprovementNotificationStreet();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
            }
        }

        #endregion

        #endregion
    }
}