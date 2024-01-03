using MapCall.Common.Testing.Selenium;
using MapCall.Common.Testing.Selenium.TestParts;
using NUnit.Framework;
using RegressionTests.Lib;
using RegressionTests.Lib.TestParts;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestEditCrewAssignment
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void TestAdministratorCanEditCrewAssignmentStartEndAndEmployeesOnJobFromGeneralScreen()
        {
           // Data.FixMaterial();
            var createdBy = Login.AsAdmin(SetUpFixtureBase.Selenium);
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                Lib.TestParts.Create.WorkOrder.WithHydrantAsset(SetUpFixtureBase.Selenium,
                    createdBy);
            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            var assignment = Assign.OrderToDefaultCrew(SetUpFixtureBase.Selenium, order);
            MakeFinal.FromCrewAssignmentsScreen.OrderWithMarkout(SetUpFixtureBase.Selenium, ref order, ref assignment, createdBy);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderCrewAssignmentValues(SetUpFixtureBase.Selenium, order, ref assignment);
        }
    }
}
