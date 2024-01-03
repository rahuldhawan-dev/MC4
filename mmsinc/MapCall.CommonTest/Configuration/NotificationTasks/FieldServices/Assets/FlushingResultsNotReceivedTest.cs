using System;
using System.Linq;
using log4net;
using MapCall.Common.Configuration.NotificationTasks.FieldServices.Assets;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using NHibernate.Linq;

namespace MapCall.CommonTest.Configuration.NotificationTasks.FieldServices.Assets
{
    [TestClass]
    public class FlushingResultsNotReceivedTest : InMemoryDatabaseTest<ServiceFlush, ServiceFlushRepository>
    {
        #region Private Members

        private Mock<INotifier> _notifier;
        private Mock<INotificationService> _notificationService;
        private Mock<ILog> _log;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private FlushingResultsNotReceived _target;

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

            _container.Inject(_dateTimeProvider.Object);

            // These are needed due to property injection on Service, but they
            // aren't actually used by the test.
            _container.Inject(new Mock<IServiceRepository>().Object);
            _container.Inject(new Mock<ITapImageRepository>().Object);

            _target = new FlushingResultsNotReceived(Repository, _notifier.Object, _notificationService.Object, _log.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetDataReturnsExpectedResults()
        {
            // I'm not gonna try to list all of the rules in the test name, it'll scroll for days.
            // This method should return results that meet all of the following criteria:
            //  - The sample status is not "Results Received"
            //  - The sample date is > 14 days old
            //  - The HasSentNotification property is false

            // The repo should convert this date to "today"(midnight), that's why there is random hour/min/sec here.
            var today = new DateTime(2018, 5, 15, 12, 4, 21);
            var twoWeeksAgo = today.Date.AddDays(-14);

            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(today);

            var receivedStatus = GetFactory<ResultsReceivedServiceFlushSampleStatusFactory>().Create();
            var otherStatus = GetFactory<TakenServiceFlushSampleStatusFactory>().Create();

            var flush = GetFactory<ServiceFlushFactory>().Create();
            flush.HasSentNotification = false;
            flush.SampleStatus = otherStatus;
            flush.SampleDate = twoWeeksAgo;

            Assert.AreSame(flush, _target.GetData().Single());

            // All variations of this should return no results.
            flush.HasSentNotification = true;
            Repository.Save(flush);
            Assert.IsFalse(_target.GetData().Any());

            flush.HasSentNotification = false;
            flush.SampleDate = twoWeeksAgo.AddDays(1);
            Repository.Save(flush);
            Assert.IsFalse(_target.GetData().Any());

            flush.SampleDate = twoWeeksAgo;
            flush.SampleStatus = receivedStatus;
            Repository.Save(flush);
            Assert.IsFalse(_target.GetData().Any());
        }

        [TestMethod]
        public void TestSendNotificationSetsHasSentNotificationToTrueAndSavesIt()
        {
            var flush = GetFactory<ServiceFlushFactory>().Create();
            Assert.IsFalse(flush.HasSentNotification, "Sanity, should be false by default.");

            _target.SendNotification(flush);

            // The task calls Repository.Save which should be doing a save and flush.
            // We need to evict and requery to ensure the updated HasSentNotification value
            // was persisted.
            Session.Evict(flush);

            flush = Session.Query<ServiceFlush>().Single(x => x.Id == flush.Id);
            Assert.IsTrue(flush.HasSentNotification);
        }

        [TestMethod]
        public void TestSendNotificationSendsNotification()
        {
            NotifierArgs resultArgs = null;
            _notificationService.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback((NotifierArgs x) => {
                resultArgs = x;
            });
            var flush = GetFactory<ServiceFlushFactory>().Create();

            _target.SendNotification(flush);

            Assert.AreEqual(resultArgs.OperatingCenterId, flush.Service.OperatingCenter.Id);
            Assert.AreEqual(resultArgs.Module, RoleModules.FieldServicesAssets);
            Assert.AreEqual(resultArgs.Purpose, "Flushing Results Not Received");
            Assert.AreSame(resultArgs.Data, flush);
            Assert.AreEqual(resultArgs.Subject, "Flushing Results Not Received");
        }

        #endregion
    }
}
