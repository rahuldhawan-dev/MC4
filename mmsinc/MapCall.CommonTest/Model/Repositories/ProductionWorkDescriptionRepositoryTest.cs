using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class ProductionWorkDescriptionRepositoryTest : InMemoryDatabaseTest<ProductionWorkDescription, ProductionWorkDescriptionRepository>
    {
        #region Tests

        [TestMethod]
        public void TestGetMaintenancePlanWorkDescriptionReturnsUniqueMaintenancePlanWorkDescription()
        {
            GetFactory<ProductionWorkDescriptionFactory>().CreateList(5);
            var expectedPwd = GetFactory<UniqueMaintenancePlanProductionWorkDescriptionFactory>().Create();

            Session.Flush();

            var actualPwd = Repository.GetMaintenancePlanWorkDescription();

            Assert.IsNotNull(actualPwd);
            Assert.AreEqual(expectedPwd, actualPwd);
        }

        [TestMethod]
        public void TestGetMaintenancePlanWorkDescriptionThrowsIfUniqueMaintenancePlanWorkDescriptionIsNotFound()
        {
            GetFactory<ProductionWorkDescriptionFactory>().CreateList(5);

            Session.Flush();

            Assert.ThrowsException<InvalidOperationException>(() => Repository.GetMaintenancePlanWorkDescription());
        }

        [TestMethod]
        public void TestGetCorrectiveActionWorkDescriptionReturnsGeneralRepairCorrectiveActionWorkDescription()
        {
            var equipmentTypeGenerator = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var orderTypes = GetFactory<OrderTypeFactory>().CreateAll();
            var routinePwd = GetEntityFactory<ProductionWorkDescription>().Create(new {
                EquipmentType = equipmentTypeGenerator, 
                Description = "Routine", 
                OrderType = orderTypes[4] // Routine
            });
            var correctivePwd = GetEntityFactory<ProductionWorkDescription>().Create(new {
                EquipmentType = equipmentTypeGenerator, 
                Description = "GENERAL REPAIR", 
                OrderType = orderTypes[2] // Corrective Action
            });

            Session.Flush();

            var newPwd = Repository.GetCorrectiveActionWorkDescription(routinePwd.EquipmentType.Id);

            Assert.IsNotNull(newPwd);
            Assert.AreNotSame(routinePwd, newPwd);
            Assert.AreEqual(correctivePwd, newPwd);
            Assert.AreEqual(orderTypes[2], newPwd.OrderType);
        }

        [TestMethod]
        public void TestGetCorrectiveActionWorkDescriptionThrowsIfGeneralRepairWorkDescriptionIsNotFound()
        {
            GetFactory<ProductionWorkDescriptionFactory>().CreateList(5);
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var routinePwd = GetFactory<UniqueMaintenancePlanProductionWorkDescriptionFactory>().Create(new {
                EquipmentType = equipmentType
            });

            Session.Flush();

            Assert.ThrowsException<InvalidOperationException>(() => Repository.GetCorrectiveActionWorkDescription(routinePwd.EquipmentType.Id));
        }

        #endregion
    }
}
