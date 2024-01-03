using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class RemoveAllEquipmentMaintenancePlanTest : MapCallMvcInMemoryDatabaseTestBase<MaintenancePlan>
    {
        private ViewModelTester<RemoveAllEquipmentMaintenancePlan, MaintenancePlan> _vmTester;
        private RemoveAllEquipmentMaintenancePlan _viewModel;
        private MaintenancePlan _entity;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<MaintenancePlan>().Create();
            _viewModel = _viewModelFactory.Build<RemoveAllEquipmentMaintenancePlan, MaintenancePlan>(_entity);
            _vmTester = new ViewModelTester<RemoveAllEquipmentMaintenancePlan, MaintenancePlan>(_viewModel, _entity);
        }

        #endregion

        [TestMethod]
        public void TestMapToEntityRemovesAllEquipmentFromMaintenancePlan()
        {
            var equipment = GetEntityFactory<Equipment>().CreateList(3);

            _entity.Equipment = equipment;

            // Sanity check
            Assert.AreEqual(3, _entity.Equipment.Count);

            _vmTester.MapToEntity();

            Assert.AreEqual(0, _entity.Equipment.Count);
        }
    }
}
