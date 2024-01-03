using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.DesignPatterns;
using RazorEngine;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    [TestClass]
    public class FieldServicesWorkManagementTest
    {
        #region Constants

        public const int WORK_ORDER_ID = 123;

        public const string OPERATING_CENTER = "some op cntr",
                            TOWN = "foo town",
                            STATE = "NJ",
                            STREET_NUMBER = "123",
                            STREET = "south st.",
                            NEAREST_CROSS_STREET = "west st.",
                            ASSET_TYPE = "pork bellies",
                            ASSET_ID = "123",
                            PREMISE_NUMBER = "12345678",
                            REQUESTER = "somebody",
                            PURPOSE = "we had the extra money",
                            WORK_DESCRIPTION = "gold water mains",
                            NOTES = "nope, chuck testa",
                            CUSTOMER_IMPACT = "OH NOES!!!",
                            REPAIR_TIME = "FOR.EV.ER",
                            MARKOUT_REQUIREMENT = "Emergency",
                            PRIORITY = "Emergency",
                            ALERT_ID = "9001";

        public const bool TRAFFIC_IMPACT = true;

        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.FieldServices.WorkManagement.{0}.cshtml";

        #endregion

        #region Private Members

        private Assembly _notificationAssembly;

        #endregion

        #region Properties

        protected Assembly NotificationAssembly
        {
            get { return _notificationAssembly ?? (_notificationAssembly = typeof(RazorNotifier).Assembly); }
        }

        #endregion

        #region Private Methods

        private void TestNotification(string name, object data, string expectedFormat, params object[] args)
        {
            var streamPath = string.Format(NOTIFICATION_PATH_FORMAT, name);
            string template;

            using (var stream = NotificationAssembly.GetManifestResourceStream(streamPath))
            {
                if (stream == null)
                {
                    Assert.Fail("Could not stream template at location {0}", streamPath);
                }

                using (var reader = new StreamReader(stream))
                {
                    template = reader.ReadToEnd();
                }
            }

            Assert.AreEqual(string.Format(expectedFormat, args),
                Razor.Parse(template, data));
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMarkoutDamageNotificationCanHandleAllTheNullsThrownAtIt()
        {
            var model = new MarkoutDamage();
            TestNotification("MarkoutDamage", model,
                @"<a>New Markout Damage Record Added</a>

 : 1/1/0001 12:00:00 AM <br />
ID: 0 <br/>
Operating Center:  <br/>
Entered by:  <br/>
Request Number:  <br/>
Damage Date and Time: 1/1/0001 12:00:00 AM <br/>
Town:  <br/>
Dig Street:  <br/>
Nearest Cross Street:  <br/>
Damage Comments:  <br/>
Damage To?:  <br/>
Was it marked out?: No <br/>
Was it mis-marked?: No <br/>
Excavator discovered damage?: No <br/>
Excavator caused damage?: No <br/>
Was 911 called?: No <br/>
Were pictures taken?: No <br/>");
        }

        [TestMethod]
        public void TestMarkoutDamageNotification()
        {
            var model = new MarkoutDamage {
                OperatingCenter =
                    new OperatingCenter {OperatingCenterCode = "OJ", OperatingCenterName = "Simpson"},
                CreatedBy = "some user",
                CreatedAt = new System.DateTime(2014, 1, 1, 1, 1, 1),
                DamageOn = new System.DateTime(2014, 2, 2, 2, 2, 2),
                Town = new Common.Model.Entities.Town {ShortName = "SHORT NAME"},
                Street = "Some street",
                NearestCrossStreet = "Some other street",
                DamageComments = "I DID THIS",
                MarkoutDamageToType = new MarkoutDamageToType {Description = "EVERYTHING!"},
                IsMarkedOut = true,
                IsMismarked = false,
                ExcavatorDiscoveredDamage = true,
                ExcavatorCausedDamage = false,
                Was911Called = true,
                WerePicturesTaken = false,
                RecordUrl = "http://www.someurl.com",
                RequestNumber = "123456"
            };

            model.SetPropertyValueByName("Id", 42);

            TestNotification("MarkoutDamage", model,
                @"<a href=""http://www.someurl.com"">New Markout Damage Record Added</a>

some user : 1/1/2014 1:01:01 AM <br />
ID: 42 <br/>
Operating Center: OJ - Simpson <br/>
Entered by: some user <br/>
Request Number: 123456 <br/>
Damage Date and Time: 2/2/2014 2:02:02 AM <br/>
Town: SHORT NAME <br/>
Dig Street: Some street <br/>
Nearest Cross Street: Some other street <br/>
Damage Comments: I DID THIS <br/>
Damage To?: EVERYTHING! <br/>
Was it marked out?: Yes <br/>
Was it mis-marked?: No <br/>
Excavator discovered damage?: Yes <br/>
Excavator caused damage?: No <br/>
Was 911 called?: Yes <br/>
Were pictures taken?: No <br/>");
        }

        #region Markout Damages SIF or SIF-P Event

        [TestMethod]
        public void TestMarkoutDamageSIFOrSIFPEventNotification()
        {
            var model = new MarkoutDamage {
                Id = 42,
                OperatingCenter =
                    new OperatingCenter {OperatingCenterCode = "NJX", OperatingCenterName = "Some Center"},
                DamageOn = new DateTime(2014, 2, 2, 2, 2, 2),
                Town = new Town {ShortName = "SHORT NAME"},
                Street = "Some street",
                NearestCrossStreet = "Some other street",
                DamageComments = "I DID THIS",
                MarkoutDamageToType = new MarkoutDamageToType {Description = "EVERYTHING!"},
                RecordUrl = "http://www.someurl.com",
                RequestNumber = "123456",
                WorkOrder = new WorkOrder {
                    Id = 27
                },
                EmployeesOnJob = "John, Jacob, Jingle, and Heimerschmidt",
                UtilityDamages = new List<MarkoutDamageUtilityDamageType> {
                    new MarkoutDamageUtilityDamageType { Description = "Sewer" },
                    new MarkoutDamageUtilityDamageType { Description = "Water" }
                }
            };

            TestNotification("MarkoutDamageSIFOrSIFPEvent", model,
                @"<a href=""http://www.someurl.com"">Markout Damage - SIF or SIF-P Event</a>

Markout Damage ID: 42 <br />
Work Order ID: 27 <br />
Employees on Job: John, Jacob, Jingle, and Heimerschmidt <br />
Request Number: 123456 <br />
Damage Date and Time: 2/2/2014 2:02:02 AM <br />
Operating Center: NJX - Some Center <br />
Town: SHORT NAME <br />
Street: Some street <br />
Nearest Cross Street: Some other street <br />
Damage Comments: I DID THIS <br />
Utilities Damaged: Sewer, Water <br />");
        }

        [TestMethod]
        public void TestMarkoutDamageSIFEventNotificationCanHandleAllTheNullsThrownAtIt()
        {
            var model = new MarkoutDamage();
            TestNotification("MarkoutDamageSIFOrSIFPEvent", model,
                @"<a>Markout Damage - SIF or SIF-P Event</a>

Markout Damage ID: 0 <br />
Work Order ID:  <br />
Employees on Job:  <br />
Request Number:  <br />
Damage Date and Time: 1/1/0001 12:00:00 AM <br />
Operating Center:  <br />
Town:  <br />
Street:  <br />
Nearest Cross Street:  <br />
Damage Comments:  <br />
Utilities Damaged:  <br />");
        }

        #endregion

        [TestMethod]
        public void TestCurbPitComplianceNotification()
        {
            var model = new OldTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithAssetType(ASSET_TYPE)
                       .WithAssetID(ASSET_ID)
                       .WithPremiseNumber(PREMISE_NUMBER)
                       .WithRequester(REQUESTER)
                       .WithPurpose(PURPOSE)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithNotes(NOTES)
                       .Build();

            TestNotification("Curb-PitCompliance", model,
                @"<h2>Work Order Completion Notification</h2>

Work Order #: {0} <br />
Town: {1} <br />
Street #: {2} <br />
Street: {3} <br />
Asset Type: {4} <br />
Asset ID: {5} <br />
Premise #: {6} <br />
Requested By: {7} <br />
Purpose: {8} <br />
Work Description: {9} <br />
Notes:<br />
{10}",
                WORK_ORDER_ID,
                TOWN,
                STREET_NUMBER,
                STREET,
                ASSET_TYPE,
                ASSET_ID,
                PREMISE_NUMBER,
                REQUESTER,
                PURPOSE,
                WORK_DESCRIPTION,
                NOTES);
        }

        [TestMethod]
        public void TestCurbPitEstimateNotification()
        {
            var model = new OldTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithAssetType(ASSET_TYPE)
                       .WithAssetID(ASSET_ID)
                       .WithPremiseNumber(PREMISE_NUMBER)
                       .WithRequester(REQUESTER)
                       .WithPurpose(PURPOSE)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithNotes(NOTES)
                       .Build();

            TestNotification("Curb-PitEstimate", model,
                @"<h2>Work Order Completion Notification</h2>

Work Order #: {0} <br />
Town: {1} <br />
Street #: {2} <br />
Street: {3} <br />
Asset Type: {4} <br />
Asset ID: {5} <br />
Premise #: {6} <br />
Requested By: {7} <br />
Purpose: {8} <br />
Work Description: {9} <br />
Notes:<br />
{10}",
                WORK_ORDER_ID,
                TOWN,
                STREET_NUMBER,
                STREET,
                ASSET_TYPE,
                ASSET_ID,
                PREMISE_NUMBER,
                REQUESTER,
                PURPOSE,
                WORK_DESCRIPTION,
                NOTES);
        }

        [TestMethod]
        public void TestCurbPitRevenueNotification()
        {
            var model = new OldTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithAssetType(ASSET_TYPE)
                       .WithAssetID(ASSET_ID)
                       .WithPremiseNumber(PREMISE_NUMBER)
                       .WithRequester(REQUESTER)
                       .WithPurpose(PURPOSE)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithNotes(NOTES)
                       .Build();

            TestNotification("Curb-PitRevenue", model,
                @"<h2>Work Order Completion Notification</h2>

Work Order #: {0} <br />
Town: {1} <br />
Street #: {2} <br />
Street: {3} <br />
Asset Type: {4} <br />
Asset ID: {5} <br />
Premise #: {6} <br />
Requested By: {7} <br />
Purpose: {8} <br />
Work Description: {9} <br />
Notes:<br />
{10}",
                WORK_ORDER_ID,
                TOWN,
                STREET_NUMBER,
                STREET,
                ASSET_TYPE,
                ASSET_ID,
                PREMISE_NUMBER,
                REQUESTER,
                PURPOSE,
                WORK_DESCRIPTION,
                NOTES);
        }

        [TestMethod]
        public void TestMainBreakEnteredNotification()
        {
            var model = new NewTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithSAPWorkOrder(123456)
                       .WithOperatingCenter(OPERATING_CENTER)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithNearestCrossStreet(NEAREST_CROSS_STREET)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithEstimatedCustomerImpact(CUSTOMER_IMPACT)
                       .WithAnticipatedRepairTime(REPAIR_TIME)
                       .WithSignificantTrafficImpact(TRAFFIC_IMPACT)
                       .WithAlertID(ALERT_ID)
                       .WithNotes(NOTES)
                       .WithCreatedBy("Uh Some Guy")
                       .WithAlertIssued(true)
                       .Build();

            TestNotification("MainBreakEntered", model,
                @"<h2>Work Order Main Break Alert</h2>

Work Order #: {0} <br />
SAP Work Order #: 123456 <br />
Operating Center: {1} -  <br />
Entered By: Uh Some Guy <br />
Address: {2} {3}, {4} {5} <br />
Nearest Cross Street: {6} <br />
Work Description: {7} <br />
Estimated Customer Impact: {8} <br />
Anticipated Repair Time: {9} <br />
Significant Traffic Impact: yes <br />
Alert Issued: yes <br />
Markout Requirement: <br/>
Notes:<br />
{10}",
                WORK_ORDER_ID,
                OPERATING_CENTER,
                STREET_NUMBER,
                STREET,
                TOWN,
                model.Town.State,
                NEAREST_CROSS_STREET,
                WORK_DESCRIPTION,
                CUSTOMER_IMPACT,
                REPAIR_TIME,
                NOTES);
        }

        [TestMethod]
        public void TestMainBreakCompletedNotification()
        {
            var dateCompleted = new DateTime(1980, 12, 8);
            var model = new NewTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithOperatingCenter(OPERATING_CENTER)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithNearestCrossStreet(NEAREST_CROSS_STREET)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithEstimatedCustomerImpact(CUSTOMER_IMPACT)
                       .WithAnticipatedRepairTime(REPAIR_TIME)
                       .WithSignificantTrafficImpact(TRAFFIC_IMPACT)
                       .WithAlertID(ALERT_ID)
                       .WithNotes(NOTES)
                       .WithCreatedBy("Uh Some Guy")
                       .WithAlertIssued(true)
                       .WithDateCompleted(dateCompleted)
                       .Build();

            TestNotification("MainBreakCompleted", model,
                @"<h2>Work Order Main Break Completed Alert</h2>

Work Order #: {0} <br />
Operating Center: {1} -  <br />
Entered By: Uh Some Guy <br />
Town: {2} <br />
Street #: {3} <br />
Street: {4} <br />
Nearest Cross Street: {5} <br />
Work Description: {6} <br />
Estimated Customer Impact: {7} <br />
Anticipated Repair Time: {8} <br />
Significant Traffic Impact: yes <br />
Alert Issued: yes <br />
Markout Requirement: <br/>
Date Completed: 12/8/1980<br />",
                WORK_ORDER_ID,
                OPERATING_CENTER,
                TOWN,
                STREET_NUMBER,
                STREET,
                NEAREST_CROSS_STREET,
                WORK_DESCRIPTION,
                CUSTOMER_IMPACT,
                REPAIR_TIME,
                ALERT_ID,
                NOTES);
        }

        [TestMethod]
        public void TestServiceLineInstallationEnteredNotification()
        {
            var model = new NewTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithOperatingCenter(OPERATING_CENTER)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithNearestCrossStreet(NEAREST_CROSS_STREET)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithNotes(NOTES)
                       .WithCreatedBy("Some guy")
                       .Build();

            TestNotification("ServiceLineInstallationEntered", model,
                @"<h2>New Service Line Installation Work Order</h2>

Work Order #: {0} <br />
Operating Center: {1} -  <br />
Town: {2} <br />
Street #: {3} <br />
Street: {4} <br />
Nearest Cross Street: {5} <br />
Work Description: {6} <br />
Created By: Some guy <br />
Notes:<br />
{7}",
                WORK_ORDER_ID,
                OPERATING_CENTER,
                TOWN,
                STREET_NUMBER,
                STREET,
                NEAREST_CROSS_STREET,
                WORK_DESCRIPTION,
                NOTES);
        }

        [TestMethod]
        public void TestServiceLineRenewalEnteredNotification()
        {
            var model = new NewTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithOperatingCenter(OPERATING_CENTER)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithNearestCrossStreet(NEAREST_CROSS_STREET)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithMarkoutRequirement(MARKOUT_REQUIREMENT)
                       .WithPriority(PRIORITY)
                       .WithCreatedBy("Some guy")
                       .WithNotes(NOTES)
                       .WithRecordUrl("http://recordUrl")
                       .Build();

            TestNotification("ServiceLineRenewalEntered", model,
                @"<h2>New Service Line Renewal Work Order</h2>

Work Order #: <a href=""http://recordUrl"">{0}</a> <br />
Operating Center: {1} -  <br />
Town: {2} <br />
Street #: {3} <br />
Street: {4} <br />
Nearest Cross Street: {5} <br />
Work Description: {6} <br />
Markout Requirement: {7} <br />
Priority: {8} <br />
Created By: Some guy <br />
Notes:<br />
{9}",
                WORK_ORDER_ID,
                OPERATING_CENTER,
                TOWN,
                STREET_NUMBER,
                STREET,
                NEAREST_CROSS_STREET,
                WORK_DESCRIPTION,
                MARKOUT_REQUIREMENT,
                PRIORITY,
                NOTES);
        }

        [TestMethod]
        public void TestServiceLineRenewalCompletedNotification()
        {
            var dateCompleted = new DateTime(1980, 12, 8);
            var model = new NewTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithOperatingCenter(OPERATING_CENTER)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithNearestCrossStreet(NEAREST_CROSS_STREET)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithDateCompleted(dateCompleted)
                       .WithCreatedBy("Some guy")
                       .WithPremiseNumber(PREMISE_NUMBER)
                       .WithPreviousServiceLineMaterial("Copper")
                       .WithCustomerServiceLineMaterial("Lead")
                       .WithCompanyServiceLineMaterial("Plastic")
                       .WithPreviousServiceLineSize("1")
                       .WithCustomerServiceLineSize("2")
                       .WithCompanyServiceLineSize("3")
                       .WithNotes(NOTES)
                       .WithRecordUrl("http://recordUrl")
                       .Build();

            TestNotification("ServiceLineRenewalCompleted", model,
                @"<h2>Service Line Renewal Completed</h2>

Work Order #: <a href=""http://recordUrl"">{0}</a> <br />
Operating Center: {1} -  <br />
Town: {2} <br />
Street #: {3} <br />
Street: {4} <br />
Nearest Cross Street: {5} <br />
Work Description: {6} <br/>
Date Completed: {7} <br/>
Created By: Some guy <br/>

Premise Number: {8} <br/><br />

Previous NJAW material: Copper<br/>
Previous NJAW size: 1<br/>
Company side material: Plastic<br/>
Company side size: 3<br/>
Customer side material: Lead<br/>
Customer side size: 2<br/>
Date notice left: <br/>

Notes:<br />
{9}",
                WORK_ORDER_ID,
                OPERATING_CENTER,
                TOWN,
                STREET_NUMBER,
                STREET,
                NEAREST_CROSS_STREET,
                WORK_DESCRIPTION,
                dateCompleted,
                PREMISE_NUMBER,
                NOTES);
        }

        [TestMethod]
        public void TestSupervisorApprovalNotification()
        {
            var model = new OldTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithAssetType(ASSET_TYPE)
                       .WithAssetID(ASSET_ID)
                       .WithPremiseNumber(PREMISE_NUMBER)
                       .WithRequester(REQUESTER)
                       .WithPurpose(PURPOSE)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithNotes(NOTES)
                       .Build();

            TestNotification("SupervisorApproval", model,
                @"<h2>Work Order Completion Notification</h2>

Work Order #: {0} <br />
Town: {1} <br />
Street #: {2} <br />
Street: {3} <br />
Asset Type: {4} <br />
Asset ID: {5} <br />
Premise #: {6} <br />
Requested By: {7} <br />
Purpose: {8} <br />
Work Description: {9} <br />
Notes:<br />
{10}",
                WORK_ORDER_ID,
                TOWN,
                STREET_NUMBER,
                STREET,
                ASSET_TYPE,
                ASSET_ID,
                PREMISE_NUMBER,
                REQUESTER,
                PURPOSE,
                WORK_DESCRIPTION,
                NOTES);
        }

        [TestMethod]
        public void TestEquipmentRepairNotification()
        {
            var model = new NewTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithAssetType(ASSET_TYPE)
                        //                .WithAssetID(ASSET_ID)
                       .WithPremiseNumber(PREMISE_NUMBER)
                       .WithRequester(REQUESTER)
                       .WithPurpose(PURPOSE)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithNotes(NOTES)
                       .WithCreatedBy("James")
                       .Build();

            TestNotification("EquipmentRepair", model,
                @"<h2>Equipment Repair Notification</h2>

Work Order #: 123 <br />
Created On: 1/1/0001 12:00:00 AM <br />
Date Received:  <br/>
Created By: James <br />
Work Description: gold water mains <br />
Notes:<br />
nope, chuck testa");
        }
        
        [TestMethod]
        public void TestFRCCEmergencyCreatedNotification()
        {
            var model = new NewTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithOperatingCenter(OPERATING_CENTER)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithAssetType(ASSET_TYPE)
                        //                .WithAssetID(ASSET_ID)
                       .WithPremiseNumber(PREMISE_NUMBER)
                       .WithRequester(REQUESTER)
                       .WithPurpose(PURPOSE)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithNotes(NOTES)
                       .WithCreatedBy("James")
                       .WithRecordUrl("http://recordUrl")
                       .Build();

            TestNotification("FRCCEmergencyCreated", model,
                @"<h2>Work Order FRCC Emergency Order Created</h2>

Work Order #: <a href=""http://recordUrl"">123</a><br />
Priority:  <br/>
Purpose: we had the extra money <br/>
Operating Center: some op cntr -  <br/>
Entered By: James <br/>
Town: foo town <br/>
Street #: 123 <br/>
Street: south st. <br/>
Nearest Cross Street:  <br/>
Work Description: gold water mains <br/>
SAP Notification #:  <br/>
SAP Work Order #:  <br/>
Date Received:  <br/>
Notes:<br/>
nope, chuck testa");
        }

        [TestMethod]
        public void TestFRCCEmergencyCompletedNotification()
        {
            var dateCompleted = new DateTime(1980, 12, 8);
            var model = new NewTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithOperatingCenter(OPERATING_CENTER)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithAssetType(ASSET_TYPE)
                       .WithDateCompleted(dateCompleted)
                       .WithPremiseNumber(PREMISE_NUMBER)
                       .WithRequester(REQUESTER)
                       .WithPurpose(PURPOSE)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithNotes(NOTES)
                       .WithCreatedBy("James")
                       .WithRecordUrl("http://recordUrl")
                       .Build();

            TestNotification("FRCCEmergencyCompleted", model,
                @"<h2>Work Order FRCC Emergency Order Completed</h2>

Work Order #: <a href=""http://recordUrl"">123</a><br />
Priority:  <br/>
Purpose: we had the extra money <br/>
Operating Center: some op cntr -  <br />
Entered By: James <br />
Town: foo town <br />
Street #: 123 <br />
Street: south st. <br />
Nearest Cross Street:  <br />
Work Description: gold water mains <br />
SAP Notification #:  <br />
SAP Work Order #:  <br />
Date Received:  <br />
Completed By:  <br />
Date Completed: 12/8/1980<br />
Notes:<br />
nope, chuck testa");
        }

        [TestMethod]
        public void TestSewerOverFlowChangedNotificationWithNullValues()
        {
            var model = new NewTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithAssetType(ASSET_TYPE)
                       .WithPremiseNumber(PREMISE_NUMBER)
                       .WithRequester(REQUESTER)
                       .WithPurpose(PURPOSE)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithNotes(NOTES)
                       .WithRecordUrl("http://recordUrl")
                       .Build();

            TestNotification("SewerOverflowChanged", model,
                @"<h2>Work Order Sewer Overflow Changed</h2>

Work Order #: <a href=""http://recordUrl"">123</a><br />
Operating Center:  <br/>
Entered By:  <br/>
Town: foo town <br />
Street #: 123 <br />
Street: south st. <br />
Nearest Cross Street:  <br />
Work Description: gold water mains <br />
Notes:<br />
nope, chuck testa");
        }

        [TestMethod]
        public void TestSewerOverFlowChangedNotificationWithNoNullValues()
        {
            var model = new NewTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithTown(TOWN)
                       .WithOperatingCenter(OPERATING_CENTER)
                       .WithCreatedBy("SomeGuy")
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithNearestCrossStreet(NEAREST_CROSS_STREET)
                       .WithAssetType(ASSET_TYPE)
                       .WithPremiseNumber(PREMISE_NUMBER)
                       .WithRequester(REQUESTER)
                       .WithPurpose(PURPOSE)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithNotes(NOTES)
                       .WithRecordUrl("http://recordUrl")
                       .Build();

            TestNotification("SewerOverflowChanged", model,
                @"<h2>Work Order Sewer Overflow Changed</h2>

Work Order #: <a href=""http://recordUrl"">123</a><br />
Operating Center: some op cntr -  <br/>
Entered By: SomeGuy <br/>
Town: foo town <br />
Street #: 123 <br />
Street: south st. <br />
Nearest Cross Street: west st. <br />
Work Description: gold water mains <br />
Notes:<br />
nope, chuck testa");
        }

        [TestMethod]
        public void TestServiceLineRenewalLeadEnteredNotification()
        {
            var model = new NewTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithOperatingCenter(OPERATING_CENTER)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithNearestCrossStreet(NEAREST_CROSS_STREET)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithMarkoutRequirement(MARKOUT_REQUIREMENT)
                       .WithPriority(PRIORITY)
                       .WithCreatedBy("Some guy")
                       .WithNotes(NOTES)
                       .WithRecordUrl("http://recordUrl")
                       .Build();

            TestNotification("ServiceLineRenewalLeadEntered", model,
                @"<h2>New Service Line Renewal-Lead Work Order</h2>

Work Order #: <a href=""http://recordUrl"">{0}</a> <br />
Operating Center: {1} -  <br />
Town: {2} <br />
Street #: {3} <br />
Street: {4} <br />
Nearest Cross Street: {5} <br />
Work Description: {6} <br />
Markout Requirement: {7} <br />
Priority: {8} <br />
Created By: Some guy <br />
Notes:<br />
{9}",
                WORK_ORDER_ID,
                OPERATING_CENTER,
                TOWN,
                STREET_NUMBER,
                STREET,
                NEAREST_CROSS_STREET,
                WORK_DESCRIPTION,
                MARKOUT_REQUIREMENT,
                PRIORITY,
                NOTES);
        }

        [TestMethod]
        public void TestServiceLineRenewalLeadCompletedNotification()
        {
            var dateCompleted = new DateTime(1980, 12, 8);
            var model = new NewTestWorkOrderBuilder()
                       .WithWorkOrderID(WORK_ORDER_ID)
                       .WithOperatingCenter(OPERATING_CENTER)
                       .WithTown(TOWN)
                       .WithStreetNumber(STREET_NUMBER)
                       .WithStreet(STREET)
                       .WithNearestCrossStreet(NEAREST_CROSS_STREET)
                       .WithWorkDescription(WORK_DESCRIPTION)
                       .WithDateCompleted(dateCompleted)
                       .WithCreatedBy("Some guy")
                       .WithPremiseNumber(PREMISE_NUMBER)
                       .WithPreviousServiceLineMaterial("Copper")
                       .WithCustomerServiceLineMaterial("Lead")
                       .WithCompanyServiceLineMaterial("Plastic")
                       .WithPreviousServiceLineSize("1")
                       .WithCustomerServiceLineSize("2")
                       .WithCompanyServiceLineSize("3")
                       .WithNotes(NOTES)
                       .WithRecordUrl("http://recordUrl")
                       .Build();

            TestNotification("ServiceLineRenewalLeadCompleted", model,
                @"<h2>Service Line Renewal-Lead Completed</h2>

Work Order #: <a href=""http://recordUrl"">{0}</a> <br />
Operating Center: {1} -  <br />
Town: {2} <br />
Street #: {3} <br />
Street: {4} <br />
Nearest Cross Street: {5} <br />
Work Description: {6} <br/>
Date Completed: {7} <br/>
Created By: Some guy <br/>

Premise Number: {8} <br/><br />

Previous NJAW material: Copper<br/>
Previous NJAW size: 1<br/>
Company side material: Plastic<br/>
Company side size: 3<br/>
Customer side material: Lead<br/>
Customer side size: 2<br/>
Date notice left: <br/>

Notes:<br />
{9}",
                WORK_ORDER_ID,
                OPERATING_CENTER,
                TOWN,
                STREET_NUMBER,
                STREET,
                NEAREST_CROSS_STREET,
                WORK_DESCRIPTION,
                dateCompleted,
                PREMISE_NUMBER,
                NOTES);
        }

        #endregion

        #region Test Model

        #region Nested Type: AssetType

        public class TestAssetType : ModelWithDescription
        {
            #region Constructors

            public TestAssetType(string description) : base(description) { }

            #endregion
        }

        #endregion

        #region Nested Type: ModelWithDescription

        public abstract class ModelWithDescription
        {
            #region Properties

            public string Description { get; set; }

            #endregion

            #region Constructors

            public ModelWithDescription(string description)
            {
                Description = description;
            }

            #endregion

            #region Exposed Methods

            public override string ToString()
            {
                return Description;
            }

            #endregion
        }

        #endregion

        #region Nested Type: Street

        public class TestStreet : ModelWithDescription
        {
            #region Constructors

            public TestStreet(string description) : base(description) { }

            #endregion
        }

        #endregion

        #region Nested Type: OperatingCenter

        public class TestOperatingCenter : ModelWithDescription
        {
            public TestOperatingCenter(string description) : base(description) { }
        }

        #endregion

        #region Nested Type: Town

        public class TestTown : ModelWithDescription
        {
            #region Constructors

            public TestTown(string description) : base(description) { }

            #endregion
        }

        #endregion

        #region Nested Type: WorkDescription

        public class TestWorkDescription : ModelWithDescription
        {
            #region Constructors

            public TestWorkDescription(string description) : base(description) { }

            #endregion
        }

        #endregion

        #region Nested Type: WorkOrder

        public abstract class WorkOrderBase
        {
            #region Properties

            public TestOperatingCenter OperatingCenter { get; set; }
            public TestTown Town { get; set; }
            public string StreetNumber { get; set; }
            public TestStreet Street { get; set; }
            public TestStreet NearestCrossStreet { get; set; }
            public TestAssetType AssetType { get; set; }
            public string AssetID { get; set; }
            public string PremiseNumber { get; set; }
            public TestWorkOrderRequester RequestedBy { get; set; }
            public TestWorkOrderPurpose DrivenBy { get; set; }
            public TestWorkDescription WorkDescription { get; set; }
            public string ORCOMServiceOrderNumber { get; set; }
            public string Notes { get; set; }
            public TestCustomerImpactRange CustomerImpactRange { get; set; }
            public TestRepairTimeRange RepairTimeRange { get; set; }
            public TestMarkoutRequirement MarkoutRequirement { get; set; }
            public TestWorkOrderPriority Priority { get; set; }
            public bool? SignificantTrafficImpact { get; set; }
            public string AlertID { get; set; }
            public bool AlertIssued { get; set; }
            public TestEmployee CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime? DateReceived { get; set; }
            public DateTime? DateCompleted { get; set; }
            public string RecordUrl { get; set; }

            #endregion
        }

        public class OldWorkOrder : WorkOrderBase
        {
            public int WorkOrderID { get; set; }
        }

        public class NewWorkOrder : WorkOrderBase
        {
            public int Id { get; set; }
        }

        #endregion

        #region Nested Type: WorkOrderPurpose

        public class TestWorkOrderPurpose : ModelWithDescription
        {
            #region Constructors

            public TestWorkOrderPurpose(string description) : base(description) { }

            #endregion
        }

        #endregion

        #region Nested Type: WorkOrderRequester

        public class TestWorkOrderRequester : ModelWithDescription
        {
            #region Constructors

            public TestWorkOrderRequester(string description) : base(description) { }

            #endregion
        }

        #endregion

        #region Nested Type: CustomerImpactRange

        public class TestCustomerImpactRange : ModelWithDescription
        {
            public TestCustomerImpactRange(string description) : base(description) { }
        }

        #endregion

        #region Nested Type: RepairTimeRange

        public class TestRepairTimeRange : ModelWithDescription
        {
            public TestRepairTimeRange(string description) : base(description) { }
        }

        #endregion

        #region Nested Type: MarkoutRequirement

        public class TestMarkoutRequirement : ModelWithDescription
        {
            public TestMarkoutRequirement(string description) : base(description) { }
        }

        #endregion

        #region Nested Type: WorkOrderPriority

        public class TestWorkOrderPriority : ModelWithDescription
        {
            public TestWorkOrderPriority(string description) : base(description) { }
        }

        #endregion

        #region NestedType Employee

        public class TestEmployee
        {
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        #endregion

        #region Nested Type: TestWorkOrderBuilder

        public abstract class TestWorkOrderBuilderBase<TOrder, TBuilder> : TestDataBuilder<TOrder>
            where TOrder : class, new()
            where TBuilder : TestWorkOrderBuilderBase<TOrder, TBuilder>
        {
            #region Private Members

            private readonly dynamic _d_order;
            protected readonly TOrder _order;

            #endregion

            #region Constructors

            public TestWorkOrderBuilderBase()
            {
                _order = new TOrder();
                _d_order = _order;
            }

            #endregion

            #region Abstract Methods

            public abstract TBuilder WithWorkOrderID(int id);
            public abstract TBuilder WithTown(string townName);
            public abstract TBuilder WithStreet(string streetName);
            public abstract TBuilder WithAssetType(string assetType);
            public abstract TBuilder WithRequester(string requester);
            public abstract TBuilder WithPurpose(string purpose);
            public abstract TBuilder WithWorkDescription(string description);
            public abstract TBuilder WithNearestCrossStreet(string nearestCrossStreet);
            public abstract TBuilder WithOperatingCenter(string operatingCenter);
            public abstract TBuilder WithEstimatedCustomerImpact(string customerImpact);
            public abstract TBuilder WithAnticipatedRepairTime(string repairTime);
            public abstract TBuilder WithMarkoutRequirement(string markoutRequirement);
            public abstract TBuilder WithPriority(string priority);
            public abstract TBuilder WithCreatedBy(string name);
            public abstract TBuilder WithRecordUrl(string url);

            #endregion

            #region Exposed Methods

            public TBuilder WithSAPWorkOrder(long sapWorkOrderNumber)
            {
                _d_order.SAPWorkOrderNumber = sapWorkOrderNumber;
                return (TBuilder)this;
            }

            public TBuilder WithAlertIssued(bool doTheThing)
            {
                _d_order.AlertIssued = doTheThing;
                return (TBuilder)this;
            }

            public TBuilder WithStreetNumber(string streetNumber)
            {
                _d_order.StreetNumber = streetNumber;
                return (TBuilder)this;
            }

            public TBuilder WithPremiseNumber(string premiseNumber)
            {
                _d_order.PremiseNumber = premiseNumber;
                return (TBuilder)this;
            }

            public TBuilder WithNotes(string notes)
            {
                _d_order.Notes = notes;
                return (TBuilder)this;
            }

            public TBuilder WithAlertID(string alertId)
            {
                _d_order.AlertID = alertId;
                return (TBuilder)this;
            }

            public TBuilder WithSignificantTrafficImpact(bool trafficImpact)
            {
                _d_order.SignificantTrafficImpact = trafficImpact;
                return (TBuilder)this;
            }

            public TBuilder WithDateCompleted(DateTime dateCompleted)
            {
                _d_order.DateCompleted = dateCompleted;
                return (TBuilder)this;
            }

            public override TOrder Build()
            {
                return _order;
            }

            #endregion
        }

        public class OldTestWorkOrderBuilder : TestWorkOrderBuilderBase<OldWorkOrder, OldTestWorkOrderBuilder>
        {
            public override OldTestWorkOrderBuilder WithWorkOrderID(int id)
            {
                _order.WorkOrderID = id;
                return this;
            }

            public OldTestWorkOrderBuilder WithAssetID(string assetID)
            {
                _order.AssetID = assetID;
                return this;
            }

            public override OldTestWorkOrderBuilder WithCreatedBy(string name)
            {
                _order.CreatedBy = new TestEmployee {Name = name};
                return this;
            }

            public override OldTestWorkOrderBuilder WithTown(string townName)
            {
                _order.Town = new TestTown(townName);
                return this;
            }

            public override OldTestWorkOrderBuilder WithStreet(string streetName)
            {
                _order.Street = new TestStreet(streetName);
                return this;
            }

            public override OldTestWorkOrderBuilder WithAssetType(string assetType)
            {
                _order.AssetType = new TestAssetType(assetType);
                return this;
            }

            public override OldTestWorkOrderBuilder WithRequester(string requester)
            {
                _order.RequestedBy = new TestWorkOrderRequester(requester);
                return this;
            }

            public override OldTestWorkOrderBuilder WithPurpose(string purpose)
            {
                _order.DrivenBy = new TestWorkOrderPurpose(purpose);
                return this;
            }

            public override OldTestWorkOrderBuilder WithWorkDescription(string description)
            {
                _order.WorkDescription = new TestWorkDescription(description);
                return this;
            }

            public override OldTestWorkOrderBuilder WithNearestCrossStreet(string nearestCrossStreet)
            {
                _order.NearestCrossStreet = new TestStreet(nearestCrossStreet);
                return this;
            }

            public override OldTestWorkOrderBuilder WithOperatingCenter(string operatingCenter)
            {
                _order.OperatingCenter = new TestOperatingCenter(operatingCenter);
                return this;
            }

            public override OldTestWorkOrderBuilder WithEstimatedCustomerImpact(string customerImpact)
            {
                _order.CustomerImpactRange = new TestCustomerImpactRange(customerImpact);
                return this;
            }

            public override OldTestWorkOrderBuilder WithAnticipatedRepairTime(string repairTime)
            {
                _order.RepairTimeRange = new TestRepairTimeRange(repairTime);
                return this;
            }

            public override OldTestWorkOrderBuilder WithMarkoutRequirement(string markoutRequirement)
            {
                _order.MarkoutRequirement = new TestMarkoutRequirement(markoutRequirement);
                return this;
            }

            public override OldTestWorkOrderBuilder WithPriority(string priority)
            {
                _order.Priority = new TestWorkOrderPriority(priority);
                return this;
            }

            public override OldTestWorkOrderBuilder WithRecordUrl(string url)
            {
                _order.RecordUrl = url;
                return this;
            }
        }

        public class NewTestWorkOrderBuilder : TestWorkOrderBuilderBase<WorkOrder, NewTestWorkOrderBuilder>
        {
            public override NewTestWorkOrderBuilder WithWorkOrderID(int id)
            {
                _order.Id = id;
                return this;
            }

            public override NewTestWorkOrderBuilder WithCreatedBy(string name)
            {
                _order.CreatedBy = new User {UserName = name, FullName = name};
                return this;
            }

            public override NewTestWorkOrderBuilder WithTown(string townName)
            {
                _order.Town = new Town {ShortName = townName, State = new State{Abbreviation = STATE}};
                return this;
            }

            public override NewTestWorkOrderBuilder WithStreet(string streetName)
            {
                _order.Street = new Street {FullStName = streetName};
                return this;
            }

            public override NewTestWorkOrderBuilder WithAssetType(string assetType)
            {
                _order.AssetType = new AssetType {Description = assetType};
                return this;
            }

            public override NewTestWorkOrderBuilder WithRequester(string requester)
            {
                _order.RequestedBy = new WorkOrderRequester {Description = requester};
                return this;
            }

            public override NewTestWorkOrderBuilder WithPurpose(string purpose)
            {
                _order.Purpose = new WorkOrderPurpose {Description = purpose};
                return this;
            }

            public override NewTestWorkOrderBuilder WithWorkDescription(string description)
            {
                _order.WorkDescription = new WorkDescription {Description = description};
                return this;
            }

            public override NewTestWorkOrderBuilder WithNearestCrossStreet(string nearestCrossStreet)
            {
                _order.NearestCrossStreet = new Street {FullStName = nearestCrossStreet};
                return this;
            }

            public override NewTestWorkOrderBuilder WithOperatingCenter(string operatingCenter)
            {
                _order.OperatingCenter = new OperatingCenter {OperatingCenterCode = operatingCenter};
                return this;
            }

            public override NewTestWorkOrderBuilder WithEstimatedCustomerImpact(string customerImpact)
            {
                _order.EstimatedCustomerImpact = new CustomerImpactRange {Description = customerImpact};
                return this;
            }

            public override NewTestWorkOrderBuilder WithAnticipatedRepairTime(string repairTime)
            {
                _order.AnticipatedRepairTime = new RepairTimeRange {Description = repairTime};
                return this;
            }

            public override NewTestWorkOrderBuilder WithMarkoutRequirement(string markoutRequirement)
            {
                _order.MarkoutRequirement = new MarkoutRequirement {Description = markoutRequirement};
                return this;
            }

            public override NewTestWorkOrderBuilder WithPriority(string priority)
            {
                _order.Priority = new WorkOrderPriority {Description = priority};
                return this;
            }

            public override NewTestWorkOrderBuilder WithRecordUrl(string url)
            {
                _order.RecordUrl = url;
                return this;
            }

            public NewTestWorkOrderBuilder WithPreviousServiceLineMaterial(string description)
            {
                _order.PreviousServiceLineMaterial = new ServiceMaterial{Description = description};
                return this;
            }

            public NewTestWorkOrderBuilder WithCompanyServiceLineMaterial(string description)
            {
                _order.CompanyServiceLineMaterial = new ServiceMaterial { Description = description };
                return this;
            }

            public NewTestWorkOrderBuilder WithCustomerServiceLineMaterial(string description)
            {
                _order.CustomerServiceLineMaterial = new ServiceMaterial { Description = description };
                return this;
            }

            public NewTestWorkOrderBuilder WithPreviousServiceLineSize(string description)
            {
                _order.PreviousServiceLineSize = new ServiceSize { ServiceSizeDescription = description };
                return this;
            }

            public NewTestWorkOrderBuilder WithCompanyServiceLineSize(string description)
            {
                _order.CompanyServiceLineSize = new ServiceSize { ServiceSizeDescription = description };
                return this;
            }

            public NewTestWorkOrderBuilder WithCustomerServiceLineSize(string description)
            {
                _order.CustomerServiceLineSize = new ServiceSize { ServiceSizeDescription = description };
                return this;
            }
        }

        #endregion

        #endregion
    }
}
