using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class ChemicalSampleSiteStatusChangedTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestChemicalSampleSiteStatusChangedNotification()
        {
            var sampleSite = new SampleSite {
                Id = 10,
                CommonSiteName = "samples-r-us",
                Status = new SampleSiteStatus { Description = "Active", Id = 1 },
                PublicWaterSupply = new PublicWaterSupply {
                    Identifier = "pws-01",
                    OperatingArea = "area-51",
                    System = "sys-01"
                },
                RecordUrl = "http://recordUrl"
            };

            var renderedTemplate = RenderTemplate("MapCall.Common.Resources.NotificationTemplates.WaterQuality.General.ChemicalSampleSiteStatusChanged.cshtml", sampleSite);

            const string expectedTemplate = @"<h2>Sample Site Change to samples-r-us</h2>

<strong>Forward this email to Central Lab if this site is a compliance site that would affect a schedule.</strong>

Sample Site: <a href=""http://recordUrl"">samples-r-us</a><br />
Public Water Supply: pws-01 - area-51 - sys-01<br />
Status: Active";

            Assert.AreEqual(expectedTemplate, renderedTemplate);
        }
    }
}
