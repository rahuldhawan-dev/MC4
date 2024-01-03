using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using NHibernate.Engine;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class OperationsJobSiteCheckListsTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.FieldServices.WorkManagement.{0}.cshtml";

        #region Tests

        #region Job Site Check List

        [TestMethod]
        public void TestJobSiteCheckListNotifications()
        {
            var model = new JobSiteCheckList {
                RecordUrl = "https://mapcall.amwater.com/modules/mvc/HealthAndSafety/JobSiteCheckList/Show/42",
                Address = "123 Fake St.",
                CreatedBy = "David Robert Jones",
                CheckListDate = new DateTime(2024, 1, 1, 1, 1, 1),
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "OJ", OperatingCenterName = "Simpson"},
                CompetentEmployee = new Employee
                    {EmployeeId = "123456", LastName = "Jones", FirstName = "Mr.", MiddleName = "Q"},
                SAPWorkOrderId = "12345",
                HasBarricadesForTrafficControl = true,
                HasExcavationFiveFeetOrDeeper = false,
                HasExcavation = true
            };
            model.SetPropertyValueByName("Id", 42);

            // These should be ordered by date in the renderererering.
            model.Excavations.Add(new JobSiteExcavation {
                ExcavationDate = new DateTime(2024, 1, 1, 1, 1, 1),
                WidthInFeet = 2.42m,
                LengthInFeet = 4m,
                DepthInInches = 954.30m,
            });

            model.Excavations.Add(new JobSiteExcavation {
                ExcavationDate = new DateTime(2014, 1, 1, 1, 1, 1),
                WidthInFeet = 1,
                LengthInFeet = 2,
                DepthInInches = 3,
            });

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "JobSiteCheckList", model);

            Assert.AreEqual(@"<h2>Job Site Check List Completed</h2>

<a href=""https://mapcall.amwater.com/modules/mvc/HealthAndSafety/JobSiteCheckList/Show/42"">View on Site</a>

Check List Id: 42 <br/>
Entered By: David Robert Jones <br />
SAP Work Order #: 12345 <br/>
MapCall Work Order #: <br/>
Operating Center: OJ - Simpson <br/>
Description of Job: <br/>
Address: 123 Fake St. <br />
Traffic: Yes <br />
Is there an excavation?: Yes <br/>
Is Excavation Over 5 ft: N/A <br/>
Competent Person: Mr. Q Jones <br/>
Excavations: <br/>
W: 1 ft., L: 2 ft., D: 3 in. <br/>
W: 2.42 ft., L: 4 ft., D: 954.30 in. <br/>",
                template.Trim());
        }

        [TestMethod]
        public void TestJobSiteCheckListNotificationsWithWorkOrder()
        {
            var workOrder = new WorkOrder {
                WorkDescription = new WorkDescription {Description = "Flergh"},
                RecordUrl = "https://mapcall.amwater.com/modules/mvc/WorkOrder/Show/1"
            };
            workOrder.SetPropertyValueByName("Id", 1);

            var model = new JobSiteCheckList {
                RecordUrl = "https://mapcall.amwater.com/modules/mvc/HealthAndSafety/JobSiteCheckList/Show/42",
                Address = "123 Fake St.",
                CreatedBy = "David Robert Jones",
                CheckListDate = new DateTime(2024, 1, 1, 1, 1, 1),
                OperatingCenter = new OperatingCenter {OperatingCenterCode = "OJ", OperatingCenterName = "Simpson"},
                CompetentEmployee = new Employee
                    {EmployeeId = "123456", LastName = "Jones", FirstName = "Mr.", MiddleName = "Q"},
                SAPWorkOrderId = "12345",
                HasBarricadesForTrafficControl = true,
                HasExcavationFiveFeetOrDeeper = false,
                MapCallWorkOrder = workOrder,
                HasExcavation = false
            };
            model.SetPropertyValueByName("Id", 42);

            // These should be ordered by date in the renderererering.
            model.Excavations.Add(new JobSiteExcavation {
                ExcavationDate = new DateTime(2024, 1, 1, 1, 1, 1),
                WidthInFeet = 2.42m,
                LengthInFeet = 4m,
                DepthInInches = 954.30m,
            });

            model.Excavations.Add(new JobSiteExcavation {
                ExcavationDate = new DateTime(2014, 1, 1, 1, 1, 1),
                WidthInFeet = 1,
                LengthInFeet = 2,
                DepthInInches = 3,
            });

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "JobSiteCheckList", model);

            Assert.AreEqual(@"<h2>Job Site Check List Completed</h2>

<a href=""https://mapcall.amwater.com/modules/mvc/HealthAndSafety/JobSiteCheckList/Show/42"">View on Site</a>

Check List Id: 42 <br/>
Entered By: David Robert Jones <br />
SAP Work Order #: 12345 <br/>
MapCall Work Order #: <a href=""https://mapcall.amwater.com/modules/mvc/WorkOrder/Show/1"">1</a>
<br/>
Operating Center: OJ - Simpson <br/>
Description of Job: Flergh<br/>
Address: 123 Fake St. <br />
Traffic: Yes <br />
Is there an excavation?: No <br/>
Is Excavation Over 5 ft: N/A <br/>
Competent Person: Mr. Q Jones <br/>
Excavations: <br/>
W: 1 ft., L: 2 ft., D: 3 in. <br/>
W: 2.42 ft., L: 4 ft., D: 954.30 in. <br/>",
                template.Trim());
        }

        #endregion

        #endregion
    }
}
