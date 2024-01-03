using MapCallScheduler.Jobs;
using MapCallScheduler.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using Quartz;
using Quartz.Impl.Triggers;
using StructureMap;
using System;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MapCallScheduler.Jobs.LIMSSynchronization;

namespace MapCallScheduler.Tests
{
    [TestClass]
    public class MapCallSchedulerJobServiceTest
    {
        #region Private Members

        private MapCallSchedulerJobService _target;
        private Mock<IMapCallSchedulerDateService> _dateService;
        private IDateTimeProvider _dateTimeProvider;
        private IContainer _container;
        private DateTime _now, _then;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _now = DateTime.Now;
            _then = _now.AddYears(1);
            _container.Inject(_dateTimeProvider = new TestDateTimeProvider(_now));
            _container.Inject((_dateService = new Mock<IMapCallSchedulerDateService>()).Object);
            _dateService.Setup(x => x.GetStartDateTime()).Returns(_then);

            _target = _container.GetInstance<MapCallSchedulerJobService>();
        }

        #endregion

        #region Nested Type: JobWithDailyAttributeNoArgs

        [Daily]
        private class JobWithDailyAttributeNoArgs : TestJobBase {}

        #endregion

        #region Nested Type: JobWithDailyAttributeOneArg

        [Daily(3)]
        private class JobWithDailyAttributeOneArg :  TestJobBase {}

        #endregion

        #region Nested Type: JobWithHourlyAttributeNoArgs

        [Hourly]
        private class JobWithHourlyAttributeNoArgs : TestJobBase {}

        #endregion

        #region Nested Type: JobWithHourlyAttributeOneArg

        [Hourly(3)]
        private class JobWithHourlyAttributeOneArg : TestJobBase {}

        #endregion

        #region Nested Type: JobWithImmediateAttribute

        [Immediate]
        private class JobWithImmediateAttribute : TestJobBase {}

        #endregion

        #region Nested Type: JobWithStartAtAttribute

        [StartAt(START_HOUR)]
        private class JobWithStartAtAttribute : TestJobBase
        {
            public const int START_HOUR = 4;
        }

        #endregion

        #region Nested Type: JobWithMinutelyAttributeNoArgs

        [Minutely]
        private class JobWithMinutelyAttributeNoArgs : TestJobBase {}

        #endregion

        #region Nested Type: JobWithMinutelyAttributeOneArg

        [Minutely(3)]
        private class JobWithMinutelyAttributeOneArg :  TestJobBase {}

        #endregion

        #region Nested Type: JobWithNoAttributes

        private class JobWithNoAttributes : TestJobBase {}

        #endregion

        #region Nested Type: JobWithSecondlyAttributeNoArgs

        [Secondly]
        private class JobWithSecondlyAttributeNoArgs : TestJobBase {}

        #endregion

        #region Nested Type: JobWithSecondlyAttributeOneArg

        [Secondly(3)]
        private class JobWithSecondlyAttributeOneArg :  TestJobBase {}

        #endregion

        #region Nested Type: TestJobBase

        // just to get rid of all the boilerplate nonsense
        private class TestJobBase : MapCallJobBase
        {
            #region Constructors

            protected TestJobBase() : base(null, null) {}

            #endregion

            #region Private Methods

            protected override void ExecuteJob(IJobExecutionContext context)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        #endregion

        #region Private Methods

        private void TestBuildTrigger<TJob>(Action<SimpleTriggerImpl> test)
            where TJob : MapCallJobBase
        {
            var name = "foo";
            var group = "bar";

            var result = _target.BuildTrigger(typeof(TJob), name, group);

            Assert.IsTrue(result.ToString().StartsWith("Trigger 'bar.foo':"));
            test((SimpleTriggerImpl)result);
        }

        #endregion

        [TestMethod]
        public void TestBuild()
        {
            var name = "foo";
            var group = "bar";

            var result = _target.Build(typeof(MapCallJobBase), name, group);

            Assert.AreSame(typeof(MapCallJobBase), result.JobType);
            Assert.IsTrue(result.ToString().StartsWith("JobDetail 'bar.foo':"));
        }

        [TestMethod]
        public void TestBuildTriggerForJobWithDailyAttributeNoArgs()
        {
            TestBuildTrigger<JobWithDailyAttributeNoArgs>(t => {
                Assert.AreEqual(new TimeSpan(1, 0, 0, 0), t.RepeatInterval); // once per day
                Assert.AreEqual(-1, t.RepeatCount); // infinite
                Assert.AreEqual(_then, t.StartTimeUtc); // when the date service says to
            });
        }

        [TestMethod]
        public void TestBuildTriggerForJobWithDailyAttributeOneArg()
        {
            TestBuildTrigger<JobWithDailyAttributeOneArg>(t => {
                Assert.AreEqual(new TimeSpan(3, 0, 0, 0), t.RepeatInterval); // once every 3 days
                Assert.AreEqual(-1, t.RepeatCount); // infinite
                Assert.AreEqual(_then, t.StartTimeUtc); // when the date service says to
            });
        }

        [TestMethod]
        public void TestBuildTriggerForJobWithHourlyAttributeNoArgs()
        {
            TestBuildTrigger<JobWithHourlyAttributeNoArgs>(t => {
                Assert.AreEqual(new TimeSpan(0, 1, 0, 0), t.RepeatInterval); // once per hour
                Assert.AreEqual(-1, t.RepeatCount); // infinite
                Assert.AreEqual(_then, t.StartTimeUtc); // when the date service says to
            });
        }

