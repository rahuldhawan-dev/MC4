using System;
using System.Linq;
using log4net;
using MapCall.Common.Configuration.NotificationTasks.HumanResources.Environmental;
using MapCall.Common.Configuration.NotificationTasks.HumanResources.Facilities;
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
using StructureMap;

namespace MapCall.CommonTest.Configuration.NotificationTasks.Facilities
{
    [TestClass]
    public class ArcFlashStudyExpiresIn1YearTest : InMemoryDatabaseTest<ArcFlashStudy>
    {
        #region Private Members

        private Mock<INotifier> _notifier;
        private Mock<INotificationService> _notificationService;
        private Mock<ILog> _log;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private RepositoryBase<ArcFlashStudy> _repository;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            //_repository = new Mock<IRepository<EnvironmentalPermit>>();
            _notifier = new Mock<INotifier>();
            _notificationService = new Mock<INotificationService>();
            _log = new Mock<ILog>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _repository = _container.GetInstance<RepositoryBase<ArcFlashStudy>>();

            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestReturnsArcFlashStudiesThatWillExpireIn1Year()
        {
            var date = DateTime.Today;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);

            var arcFlashStudiesList = GetFactory<ArcFlashStudyFactory>().CreateList(5);
            arcFlashStudiesList[0].DateLabelsApplied = date.AddYears(-4);

            _repository.Save(arcFlashStudiesList[0]);

            var target = new ArcFlashStudyExpiresIn1Year(_repository, _notifier.Object, _notificationService.Object,
                _dateTimeProvider.Object, _log.Object);

            var results = target.GetData();

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public void TestDoesNotReturnArcFlashStudiesThatHaveAlreadyExpired()
        {
            var date = DateTime.Today;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);

            var arcFlashStudiesList = GetFactory<ArcFlashStudyFactory>().CreateList(5);
            arcFlashStudiesList[0].DateLabelsApplied = date.AddYears(-5);

            _repository.Save(arcFlashStudiesList[0]);

            var target = new ArcFlashStudyExpiresIn1Year(_repository, _notifier.Object, _notificationService.Object,
                _dateTimeProvider.Object, _log.Object);

            var results = target.GetData();

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestDoesNotReturnArcFlashStudiesThatWillExpireInMoreThen1Year()
        {
            var date = DateTime.Today;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(date);

            var arcFlashStudiesList = GetFactory<ArcFlashStudyFactory>().CreateList(5);
            arcFlashStudiesList[0].DateLabelsApplied = date.AddYears(-2);

            _repository.Save(arcFlashStudiesList[0]);

            var target = new ArcFlashStudyExpiresIn1Year(_repository, _notifier.Object, _notificationService.Object,
                _dateTimeProvider.Object, _log.Object);

            var results = target.GetData();

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }

        #endregion
    }
}
