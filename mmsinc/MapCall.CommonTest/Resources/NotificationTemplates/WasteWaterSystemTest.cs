using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class WasteWaterSystemTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.Environmental.WasteWaterSystems.{0}.cshtml";

        #region Tests

        #region WasteWaterSystem Created

        [TestMethod]
        public void TestWasteWaterSystemCreatedNotification()
        {
            var model = new WasteWaterSystem {
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "NJ", OperatingCenterName = "Shrewsbury" },
                WasteWaterSystemName = "Prince William Sewage",
                PermitNumber = "NJ0123456",
                RecordUrl = "http://recordUrl",
                Id = 224
            };
            
            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "WasteWaterSystemCreated", model);

            Assert.AreEqual(@"<h2>Waste Water System Created</h2>

Waste Water System Id: <a href=""http://recordUrl"">224</a><br />
Operating Center: NJ - Shrewsbury<br />
WWSID: NJWW0224 - Prince William Sewage<br />
Waste Water System Name: Prince William Sewage<br />
Permit Number: NJ0123456", template);
        }
        
        [TestMethod]
        public void TestWasteWaterSystemCreatedNotificationWhenParametersAreNull()
        {
            var model = new WasteWaterSystem {
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "NJ", OperatingCenterName = "Shrewsbury" },
                WasteWaterSystemName = null,
                PermitNumber = null,
                RecordUrl = "http://recordUrl",
                Id = 224
            };

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "WasteWaterSystemCreated", model);

            Assert.AreEqual(@"<h2>Waste Water System Created</h2>

Waste Water System Id: <a href=""http://recordUrl"">224</a><br />
Operating Center: NJ - Shrewsbury<br />
WWSID: NJWW0224 - <br />
Waste Water System Name: <br />
Permit Number: ", template);
        }

        #endregion

        #region WasteWaterSystem Updated

        [TestMethod]
        public void TestWasteWaterSystemUpdatedNotification()
        {
            var model = new WasteWaterSystem {
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "NJ", OperatingCenterName = "Shrewsbury" },
                WasteWaterSystemName = "Prince William Sewage",
                PermitNumber = "NJ0123456",
                RecordUrl = "http://recordUrl",
                Id = 224
            };
            
            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "WasteWaterSystemUpdated", model);

            Assert.AreEqual(@"<h2>Waste Water System Updated</h2>

Waste Water System Id: <a href=""http://recordUrl"">224</a><br />
Operating Center: NJ - Shrewsbury<br />
WWSID: NJWW0224 - Prince William Sewage<br />
Waste Water System Name: Prince William Sewage<br />
Permit Number: NJ0123456", template);
        }
        
        [TestMethod]
        public void TestWasteWaterSystemUpdatedNotificationWhenParametersAreNull()
        {
            var model = new WasteWaterSystem {
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "NJ", OperatingCenterName = "Shrewsbury" },
                WasteWaterSystemName = null,
                PermitNumber = null,
                RecordUrl = "http://recordUrl",
                Id = 224
            };

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "WasteWaterSystemUpdated", model);

            Assert.AreEqual(@"<h2>Waste Water System Updated</h2>

Waste Water System Id: <a href=""http://recordUrl"">224</a><br />
Operating Center: NJ - Shrewsbury<br />
WWSID: NJWW0224 - <br />
Waste Water System Name: <br />
Permit Number: ", template);
        }
    }

    #endregion

    #endregion
}
