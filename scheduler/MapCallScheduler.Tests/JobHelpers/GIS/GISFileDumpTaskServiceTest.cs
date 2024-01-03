using System;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.JobHelpers.GIS.DumpTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.GIS
{
    [TestClass]
    public class GISFileDumpTaskServiceTest
    {
        #region Private Members

        private GISFileDumpTaskService _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);

            _target = _container.GetInstance<GISFileDumpTaskService>();
        }

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e)
        {
            e.For<IRepository<AsBuiltImage>>().Mock();
            e.For<IRepository<Hydrant>>().Mock();
            e.For<IRepository<Valve>>().Mock();
            e.For<IRepository<SewerOpening>>().Mock();
            e.For<IRepository<MostRecentlyInstalledService>>().Mock();
            e.For<IGISFileSerializer>().Mock();
            e.For<IGISFileUploader>().Mock();
            e.For<IDateTimeProvider>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion

        [TestMethod]
        public void TestGetAllDailyTasksReturnsAppropriateTasks()
        {
            var result = _target.GetAllDailyTasks().ToArray();

            var expectedTypes = new[] {
                typeof(AsBuiltImageTask),
                typeof(HydrantTask),
                typeof(ServiceTask),
                typeof(SewerOpeningTask),
                typeof(ValveTask),
            };

            Assert.AreEqual(expectedTypes.Length, result.Length);

            expectedTypes.Each(t => Assert.IsTrue(result.Any(r => r.GetType() == t)));
        }
    }
}
