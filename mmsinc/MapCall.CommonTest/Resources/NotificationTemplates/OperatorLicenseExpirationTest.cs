using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class OperatorLicenseExpirationTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestOperatorLicenseExpirationNotification()
        {
            var operatorLicense = new OperatorLicense {
                Id = 42, 
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "a1", OperatingCenterName = "Acme"},
                OperatorLicenseType = new OperatorLicenseType { Description = "Type 01" },
                Employee = new Employee { FirstName = "Bugs", LastName = "Bunny", EmployeeId = "x2" },
                LicenseLevel = "license level for test",
                LicenseSubLevel = "license sub level for test",
                LicenseNumber = "license number for test",
                LicensedOperatorOfRecord = true,
                ExpirationDate = DateTime.Now,
                RecordUrl = "http://recordUrl"
            };

            var template = RenderTemplate(
                "MapCall.Common.Resources.NotificationTemplates.HumanResources.Employee.OperatorLicenseExpiration.cshtml",
                operatorLicense);

            MyAssert.StringsAreEqual(
$@"Employee: <a href=""http://recordUrl"">{operatorLicense.Employee.FullName}</a><br />
Employee Id: {operatorLicense.Employee.EmployeeId}<br />
Operator License Type: {operatorLicense.OperatorLicenseType}<br />
Licensed Operator of Record: {operatorLicense.LicensedOperatorOfRecord}<br />
License Level/Class: {operatorLicense.LicenseLevel}<br />
License Sub-level/Sub-class: {operatorLicense.LicenseSubLevel}<br />
License Number: {operatorLicense.LicenseNumber}<br />
Operating Center: {operatorLicense.OperatingCenter}<br />
Expiration Date: {operatorLicense.ExpirationDate}", template);
        }
    }
}
