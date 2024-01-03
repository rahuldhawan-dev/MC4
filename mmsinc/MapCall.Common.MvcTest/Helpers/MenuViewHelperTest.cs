using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Helpers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Utilities.StructureMap;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Helpers
{
    // NOTE: Testing links within areas no longer works as expect in this test because FakeMvcApplicationTester knows nothing
    //       about the area route registrations for an actual site. You have to test against the url generate if no areas exist.

    [TestClass, DoNotParallelize]
    public class MenuViewHelperTest
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        private Mock<IRoleService> _roleService;
        private User _user;
        private MvcApplicationTester<FakeMvcApplication> _app;
        private FakeMvcHttpHandler _handler;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _authServ = new Mock<IAuthenticationService<User>>();
            _container.Inject(_authServ.Object);
            _roleService = new Mock<IRoleService>();
            _container.Inject(_roleService.Object);
            _container.Inject<IBasicRoleService>(_roleService.Object);
            _user = new User {Roles = new List<Role>()};
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);
            _roleService.Setup(x => x.CurrentUserRoles).Returns(_user.AggregateRoles);
            _app = new FakeMvcApplicationTester(_container);
            _handler = _app.CreateRequestHandler();
            _container.Inject<IUrlHelper>(new MapCallUrlHelper());
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _app.Dispose();
        }

        #endregion

        #region Private Methods

        private string RenderMenu()
        {
            return MenuViewHelper.RenderMenu(_handler.RequestContext).ToString();
        }

        private void SetupRoleAccess(RoleModules module, RoleActions action, bool canAccess)
        {
            _roleService.Setup(x => x.CanAccessRole(module, action, It.IsAny<OperatingCenter>())).Returns(canAccess);
        }

        #endregion

        #region Tests

        #region Testing link visibility based on site admin status

        private void TestAdminVisibility(string linkText, string url, string area, string controller)
        {
            const string expectedFormat = "<a href=\"{0}\" data-area=\"{2}\" data-controller=\"{3}\">{1}</a>";
            var expectedHtml = string.Format(expectedFormat, url, linkText, area, controller);

            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);
            var result = RenderMenu();
            Assert.IsFalse(result.Contains(expectedHtml),
                "Expected link: {0} was unexpectedly found in the following mess: \n\r {1}", expectedHtml, result);

            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            result = RenderMenu();
            Assert.IsTrue(result.Contains(expectedHtml),
                "Expected link: {0} was unexpectedly found in the following mess: \n\r {1}", expectedHtml, result);
        }

        [TestMethod]
        public void TestLinkIsVisibleIfUserIsAdmin()
        {
            TestAdminVisibility("Administration", "/Modules/mvc/Admin/AdminHome/Index", "Admin", "AdminHome");
            TestAdminVisibility("Notes", "/Modules/Notes.aspx", null, null);
            TestAdminVisibility("SAP Company Codes", "/Modules/mvc/SAPCompanyCode/Index", null, "SAPCompanyCode");
        }

        #endregion

        #region Testing link visibility based on role

        private void AssertCanNotSeeLinkWithoutRole(RoleModules module, RoleActions action, string expectedHtml)
        {
            SetupRoleAccess(module, action, false);
            var result = RenderMenu();
            Assert.IsFalse(result.Contains(expectedHtml),
                "Expected link: {0} was unexpectedly found in the following mess: \n\r {1}", expectedHtml, result);
        }

        private void AssertCanSeeLinkWithRole(RoleModules module, RoleActions action, string expectedHtml)
        {
            SetupRoleAccess(module, action, true);
            var result = RenderMenu();
            Assert.IsTrue(result.Contains(expectedHtml),
                "Expected link: {0} was not found in the following mess: \n\r {1}", expectedHtml, result);
        }

        private void TestVisibilityWithRole(RoleModules module, string linkText, string url, string area = null,
            string controller = null, RoleActions action = RoleActions.Read, bool opensInNewWindow = false)
        {
            const string expectedFormat = "<a href=\"{0}\" data-area=\"{2}\" data-controller=\"{3}\">{1}</a>";
            const string opensInNewWindowExpectedFormat =
                "<a href=\"{0}\" target=\"_blank\" data-area=\"{2}\" data-controller=\"{3}\">{1}</a>";
            var format = (opensInNewWindow ? opensInNewWindowExpectedFormat : expectedFormat);
            var expected = string.Format(format, url, linkText, area, controller);

            // First test that it doesn't display without the role
            AssertCanNotSeeLinkWithoutRole(module, action, expected);
            AssertCanSeeLinkWithRole(module, action, expected);
        }

        [TestMethod]
        public void TestVisibilityOf_OperationsTraining_Links()
        {
            Action<string, string, string, string> testWithRoute = (linkText, url, area, controller) => {
                TestVisibilityWithRole(RoleModules.OperationsTrainingRecords, linkText, url, area, controller);
            };

            testWithRoute("Training Modules", "/Modules/mvc/TrainingModule/Search", null, "TrainingModule");
            testWithRoute("Training Records", "/Modules/mvc/TrainingRecord/Search", null, "TrainingRecord");
        }

        [TestMethod]
        public void TestVisibilityOf_EventsEvents_Links()
        {
            Action<string, string, string> test = (linkText, url, controller) => {
                TestVisibilityWithRole(RoleModules.EventsEvents, linkText, url, "Events", controller);
            };

            test("Events", "/Modules/mvc/Events/Event/Search", "Event");
            test("Event Documents", "/Modules/mvc/Events/EventDocument/Search", "EventDocument");
            test("Event Types", "/Modules/mvc/Events/EventType/Index", "EventType");
        }

        [TestMethod]
        public void TestVisibilityOf_FieldServicesMeterChangeOuts_Links()
        {
            TestVisibilityWithRole(RoleModules.FieldServicesMeterChangeOuts, "Meter Change Outs",
                "/Modules/mvc/FieldOperations/MeterChangeOut/Search", "FieldOperations", "MeterChangeOut");
            TestVisibilityWithRole(RoleModules.FieldServicesMeterChangeOuts, "Meter Change Out Contracts",
                "/Modules/mvc/FieldOperations/MeterChangeOutContract/Search", "FieldOperations",
                "MeterChangeOutContract");
        }

        [TestMethod]
        public void TestVisibilityOf_FieldServicesWorkManagement_UserAdministrator_Links()
        {
            Action<string, string> test = (linkText, url) => {
                TestVisibilityWithRole(RoleModules.FieldServicesWorkManagement, linkText, url, null, null,
                    RoleActions.UserAdministrator);
            };

            Action<string, string, string, string> testWithRoute = (linkText, url, area, controller) => {
                TestVisibilityWithRole(RoleModules.FieldServicesWorkManagement, linkText, url, area, controller,
                    RoleActions.UserAdministrator);
            };

            test("Markout Planning",
                "/Modules/WorkOrders/Views/WorkOrders/MarkoutPlanning/WorkOrderMarkoutPlanningResourceView.aspx");
            testWithRoute("Pre-Planning", "/Modules/mvc/FieldOperations/WorkOrderPrePlanning/Search", "FieldOperations",
                "WorkOrderPrePlanning");
            testWithRoute("Scheduling", "/Modules/mvc/FieldOperations/WorkOrderScheduling/Search", "FieldOperations",
                "WorkOrderScheduling");
            test("Crew Management", "/Modules/WorkOrders/Views/Crews/CrewResourceView.aspx");
            testWithRoute("Restoration Type Costs", "/Modules/mvc/FieldOperations/RestorationTypeCost/Search",
                "FieldOperations", "RestorationTypeCost");
            testWithRoute("Stock Source Locations", "/Modules/mvc/FieldOperations/StockLocation/Search",
                "FieldOperations", "StockLocation");
            testWithRoute("Spoil Type Disposal Costs", "/Modules/mvc/FieldOperations/OperatingCenterSpoilRemovalCost/Search",
                "FieldOperations", "OperatingCenterSpoilRemovalCost");
            testWithRoute("Spoil Storage Locations", "/Modules/mvc/FieldOperations/SpoilStorageLocation/Search",
                "FieldOperations", "SpoilStorageLocation");
            test("Spoil Final Processing Locations",
                "/Modules/WorkOrders/Views/SpoilFinalProcessingLocations/SpoilFinalProcessingLocationResourceView.aspx");
            test("Spoils Processing", "/Modules/WorkOrders/Views/SpoilRemovals/SpoilRemovalResourceView.aspx");
            test("Stock To Issue",
                "/Modules/WorkOrders/Views/WorkOrders/StockToIssue/WorkOrderStockToIssueResourceView.aspx");
            test("Supervisor Approval",
                "/Modules/WorkOrders/Views/WorkOrders/SupervisorApproval/WorkOrderSupervisorApprovalResourceView.aspx");
            test("Restoration Processing",
                "/Modules/WorkOrders/Views/WorkOrders/RestorationProcessing/WorkOrderRestorationProcessingResourceView.aspx");
            test("SOP Processing", "/Modules/WorkOrders/Views/WorkOrders/SOPProcessing/SOPProcessingResourceView.aspx");
        }

        [TestMethod]
        public void TestVisibilityOf_HealthAndSafety_Links()
        {
            Action<string, string, string, string> test = (linkText, url, area, controller) => {
                TestVisibilityWithRole(RoleModules.OperationsHealthAndSafety, linkText, url, area, controller);
            };

            test("Job Site Check Lists", "/Modules/mvc/HealthAndSafety/JobSiteCheckList/Search", "HealthAndSafety",
                "JobSiteCheckList");
        }

        [TestMethod]
        public void TestVisibilityOf_Environmental_Links()
        {
            TestVisibilityWithRole(RoleModules.EnvironmentalGeneral, 
                "Permits", 
                "/Modules/mvc/Environmental/EnvironmentalPermit/Search", 
                "Environmental", 
                "EnvironmentalPermit");

            TestVisibilityWithRole(RoleModules.EnvironmentalGeneral, 
                "Allocation Groupings", 
                "/Modules/mvc/Environmental/AllocationPermit/Search", 
                "Environmental", 
                "AllocationPermit");

            TestVisibilityWithRole(RoleModules.EnvironmentalGeneral, 
                "Allocation Withdrawal Nodes", 
                "/Modules/mvc/Environmental/AllocationPermitWithdrawalNode/Search", 
                "Environmental", 
                "AllocationPermitWithdrawalNode");
            
            TestVisibilityWithRole(RoleModules.EnvironmentalGeneral, 
                "Allocation Transactions", 
                "/Modules/Production/AllocationTransactions.aspx");

            TestVisibilityWithRole(RoleModules.EnvironmentalGeneral, 
                "Geological Formations", 
                "/Modules/Production/GeologicalFormations.aspx");

            TestVisibilityWithRole(RoleModules.EnvironmentalGeneral, 
                "PWSID Customer Data", 
                "/Modules/Production/PWSIDCustomerData.aspx");

            TestVisibilityWithRole(RoleModules.EnvironmentalWaterSystems, 
                "PWSID", 
                "/Modules/mvc/PublicWaterSupply/Search", 
                null, 
                "PublicWaterSupply");
        }

        [TestMethod]
        public void TestVisibilityOf_HumanResourcesPositions_Links()
        {
            TestVisibilityWithRole(RoleModules.HumanResourcesPositions, "Position Groups",
                "/Modules/mvc/PositionGroup/Search", null, "PositionGroup");
        }

        [TestMethod]
        public void TestVisibilityOf_HumanResourcesSampleSites_Links()
        {
            TestVisibilityWithRole(RoleModules.HumanResourcesSampleSites, "Regulations",
                "/Modules/mvc/Regulation/Search", null, "Regulation");
        }

        [TestMethod]
        public void TestVisibilityOf_FieldServicesAssets_Links()
        {
            Action<string, string> test = (linkText, url) => {
                TestVisibilityWithRole(RoleModules.FieldServicesAssets, linkText, url);
            };

            Action<string, string, string, string> testWithRoute = (linkText, url, area, controller) => {
                TestVisibilityWithRole(RoleModules.FieldServicesAssets, linkText, url, area, controller);
            };

            testWithRoute("Sewer Opening", "/Modules/mvc/FieldOperations/SewerOpening/Search", "FieldOperations",
                "SewerOpening");
            testWithRoute("Main Inspections / Cleaning", "/Modules/mvc/FieldOperations/SewerMainCleaning/Search", "FieldOperations",
                "SewerMainCleaning");
            testWithRoute("Overflows", "/Modules/mvc/FieldOperations/SewerOverflow/Search", "FieldOperations",
                "SewerOverflow");
            test("Overflows", "/Reports/FieldServices/SewerOverflowReport.aspx");
        }

        [TestMethod]
        public void TestVisibilityOf_EngineeringPWSIDCapacity_Links()
        {
            SetupRoleAccess(RoleModules.EnvironmentalGeneral, RoleActions.Read, true);

            TestVisibilityWithRole(RoleModules.EngineeringPWSIDCapacity,
                "PWSID Capacity",
                "/Modules/mvc/Environmental/PublicWaterSupplyFirmCapacity/Search",
                "Environmental",
                "PublicWaterSupplyFirmCapacity");
        }

        #endregion

        #endregion
    }
}
