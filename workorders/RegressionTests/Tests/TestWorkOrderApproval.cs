using System.Configuration;
using MapCall.Common.Testing.Selenium;
using MMSINC.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;

namespace RegressionTests.Tests
{
    /// <summary>
    /// Summary description for TestWorkOrderApproval
    /// </summary>
    [TestFixture]
    public class TestWorkOrderApproval
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void TestNormalApproval()
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
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderIsApproved(SetUpFixtureBase.Selenium, order);
            Navigate.ToStockApproval(SetUpFixtureBase.Selenium);
            StockClerk.ApproveOrderStock(SetUpFixtureBase.Selenium, order);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderIsApproved(SetUpFixtureBase.Selenium, order);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderCannotBeCancelled(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestNormalRejectionOnMainBreakOrder()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            
            var order = WorkOrder.WithMainAssetForMainBreak(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            
            var assignment = Assign.OrderToDefaultCrew(SetUpFixtureBase.Selenium, order);
            MakeFinal.FromCrewAssignmentsScreen.OrderWithMarkoutForMainBreak(
                SetUpFixtureBase.Selenium, ref order,
                ref assignment, SetUpFixtureBase.UserName);
            Navigate.ToSupervisorApproval(SetUpFixtureBase.Selenium);
            Supervisor.RejectOrder(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestRequisitions()
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
            Supervisor.Requisition(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestApprovalWithGISNotificationValueSet()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithMainAssetForMainBreak(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            var assignment = Assign.OrderToDefaultCrew(SetUpFixtureBase.Selenium, order);
            MakeFinal.FromCrewAssignmentsScreen.OrderWithMarkoutForMainBreak(
                SetUpFixtureBase.Selenium, ref order,
                ref assignment, SetUpFixtureBase.UserName);
            Navigate.ToSupervisorApproval(SetUpFixtureBase.Selenium);
            Supervisor.ApproveMainBreakOrder(SetUpFixtureBase.Selenium, order);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderIsApproved(SetUpFixtureBase.Selenium, order);
            Navigate.ToStockApproval(SetUpFixtureBase.Selenium);
            StockClerk.ApproveOrderStock(SetUpFixtureBase.Selenium, order);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderIsApproved(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestApprovalDoesNotNullOutSOPRequired()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithMainAsset(SetUpFixtureBase.Selenium,SetUpFixtureBase.UserName);
            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            var assignment = Assign.OrderToDefaultCrew(SetUpFixtureBase.Selenium, order);
            MakeFinal.FromCrewAssignmentsScreen.OrderWithMarkout(SetUpFixtureBase.Selenium, ref order,ref assignment, SetUpFixtureBase.UserName);
            Data.ToggleSOPRequired(order, true);
            Navigate.ToSupervisorApproval(SetUpFixtureBase.Selenium);
            Supervisor.ApproveOrder(SetUpFixtureBase.Selenium, order);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderStreetOpeningPermitRequired(SetUpFixtureBase.Selenium, order);
        }
    }
}
