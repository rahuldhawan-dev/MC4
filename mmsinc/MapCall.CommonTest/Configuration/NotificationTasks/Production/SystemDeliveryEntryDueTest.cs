using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MapCall.Common.Configuration.NotificationTasks.Production;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Configuration.NotificationTasks.Production
{
    [TestClass]
    public class SystemDeliveryEntryDueTest : InMemoryDatabaseTest<Facility>
    {
        #region Private Members

        private Mock<INotifier> _notifier;
        private Mock<INotificationService> _notificationService;
        private Mock<ILog> _log;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private RepositoryBase<Facility> _repository;
        private RepositoryBase<SystemDeliveryEntry> _systemDeliveryEntryRepo;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotifier>();
            _notificationService = new Mock<INotificationService>();
            _log = new Mock<ILog>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _repository = _container.GetInstance<RepositoryBase<Facility>>();
            _systemDeliveryEntryRepo = _container.GetInstance<RepositoryBase<SystemDeliveryEntry>>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2021, 3, 1));

            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetDataPullsAllFacilitiesWhereSystemDeliveryEntryIsConfiguredAndIsActive()
        {
            var facility = GetFactory<FacilityFactory>().Create();
            var facility2 = GetFactory<FacilityFactory>().Create();
            var facility3 = GetFactory<FacilityFactory>().Create();
            var facilitySystemDeliveryConfiguration = GetFactory<FacilitySystemDeliveryEntryTypeFactory>().Create(new{IsEnabled = true, Facility = facility});
            var facilitySystemDeliveryConfiguration2 = GetFactory<FacilitySystemDeliveryEntryTypeFactory>().Create(new{IsEnabled = false, Facility = facility2});

            facility.FacilitySystemDeliveryEntryTypes.Add(facilitySystemDeliveryConfiguration);
            facility2.FacilitySystemDeliveryEntryTypes.Add(facilitySystemDeliveryConfiguration2);

            _repository.Save(facility);
            _repository.Save(facility2);
            _repository.Save(facility3);

            var target = new SystemDeliveryEntryDue(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object, _container);

            var result = target.GetData();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(x => x.Id == facility.Id));
            Assert.IsFalse(result.Any(x => x.Id == facility2.Id));
            Assert.IsFalse(result.Any(x => x.Id == facility3.Id));
        }

        [TestMethod]
        public void TestGetDataOnlyReturnsResultsOnMonday()
        {
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2021, 3, 2));
            _container.Inject(_dateTimeProvider.Object);
            var facility = GetFactory<FacilityFactory>().Create();
            var facility2 = GetFactory<FacilityFactory>().Create();
            var facility3 = GetFactory<FacilityFactory>().Create();
            var facilitySystemDeliveryConfiguration = GetFactory<FacilitySystemDeliveryEntryTypeFactory>().Create(new{IsEnabled = true, Facility = facility});
            var facilitySystemDeliveryConfiguration2 = GetFactory<FacilitySystemDeliveryEntryTypeFactory>().Create(new{IsEnabled = false, Facility = facility2});

            facility.FacilitySystemDeliveryEntryTypes.Add(facilitySystemDeliveryConfiguration);
            facility2.FacilitySystemDeliveryEntryTypes.Add(facilitySystemDeliveryConfiguration2);

            _repository.Save(facility);
            _repository.Save(facility2);
            _repository.Save(facility3);

            var target = new SystemDeliveryEntryDue(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object, _container);

            var result = target.GetData();

            Assert.AreEqual(0, result.Count());

            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(new DateTime(2021, 3, 1));
            _container.Inject(_dateTimeProvider.Object);

            target = new SystemDeliveryEntryDue(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object, _container);
            result = target.GetData();

            Assert.AreNotEqual(0, result.Count());
        }

        [TestMethod]
        public void TestSendNotificationOnlySendsNotificationForFacilityWithMissingSystemDeliveryEntryForLastWeek()
        {
            var lastMonday = _dateTimeProvider.Object.GetCurrentDate().AddWeeks(-1);
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var equipmentType = GetFactory<EquipmentTypeFlowMeterFactory>().Create();
            var equipmentStatus = GetFactory<InServiceEquipmentStatusFactory>().Create();
            var equipmentSubcategories = GetFactory<EquipmentSubCategoryFactory>().CreateList(40);
            var equipmentPurpose = GetFactory<EquipmentPurposeFactory>().Create(new {EquipmentSubCategory = equipmentSubcategories[36]});
            var facility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = operatingCenter});
            var equipment = GetFactory<EquipmentFactory>().Create(new {Facility = facility, EquipmentType = equipmentType, EquipmentStatus = equipmentStatus, EquipmentPurpose = equipmentPurpose});
            var facility2 = GetFactory<FacilityFactory>().Create();
            var systemDeliveryEntry = GetFactory<SystemDeliveryEntryFactory>().Create(new {WeekOf = _dateTimeProvider.Object.GetCurrentDate()});
            var systemDeliveryEntry2 = GetFactory<SystemDeliveryEntryFactory>().Create(new {WeekOf = lastMonday});
            facility.Equipment.Add(equipment);
            systemDeliveryEntry.Facilities.Add(facility);
            systemDeliveryEntry2.Facilities.Add(facility2);
            _repository.Save(facility);
            _systemDeliveryEntryRepo.Save(systemDeliveryEntry);
            _systemDeliveryEntryRepo.Save(systemDeliveryEntry2);

            var target = new SystemDeliveryEntryDue(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object, _container);

            target.SendNotification(facility);

            _notificationService.Verify(x => x.Notify(facility.OperatingCenter.Id, SystemDeliveryEntryDue.MODULE, SystemDeliveryEntryDue.PURPOSE, It.IsAny<SystemDeliveryEntryDueNotification>(), null, null, null), Times.Once);

            target.SendNotification(facility2);

            _notificationService.Verify(x => x.Notify(facility2.OperatingCenter.Id, SystemDeliveryEntryDue.MODULE, SystemDeliveryEntryDue.PURPOSE, It.IsAny<SystemDeliveryEntryDueNotification>(), null, null, null), Times.Never);
        }

        [TestMethod]
        public void TestSendNotificationDoesNotThrowANullRefExceptionWhenEquipmentSubCategoryIsMissing()
        {
            var lastMonday = _dateTimeProvider.Object.GetCurrentDate().AddWeeks(-1);
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var equipmentType = GetFactory<EquipmentTypeFlowMeterFactory>().Create();
            var equipmentStatus = GetFactory<InServiceEquipmentStatusFactory>().Create();
            var equipmentSubcategories = GetFactory<EquipmentSubCategoryFactory>().CreateList(40);
            var equipmentPurpose = GetFactory<EquipmentPurposeFactory>().Create(new {EquipmentSubCategory = equipmentSubcategories[36]});
            var equipmentPurpose2 = GetFactory<EquipmentPurposeFactory>().Create();
            equipmentPurpose2.EquipmentSubCategory = null;
            Session.SaveOrUpdate(equipmentPurpose2);
            var facility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = operatingCenter});
            var equipment = GetFactory<EquipmentFactory>().Create(new {Facility = facility, EquipmentType = equipmentType, EquipmentStatus = equipmentStatus, EquipmentPurpose = equipmentPurpose});
            var equipment2 = GetFactory<EquipmentFactory>().Create(new {Facility = facility, EquipmentType = equipmentType, EquipmentStatus = equipmentStatus, EquipmentPurpose = equipmentPurpose2});
            var facility2 = GetFactory<FacilityFactory>().Create();
            var systemDeliveryEntry = GetFactory<SystemDeliveryEntryFactory>().Create(new {WeekOf = _dateTimeProvider.Object.GetCurrentDate()});
            var systemDeliveryEntry2 = GetFactory<SystemDeliveryEntryFactory>().Create(new {WeekOf = lastMonday});
            facility.Equipment.Add(equipment);
            facility.Equipment.Add(equipment2);
            systemDeliveryEntry.Facilities.Add(facility);
            systemDeliveryEntry2.Facilities.Add(facility2);
            _repository.Save(facility);
            _systemDeliveryEntryRepo.Save(systemDeliveryEntry);
            _systemDeliveryEntryRepo.Save(systemDeliveryEntry2);

            var target = new SystemDeliveryEntryDue(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object, _container);

            target.SendNotification(facility);

            _notificationService.Verify(x => x.Notify(facility.OperatingCenter.Id, SystemDeliveryEntryDue.MODULE, SystemDeliveryEntryDue.PURPOSE, It.IsAny<SystemDeliveryEntryDueNotification>(), null, null, null), Times.Once);

            target.SendNotification(facility2);

            _notificationService.Verify(x => x.Notify(facility2.OperatingCenter.Id, SystemDeliveryEntryDue.MODULE, SystemDeliveryEntryDue.PURPOSE, It.IsAny<SystemDeliveryEntryDueNotification>(), null, null, null), Times.Never);
        }

        #endregion
    }
}
