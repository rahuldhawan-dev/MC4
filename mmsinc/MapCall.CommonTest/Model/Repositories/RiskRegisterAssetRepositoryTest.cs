using MMSINC.Utilities;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Moq;
using StructureMap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class RiskRegisterAssetRepositoryTest : InMemoryDatabaseTest<RiskRegisterAsset, RiskRegisterAssetRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use(new Mock<IDateTimeProvider>().Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            BaseTestInitialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            BaseTestCleanup();
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(typeof(RiskRegisterAssetFactory).Assembly).GetInstance<TestDataFactoryService>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestActionItemIsDeletedWithRiskRegisterAsset()
        {
            this.TestActionItemIsDeletedWithThing(RiskRegisterAssetMap.TABLE_NAME);
        }

        #endregion
    }
}
