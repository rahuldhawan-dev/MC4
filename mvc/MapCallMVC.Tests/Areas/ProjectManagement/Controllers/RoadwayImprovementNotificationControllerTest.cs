using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Controllers
{
    [TestClass]
    public class RoadwayImprovementNotificationControllerTest : MapCallMvcControllerTestBase<RoadwayImprovementNotificationController, RoadwayImprovementNotification>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject(new Mock<INotificationService>().Object);
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = RoadwayImprovementNotificationController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/RoadwayImprovementNotification/Search/", role);
                a.RequiresRole("~/FieldOperations/RoadwayImprovementNotification/Show/", role);
                a.RequiresRole("~/FieldOperations/RoadwayImprovementNotification/Index/", role);
                a.RequiresRole("~/FieldOperations/RoadwayImprovementNotification/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/RoadwayImprovementNotification/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/RoadwayImprovementNotification/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/RoadwayImprovementNotification/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/RoadwayImprovementNotification/AddRoadwayImprovementNotificationStreet/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/RoadwayImprovementNotification/RemoveRoadwayImprovementNotificationStreet/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/RoadwayImprovementNotification/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion
        
        #region New

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersInLookupData()
        {
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _target.New();

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<RoadwayImprovementNotification>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<RoadwayImprovementNotification>().Create(new {Description = "description 1"});
            var search = new SearchRoadwayImprovementNotification();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<RoadwayImprovementNotification>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditRoadwayImprovementNotification, RoadwayImprovementNotification>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<RoadwayImprovementNotification>(eq.Id).Description);
        }

        #endregion

        #region Add/Remove Streets

        [TestMethod]
        public void TestAddRoadwayImprovementNotificationStreetCreatesAndAddsStreet()
        {
            var roadwayImprovementNotification = GetEntityFactory<RoadwayImprovementNotification>().Create();
            var street = GetEntityFactory<Street>().Create();
            var status = GetEntityFactory<RoadwayImprovementNotificationStreetStatus>().Create();

            MyAssert.CausesIncrease(() => _target.AddRoadwayImprovementNotificationStreet(new AddRoadwayImprovementNotificationStreet(_container) {
                Id = roadwayImprovementNotification.Id,
                Street = street.Id,
                RoadwayImprovementNotificationStreetStatus = status.Id
            }), _container.GetInstance<RepositoryBase<RoadwayImprovementNotificationStreet>>().GetAll().Count);
        } 

        [TestMethod]
        public void TestRemoveRoadwayImprovementNotificationStreetRemovesStreet()
        {
            var roadwayImprovementNotification = GetEntityFactory<RoadwayImprovementNotification>().Create();
            var street = GetEntityFactory<Street>().Create();
            var status = GetEntityFactory<RoadwayImprovementNotificationStreetStatus>().Create();
            
            _target.AddRoadwayImprovementNotificationStreet(new AddRoadwayImprovementNotificationStreet(_container) {
                Id = roadwayImprovementNotification.Id,
                Street = street.Id,
                RoadwayImprovementNotificationStreetStatus = status.Id
            });

            roadwayImprovementNotification =
                Session.Load<RoadwayImprovementNotification>(roadwayImprovementNotification.Id);

            MyAssert.CausesDecrease(() => _target.RemoveRoadwayImprovementNotificationStreet(
                _viewModelFactory
                   .BuildWithOverrides<RemoveRoadwayImprovementNotificationStreet, RoadwayImprovementNotification
                    >(roadwayImprovementNotification, new {
                        RoadwayImprovementNotificationStreetId = roadwayImprovementNotification
                                                                .RoadwayImprovementNotificationStreets.First().Id
                    })), _container.GetInstance<RepositoryBase<RoadwayImprovementNotificationStreet>>().GetAll().Count);
        }

        #endregion
    }
}
