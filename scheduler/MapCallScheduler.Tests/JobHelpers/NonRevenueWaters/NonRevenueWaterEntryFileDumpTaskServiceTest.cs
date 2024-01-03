using System.Linq;
using log4net;
using MapCall.Common.Model.Repositories;
using MapCallScheduler.JobHelpers.NonRevenueWater;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.NonRevenueWaters
{
    [TestClass]
    public class NonRevenueWaterEntryFileDumpTaskServiceTest
    {
        #region Private Members

        private Container _container;
        private NonRevenueWaterEntryFileDumpTaskService _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);
            _target = _container.GetInstance<NonRevenueWaterEntryFileDumpTaskService>();
        }

        #endregion

        #region Private Methods

        private static void InitializeContainer(ConfigurationExpression e)
        {
            e.For<INonRevenueWaterEntryRepository>().Mock();
            e.For<INonRevenueWaterEntryFileSerializer>().Mock();
            e.For<INonRevenueWaterEntryFileUploader>().Mock();
            e.For<IDateTimeProvider>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void Test_GetAllDailyTasks_ReturnsCorrectTask()
        {
            var result = _target.GetAllDailyTasks().ToList();

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Any(x => x.GetType() == typeof(NonRevenueWaterEntryFileDumpTask)));
        }
    }
}
