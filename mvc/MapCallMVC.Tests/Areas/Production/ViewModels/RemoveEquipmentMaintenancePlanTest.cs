using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class RemoveEquipmentMaintenancePlanTest : MapCallMvcInMemoryDatabaseTestBase<MaintenancePlan>
    {
        private ViewModelTester<RemoveEquipmentMaintenancePlan, MaintenancePlan> _vmTester;
        private RemoveEquipmentMaintenancePlan _viewModel;
        private MaintenancePlan _entity;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<MaintenancePlan>().Create();
            _viewModel = _viewModelFactory.Build<RemoveEquipmentMaintenancePlan, MaintenancePlan>(_entity);
            _vmTester = new ViewModelTester<RemoveEquipmentMaintenancePlan, MaintenancePlan>(_viewModel, _entity);
        }

        #endregion

        [TestMethod]
        public void TestMapToEntityRemovesEquipmentFromMaintenancePlan()
        {
            var equipment = GetEntityFactory<Equipment>().CreateList(3);

            _entity.Equipment = equipment;

            // Sanity check
            Assert.AreEqual(3, _entity.Equipment.Count);

            _viewModel.Equipment = equipment.Where(x => x.Id > 1).Select(x => x.Id).ToArray();
            _vmTester.MapToEntity();

            Assert.AreEqual(1, _entity.Equipment.Count);
        }
    }
}
