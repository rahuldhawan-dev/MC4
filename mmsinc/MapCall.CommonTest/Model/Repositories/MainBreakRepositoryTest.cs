using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class MainBreakRepositoryTest : InMemoryDatabaseTest<MainBreak, MainBreakRepository>
    {
        #region Private Members

        #region Fields

        private TestDateTimeProvider DateTimeProvider =>
            (TestDateTimeProvider)_container.GetInstance<IDateTimeProvider>();

        #endregion

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Singleton().Use<TestDateTimeProvider>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(WorkOrderFactory).Assembly);
        }

        #endregion

        [TestMethod]
        public void TestGetFromPastDayGetsMainBreaksFromPastDay()
        {
            var now = DateTime.Now;
            var yesterday = now.AddDays(-1);
            DateTimeProvider.SetNow(now);
            var pastDay =
                GetEntityFactory<MainBreak>().Create(new {
                    WorkOrder = GetEntityFactory<WorkOrder>().Create(new {DateCompleted = yesterday})
                });
            var previousDay =
                GetEntityFactory<MainBreak>().Create(new {
                    WorkOrder =
                        GetEntityFactory<WorkOrder>().Create(new {DateCompleted = yesterday.Date.AddSeconds(-1)})
                });
            var today =
                GetEntityFactory<MainBreak>().Create(new {
                    WorkOrder =
                        GetEntityFactory<WorkOrder>().Create(new {DateCompleted = yesterday.AddDays(1)})
                });

            var results = Repository.GetFromPastDay();

            MyAssert.Contains(results, pastDay, "Expected main break was not in result");
            MyAssert.DoesNotContain(results, previousDay, "Main break from previousDay was in result");
            MyAssert.DoesNotContain(results, today, "Main break from today was in result");
        }

        [TestMethod]
        public void TestGetPowerPlantMainBreaks()
        {
            var town = GetEntityFactory<Town>().Create();
            var mainBreakWorkDescription = GetFactory<WaterMainBreakReplaceWorkDescriptionFactory>().Create();
            var validWorkOrder = GetEntityFactory<WorkOrder>().Create(new
                {Town = town, WorkDescription = mainBreakWorkDescription, ApprovedOn = DateTime.Now});
            var invalidWorkOrder = GetEntityFactory<WorkOrder>()
               .Create(new {Town = town, WorkDescription = mainBreakWorkDescription});
            var validMainBreak = GetFactory<MainBreakFactory>().Create(new {WorkOrder = validWorkOrder});
            var invalidMainBreak1 = GetFactory<MainBreakFactory>().Create();
            var invalidMainBreak2 = GetFactory<MainBreakFactory>().Create(new {WorkOrder = invalidWorkOrder});

            var results = Repository.GetPowerPlantMainBreaks(new EmptySearchSet<MainBreak>());

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(results.First().Id, validMainBreak.Id);
        }
    }
}
