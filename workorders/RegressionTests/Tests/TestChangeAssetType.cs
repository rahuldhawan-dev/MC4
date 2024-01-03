using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;
using Selenium;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestChangeAssetType
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void HydrantToValveTest()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithHydrantAsset(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderAssetHydrantToValve(SetUpFixtureBase.Selenium, ref order);
        }

        [Test]
        public void MainToValveWithSwappedStreetsTest()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithMainAsset(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderAssetMainToValveAndSwapStreets(SetUpFixtureBase.Selenium, ref order);
        }

        [Test]
        public void ServiceToMainTest()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder.WithServiceAsset(SetUpFixtureBase.Selenium,
                    SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderAssetServiceToMain(SetUpFixtureBase.Selenium, ref order);
        }

        [Test]
        public void ServiceToMainInvestigationTest()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder.WithServiceAsset(SetUpFixtureBase.Selenium,
                    SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderAssetServiceToMainInvestigation(SetUpFixtureBase.Selenium, ref order);
        }

        [Test]
        public void ServiceToValveTest()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder.WithServiceAsset(SetUpFixtureBase.Selenium,
                    SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderAssetServiceToValve(SetUpFixtureBase.Selenium, ref order);
        }

        [Test]
        public void ValveToMainTest()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithValveAsset(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderAssetValveToMain(SetUpFixtureBase.Selenium, ref order);
        }

        [Test]
        public void ValveToServiceTest()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder.WithValveAsset(SetUpFixtureBase.Selenium,
                    SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderAssetValveToService(SetUpFixtureBase.Selenium, ref order);
        }

        [Test]
        public void SewerOpeningToValveTest()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithSewerOpeningAsset(
                SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderAssetSewerOpeningToValve(SetUpFixtureBase.Selenium, ref order);
        }

        [Test]
        public void ServiceToMainBreakTest()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder.WithServiceAsset(SetUpFixtureBase.Selenium,
                    SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderAssetServiceToMainForMainBreak(SetUpFixtureBase.Selenium, ref order);
        }

        [Test]
        public void ServiceToMainBreakTestAfterWorkHasBeenStarted()
        {
            var selenium = SetUpFixtureBase.Selenium;
            
            Navigate.ToInput(selenium);
            var order = WorkOrder.WithServiceAsset(selenium, SetUpFixtureBase.UserName);
            
            Navigate.ToScheduling(selenium);
            var assignment = Assign.OrderToDefaultCrew(selenium, order);

            //Navigate.ToCrewAssignments(selenium);
            Crew.StartWork(selenium, order, assignment);
            Navigate.ToGeneralSearch(selenium);
            Alter.ChangeOrderAssetServiceToMainForMainBreak(selenium, ref order);
        }
    }
}
