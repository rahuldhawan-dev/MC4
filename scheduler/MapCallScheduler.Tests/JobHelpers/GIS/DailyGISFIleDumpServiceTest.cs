using log4net;
using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.JobHelpers.GIS.DumpTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.GIS
{
    [TestClass]
    public class DailyGISFileDumpServiceTest
    {
        #region Private Members

        private IContainer _container;
        private DailyGISFileDumpService _target;
        private Mock<IGISFileDumpTaskService> _taskService;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);

            _target = _container.GetInstance<DailyGISFileDumpService>();
        }

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e)
        {
            _taskService = e.For<IGISFileDumpTaskService>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void TestProcessRunsAllTasksFromTaskService()
        {
            var task = new Mock<IDailyGISFileDumpTask>();
            _taskService.Setup(x => x.GetAllTasks()).Returns(new[] {task.Object});

            _target.Process();

            task.Verify(x => x.Run());
        }
    }
}
