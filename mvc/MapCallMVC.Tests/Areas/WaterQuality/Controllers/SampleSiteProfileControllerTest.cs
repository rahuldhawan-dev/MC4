using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.WaterQuality.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class SampleSiteProfileControllerTest : MapCallMvcControllerTestBase<SampleSiteProfileController, SampleSiteProfile, IRepository<SampleSiteProfile>>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WaterQuality/SampleSiteProfile/ByPublicWaterSupply/");
            });
        }

        #endregion

        #region ByPublicWaterSupply

        [TestMethod]
        public void TestByPublicWaterSupplyReturnsSampleSiteProfilesFilteredByPublicWaterSupplyId()
        {
            var publicWaterSupplyX = GetEntityFactory<PublicWaterSupply>().Create(new {
                Identifier = "pws-x"
            });

            var publicWaterSupplyY = GetEntityFactory<PublicWaterSupply>().Create(new {
                Identifier = "pws-y"
            });

            GetEntityFactory<SampleSiteProfile>().Create(new {
                PublicWaterSupply = publicWaterSupplyX
            });

            GetEntityFactory<SampleSiteProfile>().Create(new {
                PublicWaterSupply = publicWaterSupplyX
            });

            GetEntityFactory<SampleSiteProfile>().Create(new {
                PublicWaterSupply = publicWaterSupplyY
            });

            var actionResult = (CascadingActionResult)_target.ByPublicWaterSupply(publicWaterSupplyX.Id);
            var sampleSiteProfileDisplayItems = ((IEnumerable<SampleSiteProfileDisplayItem>)actionResult.Data).ToList();

            Assert.AreEqual(2, sampleSiteProfileDisplayItems.Count);
        }

        #endregion
    }
}
