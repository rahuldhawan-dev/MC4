using MapCall.Common.Testing.Selenium;
using MMSINC.Testing.Selenium;
using NUnit.Framework;
using NUnit.Framework.Internal;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;

namespace RegressionTests.Tests
{
    /// <summary>
    /// Some basic tests that just load pages and make sure they are properly loading.
    /// Avoid users finding them instead of us.
    /// </summary>
    [TestFixture]
    public class TestViews
    {
        #region Constants

        public const string MARKOUT_DAMAGES_TAB = "//a[@id='markoutDamagesTab']/span",
            ECHOSHORE_LEAK_ALERT_LINK = "content_cphMain_cphMain_ctl00_wodvWorkOrder_fvWorkOrder_wofvInitialInformation_fvWorkOrder_hlEchoshoreLeakAlert",
            PREMISE_WARNING_DIV = "premiseWarningDiv";

        #endregion

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void TestCrewManagement()
        {
            Navigate.ToCrewManagement(SetUpFixtureBase.Selenium);
            Verify.CrewManagementDoesNotError(SetUpFixtureBase.Selenium);
        }

        [Test]
        public void TestArcCollectorLink()
        {
            var selenium = SetUpFixtureBase.Selenium;
            Navigate.ToInput(selenium);
            var order = WorkOrder.WithMainAsset(selenium, SetUpFixtureBase.UserName);
            // can see the link on the show page
            selenium.AssertAttribute("content_cphMain_cphMain_woirvParaDesWorkOrdersOI_wodvWorkOrder_fvWorkOrder_fvWorkOrder_hlCollector", "href", string.Format("https://collector.arcgis.app?itemID={0}", Data.MAP_ID));
        }
        
        [Test]
        public void TestEchoshoreLeakAlertLinkIsDisplayedWhenLinked()
        {
            var selenium = SetUpFixtureBase.Selenium;
            Navigate.ToInput(selenium);
            var order = WorkOrder.WithHydrantAsset(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            //manual sql to create an alert and link it to this order
            Data.CreateAlertForWorkOrder(order);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            selenium.Type(Verify.NecessaryIDs.GeneralSearch.TXT_WORK_ORDER_ID, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(Verify.NecessaryIDs.GeneralSearch.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(Navigate.NecessaryIDs.LNK_SELECT);
            Assert.IsTrue(selenium.IsVisible(ECHOSHORE_LEAK_ALERT_LINK));
        }
        
        [Test]
        public void TestEchoshoreLeakAlertLinkIsNotDisplayedWhenNotLinked()
        {
            var selenium = SetUpFixtureBase.Selenium;
            Navigate.ToInput(selenium);
            var order = WorkOrder.WithHydrantAsset(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            selenium.Type(Verify.NecessaryIDs.GeneralSearch.TXT_WORK_ORDER_ID, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(Verify.NecessaryIDs.GeneralSearch.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(Navigate.NecessaryIDs.LNK_SELECT);
            selenium.AssertElementIsNotPresent(ECHOSHORE_LEAK_ALERT_LINK);
        }

        [Test]
        public void TestMarkoutDamagesTabAppearsOnViews()
        {
            var selenium = SetUpFixtureBase.Selenium;
            Navigate.ToInput(selenium);
            var order = WorkOrder.WithMainAsset(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            var assignment = Assign.OrderToDefaultCrew(SetUpFixtureBase.Selenium, order);
            Navigate.ToFinalization(SetUpFixtureBase.Selenium);
            selenium.Type(Verify.NecessaryIDs.Finalization.TXT_WORK_ORDER_ID, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(Verify.NecessaryIDs.Finalization.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(Navigate.NecessaryIDs.LNK_SELECT);
            Assert.IsTrue(selenium.IsVisible(MARKOUT_DAMAGES_TAB));
            MakeFinal.FromCrewAssignmentsScreen.OrderWithMarkout(
                SetUpFixtureBase.Selenium, ref order,
                ref assignment, SetUpFixtureBase.UserName);
            Navigate.ToSupervisorApproval(SetUpFixtureBase.Selenium);
            Supervisor.ApproveOrder(SetUpFixtureBase.Selenium, order);
            Assert.IsTrue(selenium.IsVisible(MARKOUT_DAMAGES_TAB));
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            selenium.Type(Verify.NecessaryIDs.GeneralSearch.TXT_WORK_ORDER_ID, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(Verify.NecessaryIDs.GeneralSearch.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(Navigate.NecessaryIDs.LNK_SELECT);
            Assert.IsTrue(selenium.IsVisible(MARKOUT_DAMAGES_TAB));
        }

        [Test]
        public void TestPremiseLinkedToSampleSiteWarningIsDisplayedWhenPremiseIsLinked()
        {
            var selenium = SetUpFixtureBase.Selenium;
            Navigate.ToInput(selenium);
            var order = WorkOrder.WithServiceAssetWithPremiseLinkedToSampleSite(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            selenium.Type(Verify.NecessaryIDs.GeneralSearch.TXT_WORK_ORDER_ID, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(Verify.NecessaryIDs.GeneralSearch.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(Navigate.NecessaryIDs.LNK_SELECT);
            Assert.IsTrue(selenium.IsVisible(PREMISE_WARNING_DIV));
        }
        [Test]
        public void TestPremiseLinkedToSampleSiteWarningIsNotDisplayedWhenPremiseIsNotLinked()
        {
            var selenium = SetUpFixtureBase.Selenium;
            Navigate.ToInput(selenium);
            var order = WorkOrder.WithServiceAsset(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            selenium.Type(Verify.NecessaryIDs.GeneralSearch.TXT_WORK_ORDER_ID, order.WorkOrderID);
            selenium.ClickAndWaitForPageToLoad(Verify.NecessaryIDs.GeneralSearch.BTN_SEARCH);
            selenium.ClickAndWaitForPageToLoad(Navigate.NecessaryIDs.LNK_SELECT);
            Assert.IsTrue(!selenium.IsVisible(PREMISE_WARNING_DIV));
        }
    }
}
