using log4net;
using MapCallScheduler.JobHelpers.GISMessageBroker;
using MapCallScheduler.JobHelpers.GISMessageBroker.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.GISMessageBroker
{
    [TestClass]
    public class GISMessageBrokerServiceTest
    {
        #region Private Members

        private IContainer _container;
        private GISMessageBrokerService _target;
        private Mock<IGISMessageBrokerTaskService> _taskService;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);

            _target = _container.GetInstance<GISMessageBrokerService>();
        }

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e)
        {
            _taskService = e.For<IGISMessageBrokerTaskService>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void TestProcessRunsAllTasksFromTaskService()
        {
            var task = new Mock<IGISMessageBrokerTask>();
            _taskService.Setup(x => x.GetAllTasks()).Returns(new[] { task.Object });

            _target.Process();

            task.Verify(x => x.Run());
        }
    }
}
