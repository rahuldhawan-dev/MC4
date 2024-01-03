using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        ValveNormalPositionRepositoryTest : InMemoryDatabaseTest<ValveNormalPosition, ValveNormalPositionRepository>
    {
        #region Tests

        [TestMethod]
        public void TestFindByDescriptionFindsByDescription()
        {
            var vnp1 = GetFactory<ValveNormalPositionFactory>().Create(new {Description = "Neat"});
            var vnp2 = GetFactory<ValveNormalPositionFactory>().Create(new {Description = "Not"});

            Assert.AreSame(vnp1, Repository.FindByDescription("Neat"));
            Assert.AreSame(vnp2, Repository.FindByDescription("Not"));
        }

        #endregion
    }
}
