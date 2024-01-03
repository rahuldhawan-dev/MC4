using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class FieldServiceHydrantTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestHydrantNotification()
        {
            var hydrant = new Hydrant {
                Town = new Town {ShortName = "Camden"},
                FireDistrict = new FireDistrict {
                    DistrictName = "TestDistrict", UtilityDistrict = 1, UtilityName = "TestUtility", PremiseNumber = "1"
                },
                BillingDate = null,
                DateRetired = null,
                HydrantNumber = "5",
                Status = new AssetStatus {Description = "Test", Id = 1},
                Street = new Street {FullStName = "Water St."},
                CrossStreet = new Street {FullStName = "Victor St."},
                WorkOrderNumber = "MC-1631"
            };
            var model = new HydrantNotification {
                Hydrant = hydrant, UserEmail = "yes",
                UserName = "no"
            };

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.Hydrant.cshtml";
            var template = RenderTemplate(streamPath, model);
            var expected = @"
<h2>Hydrant Billing Adjustment Notification</h2>

<a>View on Site</a><br />

Town: Camden<br />
Fire District: TestDistrict<br />
Utility District: 1<br />
Billing Party: TestUtility<br />
Billing Start Date: <br />
Date Retired: <br />
Premise Number: 1<br />
Hydrant Number: 5<br />
Hydrant Status: Test<br />
Street: Water St.<br />
Cross Street: Victor St.<br />
WBS #: MC-1631<br />

Total # of Hydrants to be billed to this premise after this change: 0<br />
<br />
This report is being provided by the Operations to advise Billing of a change in the total number of hydrants to be billed to the above premise.<br />

UserName: no<br />
Email: yes<br />";

            Assert.AreEqual(expected, template);
        }
    }
}
