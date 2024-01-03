using RegressionTests.Lib.TestParts.Create;
using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestChangeOthers
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        /// <summary>
        /// Make sure the notes match themselves in this scenario. Notes were duplicating and appending to themselves.
        /// </summary>
        [Test]
        public void OrcomOrderNotesTest()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithMainAsset(SetUpFixtureBase.Selenium, SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderOrcomOrderNoChanges(SetUpFixtureBase.Selenium,
                ref order);
        }
    }
}
