using System;
using log4net;
using MapCallScheduler.JobHelpers.SapProductionWorkOrder;
using MapCallScheduler.Jobs;
using MapCallScheduler.Library.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class SapScheduledProductionWorkOrderFetcherJobTest
    {
        #region Private Members

        private SapScheduledProductionWorkOrderFetcherJob _target;
        private Mock<ILog> _log;
        private Mock<ISapScheduledProductionWorkOrderService> _service;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_log = new Mock<ILog>()).Object);
            _container.Inject((_service = new Mock<ISapScheduledProductionWorkOrderService>()).Object);
            _container.Inject(new Mock<IDeveloperEmailer>().Object);

            _target = _container.GetInstance<SapScheduledProductionWorkOrderFetcherJob>();
        }

        #endregion

        [TestMethod]
        public void TestExecuteProcessesTheService()
        {
            _target.Execute(null);

            _service.Verify(x => x.Process());
        }

        [TestMethod]
        public void TestExecuteLogsExceptionifThrownByMessageService()
        {
            var e = new Exception();
            _service.Setup(x => x.Process()).Throws(e);

            _target.Execute(null);

            _log.Verify(x => x.Error("Error in sap scheduled production work order service", e));
        }
    }
}
