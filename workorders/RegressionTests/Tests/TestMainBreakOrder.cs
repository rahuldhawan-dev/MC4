using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestMainBreakOrder
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void TestMainBreakInformationLoadsWhenVisitingMainBreakOrderInEditMode()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithMainAssetForMainBreak(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderHasExtraMainBreakInformation(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestMainBreakInformationEntryControlsLoadWhenChangingWorkDescriptionToMainBreakInFinalization()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithMainAssetAsEmergency(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            MakeFinal.FromFinalizationScreen.OrderForMainBreak(SetUpFixtureBase.Selenium, ref order);
        }
    }
}
