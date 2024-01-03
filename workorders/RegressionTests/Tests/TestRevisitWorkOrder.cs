using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestRevisitWorkOrder
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        [Test]
        public void DoTest()
        {
            //HYDRANT ORDER
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            var order = WorkOrder.WithHydrantAsset(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            order = WorkOrder.ForRevisitWithHydrantAssetWithTownSection(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName, order);
            
            //SERVICE ORDER IN TOWN WITH TOWN SECTIONS
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            order = WorkOrder.WithServiceAssetWithoutTownSection(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            order =
                WorkOrder.
                    ForRevisitWithServiceAssetWithoutTownSection(SetUpFixtureBase.Selenium,
                        SetUpFixtureBase.UserName, order);

            //SERVICE ORDER IN TOWN WITH NO TOWN SECTIONS
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            order = WorkOrder.WithServiceAssetInTownWithNoTownSections(SetUpFixtureBase.Selenium,
                SetUpFixtureBase.UserName);
            Navigate.ToInput(SetUpFixtureBase.Selenium);
            order =
                WorkOrder.
                    ForRevisitWithServiceAssetInTownWithNoTownSections(SetUpFixtureBase.Selenium,
                        SetUpFixtureBase.UserName, order);
        }
    }
}
