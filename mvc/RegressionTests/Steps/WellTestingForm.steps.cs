using System.Collections.Specialized;
using MapCall.Common.Model.Entities;
using MMSINC.Testing.SpecFlow.Library;
using MMSINC.Testing.SpecFlow.StepDefinitions;
using TechTalk.SpecFlow;

namespace RegressionTests.Steps
{
    [Binding]
    public sealed class WellTest
    {
        private readonly ScenarioContext _scenarioContext;

        public WellTest(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When("^I go to the new page for a well test with production work order: \"([^\"]+)\", operating center: \"([^\"]+)\", equipment: \"([^\"]+)\"")]
        public static void WhenIBeAtTheNewPageForWellTest(
            string productionWorkOrderIdentifier, 
            string operatingCenterIdentifier,
            string equipmentIdentifier)
        {

            var productionWorkOrder = TestObjectCache.Instance.Lookup<ProductionWorkOrder>("production work order", productionWorkOrderIdentifier);
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);
            var equipment = TestObjectCache.Instance.Lookup<Equipment>("equipment", equipmentIdentifier);

            Navigation.GivenIAmAtAPage($"Production/WellTest/New?productionWorkOrder={productionWorkOrder.Id}&equipment={equipment.Id}&operatingCenter={operatingCenter.Id}");
        }
    }
}
