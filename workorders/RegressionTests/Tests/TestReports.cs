using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;

namespace RegressionTests.Tests
{
    #region Constants

    #endregion
    
    [TestFixture]
    public class TestReports
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void TestCompletedWorkOrders()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithMainAsset(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            var assignment = Assign.OrderToDefaultCrew(SetUpFixtureBase.Selenium, order);
            MakeFinal.FromCrewAssignmentsScreen.OrderWithMarkout(
                SetUpFixtureBase.Selenium, ref order,
                ref assignment, SetUpFixtureBase.UserName);
            Navigate.ToSupervisorApproval(SetUpFixtureBase.Selenium);
            Supervisor.ApproveOrder(SetUpFixtureBase.Selenium, order);

            Navigate.ToCompletedOrders(SetUpFixtureBase.Selenium);
            Verify.CompletedOrderIsReturned(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestCrewAssignmentReports()
        { 
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var orderClosed = WorkOrder.WithMainAsset(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var orderOpen = WorkOrder.WithMainAsset(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
           
            Navigate.ToScheduling(SetUpFixtureBase.Selenium);

            var closedAssignment = Assign.OrderToDefaultCrew(SetUpFixtureBase.Selenium, orderClosed);

            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            
            var openAssignment = Assign.OrderToDefaultCrew(SetUpFixtureBase.Selenium, orderOpen);           

            //Test Closed reports
            //Can't finalize a report with open assignments. Probably need to create a new order for 
            //open assignment testing
            MakeFinal.FromCrewAssignmentsScreen.OrderWithMarkout(
                SetUpFixtureBase.Selenium, ref orderClosed,
                ref closedAssignment, SetUpFixtureBase.UserName);
            
            //Look for closed assignments
            Navigate.ToCrewAssignmentSummaryReport(SetUpFixtureBase.Selenium);
            Verify.IsOpenFilterTest(SetUpFixtureBase.Selenium, orderOpen, orderClosed, false);
            
            //Look for open assignments
            Navigate.ToCrewAssignmentSummaryReport(SetUpFixtureBase.Selenium);
            Verify.IsOpenFilterTest(SetUpFixtureBase.Selenium, orderOpen, orderClosed, true);

            //Look for open and closed assignments
            Navigate.ToCrewAssignmentSummaryReport(SetUpFixtureBase.Selenium);
            Verify.IsOpenFilterTest(SetUpFixtureBase.Selenium, orderOpen, orderClosed, null);
        }
    }
}
