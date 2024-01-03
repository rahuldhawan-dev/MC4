using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestChangeTown
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void TownAndTownSectionToTownWithNoTownSectionTest()
        {
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithMainAsset(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
            Alter.ChangeOrderTownWithNoTownSection(SetUpFixtureBase.Selenium, ref order);
        }
    }
}
