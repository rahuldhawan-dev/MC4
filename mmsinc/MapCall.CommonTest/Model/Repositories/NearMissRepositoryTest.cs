using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class NearMissRepositoryTest : InMemoryDatabaseTest<NearMiss, NearMissRepository>
    {
        private TestDateTimeProvider _dateTimeProvider;
        private DateTime _now;
        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(typeof(NearMissFactory).Assembly).GetInstance<TestDataFactoryService>();
        }

        [TestMethod]
        public void TestActionItemIsDeletedWithNearMiss()
        {
            this.TestActionItemIsDeletedWithThing(NearMissMap.TABLE_NAME);
        }

        #region RepositoryExtensions

        [TestMethod]
        public void TestNearMissForLast1Day()
        {
            var envType = GetFactory<EnvironmentalNearMissTypeFactory>().Create();
            var safetyType = GetFactory<SafetyNearMissTypeFactory>().Create();
            var safetyNearMiss = GetFactory<NearMissFactory>().Create(new { Type = safetyType });
            var safetyNearMissCreatedPrior2days = GetFactory<NearMissFactory>().Create(new { Type = safetyType });
            safetyNearMissCreatedPrior2days.CreatedAt = _now.AddDays(-2);
            Session.Save(safetyNearMissCreatedPrior2days);
            var envNearMiss = GetFactory<NearMissFactory>().Create(new { Type = envType });
            var envNearMissCreatedPrior2days = GetFactory<NearMissFactory>().Create(new { Type = envType });
            envNearMissCreatedPrior2days.CreatedAt = _now.AddDays(-2);
            Session.Save(envNearMissCreatedPrior2days);

            Session.Flush();
            var safetyNearMissInLastOneDay = _container.GetInstance<IRepository<NearMiss>>()
                                                       .GetNearMissesInPriorOneDay(_dateTimeProvider, NearMissType.Indices.SAFETY);

            var envNearMissInLastOneDay = _container.GetInstance<IRepository<NearMiss>>()
                                                    .GetNearMissesInPriorOneDay(_dateTimeProvider, NearMissType.Indices.ENVIRONMENTAL);

            Assert.AreEqual(1, Queryable.Count(safetyNearMissInLastOneDay));
            Assert.AreEqual(1, Queryable.Count(envNearMissInLastOneDay));
        }

        #endregion
    }
}
