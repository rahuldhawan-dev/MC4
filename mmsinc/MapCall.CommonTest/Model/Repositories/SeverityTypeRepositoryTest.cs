using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class SeverityTypeRepositoryTest : InMemoryDatabaseTest<SeverityType, SeverityTypeRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ISeverityTypeRepository>().Use<SeverityTypeRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetSeverityTypeById()
        {
            var expected = GetEntityFactory<SeverityType>().Create();
            var result = Repository.GetSeverityTypeById(expected.Id);
            Assert.AreSame(expected, result);
        }

        #endregion
    }
}
