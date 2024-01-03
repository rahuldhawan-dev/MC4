using MMSINC.ClassExtensions.StringExtensions;
using _271ObjectTests.Tests.Unit.Model;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorEngine;
using Subtext.TestLibrary;
using System;
using System.IO;
using System.Reflection;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Integration
{
    [TestClass]
    public class NotificationTemplatesTest : WorkOrdersTestClass<WorkOrder>
    {
        #region Constants

        public const int WORK_ORDER_ID = 123;
        public const long SAP_WORK_ORDER_NUMBER = 12345;
        public const string OP_CODE = "123", OPERATING_CENTER = "some op cntr",
                            TOWN = "foo town",
                            STREET_NUMBER = "123",
                            STREET = "south st.",
                            NEAREST_CROSS_STREET = "west st.",
                            ASSET_TYPE = "Valve",
                            ASSET_ID = "123",
                            PREMISE_NUMBER = "12345678",
                            REQUESTER = "Customer",
                            PURPOSE = "Customer",
                            WORK_DESCRIPTION = "gold water mains",
                            NOTES = "nope, chuck testa",
                            CUSTOMER_IMPACT = "OH NOES!!!",
                            REPAIR_TIME = "FOR.EV.ER",
                            MARKOUT_REQUIREMENT = "Routine",
                            PRIORITY = "Emergency",
                            ALERT_ISSUED = "no",
                            CREATED_BY = "Mr. D. Buggin",
                            DATE_COMPLETED = "12/8/1980";

        public const bool TRAFFIC_IMPACT = true;

        public const string NOTIFICATION_PATH_FORMAT =
            "MapCall.Common.Resources.NotificationTemplates.FieldServices.WorkManagement.{0}.cshtml";

        #endregion

        #region Private Members

        private Assembly _notificationAssembly;
        private WorkOrder _model;
        private HttpSimulator _simulator;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _simulator = new HttpSimulator();
            _simulator = _simulator.SimulateRequest();
            _model = new TestWorkOrderBuilder()
                .WithWorkOrderID(WORK_ORDER_ID)
                .WithSAPWorkOrderNumber(SAP_WORK_ORDER_NUMBER)
                .WithOperatingCenter(OP_CODE, OPERATING_CENTER)
                .WithTown(TOWN)
                .WithStreetNumber(STREET_NUMBER)
                .WithStreet(STREET)
                .WithNearestCrossStreet(NEAREST_CROSS_STREET)
                .WithAssetType(new TestAssetTypeBuilder<Valve>())
                .WithValve(new TestValveBuilder().WithAssetID(ASSET_ID))
                .WithPremiseNumber(PREMISE_NUMBER)
                .WithRequester(WorkOrderRequesterRepository.Customer)
                .WithPurpose(WorkOrderPurposeRepository.Customer)
                .WithPriority(TestWorkOrderPriorityBuilder.Emergency)
                .WithWorkDescription(WORK_DESCRIPTION)
                .WithEstimatedCustomerImpact(CUSTOMER_IMPACT)
                .WithRepairTimeRange(REPAIR_TIME)
                .WithTrafficImpact(TRAFFIC_IMPACT)
                .WithAlertId(ALERT_ISSUED)
                .WithCreatedBy(new Employee { FullName = CREATED_BY })
                .WithDateCompleted(DATE_COMPLETED.ToDateTime())
                .WithNotes(NOTES)
                .WithRecordUrl(WORK_ORDER_ID);
        }

        [TestCleanup]
        public void AssetTestCleanup()
        {
            _simulator.Dispose();
        }

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
            var streamPath = string.Format(NOTIFICATION_PATH_FORMAT, name.Replace(" ", ""));
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

            Assert.AreEqual(string.Format(expectedFormat, args).Replace("\n", Environment.NewLine),
                Razor.Parse(template, data).Replace("\n", Environment.NewLine));
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestAssetOrderCompletedNotification()
        {
            TestNotification("Asset Order Completed", _model,
                @"<h2>Work Order Completion Notification</h2>

Work Order #: <a href=""https://mapcall.awapps.com/Modules/WorkOrders/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=view&amp;arg={0}"">{0}</a><br />
SAP Work Order #: {1} <br />
Town: {2} <br />
Street #: {3} <br />
Street: {4} <br />
Asset Type: {5} <br />
Asset ID: {6} <br />
Premise #: {7} <br />
Requested By: {8} <br />
Purpose: {9} <br />
Work Description: {10} <br />
Notes:<br />
{11}",
                WORK_ORDER_ID,
                SAP_WORK_ORDER_NUMBER,
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
        public void TestCurbPitComplianceNotification()
        {
            TestNotification("Curb-Pit Compliance", _model,
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
            TestNotification("Curb-Pit Estimate", _model,
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
            TestNotification("Curb-Pit Revenue", _model,
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
        public void TestSupervisorApprovalNotification()
        {
            TestNotification("Supervisor Approval", _model,
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

        #endregion

        protected override WorkOrder GetValidObjectFromDatabase()
        {
            throw new NotImplementedException();
        }

        protected override void DeleteObject(WorkOrder entity)
        {
            throw new NotImplementedException();
        }
    }
}
