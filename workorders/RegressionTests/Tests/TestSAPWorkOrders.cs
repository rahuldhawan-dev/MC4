using MapCall.Common.Testing.Selenium;
using MMSINC.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestSAPWorkOrders
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(true);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void TestNormalSapOrderWithEverything()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithMainAsset(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Verify.SapStatusMatchesRegex(SetUpFixtureBase.Selenium, "content_cphMain_cphMain_woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_lblSAPErrorCode", "^Order was saved with number \\d+ and notification \\d+ successfully");
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            StockClerk.PlanMaterials(SetUpFixtureBase.Selenium, order);
            Verify.SapStatusMatchesRegex(SetUpFixtureBase.Selenium, "content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_lblSAPErrorCode", "^Order Updated Successfully$");
            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            var assignment = Assign.OrderToDefaultCrew(SetUpFixtureBase.Selenium, order);
            MakeFinal.FromCrewAssignmentsScreen.OrderWithMarkout(SetUpFixtureBase.Selenium, ref order,ref assignment, SetUpFixtureBase.UserName);
            Verify.SapStatusMatchesRegex(SetUpFixtureBase.Selenium, "content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_ctl01_fvWorkOrder_lblSAPErrorCode", "^Order Updated Successfully$");
            Navigate.ToSupervisorApproval(SetUpFixtureBase.Selenium);
            Supervisor.ApproveSapOrder(SetUpFixtureBase.Selenium, order);
            Verify.SapStatusMatchesRegex(SetUpFixtureBase.Selenium, "content_cphMain_cphMain_wofrvWorkOrders_wodvWorkOrder_fvWorkOrder_ctl01_fvWorkOrder_lblSAPErrorCode", "^Confirmation of order \\d+ saved Successfully$");
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderIsApproved(SetUpFixtureBase.Selenium, order);
            Navigate.ToStockApproval(SetUpFixtureBase.Selenium);
            // Continue Verification
            StockClerk.ApproveSapOrderStock(SetUpFixtureBase.Selenium, order);
            Verify.SapStatusMatchesRegex(SetUpFixtureBase.Selenium, "content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_woifvInitial_fvWorkOrder_lblSAPErrorCode",
                string.Join("|", 
                    "^Created Goods Issue Successfully$", 
                    "^Deficit of SL Unrestricted-use 1 EA : \\d+ D\\d+ \\d+$", 
                    "^Posting only possible in periods \\d+/\\d+ and \\d+/\\d+ in company code \\d+$"));
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderIsApproved(SetUpFixtureBase.Selenium, order);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderCannotBeCancelled(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestWBSOverridesEditableFieldRulesForSAP()
        {
            var selenium = SetUpFixtureBase.Selenium;

            Navigate.ToInput(selenium);
            var order = WorkOrder.WithMainAsset(selenium, SetUpFixtureBase.UserName);
            Verify.SapStatusMatchesRegex(selenium, "content_cphMain_cphMain_woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_lblSAPErrorCode", "^Order was saved with number \\d+ and notification \\d+ successfully");
            Navigate.ToGeneralSearch(selenium);
            Navigate.FindAndSelectOrder(selenium, order);
            selenium.ClickAndWaitForPageToLoad(Alter.NecessaryIDs.BTN_EDIT);
            
            Assert.IsTrue(selenium.IsVisible(Alter.NecessaryIDs.DDL_ASSET_TYPE));
            Assert.IsTrue(selenium.IsVisible(Alter.NecessaryIDs.DDL_DESCRIPTION_OF_WORK));
            Assert.IsTrue(selenium.IsVisible(Alter.NecessaryIDs.DDL_PMAT));
            Assert.IsTrue(selenium.IsEditable(Alter.NecessaryIDs.TXT_ACCOUNT_CHARGED));
            Assert.IsTrue(selenium.IsEditable(Alter.NecessaryIDs.TXT_PREMISE_NUMBER));

            // override the wbs
            selenium.Select(Alter.NecessaryIDs.DDL_PMAT, "RBS - Child WO to PS Project");
            selenium.WaitForEditable(Alter.NecessaryIDs.DDL_CROSS_STREET);
            selenium.WaitForEditable(Alter.NecessaryIDs.DDL_STREET);
            selenium.WaitForEditable(Alter.NecessaryIDs.DDL_DESCRIPTION_OF_WORK);
            selenium.ClickAndWaitForPageToLoad(Alter.NecessaryIDs.BTN_SAVE_INITIAL_INFORMATION);

            Assert.IsFalse(selenium.IsVisible(Alter.NecessaryIDs.DDL_ASSET_TYPE));
            Assert.IsFalse(selenium.IsElementPresent(Alter.NecessaryIDs.DDL_PMAT));
            Assert.IsTrue(selenium.IsVisible(Alter.NecessaryIDs.DDL_DESCRIPTION_OF_WORK));
            Assert.IsTrue(selenium.IsEditable(Alter.NecessaryIDs.TXT_ACCOUNT_CHARGED));
            Assert.IsTrue(selenium.IsVisible(Alter.NecessaryIDs.TXT_ACCOUNT_CHARGED));
            Assert.IsTrue(selenium.IsEditable(Alter.NecessaryIDs.TXT_PREMISE_NUMBER));

            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            var assignment = Assign.OrderToDefaultCrew(SetUpFixtureBase.Selenium, order);
            MakeFinal.FromCrewAssignmentsScreen.OrderWithMarkout(SetUpFixtureBase.Selenium, ref order, ref assignment, SetUpFixtureBase.UserName);
            Verify.SapStatusMatchesRegex(SetUpFixtureBase.Selenium, "content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_ctl01_fvWorkOrder_lblSAPErrorCode", "^Order Updated Successfully$");
            Navigate.ToSupervisorApproval(SetUpFixtureBase.Selenium);
            Supervisor.ApproveSapOrder(SetUpFixtureBase.Selenium, order);

            Navigate.ToGeneralSearch(selenium);
            Navigate.FindAndSelectOrder(selenium, order);
            selenium.ClickAndWaitForPageToLoad(Alter.NecessaryIDs.BTN_EDIT);

            Assert.IsFalse(selenium.IsVisible(Alter.NecessaryIDs.DDL_ASSET_TYPE));
            Assert.IsFalse(selenium.IsVisible(Alter.NecessaryIDs.DDL_DESCRIPTION_OF_WORK));
            Assert.IsFalse(selenium.IsElementPresent(Alter.NecessaryIDs.DDL_PMAT));
            Assert.IsFalse(selenium.IsVisible(Alter.NecessaryIDs.TXT_ACCOUNT_CHARGED));
            Assert.IsFalse(selenium.IsVisible(Alter.NecessaryIDs.TXT_PREMISE_NUMBER));
        }
    }
}