using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class SamplePlanTest : InMemoryDatabaseTest<SamplePlan>
    {
        [TestMethod]
        public void TestActiveSampleSitesReturnsSampleSitesThatAreActiveAndLeadCopper()
        {
            var activeStatus = GetFactory<ActiveSampleSiteStatusFactory>().Create();
            var inactiveStatus = GetFactory<InactiveSampleSiteStatusFactory>().Create();
            var samplePlan = new SamplePlan();
            var sampleSite1 = new SampleSite {LeadCopperSite = false, Status = activeStatus};
            var sampleSite2 = new SampleSite {LeadCopperSite = true, Status = activeStatus};
            var sampleSite3 = new SampleSite {LeadCopperSite = true, Status = inactiveStatus};
            var sampleSite4 = new SampleSite {LeadCopperSite = false, Status = activeStatus};

            samplePlan.SampleSites.Add(sampleSite1);
            samplePlan.SampleSites.Add(sampleSite2);
            samplePlan.SampleSites.Add(sampleSite3);

            Assert.AreEqual(1, samplePlan.ActiveSampleSites.Count());
        }
    }
}
