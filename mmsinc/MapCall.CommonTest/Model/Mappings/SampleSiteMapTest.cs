using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class SampleSiteMapTest : InMemoryDatabaseTest<SampleSite>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IDateTimeProvider>().Use<DateTimeProvider>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestDeletingASampleSiteDeletesOrphanedBracketSiteRecords()
        {
            var sampleSite1 = GetFactory<SampleSiteFactory>().Create();
            var sampleSite2 = GetFactory<SampleSiteFactory>().Create();

            var bracketLocationType = GetEntityFactory<SampleSiteBracketSiteLocationType>().Create();

            var bracketSite = new SampleSiteBracketSite {
                SampleSite = sampleSite1,
                BracketSampleSite = sampleSite2,
                BracketSiteLocationType = bracketLocationType
            };

            Session.Save(bracketSite);

            Session.Clear();

            sampleSite1 = Session.Query<SampleSite>().Single(x => x.Id == sampleSite1.Id);

            Assert.AreEqual(1, sampleSite1.BracketSites.Count);

            Session.Delete(sampleSite1);
            Session.Flush();

            bracketSite = Session.Query<SampleSiteBracketSite>().SingleOrDefault(x => x.Id == bracketSite.Id);
            Assert.IsNull(bracketSite, "Should have been deleted automatically");

            sampleSite2 = Session.Query<SampleSite>().Single(x => x.Id == sampleSite2.Id);
            Assert.IsNotNull(sampleSite2, "Should not have been deleted");
        }

        #endregion
    }
}
