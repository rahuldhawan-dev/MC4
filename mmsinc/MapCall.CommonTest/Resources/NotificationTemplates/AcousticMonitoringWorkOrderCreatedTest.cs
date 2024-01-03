using System;
using System.IO;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using RazorEngine;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class AcousticMonitoringWorkOrderCreatedTest : BaseNotificationTest
    {
        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.FieldServices.WorkManagement.{0}.cshtml";

        #region Tests

        #region Notification Tests

        [TestMethod]
        public void TestAcousticMonitoringWorkOrderCreatedNotification()
        {
            var model = new WorkOrder {
                RecordUrl = "http://recordUrl",
                Id = 123,
                OperatingCenter = new OperatingCenter
                    { OperatingCenterName = "TestOC", OperatingCenterCode = "NJ7" },
                Town = new Town { ShortName = "Anytown", State = new State { Name = "Newer Jersey" } },
                StreetNumber = "12",
                Street = new Street { FullStName = "100000 water jr st" },
                NearestCrossStreet = new Street { FullStName = "200000 water1 jr st" },
                Priority = new WorkOrderPriority { Description = "Emergency" },
                Purpose = new WorkOrderPurpose { Description = "Compliance" },
                AcousticMonitoringType = new AcousticMonitoringType { Description = "aType" },
                CreatedBy = new User { UserName = "test user" },
                WorkDescription = new WorkDescription { Description = "Work Work" },
                AssetType = new AssetType { Description = "Hydrant", Id = 2 },
                AssetId = new WorkOrderAssetId { AssetId = "12345" },
                Hydrant = new Hydrant(),
                SAPNotificationNumber = 123456,
                SAPWorkOrderNumber = 123789,
                DateReceived = Convert.ToDateTime("01/01/2009 11:52:02 AM")
            };

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "AcousticMonitoringOrderCreated", model);

            Assert.AreEqual(@"<h2>Work Order Acoustic Monitoring Order Created</h2>

Work Order #: <a href=""http://recordUrl"">123</a><br />
Operating Center: NJ7 - TestOC <br />
Town: Anytown <br />
Street #: 12 <br />
Street: 100000 water jr st <br />
Nearest Cross Street: 200000 water1 jr st <br />
Priority: Emergency <br />
Purpose: Compliance <br/>
Acoustic Monitoring Type: aType <br />
Entered By: test user <br />
Work Description: Work Work <br />
Asset Type: Hydrant <br />
Asset ID: 12345 <br />
SAP Notification #: 123456 <br />
SAP Work Order #: 123789 <br />
Date Received: 1/1/2009 11:52:02 AM <br />", template);
        }

        [TestMethod]
        public void TestAcousticMonitoringWorkOrderCreatedNotificationValuesBlank()
        {
            var model = new WorkOrder {
                RecordUrl = "http://recordUrl",
                Id = 123,
                OperatingCenter = new OperatingCenter {OperatingCenterName = "TestOC", OperatingCenterCode = "NJ7"},
                Town = new Town {ShortName = "Anytown", State = new State {Name = "Newer Jersey"}},
                StreetNumber = "12",
                Street = new Street { FullStName = "100000 water jr st"},
                NearestCrossStreet = new Street {FullStName = "200000 water1 jr st"},
                Priority = new WorkOrderPriority {Description = "Emergency"},
                Purpose = new WorkOrderPurpose {Description = "Compliance"},
                AcousticMonitoringType = new AcousticMonitoringType {Description = "aType"},
                CreatedBy = new User {UserName = "test user"},
                WorkDescription = new WorkDescription {Description = "Work Work"},
                AssetType = new AssetType {Description = "Hydrant", Id = 2},
                AssetId = new WorkOrderAssetId { AssetId = "12345" },
                Hydrant = new Hydrant()
            };

            var template = RenderTemplate(NOTIFICATION_PATH_FORMAT, "AcousticMonitoringOrderCreated", model);

            Assert.AreEqual(@"<h2>Work Order Acoustic Monitoring Order Created</h2>

Work Order #: <a href=""http://recordUrl"">123</a><br />
Operating Center: NJ7 - TestOC <br />
Town: Anytown <br />
Street #: 12 <br />
Street: 100000 water jr st <br />
Nearest Cross Street: 200000 water1 jr st <br />
Priority: Emergency <br />
Purpose: Compliance <br/>
Acoustic Monitoring Type: aType <br />
Entered By: test user <br />
Work Description: Work Work <br />
Asset Type: Hydrant <br />
Asset ID: 12345 <br />
SAP Notification #:  <br />
SAP Work Order #:  <br />
Date Received:  <br />", template);
        }

        #endregion

        #endregion
   } 
}
