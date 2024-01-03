using System;
using System.Collections.Generic;
using log4net;
using MapCall.Common.Utility.Scheduling;
using MapCallScheduler.JobHelpers;
using MapCallScheduler.Jobs;
using MapCallScheduler.Library.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class MapCallNotifierJobTest
    {
        #region Private Members

        private MapCallNotifierJob _target;
        private Mock<ILog> _log;
        private Mock<INotifierTaskService> _taskService;
        private Mock<IDeveloperEmailer> _developerEmailer;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_log = new Mock<ILog>()).Object);
            _container.Inject((_taskService = new Mock<INotifierTaskService>()).Object);
            _container.Inject((_developerEmailer = new Mock<IDeveloperEmailer>()).Object);

            _target = _container.GetInstance<MapCallNotifierJob>();
        }

        #endregion

        [TestMethod]
        public void TestExecuteRunsAnyTasksFoundByTheTaskService()
        {
            var task = new Mock<ITask>();

            _taskService.Setup(x => x.GetAllTasks()).Returns(new List<ITask> {task.Object});

            _target.Execute(null);

            task.Verify(x => x.Run());
        }

        [TestMethod]
        public void TestExecuteDoesNotThrowExceptionIfAnyTaskThrowsException()
        {
            var task = new Mock<ITask>();
            var e = new InvalidOperationException();

            _taskService.Setup(x => x.GetAllTasks()).Returns(new List<ITask> {task.Object});
            task.Setup(x => x.Run()).Throws(e);

            MyAssert.DoesNotThrow(() => _target.Execute(null));
        }

        [TestMethod]
        public void TestExecuteEmailsExecptionToDeveloperIfAnyTaskThrowsException()
        {
            var task = new Mock<ITask>();
            var e = new InvalidOperationException();

            _taskService.Setup(x => x.GetAllTasks()).Returns(new List<ITask> {task.Object});
            task.Setup(x => x.Run()).Throws(e);

            _target.Execute(null);

            _developerEmailer.Verify(x => x.SendErrorMessage("MapCallScheduler: Error in MapCallNotifierJob", e));
        }

        [TestMethod]
        public void TestExecuteKeepsRunningIfOneTaskErrors()
        {
            var task0 = new Mock<ITask>();
            var task1 = new Mock<ITask>();

            _taskService.Setup(x => x.GetAllTasks()).Returns(new[] {task0.Object, task1.Object});
            task0.Setup(x => x.Run()).Throws<Exception>();

            _target.Execute(null);

            task1.Verify(x => x.Run());
        }
    }
}
