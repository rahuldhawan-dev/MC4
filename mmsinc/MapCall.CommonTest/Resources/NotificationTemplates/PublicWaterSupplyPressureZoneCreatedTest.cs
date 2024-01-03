using System;
using System.IO;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using RazorEngine;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class PublicWaterSupplyPressureZoneCreatedTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.Environmental.WaterSystems.{0}.cshtml";

        #region Tests

        #region Notification Tests

        [TestMethod]
        public void TestPublicWaterSupplyPressureZoneCreatedNotification()
        {

            var model = new PublicWaterSupplyPressureZone
            {
                RecordUrl = "http://recordUrl",
                Id = 123,
                PublicWaterSupply = new PublicWaterSupply {
                    Identifier = "pws-01",
                    OperatingArea = "area-51",
                    System = "sys-01",
                    RecordUrl = "http://recordUrlForPWS",
                },
            };

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "PublicWaterSupplyPressureZoneCreated", model);

            Assert.AreEqual(@"PWSID: <a href=""http://recordUrlForPWS"">pws-01 - sys-01</a><br />
Pressure Zone: <a href=""http://recordUrl""></a>
", template);
        }

        #endregion

        #endregion
   } 
}
