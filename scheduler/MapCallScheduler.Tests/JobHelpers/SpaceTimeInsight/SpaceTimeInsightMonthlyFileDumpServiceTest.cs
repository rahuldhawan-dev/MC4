using log4net;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SpaceTimeInsight
{
    [TestClass]
    public class SpaceTimeInsightMonthlyFileDumpServiceTest
    {
        #region Private Members

        private SpaceTimeInsightMonthlyFileDumpService _target;
        private IContainer _container;
        private Mock<ILog> _log;
        private Mock<ISpaceTimeInsightFileDumpTaskService> _taskService;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_log = new Mock<ILog>()).Object);
            _container.Inject((_taskService = new Mock<ISpaceTimeInsightFileDumpTaskService>()).Object);

            _target = _container.GetInstance<SpaceTimeInsightMonthlyFileDumpService>();
        }

        #endregion

        [TestMethod]
        public void TestProcessRunsAllTasksFromTaskService()
        {
            var task = new Mock<ISpaceTimeInsightFileDumpTask>();
            _taskService.Setup(s => s.GetAllMonthlyTasks()).Returns(new[] {task.Object});

            _target.Process();

            task.Verify(x => x.Run());
        }
    }
}