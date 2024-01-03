using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class OperatingCenterTest
    {
        [TestMethod]
        public void TestPlanningPlantsReturnCorrectPlanningPlant()
        {
            var operatingCenter = new OperatingCenter();
            var productionPlanningPlant = new PlanningPlant
                {Description = "Production A", Code = "P123", OperatingCenter = operatingCenter};
            var distributionPlanningPlant = new PlanningPlant
                {Description = "Distribution A", Code = "D123", OperatingCenter = operatingCenter};
            var sewerPlanningPlant = new PlanningPlant
                {Description = "Sewer A", Code = "S123", OperatingCenter = operatingCenter};
            operatingCenter.PlanningPlants.Add(productionPlanningPlant);
            operatingCenter.PlanningPlants.Add(distributionPlanningPlant);
            operatingCenter.PlanningPlants.Add(sewerPlanningPlant);

            Assert.AreEqual(productionPlanningPlant, operatingCenter.ProductionPlanningPlant);
            Assert.AreEqual(distributionPlanningPlant, operatingCenter.DistributionPlanningPlant);
            Assert.AreEqual(sewerPlanningPlant, operatingCenter.SewerPlanningPlant);
        }
    }
}
