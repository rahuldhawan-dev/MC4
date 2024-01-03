using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class TrafficControlTicketEnteredTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.FieldServices.WorkManagement.{0}.cshtml";

        [TestMethod]
        public void TestNotification()
        {
            var model = new TrafficControlTicket {
                InvoiceDate = new DateTime(1980, 1, 1),
                InvoiceNumber = "123-ABC",
                InvoiceAmount = 125m,
                BillingParty = new BillingParty {EstimatedHourlyRate = 125m},
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"},
                Town = new Town {ShortName = "Silent Hill"},
                SAPWorkOrderNumber = 43,
                WorkStartDate = new DateTime(1980, 1, 2),
                WorkEndDate = new DateTime(1980, 1, 3),
                TotalHours = 1,
                NumberOfOfficers = 1,
                RecordUrl = "https://234231",
                PaidByNJAW = false
            };
            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "TrafficControlTicketEntered", model);

            Assert.AreEqual(@"<h2>Traffic Control Ticket - Entered</h2>

Operating Center: NJ7 - Shrewsbury<br/>
Town: Silent Hill<br />
SAP WorkOrder: 43<br />
MapCall WorkOrder: <br />
Work Start: 1/2/1980 12:00:00 AM<br />
Work End: 1/3/1980 12:00:00 AM<br />
Total Hours: 1<br />
Number of Officers: 1<br />

https://234231<br />", template);
        }
    }
}
