using System.Linq;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class EquipmentModelRepositoryTest : InMemoryDatabaseTest<EquipmentModel, EquipmentModelRepository>
    {
        #region Tests

        [TestMethod]
        public void TestGetByEquipmentManufacturerReturnsEquipmentWithEquipmentManufacturer()
        {
            var manufacturer1 = GetFactory<EquipmentManufacturerFactory>().Create();
            var manufacturer2 = GetFactory<EquipmentManufacturerFactory>().Create();
            var model1 = GetFactory<EquipmentModelFactory>().Create(new {EquipmentManufacturer = manufacturer1});
            var model2 = GetFactory<EquipmentModelFactory>().Create(new {EquipmentManufacturer = manufacturer2});

            var result = Repository.GetByEquipmentManufacturerId(manufacturer1.Id);

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(model1));
            Assert.IsFalse(result.Contains(model2));
        }

        #endregion
    }
}
