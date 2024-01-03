using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class FieldServicesServiceLargeOrFireTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestNotificationIsGeneratedCorrectly()
        {
            var service = new Service {
                ServiceNumber = 1234567890,
                PremiseNumber = "321",
                OperatingCenter = new OperatingCenter
                    {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsburghy"},
                Town = new Town {ShortName = "Shrewsbury", FullName = "Shrewbury Twp"},
                DateInstalled = new DateTime(1980, 12, 8),
                Street = new Street {FullStName = "Main St"},
                StreetNumber = "123",
                ServiceSize = new ServiceSize {ServiceSizeDescription = "12\""},
                Name = "Lennon Construction",
                State = new State {Abbreviation = "NJ"}
            };
            var email = "someone@somesite.com";
            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.FieldServices.Assets.ServiceLargeOrFire.cshtml";
            var template = RenderTemplate(streamPath, new ServiceNotification {Service = service});
            Assert.AreEqual(@"

<h2>Service Installation Notification - Large Service or Fire</h2>

Service #: 1234567890<br />
Premise #: 321<br />
OpCode: NJ7 - Shrewsburghy<br />
Category of Service: <br />
Date Installed : 12/8/1980 12:00:00 AM<br />
Street : <br />
Town: Shrewsbury<br />
Size of Service: 12&quot;<br />
Customer Name: Lennon Construction<br />
Email: <br />",
                template);
        }
    }
}
