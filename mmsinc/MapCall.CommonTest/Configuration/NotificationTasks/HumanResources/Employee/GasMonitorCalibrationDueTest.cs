using System.Linq;
using log4net;
using MapCall.Common.Configuration.NotificationTasks.Production;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;

namespace MapCall.CommonTest.Configuration.NotificationTasks.HumanResources.Employee
{
    [TestClass]
    public class GasMonitorCalibrationDueTest : InMemoryDatabaseTest<GasMonitor>
    {
        #region Private Members

        private Mock<INotifier> _notifier;
        private Mock<INotificationService> _notificationService;
        private Mock<ILog> _log;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private RepositoryBase<GasMonitor> _repository;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotifier>();
            _notificationService = new Mock<INotificationService>();
            _log = new Mock<ILog>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _repository = _container.GetInstance<RepositoryBase<GasMonitor>>();

            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSendNotificationCallsTheNotificationServiceProperlyWhenEmployeeAssignedHasAnEmailAddress()
        {
            var emailAddress = "foo@bar.baz";
            var employee = GetFactory<EmployeeFactory>().Create(new {EmailAddress = emailAddress});
            var equipment = GetFactory<GasMonitorEquipmentFactory>()
               .Create(new {OperatingCenter = GetFactory<OperatingCenterFactory>().Create()});
            var entity = GetFactory<GasMonitorFactory>()
               .Create(new {Equipment = equipment, AssignedEmployee = employee});
            var target = new GasMonitorCalibrationDue(_repository, _notifier.Object, _notificationService.Object,
                _log.Object, _dateTimeProvider.Object);

            // Act
            target.SendNotification(entity);

            // Assert
            _notifier.Verify(x => x.Notify(
                GasMonitorCalibrationDue.APPLICATION,
                GasMonitorCalibrationDue.MODULE,
                GasMonitorCalibrationDue.PURPOSE,
                It.Is<GasMonitor>(gm =>
                    gm.Id == entity.Id && gm.RecordUrl.EndsWith($"HealthAndSafety/GasMonitor/Show/{entity.Id}")),
                emailAddress,
                GasMonitorCalibrationDue.PURPOSE,
                null,
                null
            ), Times.Once);
        }

        [TestMethod]
        public void TestSendNotificationDoesNotCallTheNotificationServiceWhenEmployeeAssignedHasNoEmailAddress()
        {
            var employee = GetFactory<EmployeeFactory>().Create(new {EmailAddress = ""});
            var equipment = GetFactory<GasMonitorEquipmentFactory>()
               .Create(new {OperatingCenter = GetFactory<OperatingCenterFactory>().Create()});
            var entity = GetFactory<GasMonitorFactory>()
               .Create(new {Equipment = equipment, AssignedEmployee = employee});
            var target = new GasMonitorCalibrationDue(_repository, _notifier.Object, _notificationService.Object,
                _log.Object, _dateTimeProvider.Object);

            // Act
            target.SendNotification(entity);

            // Assert
            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        [TestMethod]
        public void TestSendNotificationDoesNotCallTheNotificationServiceIfTheEmployeeIsNotAssigned()
        {
            var equipment = GetFactory<GasMonitorEquipmentFactory>()
               .Create(new {OperatingCenter = GetFactory<OperatingCenterFactory>().Create()});
            var entity = GetFactory<GasMonitorFactory>().Create(new {Equipment = equipment});
            var target = new GasMonitorCalibrationDue(_repository, _notifier.Object, _notificationService.Object,
                _log.Object, _dateTimeProvider.Object);

            // Act
            target.SendNotification(entity);

            // Assert
            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        #endregion
    }
}
