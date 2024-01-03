using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.WaterQuality.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class SampleSiteLeadCopperTierClassificationControllerTest : MapCallMvcControllerTestBase<SampleSiteLeadCopperTierClassificationController, SampleSiteLeadCopperTierClassification, IRepository<SampleSiteLeadCopperTierClassification>>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WaterQuality/SampleSiteLeadCopperTierClassification/ByState/");
            });
        }

        #endregion

        #region ByState

        [TestMethod]
        public void TestByStateReturnsRecordsFilteredByState()
        {
            var tier1 = GetFactory<TierOneSampleSiteLeadCopperTierClassificationFactory>().Create();
            var tier2 = GetFactory<TierTwoSampleSiteLeadCopperTierClassificationFactory>().Create();
            var tier3 = GetFactory<TierThreeSampleSiteLeadCopperTierClassificationFactory>().Create();

            var pa = GetFactory<StateFactory>().Create(new { Abbreviation = "PA" });
            var nj = GetFactory<StateFactory>().Create(new { Abbreviation = "NJ" });
            var il = GetFactory<StateFactory>().Create(new { Abbreviation = "IL" });
            var oh = GetFactory<StateFactory>().Create(new { Abbreviation = "OH" });
            var me = GetFactory<StateFactory>().Create(new { Abbreviation = "ME" });
            var ca = GetFactory<StateFactory>().Create(new { Abbreviation = "CA" });

            tier1.States.Add(pa);
            tier1.States.Add(nj);

            tier2.States.Add(il);
            tier2.States.Add(oh);

            tier3.States.Add(me);
            tier3.States.Add(ca);
            tier3.States.Add(pa);

            Session.Flush();

            var actionResult = (CascadingActionResult)_target.ByState(pa.Id);
            var items = ((IEnumerable<dynamic>)actionResult.Data).ToList();

            Assert.AreEqual(1, items.Count(x => x.Id == tier1.Id));
            Assert.AreEqual(0, items.Count(x => x.Id == tier2.Id));
            Assert.AreEqual(1, items.Count(x => x.Id == tier3.Id));
        }

        #endregion
    }
}
