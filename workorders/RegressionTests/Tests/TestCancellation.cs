using System;
using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestCancellation
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        private Types.WorkOrder CreateOrder(Func<IExtendedSelenium, string, Types.WorkOrder> createFn)
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = createFn(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            return order;
        }

        [Test]
        public void TestCancelledOrderDoesNotShowUpInPlanning()
        {
            var order = CreateOrder(WorkOrder.WithRoutineMarkoutRequirement);

            Navigate.ToPlanning(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderAppearsInPlanning(SetUpFixtureBase.Selenium, order);

            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.CancelOrder(SetUpFixtureBase.Selenium, order);

            Navigate.ToPlanning(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderDoesNotAppearInPlanning(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestCancelledOrderDoesNotShowUpInPrePlanning()
        {
            var order = CreateOrder(WorkOrder.WithRoutineMarkoutRequirement);

            Navigate.ToPrePlanning(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderAppearsInPrePlanning(SetUpFixtureBase.Selenium, order);

            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.CancelOrder(SetUpFixtureBase.Selenium, order);

            Navigate.ToPrePlanning(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderDoesNotAppearInPrePlanning(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestCancelledOrderDoesNotShowUpInMarkoutPlanning()
        {
            var order = CreateOrder(WorkOrder.WithRoutineMarkoutRequirement);

            Navigate.ToMarkoutPlanning(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderAppearsInMarkoutPlanning(SetUpFixtureBase.Selenium, order);

            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.CancelOrder(SetUpFixtureBase.Selenium, order);

            Navigate.ToMarkoutPlanning(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderDoesNotAppearInMarkoutPlanning(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestCancelledOrderDoesNotShowUpInScheduling()
        {
            var order = CreateOrder(WorkOrder.WithNoMarkoutRequirement);

            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderAppearsInScheduling(SetUpFixtureBase.Selenium, order);

            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.CancelOrder(SetUpFixtureBase.Selenium, order);

            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderDoesNotAppearInScheduling(SetUpFixtureBase.Selenium, order);
        }

        // TODO: Could not get this working:
        //[Test]
        //public void TestCancelledOrderDoesNotShowUpInRestorationProcessing()
        //{
        //    var order = CreateOrder(WorkOrder.WithNoMarkoutRequirement);

        //    Navigate.ToRestorationProcessing(SetUpFixtureBase.Selenium);
        //    Verify.CurrentOrderAppearsInRestorationProcessing(SetUpFixtureBase.Selenium, order);

        //    Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
        //    Alter.CancelOrder(SetUpFixtureBase.Selenium, order);

        //    Navigate.ToRestorationProcessing(SetUpFixtureBase.Selenium);
        //    Verify.CurrentOrderDoesNotAppearInRestorationProcessing(SetUpFixtureBase.Selenium, order);
        //}

        [Test]
        public void TestCancelledOrderDoesNotShowUpInSOPProcessing()
        {
            var order = CreateOrder(WorkOrder.WithNoMarkoutRequirementButSOPRequired);

            Navigate.ToSOPProcessing(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderAppearsInSOPProcessing(SetUpFixtureBase.Selenium, order);

            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.CancelOrder(SetUpFixtureBase.Selenium, order);

            Navigate.ToSOPProcessing(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderDoesNotAppearInSOPProcessing(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestCancellationIsNotAvailableInStockApproval()
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

            Navigate.ToStockApproval(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderAppearsInStockApproval(SetUpFixtureBase.Selenium, order);

            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderCannotBeCancelled(SetUpFixtureBase.Selenium, order);
        }

        /// <summary>
        /// Is this FAILING? MAKE SURE YOU'RE DATABASE IN WEB.CONFIG IS THE SAME AS THE TEST
        /// </summary>
        [Test]
        public void TestCancelledOrderDoesNotShowUpInFinalization()
        {
            try
            {
                Data.CreateTAndDFacilities();
                var order = CreateOrder(WorkOrder.WithEquipmentAsset); /// Is this FAILING? MAKE SURE YOU'RE DATABASE IN WEB.CONFIG IS THE SAME AS THE TEST

                Navigate.ToFinalization(SetUpFixtureBase.Selenium);
                Verify.CurrentOrderAppearsInFinalization(
                    SetUpFixtureBase.Selenium, order);

                Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
                Alter.CancelOrder(SetUpFixtureBase.Selenium, order);

                Navigate.ToFinalization(SetUpFixtureBase.Selenium);
                Verify.CurrentOrderDoesNotAppearInFinalization(
                    SetUpFixtureBase.Selenium, order);
            }
            finally
            {
                Data.RollbackTAndDFacilities();
            }
        }

        [Test]
        public void TestCancellationIsNotAvailableInSupervisorApproval()
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
            Verify.CurrentOrderAppearsInSupervisorApproval(SetUpFixtureBase.Selenium, order);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderCannotBeCancelled(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestCancellationIsNotAvailableForOrdersWithActiveCrewAssignments()
        {
            var order = CreateOrder(WorkOrder.WithNoMarkoutRequirement);

            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            Assign.OrderToDefaultCrew(SetUpFixtureBase.Selenium, order);
            
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderCannotBeCancelled(SetUpFixtureBase.Selenium, order);
        }

        /// <summary>
        /// Added this to an existing test to save regression test time
        /// TestWorkOrderApproval.TestNormalApproval
        /// </summary>
        [Test]
        public void TestCancellationIsNotAvailableForCompletedOrder()
        {
            
        }

        [Test]
        public void TestCancellationIsNotAvailableForOrdersAfterCancellingThem()
        {
            var order = CreateOrder(WorkOrder.WithNoMarkoutRequirement);

            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.CancelOrder(SetUpFixtureBase.Selenium, order);

            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderCannotBeCancelled(SetUpFixtureBase.Selenium, order);
        }
    }
}
