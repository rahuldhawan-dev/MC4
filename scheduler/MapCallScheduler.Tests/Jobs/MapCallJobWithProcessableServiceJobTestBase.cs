using System;
using log4net;
using MapCallScheduler.Jobs;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Jobs
{
    public class MapCallJobWithProcessableServiceJobTestBase<TJob, TService>
        where TJob : MapCallJobWithProcessableServiceBase<TService>
        where TService : class, IProcessableService
    {
        #region Private Members

        protected TJob _target;
        protected IContainer _container;
        protected Mock<ILog> _log;
        protected Mock<TService> _service;
        protected Mock<IDeveloperEmailer> _emailer;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_log = new Mock<ILog>()).Object);
            _container.Inject((_service = new Mock<TService>()).Object);
            _container.Inject((_emailer = new Mock<IDeveloperEmailer>()).Object);

            InitializeBeforeTarget();

            _target = _container.GetInstance<TJob>();
        }

        protected virtual void InitializeBeforeTarget()
        {
            // noop
        }

        #endregion

        [TestMethod]
        public void TestExecuteProcessesTheService()
        {
            _target.Execute(null);

            _service.Verify(x => x.Process());
            _log.Verify(x => x.Info($"Executing Job {this._target.GetType().Name}"), Times.Once);
        }

        [TestMethod]
        public void TestExecuteLogsExceptionifThrownByService()
        {
            var e = new Exception();
            _service.Setup(x => x.Process()).Throws(e);

            _target.Execute(null);

            _log.Verify(x => x.Error($"Error in {_target.GetServiceName()}", e));
        }

        [TestMethod]
        public void TestExecuteEmailsExceptionToDeveloperIfThrownByService()
        {
            var e = new Exception("OH NOES!!!");
            _service.Setup(x => x.Process()).Throws(e);

            _target.Execute(null);

            _emailer.Verify(x => x.SendErrorMessage($"MapCallScheduler: Error in {_target.GetServiceName()}", e));
        }
    }
}