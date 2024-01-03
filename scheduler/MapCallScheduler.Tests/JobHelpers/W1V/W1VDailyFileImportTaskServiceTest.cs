using System.Linq;
using log4net;
using MapCall.Common.Model.Repositories;
using MapCallScheduler.JobHelpers.W1V;
using MapCallScheduler.JobHelpers.W1V.ImportTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.W1V
{
    [TestClass]
    public class W1VDailyFileImportTaskServiceTest
    {
        #region Private Members

        private W1VDailyFileImportTaskService _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);

            _target = _container.GetInstance<W1VDailyFileImportTaskService>();
        }

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e)
        {
            e.For<IShortCycleCustomerMaterialRepository>().Mock();
            e.For<IW1VFileDownloadService>().Mock();
            e.For<IW1VFileParser>().Mock();
            e.For<IW1VRecordMapper>().Mock();
            e.For<IDateTimeProvider>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void Test_GetAllTasks_ReturnsAppropriateTasks()
        {
            var result = _target.GetAllTasks().ToArray();

            var expectedTypes = new[] { typeof(CustomerMaterialTask) };

            Assert.AreEqual(expectedTypes.Length, result.Length);

            expectedTypes.Each(t => Assert.IsTrue(result.Any(r => r.GetType() == t)));
        }
    }
}
