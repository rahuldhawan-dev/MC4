using System.Linq;
using log4net;
using MapCall.Common.Model.Repositories;
using MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SystemDeliveryEntry
{
    [TestClass]
    public class SystemDeliveryEntryFileDumpTaskServiceTest
    {
        #region Private Members

        private SystemDeliveryEntryFileDumpTaskService _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);

            _target = _container.GetInstance<SystemDeliveryEntryFileDumpTaskService>();
        }

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e)
        {
            e.For<ISystemDeliveryEntryRepository>().Mock();
            e.For<ISystemDeliveryEntryFileSerializer>().Mock();
            e.For<ISystemDeliveryEntryFileUploader>().Mock();
            e.For<IDateTimeProvider>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void TestGetAllDailyTasksReturnsAppropriateTask()
        {
            var result = _target.GetAllDailyTasks().ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(result.Any(t => t.GetType() == typeof(SystemDeliveryEntryFileDumpTask)));
        }
    }
}
