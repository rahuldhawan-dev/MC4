using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestRestorationRefresh
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void TestOrderInFinalizationDoesNotResubmitSpoils()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithValveAsset(
                SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            var assignment = Assign.OrderToDefaultCrew(SetUpFixtureBase.Selenium, order);
            MakeFinal.FromCrewAssignmentsScreen.OrderWithSpoilAndRestoration(
                SetUpFixtureBase.Selenium, ref order, ref assignment);
        }
    }
}
