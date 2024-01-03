using log4net;
using MapCallScheduler.JobHelpers.W1V;
using MapCallScheduler.JobHelpers.W1V.ImportTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.W1V
{
    [TestClass]
    public class DailyW1VFileImportServiceTest
    {
        #region Private Members

        private IContainer _container;
        private DailyW1VFileImportService _target;
        private Mock<IW1VDailyFileImportTaskService> _taskService;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);

            _target = _container.GetInstance<DailyW1VFileImportService>();
        }

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e)
        {
            _taskService = e.For<IW1VDailyFileImportTaskService>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void TestProcessRunsAllTasksFromTaskService()
        {
            var task = new Mock<IDailyW1VFileImportTask>();
            _taskService.Setup(x => x.GetAllTasks()).Returns(new[] { task.Object });

            _target.Process();

            task.Verify(x => x.Run());
        }
    }
}
