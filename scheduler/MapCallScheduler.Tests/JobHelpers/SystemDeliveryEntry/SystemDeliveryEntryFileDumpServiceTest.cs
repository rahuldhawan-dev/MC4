using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SystemDeliveryEntry
{
    [TestClass]
    public class SystemDeliveryEntryFileDumpServiceTest
    {
        #region Private Members

        private IContainer _container;
        private MonthlySystemDeliveryEntryFileDumpService _target;
        private Mock<ISystemDeliveryEntryFileDumpTaskService> _taskService;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);

            _target = _container.GetInstance<MonthlySystemDeliveryEntryFileDumpService>();
        }

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e)
        {
            _taskService = e.For<ISystemDeliveryEntryFileDumpTaskService>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void TestProcessRunsAllTasksFromTaskService()
        {
            var task = new Mock<ISystemDeliveryEntryFileDumpTask>();
            _taskService.Setup(x => x.GetAllDailyTasks()).Returns(new[] {task.Object});

            _target.Process();

            task.Verify(x => x.Run());
        }
    }
}
