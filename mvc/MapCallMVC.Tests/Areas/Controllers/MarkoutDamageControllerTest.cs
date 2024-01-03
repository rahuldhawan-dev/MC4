using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class MarkoutDamageControllerTest : MapCallMvcControllerTestBase<MarkoutDamageController, MarkoutDamage, MarkoutDamageRepository>
    {
        #region Fields

        private OperatingCenter _opCenter;
        private Mock<INotificationService> _notifier;

        private MarkoutDamageToType _otherMarkoutDamageToType;
        private MarkoutDamageToType _ourMarkoutDamageToType;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _opCenter = GetFactory<OperatingCenterFactory>().Create();
            var user = GetEntityFactory<User>().Create(new {DefaultOperatingCenter = _opCenter, FullName = "Full Name"});

            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices}),
                Module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.FieldServicesWorkManagement}),
                Action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read}),
                OperatingCenter = _opCenter,
                User = user
            });

            Session.Save(user);

            return user;
        }

        [TestInitialize]
        public void TestInitialize()
        {

            _notifier = new Mock<INotificationService>();
            _container.Inject(_notifier.Object);

            // These need to exist for some validation things to pass.
            _otherMarkoutDamageToType = GetFactory<MarkoutDamageToTypeFactory>().Create(new { Description = MarkoutDamageToType.ImportantDescriptions.OTHERS });
            _ourMarkoutDamageToType = GetFactory<MarkoutDamageToTypeFactory>().Create(new { Description = MarkoutDamageToType.ImportantDescriptions.OURS });
            _target = Request.CreateAndInitializeController<MarkoutDamageController>();
        }

        #endregion

        #region Tests

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/FieldOperations/MarkoutDamage/Index/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/MarkoutDamage/Search/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/MarkoutDamage/Show/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/MarkoutDamage/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/MarkoutDamage/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/MarkoutDamage/New/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/MarkoutDamage/Create/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/MarkoutDamage/Destroy/", module, RoleActions.Delete);
            });
        }

        #endregion

        #region Search/Index

        [TestMethod]
        public void TestIndexRespondsToMapWithExpectedModels()
        {
            InitializeControllerAndRequest("~/FieldOperations/MarkoutDamage/Index.map");
            var good = GetFactory<MarkoutDamageFactory>().Create(new { OperatingCenter = _opCenter });
            var bad = GetFactory<MarkoutDamageFactory>().Create(new { OperatingCenter = typeof(UniqueOperatingCenterFactory) });
            var model = new SearchMarkoutDamage
            {
                OperatingCenter = good.OperatingCenter.Id
            };
            var result = (MapResult)_target.Index(model);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
            Assert.IsTrue(result.ModelStateIsValid);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetFactory<MarkoutDamageFactory>().Create();
            var entity1 = GetFactory<MarkoutDamageFactory>().Create();
            var search = new SearchMarkoutDamage();
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

        [TestMethod]
        public void TestSearchReturnsOnlySignedOffMarkoutDamageRecordsIfIsSignedOffIsTrue()
        {
            var signedOff = GetFactory<MarkoutDamageFactory>().Create(new
            {
                OperatingCenter = _opCenter,
                SupervisorSignOffEmployee = typeof(ActiveEmployeeFactory)
            });

            var anotherSignedOff = GetFactory<MarkoutDamageFactory>().Create(new
            {
                OperatingCenter = _opCenter,
                SupervisorSignOffEmployee = typeof(ActiveEmployeeFactory)
            });

            var notSignedOff = GetFactory<MarkoutDamageNotSignedOffBySupervisorFactory>().Create(new
            {
                OperatingCenter = _opCenter
            });
            var anotherNotSignedOff = GetFactory<MarkoutDamageNotSignedOffBySupervisorFactory>().Create(new
            {
                OperatingCenter = _opCenter
            });
            var search = new SearchMarkoutDamage
            {
                IsSignedOffBySupervisor = true
            };

            _target.Index(search);

            Assert.IsNotNull(search.Results, "This should like not be null and stuff.");

            Assert.IsTrue(search.Results.Contains(signedOff));
            Assert.IsTrue(search.Results.Contains(anotherSignedOff));
            Assert.IsFalse(search.Results.Contains(notSignedOff));
            Assert.IsFalse(search.Results.Contains(anotherNotSignedOff));
        }

        [TestMethod]
        public void TestSearchIndexReturnsOnlyNonSignedOffMarkoutDamageRecordsIfIsSignedOffIsFalse()
        {
            var signedOff = GetFactory<MarkoutDamageFactory>().Create(new
            {
                OperatingCenter = _opCenter,
                SupervisorSignOffEmployee = typeof(ActiveEmployeeFactory)
            });

            var anotherSignedOff = GetFactory<MarkoutDamageFactory>().Create(new
            {
                OperatingCenter = _opCenter,
                SupervisorSignOffEmployee = typeof(ActiveEmployeeFactory)
            });

            var notSignedOff = GetFactory<MarkoutDamageNotSignedOffBySupervisorFactory>().Create(new
            {
                OperatingCenter = _opCenter
            });
            var anotherNotSignedOff = GetFactory<MarkoutDamageNotSignedOffBySupervisorFactory>().Create(new
            {
                OperatingCenter = _opCenter
            });

            var search = new SearchMarkoutDamage
            {
                IsSignedOffBySupervisor = false
            };

            _target.Index(search);

            Assert.IsFalse(search.Results.Contains(signedOff));
            Assert.IsFalse(search.Results.Contains(anotherSignedOff));
            Assert.IsTrue(search.Results.Contains(notSignedOff));
            Assert.IsTrue(search.Results.Contains(anotherNotSignedOff));
        }

        [TestMethod]
        public void TestIndexByStateAndCounty()
        {
            var state1 = GetFactory<StateFactory>().Create(new { Abbreviation = "NJ" });
            var state2 = GetFactory<StateFactory>().Create(new { Abbreviation = "NY" });
            var county1 = GetFactory<CountyFactory>().Create(new { State = state1 });
            var county2 = GetFactory<CountyFactory>().Create(new { State = state2 });
            var town1 = GetFactory<TownFactory>().Create(new { State = state1, County = county1 });
            var town2 = GetFactory<TownFactory>().Create(new { State = state2, County = county2 });
            var entity0 = GetFactory<MarkoutDamageFactory>().Create(new { Town = town1 });
            var entity1 = GetFactory<MarkoutDamageFactory>().Create(new { Town = town2 });
            var search = new SearchMarkoutDamage();

            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(entity0));
            Assert.IsTrue(search.Results.Contains(entity1));

            search.State = state1.Id;

            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(entity0));
            Assert.IsFalse(search.Results.Contains(entity1));

            search.State = null;
            search.County = county2.Id;

            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(entity1));
            Assert.IsFalse(search.Results.Contains(entity0));
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestNewWithWorkOrderPrePopulatesExpectedFields()
        {
            var iconSet = GetEntityFactory<IconSet>().CreateList(12);
            var coordinate = GetEntityFactory<Coordinate>().Create(new { Latitude = -10m, Longitude = 10m });
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {coordinate.Latitude, coordinate.Longitude, Coordinate = coordinate });
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, workOrder.OperatingCenter, _currentUser, RoleActions.Read);

            var result = (ViewResult)_target.New(workOrder.Id);
            var resultModel = (CreateMarkoutDamage)result.Model;

            Assert.AreEqual(workOrder.Id, resultModel.WorkOrder);
            Assert.AreEqual(workOrder.OperatingCenter.Id, resultModel.OperatingCenter);
            Assert.AreEqual(workOrder.OperatingCenter.State.Id, resultModel.State);
            Assert.AreEqual(workOrder.Town.County.Id, resultModel.County);
            Assert.AreEqual(workOrder.Town.Id, resultModel.Town);
            Assert.IsNotNull(resultModel.Coordinate);
            Assert.AreEqual(coordinate.Latitude, workOrder.Latitude );
            Assert.AreEqual(coordinate.Longitude, workOrder.Longitude );
        }
        
        [TestMethod]
        public void TestCreateSendsNewMarkoutDamageRecordNotificationEmail()
        {
            var ent = GetFactory<MarkoutDamageFactory>().Create();
            
            var model = _viewModelFactory.Build<CreateMarkoutDamage, MarkoutDamage>( ent);
            model.Id = 0;

            // If this fails, the rest of the test will fail. Sanity check.
            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(MarkoutDamageController.ROLE_MODULE, resultArgs.Module);
            Assert.AreEqual(MarkoutDamageController.NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.AreEqual("http://localhost/FieldOperations/MarkoutDamage/Show/" + entity.Id, entity.RecordUrl);
            Assert.IsNull(resultArgs.Subject);
        }

        [TestMethod]
        public void TestCreateSendsMarkoutDamageSIForSIFPEventNotificationEmailIfUtilityDamagesAreGasOrElectric()
        {
            var electricType = GetFactory<GasMarkoutDamageUtilityDamageTypeFactory>().Create();
            var gasType = GetFactory<ElectricMarkoutDamageUtilityDamageTypeFactory>().Create();

            foreach (var utilityDamageType in new[] { electricType, gasType })
            {
                var ent = GetFactory<MarkoutDamageFactory>().Create();
                
                var model = _viewModelFactory.Build<CreateMarkoutDamage, MarkoutDamage>(ent);
                model.Id = 0;
                model.UtilityDamages = new[] { utilityDamageType.Id }; 

                // If this fails, the rest of the test will fail. Sanity check.
                ValidationAssert.ModelStateIsValid(model);

                NotifierArgs resultArgs = null;

                // HUGE NOTE: This test is flimsy. The notifier is going to get the callback twice,
                // once for the new record notification, then again for the SIF event notification.
                // If we ever add a third notification during Create, we're gonna have to redo how
                // all of the notification tests work in this controller test.
                _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
                var result = _target.Create(model);
                var entity = Repository.Find(model.Id);

                Assert.AreSame(entity, resultArgs.Data);
                Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
                Assert.AreEqual(MarkoutDamageController.ROLE_MODULE, resultArgs.Module);
                Assert.AreEqual(MarkoutDamageController.SIF_OR_SIFP_EVENT_NOTIFICATION_PURPOSE, resultArgs.Purpose);
                Assert.AreEqual("http://localhost/FieldOperations/MarkoutDamage/Show/" + entity.Id, entity.RecordUrl);
                Assert.IsNull(resultArgs.Subject);
            }
        }

        [TestMethod]
        public void TestCreateDoesNotSendMarkoutDamageSIForSIFPEventNotificationEmailIfUtilityDamagesAreNotGasOrElectric()
        {
            var communicationType = GetFactory<CommunicationMarkoutDamageUtilityDamageTypeFactory>().Create();
            var sewerType = GetFactory<SewerMarkoutDamageUtilityDamageTypeFactory>().Create();
            var waterType = GetFactory<WaterMarkoutDamageUtilityDamageTypeFactory>().Create();

            foreach (var utilityDamageType in new[] { communicationType, sewerType, waterType })
            {
                var ent = GetFactory<MarkoutDamageFactory>().Create();
                
                var model = _viewModelFactory.Build<CreateMarkoutDamage, MarkoutDamage>(ent);
                model.Id = 0;
                model.UtilityDamages = new[] { utilityDamageType.Id }; 

                // If this fails, the rest of the test will fail. Sanity check.
                ValidationAssert.ModelStateIsValid(model);

                NotifierArgs resultArgs = null;

                // HUGE NOTE: This test is flimsy. The notifier is going to get the callback
                // once for the new record notification. We have to test that it's *not*
                // the SIF event notification rather than just testing that the Notifier was 
                // never triggered. If we ever add a third notification here, this test will
                // likely break. But so will all the other notification tests in this controller test.
                _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
                var result = _target.Create(model);
                var entity = Repository.Find(model.Id);

                Assert.AreNotEqual(MarkoutDamageController.SIF_OR_SIFP_EVENT_NOTIFICATION_PURPOSE, resultArgs.Purpose);
            }
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var good = GetFactory<MarkoutDamageFactory>().Create(new { OperatingCenter = _opCenter });
            InitializeControllerAndRequest("~/FieldOperations/MarkoutDamage/Show/" + good.Id + ".frag");

            var result = _target.Show(good.Id);
            MvcAssert.IsViewNamed(result, "_ShowPopup");
            MvcAssert.IsViewWithModel(result, good);
        }

        [TestMethod]
        public void TestShowRespondsToMap()
        {
            var good = GetFactory<MarkoutDamageFactory>().Create(new { OperatingCenter = _opCenter });
            var bad = GetFactory<MarkoutDamageFactory>().Create(new { OperatingCenter = _opCenter });
            InitializeControllerAndRequest("~/FieldOperations/MarkoutDamage/Show/" + good.Id + ".map");

            var result = (MapResult)_target.Show(good.Id);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
            Assert.IsTrue(result.ModelStateIsValid);
        }

        #endregion

        #endregion
    }
}
