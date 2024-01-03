using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using MapCallScheduler.Jobs;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using Quartz;
using Quartz.Impl.AdoJobStore;
using StructureMap;
using MMSINC.ClassExtensions.MemberInfoExtensions;

namespace MapCallScheduler.Tests
{
    [TestClass]
    public class MapCallSchedulerServiceTest
    {
        #region Private Members

        private MapCallSchedulerService _target;
        private Mock<IScheduler> _scheduler;
        private Mock<IMapCallSchedulerJobService> _jobService;
        private Mock<ITrigger> _trigger;
        private Mock<ILog> _log;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_scheduler = new Mock<IScheduler>()).Object);
            _container.Inject((_jobService = new Mock<IMapCallSchedulerJobService>()).Object);
            _container.Inject((_log = new Mock<ILog>()).Object);

            _jobService.Setup(x => x.GetAllJobs()).Returns(new[] {typeof(MapCallJobBase)});

            _target = _container.GetInstance<MapCallSchedulerService>();

            _trigger = new Mock<ITrigger>();
            _jobService
                .Setup(x => x.BuildTrigger(typeof(MapCallJobBase), "MapCallTriggerBase", "MapCallGroupBase"))
                .Returns(_trigger.Object);
        }

        #endregion

        #region Start()

        [TestMethod]
        public void TestStartAddsJobTriggerKeyToCollection()
        {
            MyAssert.CausesIncrease(() => _target.Start(), () => _target.TriggerKeys.Count);
        }

        [TestMethod]
        public void TestStartSchedulesJob()
        {
            var jobDetail = new Mock<IJobDetail>();
            _jobService
                .Setup(x => x.Build(typeof(MapCallJobBase), "MapCallJobBase", "MapCallGroupBase"))
                .Returns(jobDetail.Object);
            var jobKey = new JobKey("Foo");
            jobDetail.Setup(x => x.Key).Returns(jobKey);
            _target.Start();

            _scheduler.Verify(x => x.ScheduleJob(jobDetail.Object, _trigger.Object, CancellationToken.None));
            _log.Verify(x => x.Info(It.Is<string>(y => y == "Job: MapCallJobBase - Key: DEFAULT.Foo Scheduled, Description:  JobTye:  Disallowed: False durable: False startat: 1/1/0001 12:00:00 AM +00:00 0")));
            _scheduler.Verify(x => x.TriggerJob(It.IsAny<JobKey>(), CancellationToken.None), Times.Never);

        }

        [TestMethod]
        public void TestStartWithArgumentSchedulesSingleJob()
        {
            var jobDetail = new Mock<IJobDetail>();
            var trigger = new Mock<ITrigger>();
            _jobService
                .Setup(x => x.Build(typeof(HeartbeatJob), "HeartbeatJob", "HeartbeatGroup"))
                .Returns(jobDetail.Object);
            _jobService
                .Setup(x => x.BuildTrigger(typeof(HeartbeatJob), "HeartbeatTrigger", "HeartbeatGroup"))
                .Returns(trigger.Object);

            _target.Start(typeof(HeartbeatJob));

            _scheduler.Verify(x => x.ScheduleJob(jobDetail.Object, trigger.Object, CancellationToken.None));
        }

        [TestMethod]
        public void TestStartStartsScheduler()
        {
            _target.Start();

            _scheduler.Verify(x => x.Start(CancellationToken.None));
        }

        [TestMethod]
        public void TestStartSchedulesJobAndTriggersImmediately()
        {
            var jobDetail = new Mock<IJobDetail>();
            _jobService
                .Setup(x => x.Build(typeof(MapCallJobBase), "MapCallJobBase", "MapCallGroupBase"))
                .Returns(jobDetail.Object);
            var jobKey = new JobKey("Foo");
            jobDetail.Setup(x => x.Key).Returns(jobKey);
            jobDetail.Setup(x => x.JobType).Returns(jobDetail.GetType());
            _jobService.Setup(x => x.HasImmediateAttribute(It.IsAny<Type>())).Returns(true);
            _target.Start();

            _scheduler.Verify(x => x.ScheduleJob(jobDetail.Object, _trigger.Object, CancellationToken.None));
            _log.Verify(x => x.Info(It.Is<string>(y => y == "Job: MapCallJobBase - Key: DEFAULT.Foo Scheduled, Description:  JobTye: Moq.Mock`1[Quartz.IJobDetail] Disallowed: False durable: False startat: 1/1/0001 12:00:00 AM +00:00 0")));
            _scheduler.Verify(x => x.TriggerJob(It.IsAny<JobKey>(), CancellationToken.None));
        }

        #endregion

        #region Stop()

        [TestMethod]
        public void TestStopClearsTriggerKeys()
        {
            _target.Start();

            MyAssert.CausesDecrease(() => _target.Stop(), () => _target.TriggerKeys.Count);
        }

        [TestMethod]
        public void TestStopShutsDownScheduler()
        {
            _target.Start();

            _target.Stop();

            _scheduler.Verify(x => x.Shutdown(CancellationToken.None));
        }

        [TestMethod]
        public void TestStopUnschedulesAllJobs()
        {
            _target.Start();

            _target.Stop();

            _scheduler.Verify(x => x.UnscheduleJobs(new ReadOnlyCollection<TriggerKey>(_target.TriggerKeys), CancellationToken.None));
        }

        #endregion
    }
}
