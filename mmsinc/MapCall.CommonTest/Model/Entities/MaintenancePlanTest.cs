using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class MaintenancePlanTest : InMemoryDatabaseTest<MaintenancePlan>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
        }

        #endregion

        [TestMethod]
        public void TestToStringReturnsIdFacilityTaskGroup()
        {
            // Arrange
            var facility = new Facility {
                Id = 13,
                OperatingCenter = new OperatingCenter { OperatingCenterCode = "NJSB" },
                FacilityName = "Some Facility"
            };

            var taskGroup = new TaskGroup {
                Id = 21, 
                TaskGroupName = "Task Group TestName"
            };

            var productionWorkOrderFrequency = new ProductionWorkOrderFrequency {
                Id = 1, 
                Name = "Some Production Work Order Frequency"
            };

            var target = new MaintenancePlan {
                Id = 99, Facility = facility, TaskGroup = taskGroup,
                ProductionWorkOrderFrequency = productionWorkOrderFrequency
            };

            // Act
            var result = target.ToString();

            // Assert
            Assert.AreEqual("Some Facility : Task Group TestName : SOME PRODUCTION WORK ORDER FREQUENCY", result);
        }

        #region AssetCount

        [TestMethod]
        public void Test_CountEquipmentMaintenancePlansByMaintenancePlan_ReturnsTheCorrectCount()
        {
            var maintenancePlan = GetFactory<MaintenancePlanFactory>().Create();
            var equipment = GetFactory<EquipmentFactory>().Create();
            maintenancePlan.Equipment.Add(equipment);
            Session.Flush();
            Session.Clear();

            var result = Session.Get<MaintenancePlan>(maintenancePlan.Id);

            Assert.AreEqual(1, result.CountEquipmentMaintenancePlansByMaintenancePlan.AssetCount);
        }

        #endregion
    }
}