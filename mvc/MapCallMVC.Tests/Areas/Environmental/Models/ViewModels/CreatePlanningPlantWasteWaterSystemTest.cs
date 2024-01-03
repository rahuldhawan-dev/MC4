using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class CreatePlanningPlantWasteWaterSystemTest : ViewModelTestBase<PlanningPlantWasteWaterSystem, CreatePlanningPlantWasteWaterSystem>
    {
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.PlanningPlant, GetEntityFactory<PlanningPlant>().Create());
            _vmTester.CanMapBothWays(x => x.WasteWaterSystem, GetEntityFactory<WasteWaterSystem>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.PlanningPlant);
            ValidationAssert.PropertyIsRequired(x => x.WasteWaterSystem);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.PlanningPlant, GetEntityFactory<PlanningPlant>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation() { }
    }
}
