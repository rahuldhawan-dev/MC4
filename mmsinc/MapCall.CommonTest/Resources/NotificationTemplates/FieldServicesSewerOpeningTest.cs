using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class FieldServicesSewerOpeningTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestSewerOpeningNotification()
        {
            var opening = new SewerOpening {
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"},
                Town = new Town {ShortName = "Camden"},
                OpeningNumber = "123",
                Status = new AssetStatus {Description = "Active"},
                Street = new Street {FullStName = "Water St."},
                TaskNumber = "ABC",
                SewerOpeningType = new SewerOpeningType {Description = "Manhole"}
            };
            var model = new SewerOpeningNotification {
                SewerOpening = opening,
                UserEmail = "Nobody",
                UserName = "NoOne"
            };

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.SewerOpening.cshtml";

            var template = RenderTemplate(streamPath, model);

            var expected = @"<h2>Opening Status Change Notification</h2>

<a>View on Site</a><br />

Operating Center: NJ7 - Shrewsbury<br />
Town: Camden<br />
Opening Number: 123<br />
Opening Type: Manhole<br />
Opening Status: Active<br />
Street: Water St.<br />
WBS #: ABC<br />
Updated By:<br />
UserName: NoOne<br />
Email: Nobody<br />";

            Assert.AreEqual(expected, template);
        }

        [TestMethod]
        public void TestSewerOpeningNotificationWhenStreetAndStatusAreNull()
        {
            var opening = new SewerOpening {
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"},
                Town = new Town {ShortName = "Camden"},
                OpeningNumber = "123",
                Status = null,
                Street = null,
                TaskNumber = "ABC"
            };

            var model = new SewerOpeningNotification {
                SewerOpening = opening,
                UserEmail = "Nobody",
                UserName = "NoOne"
            };

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.SewerOpening.cshtml";

            var template = RenderTemplate(streamPath, model);

            var expected = @"<h2>Opening Status Change Notification</h2>

<a>View on Site</a><br />

Operating Center: NJ7 - Shrewsbury<br />
Town: Camden<br />
Opening Number: 123<br />
Opening Type: <br />
Opening Status: <br />
Street: <br />
WBS #: ABC<br />
Updated By:<br />
UserName: NoOne<br />
Email: Nobody<br />";

            Assert.AreEqual(expected, template);
        }
    }
}
