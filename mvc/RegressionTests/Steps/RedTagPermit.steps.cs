using System.Collections.Specialized;
using MapCall.Common.Model.Entities;
using MMSINC.Testing.SpecFlow.Library;
using MMSINC.Testing.SpecFlow.StepDefinitions;
using TechTalk.SpecFlow;

namespace RegressionTests.Steps
{
    [Binding]
    public sealed class RedTagPermit
    {
        private readonly ScenarioContext _scenarioContext;

        public RedTagPermit(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When("^I go to the new page for a red tag permit with production work order: \"([^\"]+)\", operating center: \"([^\"]+)\", equipment: \"([^\"]+)\"")]
        public static void WhenIBeAtTheNewPageForARedTagPermit(
            string productionWorkOrderIdentifier,
            string operatingCenterIdentifier,
            string equipmentIdentifier)
        {

            var productionWorkOrder = TestObjectCache.Instance.Lookup<ProductionWorkOrder>("production work order", productionWorkOrderIdentifier);
            var operatingCenter = TestObjectCache.Instance.Lookup<OperatingCenter>("operating center", operatingCenterIdentifier);
            var equipment = TestObjectCache.Instance.Lookup<Equipment>("equipment", equipmentIdentifier);

            Navigation.GivenIAmAtAPage($"HealthAndSafety/RedTagPermit/New?productionWorkOrder={productionWorkOrder.Id}&operatingCenter={operatingCenter.Id}&equipment={equipment.Id}");
        }
    }
}
