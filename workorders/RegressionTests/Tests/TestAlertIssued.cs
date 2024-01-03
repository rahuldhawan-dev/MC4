using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestAlertIssued
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void TestMainBreakInputSetsAlertStartedIfAlertIssued()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = 
                WorkOrder.WithMainAssetForMainBreak(SetUpFixtureBase.Selenium,
                    SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderHasAlertIssued(SetUpFixtureBase.Selenium, order, true);
        }

        [Test]
        public void TestMainBreakInputDoesNotSetAlertStartedIfAlertNotIssued()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder.WithMainAssetForMainBreak(SetUpFixtureBase.Selenium,
                    SetUpFixtureBase.UserName, false);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderHasAlertIssued(SetUpFixtureBase.Selenium, order, false);
        }

        [Test]
        public void TestMainBreakEditDoesNotSetAlertStartedIfAlertNotIssuedIsNotSetToYes()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder.WithMainAssetForMainBreak(SetUpFixtureBase.Selenium,
                    SetUpFixtureBase.UserName, false);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderAlertIssued(SetUpFixtureBase.Selenium, order, false);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderHasAlertIssued(SetUpFixtureBase.Selenium, order, false);
        }

        [Test]
        public void TestMainBreakEditSetsAlertStartedIfAlertIssuedSetToYes()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder.WithMainAssetForMainBreak(SetUpFixtureBase.Selenium,
                    SetUpFixtureBase.UserName, false);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderAlertIssued(SetUpFixtureBase.Selenium, order, true);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderHasAlertIssued(SetUpFixtureBase.Selenium, order, true);
        }
    }
}