using System.Web.Mvc;
using log4net;
using MapCallScheduler.JobHelpers.SAPDataSyncronization;
using MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SAPDataSyncronization
{
    [TestClass]
    public class SAPSyncronizationServiceTest
    {
        #region Private Members

        private SAPSyncronizationService _target;
        private Mock<ILog> _log;
        private Mock<ISAPSyncronizationTaskService> _taskService;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            var container = new Container();
            container.Inject((_log = new Mock<ILog>()).Object);
            container.Inject((_taskService = new Mock<ISAPSyncronizationTaskService>()).Object);
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));

            _target = container.GetInstance<SAPSyncronizationService>();
        }

        [TestMethod]
        public void TestProcessRunsAllTasksFromTaskService()
        {
            var task = new Mock<SAPSyncronizationTaskBase>();
            _taskService.Setup(x => x.GetAllTasks()).Returns(new[] {task.Object});

            _target.Process();

            _log.Verify(x => x.Info(It.Is<string>(m => m == $"Running sap syncronization task SAPSyncronizationTaskBaseProxy")), Times.Once);
            task.Verify(x => x.Run());
            _log.Verify(x => x.Info(It.Is<string>(m => m == $"Completed sap syncronization task SAPSyncronizationTaskBaseProxy")), Times.Once);
        }
    }
}