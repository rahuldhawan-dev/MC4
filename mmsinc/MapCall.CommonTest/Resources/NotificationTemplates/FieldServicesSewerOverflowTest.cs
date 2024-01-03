using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class FieldServicesSewerOverflowTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestSewerOverflowNotification()
        {
            var sewerOverflow = new SewerOverflow {
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"},
                Town = new Town {ShortName = "Lakewood"},
                RecordUrl = "http://arecordUrl/show/1"
            };
            sewerOverflow.SetPropertyValueByName("Id", 121);

            var streamPath = "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.SewerOverflow.cshtml";
            var template = RenderTemplate(streamPath, sewerOverflow);

            Assert.AreEqual(@"<h2>Sewer Overflow</h2>

A sewer overflow was entered into MapCall. Please print and mail the DEP Letter.<br/>
SewerOverflowID: 121<br/>
http://arecordUrl/show/1",
                template);
        }
    }
}
