using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using RegressionTests.Lib.TestParts;
using RegressionTests.Lib.TestParts.Create;

namespace RegressionTests.Tests
{
    [TestFixture]
    public class TestEquipmentOrder
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Data.ToggleSAP(false);
            Data.ToggleMarkoutsEditable(false);
        }

        /// <summary>
        /// IS THIS FAILING????????????????????
        /// It's likely because it manually
        /// creates a work order with a connection
        /// string that is pointing at a different
        /// db than the regression site. Check web.config
        /// and regressiontests/app.config are in sync.
        /// </summary>
        [Test]
        public void TestUserCanCreateAnEquipmentOrder()
        {
            try
            {
                Data.CreateTAndDFacilities();
                //Navigate.ToInput(SetUpFixtureBase.Selenium);
                var order =
                    WorkOrder.WithEquipmentAsset(SetUpFixtureBase.Selenium,
                        SetUpFixtureBase.UserName);
                Navigate.ToGeneralSearch(SetUpFixtureBase.Selenium);
                Verify.CurrentOrderHasEquipmentPurpose(SetUpFixtureBase.Selenium,
                    order);
                Navigate.ToFinalization(SetUpFixtureBase.Selenium);
                Verify.CurrentOrderAppearsInFinalization(SetUpFixtureBase.Selenium, order);

            }
            finally
            {
                Data.RollbackTAndDFacilities();
            }
        }
    }
}
