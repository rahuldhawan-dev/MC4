using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class JobObservationControllerTest : MapCallMvcControllerTestBase<JobObservationController, JobObservation>
    {
        #region Fields

        private User _user;
        private OperatingCenter _operatingCenter;
        private Mock<INotificationService> _notifier;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _notifier = e.For<INotificationService>().Mock();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            _user = GetEntityFactory<User>().Create(new {DefaultOperatingCenter = _operatingCenter});
            _authenticationService.Setup(x => x.CurrentUser).Returns(_user);
        }
        
        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.OperationsHealthAndSafety;
            Authorization.Assert(a => {
                a.RequiresRole("~/HealthAndSafety/JobObservation/Show/", role, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/JobObservation/Search/", role, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/JobObservation/Index/", role, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/JobObservation/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/JobObservation/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/JobObservation/New/", role, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/JobObservation/Create/", role, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/JobObservation/Destroy/", role, RoleActions.Delete);

            });
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var entity = GetEntityFactory<JobObservation>().Create();
            InitializeControllerAndRequest("~/HealthAndSafety/JobObservation/Show" + entity.Id + ".frag");

            var result = _target.Show(entity.Id);

            MvcAssert.IsViewNamed(result, "_ShowPopup");
            MvcAssert.IsViewWithModel(result, entity);
        }

        [TestMethod]
        public void TestShowRespondsToMap()
        {
            var good = GetEntityFactory<JobObservation>().Create();
            var bad = GetEntityFactory<JobObservation>().Create();
            InitializeControllerAndRequest("~/HealthAndSafety/JobObservation/Show" + good.Id + ".map");

            var result = (MapResult)_target.Show(good.Id);
            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
            Assert.IsTrue(result.ModelStateIsValid);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<JobObservation>().Create(new { TaskObserved = "description 0"});
            var entity1 = GetEntityFactory<JobObservation>().Create(new { TaskObserved = "description 1"});
            var search = new SearchJobObservation();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.TaskObserved, "TaskObserved");
                helper.AreEqual(entity1.TaskObserved, "TaskObserved", 1);
            }
        }

        #endregion

        #region Update 

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<JobObservation>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditJobObservation, JobObservation>(eq, new {
                TaskObserved = expected
            }));

            Assert.AreEqual(expected, Session.Get<JobObservation>(eq.Id).TaskObserved);
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSendsNotificationEmail()
        {
            var ent = GetEntityFactory<JobObservation>().Create();
            var model = _viewModelFactory.Build<CreateJobObservation, JobObservation>( ent);
            model.Id = 0;
            ValidationAssert.ModelStateIsValid(model);
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(JobObservationController.ROLE, resultArgs.Module);
            Assert.AreEqual(JobObservationController.NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.AreEqual("http://localhost/HealthAndSafety/JobObservation/Show/" + entity.Id, entity.RecordUrl);
            Assert.IsNull(resultArgs.Subject);
        }

        [TestMethod]
        public void TestNewWithValidIdSetsDefaultValues()
        {
            var icons = GetFactory<MapIconFactory>().Create(new {FileName = "pin_black"});

            _currentUser.IsAdmin = true;
            _authenticationService.Setup(x => x.CurrentUser).Returns(_currentUser);
            var workDescription = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workOrder = GetFactory<WorkOrderFactory>().Create(new { WorkDescription = workDescription });

            var result = (ViewResult)_target.New(workOrder.Id, false);
            var model = (CreateJobObservation)result.Model;
            var coordinate = Session.Get<Coordinate>(model.Coordinate);

            Assert.AreEqual(workOrder.OperatingCenter.Id, model.OperatingCenter);
            Assert.AreEqual(Convert.ToDecimal(workOrder.Latitude), coordinate.Latitude);
            Assert.AreEqual(Convert.ToDecimal(workOrder.Longitude), coordinate.Longitude);
            Assert.AreEqual(String.Format("{0} {1}", workOrder.StreetAddress, workOrder.TownAddress), model.Address);
            Assert.AreEqual(workOrder.Id, model.WorkOrder);
            Assert.AreEqual("/Coordinate/Create", model.CoordinateCreateUrl);
            Assert.AreEqual(workDescription.Description, model.TaskObserved);
        }

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
    }
}
