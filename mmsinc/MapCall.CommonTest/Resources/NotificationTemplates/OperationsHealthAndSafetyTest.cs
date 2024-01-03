using System;
using System.IO;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using RazorEngine;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class OperationsHealthAndSafetyTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.Operations.HealthAndSafety.{0}.cshtml";

        #region Tests

        #region Training Record

        [TestMethod]
        public void TestTailgateTalkRecordNotification()
        {
            var model = new TailgateTalk {
                HeldOn = new DateTime(2003, 6, 27, 12, 30, 00),
                PresentedBy = new Employee {FirstName = "Rube", LastName = "Sofer", EmployeeId = "123"},
                Topic = new TailgateTalkTopic {Topic = "Extraction", OrmReferenceNumber = "1234"},
                TrainingTimeHours = 4
            };
            model.TailgateTalkEmployees.Add(new TailgateTalkEmployee
                {Employee = new Employee {FirstName = "Daisy", LastName = "Adair", EmployeeId = "124"}});
            model.TailgateTalkEmployees.Add(new TailgateTalkEmployee
                {Employee = new Employee {FirstName = "Georgia", LastName = "Lass", EmployeeId = "125"}});

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "TailgateTalk", model);

            Assert.AreEqual(@"<h2>Tailgate Talk</h2>

Held On: 6/27/2003<br />
Held By: Rube Sofer<br />
Tailgate Topic ID: 0<br />
Topic: Extraction<br />
Topic Description: 0 - Extraction<br />
Training Time Hours: 4<br />
TailgateTalkID: 0<br />
ORM Reference #: 1234<br />

<h3>Attendees (2)</h3>
Adair, Daisy : 124<br/>
Lass, Georgia : 125<br/>
", template);
        }

        #endregion

        #region ShortCycleWorkOrderSafetyBrief

        [TestMethod]
        public void TestShortCycleWorkOrderSafetyBriefNotification()
        {
            var model = new ShortCycleWorkOrderSafetyBriefNotificationViewModel {
                RecordUrl =
                    "https://mapcall.awapps.com/modules/mvc/HealthAndSafety/ShortCycleWorkOrderSafetyBrief/Show/1"
            };

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "ShortCycleSafetyBrief", model);

            Assert.AreEqual(
                @"You are receiving this email because an FSR on your team responded No to one or more of the safety questions. Please click <a href=""https://mapcall.awapps.com/modules/mvc/HealthAndSafety/ShortCycleWorkOrderSafetyBrief/Show/1"">here</a> to view the Safety Brief Form",
                template);
        }

        #endregion

        #endregion
    }
}
