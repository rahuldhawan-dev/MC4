using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class JobObservationTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.Operations.HealthAndSafety.{0}.cshtml";

        [TestMethod]
        public void TestJobObservationNotification()
        {
            var model = new JobObservation {
                RecordUrl = "http://www.something/thing/show/42",
                CreatedBy = new User {FullName = "test user"},
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"},
                ObservationDate = new DateTime(2014, 1, 1, 1, 1, 1),
                Department = new JobCategory {Description = "doing stuff"},
                Address = "location",
                TaskObserved = "description",
                Deficiencies = "none",
                OverallQualityRating = new OverallQualityRating {Description = "not too shabby"},
                OverallSafetyRating = new OverallSafetyRating {Description = "good"}
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "JobObservation", model);

            Assert.AreEqual(@"<h2>Job Observation - Created</h2>

Id: http://www.something/thing/show/42<br/>
Created By: test user<br/>
Operating Center: NJ7 - Shrewsbury<br/>
Observation Date: 1/1/2014<br/>
Department: doing stuff<br/>
Address: location<br/>
Task Observed: description<br/>
Deficiencies or Additional Comments: none<br />
Overall Safety Rating: good<br/>
Overall Quality Rating: not too shabby<br/>
", template);
        }

        [TestMethod]
        public void TestJobObservationNotificationWhenParametersAreNull()
        {
            var model = new JobObservation {CreatedBy = new User()};
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "JobObservation", model);

            Assert.AreEqual(@"<h2>Job Observation - Created</h2>

Id: <br/>
Created By: <br/>
Operating Center: <br/>
Observation Date: <br/>
Department: <br/>
Address: <br/>
Task Observed: <br/>
Deficiencies or Additional Comments: <br />
Overall Safety Rating: <br/>
Overall Quality Rating: <br/>
", template);
        }
    }
}
