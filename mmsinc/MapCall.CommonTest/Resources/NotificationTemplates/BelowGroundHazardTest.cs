using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class BelowGroundHazardTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestBelowGroundHazardNotification()
        {
            var belowGroundHazard = new BelowGroundHazard {
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" },
                Town = new Town { 
                    ShortName = "Long Branch", 
                    State = new State {
                        Name = "New Jersey",
                        Abbreviation = "NJ"
                    }
                },
                HazardType = new HazardType { Description = "Electrical" },
                AssetStatus = new AssetStatus { Description = "ACTIVE" },
                Id = 101
            };

            var model = new BelowGroundHazardNotification {
                BelowGroundHazard = belowGroundHazard,
                CreatedBy = "abc",
                CreatedOn = new DateTime(2021, 11, 11),
                RecordUrl = "https://mapcall.amwater.com/modules/mvc/Facilities/BelowGroundHazard/Show/42"
            };

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.BelowGroundHazardCreated.cshtml";
            var template = RenderTemplate(streamPath, model);

            Assert.AreEqual(@"<h2>MapCall Notification - Below Ground Hazard Created</h2>

<a href=""https://mapcall.amwater.com/modules/mvc/Facilities/BelowGroundHazard/Show/42"">""New Below Ground Hazard Created: abc: 11/11/2021 12:00:00 AM""</a><br/>

ID: 101<br/>
State: NJ<br/>
Operating Center: NJ7 - Shrewsbury<br/>
Town: Long Branch<br/>
Hazard Type: Electrical<br/>
Asset Status: ACTIVE<br/>", template);
        }
    }
}
