using System.Linq;
using Historian.Data.Client.Repositories;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SpaceTimeInsight
{
    [TestClass]
    public class SpaceTimeInsightFileDumpTaskServiceTest
    {
        #region Private Members

        private SpaceTimeInsightFileDumpTaskService _target;
        protected IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);

            _target = _container.GetInstance<SpaceTimeInsightFileDumpTaskService>();
        }

        private void InitializeContainer(ConfigurationExpression e)
        {
            e.For<IRepository<HydrantInspection>>().Mock();
            e.For<IRepository<WorkOrder>>().Mock();
            e.For<IMainBreakRepository>().Mock();
            e.For<IRawDataRepository>().Mock();
            e.For<IDateTimeProvider>().Mock();
            e.For<ISpaceTimeInsightJsonFileSerializer>().Mock();
            e.For<ISpaceTimeInsightFileUploadService>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void GetGetAllDailyTasksReturnsAppropriateTasks()
        {
            var result = _target.GetAllDailyTasks().ToArray();

            var expectedTypes = new[] {
                typeof(MainBreakTask), typeof(WorkOrderTask), typeof(TankLevelTask), typeof(InterconnectTask)
            };

            Assert.AreEqual(expectedTypes.Length, result.Length);

            expectedTypes.Each(t => Assert.IsTrue(result.Any(r => r.GetType() == t)));
        }

        [TestMethod]
        public void GetGetAllMonthlyTasksReturnsAppropriateTasks()
        {
            var result = _target.GetAllMonthlyTasks().ToArray();

            var expectedTypes = new[] {
                typeof(HydrantInspectionTask)
            };

            Assert.AreEqual(expectedTypes.Length, result.Length);

            expectedTypes.Each(t => Assert.IsTrue(result.Any(r => r.GetType() == t)));
        }
    }
}
