using System;
using System.Linq;
using log4net;
using MapCall.Common.Configuration.NotificationTasks.HealthandSafety;
using MapCall.Common.Configuration.NotificationTasks.Production;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;

namespace MapCall.CommonTest.Configuration.NotificationTasks.HealthandSafety
{
    [TestClass]
    public class EnvironmentalNearMissNotificationTest : InMemoryDatabaseTest<NearMiss>
    {
        #region Private Members

        private Mock<INotifier> _notifier;
        private Mock<INotificationService> _notificationService;
        private Mock<ILog> _log;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private RepositoryBase<NearMiss> _repository;
        private EnvironmentalNearMiss _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotifier>();
            _notificationService = new Mock<INotificationService>();
            _log = new Mock<ILog>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);
            _repository = _container.GetInstance<RepositoryBase<NearMiss>>();
            _container.Inject(_dateTimeProvider.Object);
            // These are needed due to property injection on Service, but they
            // aren't actually used by the test.
            _container.Inject(new Mock<IServiceRepository>().Object);
            _container.Inject(new Mock<ITapImageRepository>().Object);
            _target = new EnvironmentalNearMiss(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider.Object);
        }

        #endregion

        #region Tests
        [TestMethod]
        public void TestGetDataReturnsNotificationBetweenNowAndLastOneDay()
        {
            var envType = GetFactory<EnvironmentalNearMissTypeFactory>().Create();
            var safetyType = GetFactory<SafetyNearMissTypeFactory>().Create();
            var envNearMiss1 = GetFactory<NearMissFactory>().Create(new { Type = envType });
            var envNearMiss2 = GetFactory<NearMissFactory>().Create(new { Type = envType });
            var safetyNearMiss1 = GetFactory<NearMissFactory>().Create(new { Type = safetyType});

            _repository.Save(envNearMiss1);
            _repository.Save(envNearMiss2);
            _repository.Save(safetyNearMiss1);
            var result = _target.GetData();
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void TestSendNotificationForNearMissOnCreate()
        {
            var opcPrime = GetFactory<UniqueOperatingCenterFactory>().Create();
            var nearMiss = GetFactory<NearMissFactory>().Create(new { OperatingCenter = opcPrime });
            _notificationService.Setup(x => x.Notify(opcPrime.Id, EnvironmentalNearMiss.MODULE,
                SafetyNearMiss.PURPOSE, nearMiss, EnvironmentalNearMiss.PURPOSE, null,null));
            // Act
            _target.SendNotification(nearMiss);
            _notificationService.Verify(x => x.Notify(opcPrime.Id, EnvironmentalNearMiss.MODULE, EnvironmentalNearMiss.PURPOSE, nearMiss, EnvironmentalNearMiss.PURPOSE, null, null), Times.Once);
        }

        #endregion
    }
}
