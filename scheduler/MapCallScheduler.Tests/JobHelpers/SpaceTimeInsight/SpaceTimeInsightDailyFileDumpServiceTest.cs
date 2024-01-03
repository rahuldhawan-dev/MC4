using log4net;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SpaceTimeInsight
{
    [TestClass]
    public class SpaceTimeInsightDailyFileDumpServiceTest
    {
        #region Private Members

        private SpaceTimeInsightDailyFileDumpService _target;
        private Mock<ILog> _log;
        private Mock<ISpaceTimeInsightFileDumpTaskService> _taskService;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();

            _container.Inject((_log = new Mock<ILog>()).Object);
            _container.Inject((_taskService = new Mock<ISpaceTimeInsightFileDumpTaskService>()).Object);

            _target = _container.GetInstance<SpaceTimeInsightDailyFileDumpService>();
        }

        #endregion

        [TestMethod]
        public void TestProcessRunsAllTasksFromTaskService()
        {
            var task = new Mock<ISpaceTimeInsightFileDumpTask>();
            _taskService.Setup(s => s.GetAllDailyTasks()).Returns(new[] {task.Object});

            _target.Process();

            task.Verify(x => x.Run());
        }
    }
}
