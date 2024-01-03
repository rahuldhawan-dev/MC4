using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class ServiceLineProtectionTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.ServiceLineProtection.{0}.cshtml";

        [TestMethod]
        public void TestInvestigationCreatedNotification()
        {
            var model = new {
                Id = 42,
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"},
                CustomerCity = new Town {ShortName = "SHORT NAME"},
                CustomerServiceMaterial = new Common.Model.Entities.ServiceMaterial {Description = "Lead"},
                RecordUrl = "Foo/1",
                CustomerServiceSize = new Common.Model.Entities.ServiceSize
                    {Size = 1.5m, ServiceSizeDescription = "1 1/2"}
            };
            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "ServiceLineProtectionInvestigationCreated", model);

            Assert.AreEqual(@"<h2>Service Line Protection - Investigation Created</h2>

Operating Center: NJ4 - Lakewood <br />
City: SHORT NAME <br/>
Customer Service Material: Lead<br/>
Customer Service Size: 1 1/2<br/>
Foo/1", template);
        }
    }
}
