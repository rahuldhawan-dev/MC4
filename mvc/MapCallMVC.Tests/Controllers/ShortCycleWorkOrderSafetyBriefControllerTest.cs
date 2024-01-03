using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class ShortCycleWorkOrderSafetyBriefControllerTest : MapCallMvcControllerTestBase<ShortCycleWorkOrderSafetyBriefController, ShortCycleWorkOrderSafetyBrief>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        private Mock<INotificationService> _notifier;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _user = GetFactory<UserFactory>().Create(new {Employee = GetEntityFactory<Employee>().Create()});
            _user.Employee.ReportsTo = GetEntityFactory<Employee>().Create();
            return _user;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>()
             .Add(new TestDateTimeProvider(_now = DateTime.Now));
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
            _notifier = e.For<INotificationService>().Mock();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);

            options.InitializeCreateViewModel = (model) => {
                ((CreateShortCycleWorkOrderSafetyBrief)model).HazardTypes = new [] {GetEntityFactory<ShortCycleWorkOrderSafetyBriefHazardType>().Create().Id};
                ((CreateShortCycleWorkOrderSafetyBrief)model).LocationTypes = new [] {GetEntityFactory<ShortCycleWorkOrderSafetyBriefLocationType>().Create().Id};
                ((CreateShortCycleWorkOrderSafetyBrief)model).PPETypes = new [] {GetEntityFactory<ShortCycleWorkOrderSafetyBriefPPEType>().Create().Id};
                ((CreateShortCycleWorkOrderSafetyBrief)model).ToolTypes = new [] {GetEntityFactory<ShortCycleWorkOrderSafetyBriefToolType>().Create().Id};
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = ShortCycleWorkOrderSafetyBriefController.ROLE;
                const string path = "~/ShortCycleWorkOrderSafetyBrief/";
                a.RequiresRole(path + "Search", role, RoleActions.Read);
                a.RequiresRole(path + "Show", role, RoleActions.Read);
                a.RequiresRole(path + "Index", role, RoleActions.Read);
                a.RequiresRole(path + "Create", role, RoleActions.Add);
                a.RequiresRole(path + "New", role, RoleActions.Add);
            });
        }

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            var result = (ViewResult)_target.New();
            var resultModel = (CreateShortCycleWorkOrderSafetyBrief)result.Model;

            Assert.AreEqual(_user.Employee.Id, resultModel.FSR);
            Assert.AreEqual(_user.Employee.FullName, resultModel.FSRName);
            Assert.AreEqual(_now.Date, resultModel.DateCompleted.Value.Date);
            MvcAssert.IsViewNamed(result, "New");
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ShortCycleWorkOrderSafetyBrief>().Create(
                new {
                    HasCompletedDailyStretchingRoutine = true, 
                    LocationTypes = new List<ShortCycleWorkOrderSafetyBriefLocationType> 
                    {
                        new ShortCycleWorkOrderSafetyBriefLocationType{Id = 1, Description = "Meter Reading"},
                        new ShortCycleWorkOrderSafetyBriefLocationType{Id = 2, Description = "FSR Work (Residence)"}
                    },
                    HazardTypes = new List<ShortCycleWorkOrderSafetyBriefHazardType>
                    {
                        new ShortCycleWorkOrderSafetyBriefHazardType{Id = 1, Description = "Slips, Trips, Falls"},
                        new ShortCycleWorkOrderSafetyBriefHazardType{Id = 2, Description = "Cuts/Abrasions"}
                    },
                    PPETypes = new List<ShortCycleWorkOrderSafetyBriefPPEType>
                    {
                        new ShortCycleWorkOrderSafetyBriefPPEType{Id = 1, Description = "Hardhat"},
                        new ShortCycleWorkOrderSafetyBriefPPEType{Id = 2, Description = "Hearing Protection"}
                    },
                    ToolTypes = new List<ShortCycleWorkOrderSafetyBriefToolType>
                    {
                        new ShortCycleWorkOrderSafetyBriefToolType{Id = 1, Description = "Hand Tools"},
                        new ShortCycleWorkOrderSafetyBriefToolType{Id = 2, Description = "Pump/Vaccum"}
                    }
                });
            var entity1 = GetEntityFactory<ShortCycleWorkOrderSafetyBrief>().Create(new {HasCompletedDailyStretchingRoutine = true});
            var search = new SearchShortCycleWorkOrderSafetyBrief();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.HasCompletedDailyStretchingRoutine, "HasCompletedDailyStretchingRoutine");
                helper.AreEqual(entity1.HasCompletedDailyStretchingRoutine, "HasCompletedDailyStretchingRoutine", 1);
                helper.AreEqual("Meter Reading, FSR Work (Residence)", "LocationTypes");
                helper.AreEqual("Slips, Trips, Falls, Cuts/Abrasions", "HazardTypes");
                helper.AreEqual("Hardhat, Hearing Protection", "PPETypes");
                helper.AreEqual("Hand Tools, Pump/Vaccum", "ToolTypes");
            }
        }

        #endregion

        #region Notifications

        [TestMethod]
        public void CreateSendsNotificationWhenHasCompletedDailyStretchingRoutineIsFalse()
        {
            var scwosb = GetEntityFactory<ShortCycleWorkOrderSafetyBrief>().Create(new{HasCompletedDailyStretchingRoutine = false, HasPerformedInspectionOnVehicle = true, IsPPEInGoodCondition = true});
            var model = _viewModelFactory.Build<CreateShortCycleWorkOrderSafetyBrief, ShortCycleWorkOrderSafetyBrief>(scwosb);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, ((ShortCycleWorkOrderSafetyBriefNotificationViewModel)resultArgs.Data).Entity);
            Assert.AreEqual(_user.Employee.ReportsTo.EmailAddress, resultArgs.Address);
            Assert.AreEqual(0, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.OperationsHealthAndSafety, resultArgs.Module);
            Assert.AreEqual(ShortCycleWorkOrderSafetyBriefController.NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.IsNull(resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        [TestMethod]
        public void CreateSendsNotificationHasPerformedInspectionOnVehicleIsFalse()
        {
            var scwosb = GetEntityFactory<ShortCycleWorkOrderSafetyBrief>().Create(new{HasCompletedDailyStretchingRoutine = true, HasPerformedInspectionOnVehicle = false, IsPPEInGoodCondition = true});
            var model = _viewModelFactory.Build<CreateShortCycleWorkOrderSafetyBrief, ShortCycleWorkOrderSafetyBrief>(scwosb);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, ((ShortCycleWorkOrderSafetyBriefNotificationViewModel)resultArgs.Data).Entity);
            Assert.AreEqual(_user.Employee.ReportsTo.EmailAddress, resultArgs.Address);
            Assert.AreEqual(0, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.OperationsHealthAndSafety, resultArgs.Module);
            Assert.AreEqual(ShortCycleWorkOrderSafetyBriefController.NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.IsNull(resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        [TestMethod]
        public void CreateSendsNotificationWhenIsPPEInGoodConditionIsFalse()
        {
            var scwosb = GetEntityFactory<ShortCycleWorkOrderSafetyBrief>().Create(new{HasCompletedDailyStretchingRoutine = true, HasPerformedInspectionOnVehicle = true, IsPPEInGoodCondition = false});
            var model = _viewModelFactory.Build<CreateShortCycleWorkOrderSafetyBrief, ShortCycleWorkOrderSafetyBrief>(scwosb);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, ((ShortCycleWorkOrderSafetyBriefNotificationViewModel)resultArgs.Data).Entity);
            Assert.AreEqual(_user.Employee.ReportsTo.EmailAddress, resultArgs.Address);
            Assert.AreEqual(0, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.OperationsHealthAndSafety, resultArgs.Module);
            Assert.AreEqual(ShortCycleWorkOrderSafetyBriefController.NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.IsNull(resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        [TestMethod]
        public void CreateDoesNotSendNotificationWhenAllPropertiesAreTrue()
        {
            var scwosb = GetEntityFactory<ShortCycleWorkOrderSafetyBrief>().Create(new{HasCompletedDailyStretchingRoutine = true, HasPerformedInspectionOnVehicle = true, IsPPEInGoodCondition = true});
            var model = _viewModelFactory.Build<CreateShortCycleWorkOrderSafetyBrief, ShortCycleWorkOrderSafetyBrief>(scwosb);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        #endregion
    }
}
