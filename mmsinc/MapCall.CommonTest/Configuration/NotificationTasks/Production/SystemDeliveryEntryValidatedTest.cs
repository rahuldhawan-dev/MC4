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
    public class SystemDeliveryEntryValidatedTest : InMemoryDatabaseTest<SystemDeliveryEntry>
    {
        #region Private Members

        private Mock<INotifier> _notifier;
        private Mock<INotificationService> _notificationService;
        private Mock<ILog> _log;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private RepositoryBase<SystemDeliveryEntry> _repository;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotifier>();
            _notificationService = new Mock<INotificationService>();
            _log = new Mock<ILog>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _repository = _container.GetInstance<RepositoryBase<SystemDeliveryEntry>>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2021, 3, 1));

            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void TestGetDataReturnsSystemDeliveryEntriesWithCorrectWeekOfAndHaveBeenValidated()
        {
            var systemDeliveryEntries = GetFactory<SystemDeliveryEntryFactory>().CreateList(3);
            systemDeliveryEntries[0].IsValidated = true;
            systemDeliveryEntries[0].WeekOf = _dateTimeProvider.Object.GetCurrentDate().AddWeeks(-1);
            systemDeliveryEntries[1].IsValidated = true;
            systemDeliveryEntries[2].WeekOf = _dateTimeProvider.Object.GetCurrentDate();

            _repository.Save(systemDeliveryEntries[0]);
            _repository.Save(systemDeliveryEntries[1]);
            _repository.Save(systemDeliveryEntries[2]);

            var target = new SystemDeliveryEntryValidated(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object);

            var result = target.GetData();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(systemDeliveryEntries[0].Id, result.First().Id);
            Assert.IsTrue(result.First().IsValidated.Value);
        }

        [TestMethod]
        public void TestGetDataOnlyReturnsDataOnMonday()
        {
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2021, 3, 2));
            _container.Inject(_dateTimeProvider.Object);
            var systemDeliveryEntries = GetFactory<SystemDeliveryEntryFactory>().CreateList(3);
            systemDeliveryEntries[0].IsValidated = true;
            systemDeliveryEntries[0].WeekOf = _dateTimeProvider.Object.GetCurrentDate().AddWeeks(-1);
            systemDeliveryEntries[1].IsValidated = true;
            systemDeliveryEntries[2].WeekOf = _dateTimeProvider.Object.GetCurrentDate();

            _repository.Save(systemDeliveryEntries[0]);
            _repository.Save(systemDeliveryEntries[1]);
            _repository.Save(systemDeliveryEntries[2]);

            var target = new SystemDeliveryEntryValidated(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object);

            var result = target.GetData();

            Assert.AreEqual(0, result.Count());

            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2021, 3, 1));
            _container.Inject(_dateTimeProvider.Object);

            systemDeliveryEntries[0].IsValidated = true;
            systemDeliveryEntries[0].WeekOf = _dateTimeProvider.Object.GetCurrentDate().AddWeeks(-1);
            systemDeliveryEntries[1].IsValidated = true;
            systemDeliveryEntries[2].WeekOf = _dateTimeProvider.Object.GetCurrentDate();

            _repository.Save(systemDeliveryEntries[0]);
            _repository.Save(systemDeliveryEntries[1]);
            _repository.Save(systemDeliveryEntries[2]);

            target = new SystemDeliveryEntryValidated(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object);
            result = target.GetData();

            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public void TestSendNotificationNotifiesCorrectly()
        {
            var equipment = GetFactory<EquipmentFactory>().Create(new {OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create()});
            var equipment2 = GetFactory<EquipmentFactory>().Create(new {OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create()});
            var employee = GetFactory<EmployeeFactory>().Create(new {EmailAddress = "lolz@lololol.com"});
            var facility = GetEntityFactory<Facility>().Create(new{OperatingCenter = equipment.OperatingCenter});
            var facility2 = GetEntityFactory<Facility>().Create(new{OperatingCenter = equipment2.OperatingCenter});
            var systemDeliveryFacilityEntry = GetFactory<SystemDeliveryFacilityEntryFactory>().Create(new{Facility = facility});
            var systemDeliveryFacilityEntry2 = GetFactory<SystemDeliveryFacilityEntryFactory>().Create(new{Facility = facility2});
            var systemDeliveryEntry = GetFactory<SystemDeliveryEntryFactory>().Create(new {EnteredBy = employee});
            systemDeliveryEntry.FacilityEntries.Add(systemDeliveryFacilityEntry);
            systemDeliveryEntry.FacilityEntries.Add(systemDeliveryFacilityEntry2);

            var target = new SystemDeliveryEntryValidated(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object);

            target.SendNotification(systemDeliveryEntry);

            _notificationService.Verify(x => x.Notify(systemDeliveryFacilityEntry.Facility.OperatingCenter.Id, SystemDeliveryEntryValidated.MODULE, SystemDeliveryEntryValidated.PURPOSE, It.IsAny<SystemDeliveryEntryNotification>(), null, null, null), Times.Once);
            _notificationService.Verify(x => x.Notify(systemDeliveryFacilityEntry2.Facility.OperatingCenter.Id, SystemDeliveryEntryValidated.MODULE, SystemDeliveryEntryValidated.PURPOSE, It.IsAny<SystemDeliveryEntryNotification>(), null, null, null), Times.Once);
        }

        #endregion
    }
}
