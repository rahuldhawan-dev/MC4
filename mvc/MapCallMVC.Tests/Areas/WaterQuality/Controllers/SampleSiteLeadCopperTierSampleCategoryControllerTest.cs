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
    public class SampleSiteLeadCopperTierSampleCategoryControllerTest : MapCallMvcControllerTestBase<SampleSiteLeadCopperTierSampleCategoryController, SampleSiteLeadCopperTierSampleCategory, IRepository<SampleSiteLeadCopperTierSampleCategory>>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WaterQuality/SampleSiteLeadCopperTierSampleCategory/BySampleSiteLeadCopperTierClassification/");
            });
        }

        #endregion

        #region ByState

        [TestMethod]
        public void TestByBySampleSiteLeadCopperTierClassificationReturnsRecordsFilteredBySampleSiteLeadCopperTierClassification()
        {
            var tier1 = GetFactory<TierOneSampleSiteLeadCopperTierClassificationFactory>().Create();
            var tier2 = GetFactory<TierTwoSampleSiteLeadCopperTierClassificationFactory>().Create();
            var tier3 = GetFactory<TierThreeSampleSiteLeadCopperTierClassificationFactory>().Create();

            var sampleCategory1ForTier1 = GetFactory<SampleSiteLeadCopperTierSampleCategoryFactory>().Create(new {
                DisplayValue = "i"
            });
            var sampleCategory1ForTier2 = GetFactory<SampleSiteLeadCopperTierSampleCategoryFactory>().Create(new { 
                DisplayValue = "i"
            });
            var sampleCategory1ForTier3 = GetFactory<SampleSiteLeadCopperTierSampleCategoryFactory>().Create(new { 
                DisplayValue = "i"
            });
            var sampleCategory2ForTier3 = GetFactory<SampleSiteLeadCopperTierSampleCategoryFactory>().Create(new {
                DisplayValue = "ii"
            });

            sampleCategory1ForTier1.TierClassifications.Add(tier1);
            sampleCategory1ForTier2.TierClassifications.Add(tier2);
            sampleCategory1ForTier3.TierClassifications.Add(tier3);
            sampleCategory2ForTier3.TierClassifications.Add(tier3);

            Session.Flush();

            var actionResult = (CascadingActionResult)_target.BySampleSiteLeadCopperTierClassification(tier3.Id);
            var items = ((IEnumerable<dynamic>)actionResult.Data).ToList();

            Assert.AreEqual(2, items.Count);

            Assert.AreEqual(0, items.Count(x => x.Id == sampleCategory1ForTier1.Id));
            Assert.AreEqual(0, items.Count(x => x.Id == sampleCategory1ForTier2.Id));
            Assert.AreEqual(1, items.Count(x => x.Id == sampleCategory1ForTier3.Id));
            Assert.AreEqual(1, items.Count(x => x.Id == sampleCategory2ForTier3.Id));
        }

        #endregion
    }
}
