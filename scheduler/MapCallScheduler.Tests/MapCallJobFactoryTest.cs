using System;
using System.Threading.Tasks;
using log4net;
using MapCallScheduler.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using Quartz;
using Quartz.Spi;
using StructureMap;

namespace MapCallScheduler.Tests
{
    [TestClass]
    public class MapCallJobFactoryTest
    {
        #region Private Members

        private MapCallJobFactory _target;
        private Mock<IContainer> _container;
        private Mock<IScheduler> _scheduler;
        private Mock<ILog> _log;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Mock<IContainer>();
            _scheduler = new Mock<IScheduler>();
            _log = new Mock<ILog>();
            _target = new MapCallJobFactory(_container.Object, _log.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _container.VerifyAll();
            _scheduler.VerifyAll();
        }

        #endregion

        #region Nested Type: MockJob

        private class MockJob : IJob
        {
            #region Exposed Methods

            public void Execute(IJobExecutionContext context)
            {
                throw new NotImplementedException();
            }

            Task IJob.Execute(IJobExecutionContext context)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        [NoConfigureSession]
        private class NoConfigureSessionMockJob : MockJob { }

        #endregion

        private IJob TestNewJob(bool throwException = false)
        {
            return TestNewJob<MockJob>(throwException);
        }

        private IJob TestNewJob<TJob>(bool throwException = false)
            where TJob : IJob, new()
        {
            var jobType = typeof(TJob);
            var detail = new Mock<IJobDetail>();
            var bundle = new TriggerFiredBundle(detail.Object, null, null, false, DateTimeOffset.Now, null, null, null);
            var profile = jobType.HasAttribute<NoConfigureSessionAttribute>()
                ? DependencyRegistry.NO_REGISTER_ISESSION
                : DependencyRegistry.REGISTER_ISESSION;

            _container.Setup(x => x.GetNestedContainer(profile)).Returns(_container.Object);
            detail.SetupGet(x => x.JobType).Returns(jobType);

            var containerSetup = _container.Setup(x => x.GetInstance(jobType));

            if (throwException)
            {
                containerSetup.Throws<Exception>();
            }
            else
            {
                containerSetup.Returns(new TJob());
            }

            return _target.NewJob(bundle, _scheduler.Object);
        }

        [TestMethod]
        public void TestNewJobReturnsJob()
        {
            Assert.IsInstanceOfType(TestNewJob(), typeof(MockJob));
        }

        [TestMethod]
        public void TestNewJobWithNoConfigureSessionJob()
        {
            Assert.IsInstanceOfType(TestNewJob<NoConfigureSessionMockJob>(), typeof(NoConfigureSessionMockJob));
        }

        [TestMethod]
        public void TestNewJobThrowsSchedulerExceptionIfSomethingGoesWrong()
        {
            MyAssert.Throws<SchedulerException>(() => TestNewJob(true));
        }

        [TestMethod]
        public void TestReturnJobIsAThingThatDoesNotDoMuch()
        {
            _target.ReturnJob(null);

            _container.VerifyAll();
            _scheduler.VerifyAll();
        }
    }
}
