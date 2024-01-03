using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.Common.Model.Mappings;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class OperationsTrainingTrainingRecordTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestTrainingRecordNotification()
        {
            var dataType1 = new DataType {
                TableName = TrainingRecordMap.TABLE_NAME, Name = TrainingRecord.DataTypeNames.EMPLOYEES_ATTENDED
            };
            var dataType2 = new DataType {
                TableName = TrainingRecordMap.TABLE_NAME, Name = TrainingRecord.DataTypeNames.EMPLOYEES_SCHEDULED
            };
            var model = new TrainingRecord {
                ClassLocation = new ClassLocation {
                    Description = "Conference Room",
                    OperatingCenter = new OperatingCenter
                        {OperatingCenterCode = "HT", OperatingCenterName = "Happy Times"}
                },
                HeldOn = new DateTime(2003, 6, 27, 12, 30, 00),
                Instructor = new Employee {FirstName = "Delores", LastName = "Herbig", EmployeeId = "123"},
                SecondInstructor = new Employee {FirstName = "Georgia", LastName = "Lass", EmployeeId = "321"},
                TrainingModule = new TrainingModule {TotalHours = 15},
                RecordUrl = "http://recordUrl"
            };
            model.EmployeesAttended.Add(new TrainingRecordAttendedEmployee
                {Employee = new Employee {FirstName = "Daisy", LastName = "Adair", EmployeeId = "124"}});
            model.EmployeesAttended.Add(new TrainingRecordAttendedEmployee
                {Employee = new Employee {FirstName = "Georgia", LastName = "Lass", EmployeeId = "125"}});
            model.TrainingSessions.Add(new TrainingSession {
                EndDateTime = new DateTime(2003, 6, 27, 12, 30, 00),
                StartDateTime = new DateTime(2003, 6, 27, 12, 00, 00)
            });
            var streamPath =
                "MapCall.Common.Resources.NotificationTemplates.Operations.TrainingRecords.TrainingRecord.cshtml";
            var template = RenderTemplate(streamPath, model);

            Assert.AreEqual(@"<h2>Training Record</h2>

Scheduled Date:  <br/>
Training Sessions: <br/>
    <span>&nbsp;&nbsp;&nbsp;&nbsp;6/27/2003 12:00:00 PM -- 6/27/2003 12:30:00 PM</span><br/>
Held On: 6/27/2003 <br />
Instructor: Delores Herbig <br />
Second Instructor: Georgia Lass <br />
Training Module Title:  <br />
Total Hours: 15 <br />
Class Location: HT - Conference Room <br />

http://recordUrl

    <h3>Attendees (2)</h3>
Adair, Daisy : 124        <br/>
Lass, Georgia : 125        <br/>
", template);
        }
    }
}
