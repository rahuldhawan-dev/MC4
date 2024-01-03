using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class FieldServicesValvesTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestValveNotification()
        {
            var valve = new Valve {
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"},
                Town = new Town {ShortName = "Long Branch"},
                Street = new Street {FullStName = "Easy St."},
                CrossStreet = new Street {FullStName = "Uptown Blvd."},
                ValveNumber = "VLB-1",
                Status = new AssetStatus {Description = "Active"},
                WorkOrderNumber = "MC-1631",
                Initiator = new User {FullName = "Bill Preston", UserName = "bpreston", Email = "bpreston@nowhere.com"}
            };
            valve.SetPropertyValueByName("Id", 101);
            var model = new ValveNotification {Valve = valve, UserEmail = "a@b.c", UserName = "abc"};

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.Valve.cshtml";
            var template = RenderTemplate(streamPath, model);

            Assert.AreEqual(@"<h2>Valve Status Change Notification</h2>

<a>View on Site</a><br />

Operating Center: NJ7 - Shrewsbury<br/>
Town: Long Branch<br />
Valve Number: VLB-1<br />
Valve Status: Active<br />
Street: Easy St.<br />
Cross Street: Uptown Blvd.<br />
WBS #: MC-1631<br />
    
        Initiator UserName: bpreston<br />
        Initiator Name: Bill Preston<br />
        Initiator Email: bpreston@nowhere.com<br />
    
<br />
Updated By:<br/>
UserName: abc<br />
Email: a@b.c<br />", template);
        }

        [TestMethod]
        public void TestValveNotificationDoesNotCrashAndBurnBecauseOneOf200000ValvesDoesNotHaveAnInitator()
        {
            var valve = new Valve {
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"},
                Town = new Town {ShortName = "Long Branch"},
                Street = new Street {FullStName = "Easy St."},
                CrossStreet = new Street {FullStName = "Uptown Blvd."},
                WorkOrderNumber = "MC-1631",
                ValveNumber = "VLB-1",
                Status = new AssetStatus {Description = "Active"},
                Initiator = null
            };
            valve.SetPropertyValueByName("Id", 101);
            var model = new ValveNotification {Valve = valve, UserEmail = "b@c.com", UserName = "bccom"};
            var streamPath = "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.Valve.cshtml";
            var template = RenderTemplate(streamPath, model);

            Assert.AreEqual(@"<h2>Valve Status Change Notification</h2>

<a>View on Site</a><br />

Operating Center: NJ7 - Shrewsbury<br/>
Town: Long Branch<br />
Valve Number: VLB-1<br />
Valve Status: Active<br />
Street: Easy St.<br />
Cross Street: Uptown Blvd.<br />
WBS #: MC-1631<br />
<br />
Updated By:<br/>
UserName: bccom<br />
Email: b@c.com<br />", template);
        }
    }
}
