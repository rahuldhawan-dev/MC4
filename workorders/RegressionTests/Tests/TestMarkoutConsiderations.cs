using System;
using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestMarkoutConsiderations
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void TestOrderCreatedWithEmergencyMarkoutRequirementBypassesPlanning()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder
                    .WithEmergencyMarkoutRequirement(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderAppearsInScheduling(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestOrderCreatedWithNoMarkoutRequirementBypassesPlanning()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder
                    .WithNoMarkoutRequirement(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToPlanning(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderDoesNotAppearInPlanning(SetUpFixtureBase.Selenium, order);
            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderAppearsInScheduling(SetUpFixtureBase.Selenium, order);
        }

        [Test]
        public void TestOrderCreatedWithRoutineMarkoutRequirementDoesNotBypassPlanning()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder
                    .WithRoutineMarkoutRequirement(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToPlanning(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderAppearsInPlanning(SetUpFixtureBase.Selenium, order);
            Navigate.ToScheduling(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderDoesNotAppearInScheduling(SetUpFixtureBase.Selenium, order);
        }
        
        [Test]
        public void TestMarkoutsNotEditableDoesNotAllowReadyAndExpirationDateEntry()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder
                    .WithRoutineMarkoutRequirement(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToPlanning(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderAppearsInPlanning(SetUpFixtureBase.Selenium, order);
                        
            var markout = Markout.WithDateOfRequest(SetUpFixtureBase.Selenium, order, DateTime.Now.ToString());
        }

        [Test]
        public void TestMarkoutsEditableAllowReadyAndExpirationDateEntry()
        {
            Data.ToggleMarkoutsEditable(true);
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order =
                WorkOrder
                    .WithRoutineMarkoutRequirement(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToPlanning(SetUpFixtureBase.Selenium);
            Verify.CurrentOrderAppearsInPlanning(SetUpFixtureBase.Selenium, order);

            var now = DateTime.Now;
            var markout = Markout.WithAllDates(SetUpFixtureBase.Selenium, order, now, now.AddDays(1), now.AddDays(2));
            Data.ToggleMarkoutsEditable(false);
        }
        
    }
}
