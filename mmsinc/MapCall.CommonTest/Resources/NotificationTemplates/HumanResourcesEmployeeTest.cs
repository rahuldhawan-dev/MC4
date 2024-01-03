using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class HumanResourcesEmployeeTest : BaseNotificationTest
    {
        [TestMethod]
        public void TestMedicalCertificateExpiration()
        {
            var dueDate = DateTime.Now;
            var id = "12345678";
            var firstName = "John";
            var lastName = "Smith";
            var model = new Employee {
                EmployeeId = id,
                FirstName = firstName,
                LastName = lastName,
                MedicalCertificateExpirationDate = dueDate
            };
            var template =
                RenderTemplate(
                    "MapCall.Common.Resources.NotificationTemplates.HumanResources.Employee.MedicalCertificateExpiration.cshtml",
                    model);

            Assert.AreEqual(String.Format(@"Employee: {0}, {1}<br/>
Employee Id: {2}<br/>

Expiration Date: {3}", lastName, firstName, id, dueDate.ToShortDateString()), template);
        }
    }
}
