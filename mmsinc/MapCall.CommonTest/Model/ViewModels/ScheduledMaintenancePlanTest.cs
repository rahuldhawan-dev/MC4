using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.ViewModels
{
    [TestClass]
    public class ScheduledMaintenancePlanTest : InMemoryDatabaseTest<MaintenancePlan>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container.Inject<IDateTimeProvider>(new DateTimeProvider());
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestScheduledMaintenancePlanConstructorProperlyAssignsEachField()
        {
            var plan = _container.GetInstance<MaintenancePlanFactory>().Create();
            var equipment = _container.GetInstance<EquipmentFactory>().Create();
            var equipmentType = _container.GetInstance<EquipmentTypeFactory>().Create();
            equipment.MaintenancePlans.Add(new EquipmentMaintenancePlan { Equipment = equipment, MaintenancePlan = plan });
            plan.Equipment.Add(equipment);
            plan.EquipmentTypes.Add(equipmentType);

            var scheduledPlan = new ScheduledMaintenancePlan(plan);

            Assert.AreEqual(scheduledPlan.LocalTaskDescription, plan.LocalTaskDescription);
            Assert.AreEqual(scheduledPlan.Equipment.First(), equipment);
            Assert.AreEqual(scheduledPlan.Facility, plan.Facility);
            Assert.AreEqual(scheduledPlan.MaintenancePlanId, plan.Id);
            Assert.AreEqual(scheduledPlan.OperatingCenterId, plan.OperatingCenter.Id);
            Assert.AreEqual(scheduledPlan.PlanningPlantId, plan.PlanningPlant.Id);
            Assert.AreEqual(scheduledPlan.ProductionWorkOrderFrequencyId, plan.ProductionWorkOrderFrequency.Id);
            Assert.AreEqual(scheduledPlan.Start, plan.Start);
            Assert.AreEqual(scheduledPlan.WorkDescription.Id, plan.WorkDescription.Id);
            Assert.AreEqual(scheduledPlan.EquipmentType.Id, plan.EquipmentTypes.FirstOrDefault().Id);
        }

        #endregion
    }
}
