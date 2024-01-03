using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class WasteWaterSystemBasinCreatedTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.Environmental.WasteWaterSystems.{0}.cshtml";

        #region Tests

        #region WasteWaterSystemBasin Created

        [TestMethod]
        public void TestWasteWaterSystemBasinCreatedNotification()
        {
            var model = new WasteWaterSystemBasin {
                RecordUrl = "http://recordUrl",
                Id = 224,
                BasinName = "the name",
                WasteWaterSystem = new WasteWaterSystem {
                    OperatingCenter = new OperatingCenter
                        {OperatingCenterCode = "NJ", OperatingCenterName = "Shrewsbury"},
                    WasteWaterSystemName = "Prince William Sewage",
                    PermitNumber = "NJ0123456",
                    RecordUrl = "http://wwsRecordUrl",
                    Id = 224
                },
            };

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "WasteWaterSystemBasinCreated", model);

            Assert.AreEqual(@"WWSID: <a href=""http://wwsRecordUrl"">NJWW0224 - Prince William Sewage</a><br />
Water Basin Name: <a href=""http://recordUrl"">the name</a>", template);
        }

        #endregion

        #endregion
    }
}
