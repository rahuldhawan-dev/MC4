using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class NotificationControllerTest : MapCallMvcControllerTestBase<NotificationController, Notification>
    {
        #region Fields

        private Mock<INotificationService> _notifier;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotificationService>();
            _container.Inject<IRepository<TrainingRecord>>(_container.GetInstance<RepositoryBase<TrainingRecord>>());
            _container.Inject(_notifier.Object);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // The Create action does not use perform the usual database actions.
            // It just sends notifications.
            options.CreateRedirectsToReferrerOnSuccess = true;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Notification/Create/");
            });
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // noop. this action doesn't save anything.
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // noop. no validation checks are done.
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // noop override because auto test data doesn't work with this for some reason.
            // also tested below
        }

        [TestMethod]
        public void TestCreateSendsNotificationRedirectsAndDisplaysSuccessMessage()
        {
            var trainingRecord = GetEntityFactory<TrainingModule>().Create();

            var notification = new Notification {
                Id = trainingRecord.Id,
                RoleModule = RoleModules.OperationsTrainingRecords,
                NotificationPurpose = "Foo", 
                OperatingCenterId = 0,
                EntityType = typeof(TrainingModule).AssemblyQualifiedName
            };

            var args = new NotifierArgs() {
                OperatingCenterId = 0,
                Module = notification.RoleModule,
                Purpose = notification.NotificationPurpose,
                Data = trainingRecord
            };

            _notifier.Setup(x => x.Notify(args));

            var result = _target.Create(_viewModelFactory.Build<CreateNotification, Notification>(notification)) as RedirectToRouteResult;

            _target.AssertTempDataContainsMessage(NotificationController.SUCCESS_MESSAGE,
                MMSINC.Controllers.ControllerBase.SUCCESS_MESSAGE_KEY);
            Assert.AreEqual(result.RouteValues["Controller"], "Home");
            Assert.AreEqual(result.RouteValues["Action"], "Index");
        }

        [TestMethod]
        public void TestCreateSendsNotificationRedirectsAndDisplaysSuccessMessageForAModelWithoutRecordUrl()
        {
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => null);

            var trainingModule = GetEntityFactory<TrainingModule>().Create();

            var notification = new Notification
            {
                Id = trainingModule.Id,
                RoleModule = RoleModules.OperationsTrainingRecords,
                NotificationPurpose = "Foo",
                OperatingCenterId = 0,
                EntityType = typeof(TrainingModule).AssemblyQualifiedName
            };

            var args = new NotifierArgs()
            {
                OperatingCenterId = 0,
                Module = notification.RoleModule,
                Purpose = notification.NotificationPurpose,
                Data = trainingModule
            };

            _notifier.Setup(x => x.Notify(args));

            var result = _target.Create(_viewModelFactory.Build<CreateNotification, Notification>(notification)) as RedirectToRouteResult;

            _target.AssertTempDataContainsMessage(NotificationController.SUCCESS_MESSAGE,
                MMSINC.Controllers.ControllerBase.SUCCESS_MESSAGE_KEY);

            Assert.AreEqual(result.RouteValues["Controller"], "Home");
            Assert.AreEqual(result.RouteValues["Action"], "Index");
        }

        [TestMethod]
        public void TestCreateRedirectsToRefererrerWithFragment()
        {
            var url = "http://somesite.com";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));

            var trainingRecord = GetEntityFactory<TrainingRecord>().Create();

            var notification = new Notification
            {
                Id = trainingRecord.Id,
                RoleModule = RoleModules.OperationsTrainingRecords,
                NotificationPurpose = "Foo",
                OperatingCenterId = 0,
                EntityType = typeof(TrainingRecord).AssemblyQualifiedName
            };

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>()));

            var result = _target.Create(_viewModelFactory.Build<CreateNotification, Notification>(notification)) as RedirectResult;

            Assert.AreEqual(url + NotificationController.FRAGMENT_IDENTIFIER, result.Url);
        }

        [TestMethod]
        public void TestCreateSendsSpecialNotificationForTrainingRecords()
        {
            var url = "http://somesite.com";
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => new Uri(url));

            var trainingRecord = GetEntityFactory<TrainingRecord>().Create();
            trainingRecord.ClassLocation = new ClassLocation {Name = "Some training class location", OperatingCenter = GetEntityFactory<OperatingCenter>().Create(new { OperatingCenterCode = "QQ1"}) };
            var employee = new Employee {EmailAddress = "some@emailthing.com"};
            var schedEmployee = new TrainingRecordScheduledEmployee {Employee = employee};
            trainingRecord.EmployeesScheduled.Add(schedEmployee);
            var sess = new TrainingSession();
            trainingRecord.TrainingSessions.Add(sess);

            var notification = new Notification
            {
                Id = trainingRecord.Id,
                RoleModule = RoleModules.OperationsTrainingRecords,
                NotificationPurpose = "Foo",
                OperatingCenterId = 0,
                EntityType = typeof(TrainingRecord).AssemblyQualifiedName
            };

            NotifierArgs args = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback((NotifierArgs e) => args = e);

            _target.Create(_viewModelFactory.Build<CreateNotification, Notification>(notification));

            var calendarAttachment = args.Attachments.Single();
            var calendar = System.Text.Encoding.UTF8.GetString(calendarAttachment.BinaryData);

            Assert.IsTrue(calendar.Contains("LOCATION:QQ1 - Some training class location"));
        }
    }
}
