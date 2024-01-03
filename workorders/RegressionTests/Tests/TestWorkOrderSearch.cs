using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestWorkOrderSearch
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void TestWorkOrderPrePlanningSearch()
        {
            Navigate.ToPlanning(SetUpFixtureBase.Selenium);
            Verify.PlanningSearchDoesNotError(SetUpFixtureBase.Selenium);
        }

        [Test]
        public void TestWorkOrderMarkoutPlanningSearch()
        {
            Navigate.ToMarkoutPlanning(SetUpFixtureBase.Selenium);
            Verify.MarkoutPlanningSearchDoesNotError(SetUpFixtureBase.Selenium);
        }

        [Test]
        public void TestSOPProcessingSearch()
        {
            Navigate.ToSOPProcessing(SetUpFixtureBase.Selenium);
            Verify.SOPProcessingSearchDoesNotError(SetUpFixtureBase.Selenium);
        }

        [Test]
        public void TestFinalizationSearch()
        {
            Navigate.ToFinalization(SetUpFixtureBase.Selenium);
            Verify.FinalizationSearchDoesNotError(SetUpFixtureBase.Selenium);
        }

        [Test]
        public void TestRestorationProcessingSearch()
        {
            Navigate.ToRestorationProcessing(SetUpFixtureBase.Selenium);
            Verify.RestorationProcessingSearchDoesNotError(SetUpFixtureBase.Selenium);
        }
    }
}
