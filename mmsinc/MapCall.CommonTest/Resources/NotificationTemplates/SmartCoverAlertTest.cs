using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class SmartCoverAlertTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestSmartCoverAlertAcknowledgementNotification()
        {
            var sewerOpening = new SewerOpening {
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" },
                Town = new Town {
                    ShortName = "Long Branch",
                    State = new State {
                        Name = "New Jersey",
                        Abbreviation = "NJ"
                    }
                },
                OpeningNumber = "1234567",
                Id = 42,
                RecordUrl = "https://mapcall.amwater.com/modules/mvc/FieldOperations/SewerOpening/Show/42"
            };

            var model = new SmartCoverAlert {
                Id = 1,
                SewerOpening = sewerOpening,
                Acknowledged = true,
                DateReceived = new DateTime(2021, 11, 11, 10, 23, 59),
                RecordUrl = "https://mapcall.amwater.com/modules/mvc/FieldOperations/SmartCoverAlert/Show/1"
            };

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.SmartCoverAlertAcknowledged.cshtml";
            var template = RenderTemplate(streamPath, model);

            Assert.AreEqual(@"<h2>MapCall Notification - Smart Cover Alert Acknowledged</h2>

Alert Id: <a href=""https://mapcall.amwater.com/modules/mvc/FieldOperations/SmartCoverAlert/Show/1"">1</a><br/>
State: NJ<br/>
Operating Center: NJ7 - Shrewsbury<br/>
Town: Long Branch<br/>
Sewer Opening Number: <a href=""https://mapcall.amwater.com/modules/mvc/FieldOperations/SewerOpening/Show/42"">1234567</a><br/>
Date Received: 11/11/2021 10:23:59 AM<br/>
Acknowledged: True<br/>", template);
        }
    }
}