        [TestMethod]
        public void TestBuildTriggerForJobWithHourlyAttributeOneArg()
        {
            TestBuildTrigger<JobWithHourlyAttributeOneArg>(t => {
                Assert.AreEqual(new TimeSpan(0, 3, 0, 0), t.RepeatInterval); // once every 3 hours
                Assert.AreEqual(-1, t.RepeatCount); // infinite
                Assert.AreEqual(_then, t.StartTimeUtc); // when the date service says to
            });
        }

        [TestMethod]
        public void TestBuildTriggerForJobWithImmediateAttribute()
        {
            TestBuildTrigger<JobWithImmediateAttribute>(t => {
                Assert.AreEqual(new TimeSpan(1, 0, 0, 0), t.RepeatInterval); // once per day
                Assert.AreEqual(-1, t.RepeatCount); // infinite
                Assert.AreEqual(_now.AddSeconds(1), t.StartTimeUtc); // right away
            });
        }

        [TestMethod]
        public void TestBuildTriggerForJobWithStartAtAttribute()
        {
            TestBuildTrigger<JobWithStartAtAttribute>(t => {
                Assert.AreEqual(new TimeSpan(1, 0, 0, 0), t.RepeatInterval); // once per day
                Assert.AreEqual(-1, t.RepeatCount); // infinite
                Assert.AreEqual(_now.GetNext(JobWithStartAtAttribute.START_HOUR), t.StartTimeUtc); // right away
            });
        }

        [TestMethod]
        public void TestBuildTriggerForJobWithMinutelyAttributeNoArgs()
        {
            TestBuildTrigger<JobWithMinutelyAttributeNoArgs>(t => {
                Assert.AreEqual(new TimeSpan(0, 0, 1, 0), t.RepeatInterval); // once per minute
                Assert.AreEqual(-1, t.RepeatCount); // infinite
                Assert.AreEqual(_then, t.StartTimeUtc); // when the date service says to
            });
        }

        [TestMethod]
        public void TestBuildTriggerForJobWithMinutelyAttributeOneArg()
        {
            TestBuildTrigger<JobWithMinutelyAttributeOneArg>(t => {
                Assert.AreEqual(new TimeSpan(0, 0, 3, 0), t.RepeatInterval); // once every 3 minutes
                Assert.AreEqual(-1, t.RepeatCount); // infinite
                Assert.AreEqual(_then, t.StartTimeUtc); // when the date service says to
            });
        }

        [TestMethod]
        public void TestBuildTriggerForJobWithNoAttributes()
        {
            TestBuildTrigger<JobWithNoAttributes>(t => {
                Assert.AreEqual(new TimeSpan(1, 0, 0, 0), t.RepeatInterval); // once per day
                Assert.AreEqual(-1, t.RepeatCount); // infinite
                Assert.AreEqual(_then, t.StartTimeUtc); // when the date service says to
            });
        }

        [TestMethod]
        public void TestBuildTriggerForJobWithSecondlyAttributeNoArgs()
        {
            TestBuildTrigger<JobWithSecondlyAttributeNoArgs>(t => {
                Assert.AreEqual(new TimeSpan(0, 0, 0, 1), t.RepeatInterval); // once per second
                Assert.AreEqual(-1, t.RepeatCount); // infinite
                Assert.AreEqual(_then, t.StartTimeUtc); // when the date service says to
            });
        }

        [TestMethod]
        public void TestBuildTriggerForJobWithSecondlyAttributeOneArg()
        {
            TestBuildTrigger<JobWithSecondlyAttributeOneArg>(t => {
                Assert.AreEqual(new TimeSpan(0, 0, 0, 3), t.RepeatInterval); // once every 3 seconds
                Assert.AreEqual(-1, t.RepeatCount); // infinite
                Assert.AreEqual(_then, t.StartTimeUtc); // when the date service says to
            });
        }

        [TestMethod]
        public void TestGetAllJobsGetsADecentSmatteringOfJobTypes()
        {
            var expected = new[] {
                typeof(AssetUploadProcessorJob),
                typeof(DailyGISFileDumpJob),
                typeof(DailyGISFileImportJob),
                typeof(DailyW1VFileImportJob),
                //typeof(DailySpaceTimeInsightFileDumpJob),
                typeof(GISMessageBrokerIntegrationJob),
                typeof(HeartbeatJob),
                typeof(LeakAlertUpdaterJob),
                typeof(MapCallNotifierJob),
                typeof(MapCallRoutineProductionWorkOrderJob),
                typeof(MarkoutTicketFetcherJob),
                typeof(MeterChangeOutStatusUpdateJob),
                typeof(MonthlyNonRevenueWaterEntryFileDumpJob),
                typeof(MonthlySpaceTimeInsightFileDumpJob),
                typeof(MonthlySystemDeliveryEntryFileDumpJob),
                typeof(SAPEquipmentSyncronizationJob),
                typeof(SapEmployeeUpdaterJob),
                typeof(SapMaterialUpdaterJob),
                typeof(SapPremiseUpdaterJob),
                typeof(SapScheduledProductionWorkOrderFetcherJob),
                //typeof(ScadaTagFetcherJob),
                typeof(SampleSiteProfileSyncJob),
                typeof(ServicePremiseLinkJob),
                typeof(SmartCoverAlertLinkJob),
                typeof(SapChemicalUpdaterJob),
                typeof(SampleSiteDeactivateOnNewServiceRecordJob),
                typeof(NonRevenueWaterEntryCreatorJob),
                typeof(SapWaterQualityComplaintJob),
                typeof(NSIPremiseFileLinkJob)
            };

            var result = _target.GetAllJobs();

            expected.Each(t => MyAssert.Contains(result, t));
            result.Each(t => MyAssert.Contains(expected, t, $"You need to add {t.Name} to this test"));
        }
    }
}
