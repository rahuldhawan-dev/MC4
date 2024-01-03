using log4net;
using MapCallScheduler.JobHelpers.NonRevenueWater;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.NonRevenueWaters
{
    [TestClass]
    public class NonRevenueWaterEntryFileDumpServiceTest
    {
        #region Private Members

        private IContainer _container;
        private MonthlyNonRevenueWaterEntryFileDumpService _target;
        private Mock<INonRevenueWaterEntryFileDumpTaskService> _taskService;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);
            _target = _container.GetInstance<MonthlyNonRevenueWaterEntryFileDumpService>();
        }

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e)
        {
            _taskService = e.For<INonRevenueWaterEntryFileDumpTaskService>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void Test_Process_RunsAllTasksFromTaskService()
        {
            var task = new Mock<INonRevenueWaterEntryFileDumpTask>();
            _taskService.Setup(x => x.GetAllDailyTasks()).Returns(new[] { task.Object });

            _target.Process();

            task.Verify(x => x.Run());
        }
    }
}
