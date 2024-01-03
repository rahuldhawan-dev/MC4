using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class SampleSiteRepositoryTest : InMemoryDatabaseTest<SampleSite, SampleSiteRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IDateTimeProvider>().Mock();
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetByPublicWaterSupplyReturnsSampleSitesWithMatchingPWSID()
        {
            var pwsid = GetFactory<PublicWaterSupplyFactory>().Create();
            var ss1good = GetFactory<SampleSiteFactory>().Create(new {PublicWaterSupply = pwsid});
            var ss2good = GetFactory<SampleSiteFactory>().Create(new {PublicWaterSupply = pwsid});
            var ss3bad = GetFactory<SampleSiteFactory>().Create();

            var results = Repository.GetByPublicWaterSupply(pwsid.Id).Items;
            Assert.IsTrue(results.Any(x => x.Id == ss1good.Id));
            Assert.IsTrue(results.Any(x => x.Id == ss2good.Id));
            Assert.IsFalse(results.Any(x => x.Id == ss3bad.Id));
        }

        #endregion
    }
}
