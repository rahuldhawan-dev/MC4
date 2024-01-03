using System;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class MarkoutTicketFetcherJobTest : MapCallJobWithProcessableServiceJobTestBase<MarkoutTicketFetcherJob, IOneCallMessageService>
    {
        #region Private Members

        private Mock<IOneCallMessageHeartbeatService> _heartbeatService;

        #endregion

        #region Setup/Teardown

        protected override void InitializeBeforeTarget()
        {
            _container.Inject((_heartbeatService = new Mock<IOneCallMessageHeartbeatService>()).Object);
        }

        #endregion

        [TestMethod]
        public void TestExecuteProcessesTheHeartbeatService()
        {
            _target.Execute(null);

            _heartbeatService.Verify(x => x.Process());
        }

        [TestMethod]
        public void TestExecuteThrowsExceptionIfHeartbeatServiceThrowsException()
        {
            _heartbeatService.Setup(x => x.Process()).Throws<InvalidOperationException>();

            MyAssert.Throws<InvalidOperationException>(() => _target.Execute(null));
        }
    }
}