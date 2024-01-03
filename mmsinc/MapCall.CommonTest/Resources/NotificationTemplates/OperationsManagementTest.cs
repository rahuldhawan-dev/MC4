using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class OperationsManagementTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.Operations.Management.{0}.cshtml";

        #region AbsenceNotifiationSupervisor

        [TestMethod]
        public void TestAbsenceNotificationSupervisorNotification()
        {
            var supervisor = new Employee {EmailAddress = "foo@fighters.com"};
            var model = new AbsenceNotification {
                Employee = new Employee {
                    EmployeeId = "123456", LastName = "Jones", FirstName = "Mr.", MiddleName = "Q",
                    ReportsTo = supervisor
                },
                LastDayOfWork = new DateTime(1980, 1, 3),
                StartDate = new DateTime(1980, 1, 4),
                EmployeeAbsenceClaim = new EmployeeAbsenceClaim {Description = "Flergh"},
                HumanResourcesNotes = "Some notes",
                EmployeeFMLANotification = new EmployeeFMLANotification {Description = "Neat"}
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "AbsenceNotificationSupervisor", model);

            Assert.AreEqual(@"<h2>Absence Notification</h2>

AbsenceNotificationID: 42<br />
Employee: Mr. Q Jones<br />
Last Day of Work: 1/3/1980<br />
Absence Start Date: 1/4/1980<br />
Employee Absence Claim: Flergh<br />
Employee Notified FMLA Process: Neat<br />
Supervisor Notes: Some notes<br />", template);
        }

        [TestMethod]
        public void TestAbsenceNotificationSupervisorNotificationWhenAllParametersAreNull()
        {
            var supervisor = new Employee {EmailAddress = "foo@fighters.com"};
            var model = new AbsenceNotification();

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "AbsenceNotificationSupervisor", model);

            Assert.AreEqual(@"<h2>Absence Notification</h2>

AbsenceNotificationID: 0<br />
Employee: <br />
Last Day of Work: <br />
Absence Start Date: <br />
Employee Absence Claim: <br />
Employee Notified FMLA Process: <br />
Supervisor Notes: <br />", template);
        }

        #endregion

        #region AbsenceNotificationEntry

        [TestMethod]
        public void TestAbsenceNotificationEntryNotification()
        {
            var supervisor = new Employee {EmailAddress = "foo@fighters.com"};
            var model = new AbsenceNotification {
                Employee = new Employee {
                    EmployeeId = "123456", LastName = "Jones", FirstName = "Mr.", MiddleName = "Q",
                    ReportsTo = supervisor
                },
                LastDayOfWork = new DateTime(1980, 1, 3),
                StartDate = new DateTime(1980, 1, 4),
                EmployeeAbsenceClaim = new EmployeeAbsenceClaim {Description = "Flergh"},
                HumanResourcesNotes = "Some notes",
                EmployeeFMLANotification = new EmployeeFMLANotification {Description = "Neat"}
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "AbsenceNotificationEntry", model);

            Assert.AreEqual(@"<h2>Absence Notification</h2>

AbsenceNotificationID: 42<br />
Employee: Mr. Q Jones<br />
Last Day of Work: 1/3/1980<br />
Absence Start Date: 1/4/1980<br />
Employee Absence Claim: Flergh<br />
Employee Notified FMLA Process: Neat<br />
Supervisor Notes: Some notes<br />", template);
        }

        [TestMethod]
        public void TestAbsenceNotificationEntryNotificationWhenAllParametersAreNull()
        {
            var supervisor = new Employee {EmailAddress = "foo@fighters.com"};
            var model = new AbsenceNotification();

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "AbsenceNotificationEntry", model);

            Assert.AreEqual(@"<h2>Absence Notification</h2>

AbsenceNotificationID: 0<br />
Employee: <br />
Last Day of Work: <br />
Absence Start Date: <br />
Employee Absence Claim: <br />
Employee Notified FMLA Process: <br />
Supervisor Notes: <br />", template);
        }

        #endregion

        [TestMethod]
        public void TestFamilyMedicalLeaveActCaseNotification()
        {
            var supervisor = new Employee {EmailAddress = "foo@fighters.com"};
            var model = new FamilyMedicalLeaveActCase {
                Employee = new Employee {
                    EmployeeId = "123456", LastName = "Jones", FirstName = "Mr.", MiddleName = "Q",
                    ReportsTo = supervisor
                },
                PackageDateDue = new DateTime(1980, 1, 1),
                PackageDateSent = new DateTime(1980, 1, 2),
                StartDate = new DateTime(1980, 1, 4),
                EndDate = new DateTime(1980, 1, 5),
                FrequencyDays = "5 days",
                CertificationExtended = false,
                CompanyAbsenceCertification = new CompanyAbsenceCertification {Description = "Blergh"},
            };
            model.SetPropertyValueByName("Id", 42);

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "FMLACase", model);

            Assert.AreEqual(@"<h2>FMLA Case</h2>

FMLA CaseID: 42<br/>
Employee: Mr. Q Jones<br/>
StartDate: 1/4/1980 12:00:00 AM<br/>
EndDate: 1/5/1980 12:00:00 AM<br/>
Frequency Days: 5 days<br/>
CertificationExtended: No<br/>
CompanyAbsenceCertification: Blergh<br/>
ChronicCondition: No<br/>
FMLAPackageDateSent: 1/2/1980 12:00:00 AM<br/>
FMLAPackageDateDue: 1/1/1980 12:00:00 AM<br/>", template);
        }
    }
}
