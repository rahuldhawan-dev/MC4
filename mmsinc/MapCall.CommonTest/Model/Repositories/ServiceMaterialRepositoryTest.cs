using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class ServiceMaterialRepositoryTest : InMemoryDatabaseTest<ServiceMaterial, RepositoryBase<ServiceMaterial>>
    {
        #region Tests

        [TestMethod]
        public void TestGetLeadServiceMaterialDoesExactlyThat()
        {
            var notLead = GetFactory<ServiceMaterialFactory>().Create(new { Description = "Not Lead"});
            var lead = GetFactory<ServiceMaterialFactory>().Create(new {Description = "Lead"});

            var result = Repository.GetLeadServiceMaterial();
            Assert.AreSame(lead, result);
        }

        #endregion
    }
}
