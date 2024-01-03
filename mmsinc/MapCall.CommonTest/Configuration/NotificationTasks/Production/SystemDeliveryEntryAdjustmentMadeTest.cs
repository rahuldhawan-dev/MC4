using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MapCall.Common.Configuration.NotificationTasks.Production;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;

namespace MapCall.CommonTest.Configuration.NotificationTasks.Production
{
    [TestClass]
    public class SystemDeliveryEntryAdjustmentMadeTest : InMemoryDatabaseTest<SystemDeliveryFacilityEntryAdjustment>
    {
        #region Private Members

        private Mock<INotifier> _notifier;
        private Mock<INotificationService> _notificationService;
        private Mock<ILog> _log;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private RepositoryBase<SystemDeliveryFacilityEntryAdjustment> _repository;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotifier>();
            _notificationService = new Mock<INotificationService>();
            _log = new Mock<ILog>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _repository = _container.GetInstance<RepositoryBase<SystemDeliveryFacilityEntryAdjustment>>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2021, 3, 1));

            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void TestGetDataReturnsSystemDeliveryFacilityEntryAdjustmentsWithLastUpdatedBetweenNowAndLastWeek()
        {
            var systemDeliveryFacilityEntryAdjustment = GetFactory<SystemDeliveryFacilityEntryAdjustmentFactory>().CreateList(10);
            systemDeliveryFacilityEntryAdjustment[0].DateTimeEntered = _dateTimeProvider.Object.GetCurrentDate().AddWeeks(-1);
            systemDeliveryFacilityEntryAdjustment[1].DateTimeEntered = _dateTimeProvider.Object.GetCurrentDate().AddDays(-1);
            systemDeliveryFacilityEntryAdjustment[2].DateTimeEntered = _dateTimeProvider.Object.GetCurrentDate().AddDays(-2);
            systemDeliveryFacilityEntryAdjustment[3].DateTimeEntered = _dateTimeProvider.Object.GetCurrentDate().AddDays(-3);

            _repository.Save(systemDeliveryFacilityEntryAdjustment[0]);
            _repository.Save(systemDeliveryFacilityEntryAdjustment[1]);
            _repository.Save(systemDeliveryFacilityEntryAdjustment[2]);
            _repository.Save(systemDeliveryFacilityEntryAdjustment[3]);

            var target = new SystemDeliveryEntryAdjustmentMade(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object);
            var result = target.GetData();

            Assert.AreEqual(4, result.Count());
            Assert.IsTrue(result.Any(x => x.Id == systemDeliveryFacilityEntryAdjustment[0].Id));
            Assert.IsTrue(result.Any(x => x.Id == systemDeliveryFacilityEntryAdjustment[1].Id));
            Assert.IsTrue(result.Any(x => x.Id == systemDeliveryFacilityEntryAdjustment[2].Id));
            Assert.IsTrue(result.Any(x => x.Id == systemDeliveryFacilityEntryAdjustment[3].Id));
        }

        [TestMethod]
        public void GetDataOnlyReturnsDataOnMonday()
        {
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2021, 3, 2));
            _container.Inject(_dateTimeProvider.Object);
            var systemDeliveryFacilityEntryAdjustments = GetFactory<SystemDeliveryFacilityEntryAdjustmentFactory>().CreateList(10);
            systemDeliveryFacilityEntryAdjustments[0].DateTimeEntered = _dateTimeProvider.Object.GetCurrentDate().AddWeeks(-1);
            systemDeliveryFacilityEntryAdjustments[1].DateTimeEntered = _dateTimeProvider.Object.GetCurrentDate().AddDays(-1);
            systemDeliveryFacilityEntryAdjustments[2].DateTimeEntered = _dateTimeProvider.Object.GetCurrentDate().AddDays(-2);
            systemDeliveryFacilityEntryAdjustments[3].DateTimeEntered = _dateTimeProvider.Object.GetCurrentDate().AddDays(-3);

            _repository.Save(systemDeliveryFacilityEntryAdjustments[0]);
            _repository.Save(systemDeliveryFacilityEntryAdjustments[1]);
            _repository.Save(systemDeliveryFacilityEntryAdjustments[2]);
            _repository.Save(systemDeliveryFacilityEntryAdjustments[3]);

            var target = new SystemDeliveryEntryAdjustmentMade(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object);
            var result = target.GetData();

            Assert.AreEqual(0, result.Count());

            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2021, 3, 1));
            _container.Inject(_dateTimeProvider.Object);
            target = new SystemDeliveryEntryAdjustmentMade(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object);
            result = target.GetData();

            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public void TestSendNotificationsSentNotificationsCorrectly()
        {
            var equipment = GetFactory<EquipmentFactory>().Create(new {OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create()});
            var employee = GetFactory<EmployeeFactory>().Create(new {EmailAddress = "lol@lol.com"});
            var systemDeliveryEntry = GetFactory<SystemDeliveryEntryFactory>().Create(new {EnteredBy = employee});
            var systemDeliveryFacilityEntry = GetFactory<SystemDeliveryFacilityEntryFactory>().Create();
            var systemDeliveryFacilityEntryAdjustment = GetFactory<SystemDeliveryFacilityEntryAdjustmentFactory>().Create(new {SystemDeliveryFacilityEntry = systemDeliveryFacilityEntry, SystemDeliveryEntry = systemDeliveryEntry});

            var target = new SystemDeliveryEntryAdjustmentMade(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object);
            target.SendNotification(systemDeliveryFacilityEntryAdjustment);

            _notificationService.Verify(x => x.Notify(systemDeliveryFacilityEntry.Facility.OperatingCenter.Id, SystemDeliveryEntryAdjustmentMade.MODULE, SystemDeliveryEntryAdjustmentMade.PURPOSE, It.IsAny<SystemDeliveryFacilityEntryAdjustment>(), null, null, null), Times.Once);
        }

        #endregion
    }
}
