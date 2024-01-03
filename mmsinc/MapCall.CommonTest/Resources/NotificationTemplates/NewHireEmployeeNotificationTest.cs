using System;
using System.IO;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using RazorEngine;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class NewHireEmployeeNotificationTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.HumanResources.Employee.{0}.cshtml";

        #region Tests

        [TestMethod]
        public void TestNewEmployeeHireNotificationWorksWithSomeMissingValues()
        {
            var model = new Employee {Id = 2, EmployeeId = "1"};

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "NewHireEmailNotification", model);

            Assert.AreEqual(@"
<h2>New Hire Email Notification</h2>

Employee: 
<br />
Employee Id: 1
<br />
Date of Hire: 
<br />
Position Group: 
<br />
Supervisor: ", template);
        }

        [TestMethod]
        public void TestNewEmployeeHireNotification()
        {
            var unit = new SAPCompanyCode();
            var positionGroup = new PositionGroup {
                Id = 1,
                SAPPositionGroupKey = "Key1",
                PositionDescription = "Some Description",
                Group = "special force",
                BusinessUnit = "42",
                SAPCompanyCode = unit,
                BusinessUnitDescription = "answers"
            };

            var sampleDate = new DateTime(2003, 6, 27, 12, 30, 00);
            var supervisor = new Employee
                {EmailAddress = "foo@fighters.com", FirstName = "Rube", LastName = "Sofer", EmployeeId = "123"};
            var model = new Employee {
                Id = 123,
                LastName = "Jones",
                FirstName = "Mr.",
                MiddleName = "Q",
                EmployeeId = "123456",
                ReportsTo = supervisor,
                DateHired = sampleDate,
                PositionGroup = positionGroup
            };

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "NewHireEmailNotification", model);

            Assert.AreEqual(@"
<h2>New Hire Email Notification</h2>

Employee: Mr. Q Jones
<br />
Employee Id: 123456
<br />
Date of Hire: 6/27/2003 12:30:00 PM
<br />
Position Group: special force - Some Description - 42 - answers -  - Key1
<br />
Supervisor: Rube Sofer", template);
        }

        #endregion
    }
}
