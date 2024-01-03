using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.ViewModels 
{
    [TestClass]
    public class AddEquipmentMaintenancePlanTest : MapCallMvcInMemoryDatabaseTestBase<MaintenancePlan>
    {
        private ViewModelTester<AddEquipmentMaintenancePlan, MaintenancePlan> _vmTester;
        private AddEquipmentMaintenancePlan _viewModel;
        private MaintenancePlan _entity;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<MaintenancePlan>().Create();
            _viewModel = _viewModelFactory.Build<AddEquipmentMaintenancePlan, MaintenancePlan>(_entity);
            _vmTester = new ViewModelTester<AddEquipmentMaintenancePlan, MaintenancePlan>(_viewModel, _entity);
        }

        #endregion

        [TestMethod]
        public void TestMapToEntityAddsEquipmentToMaintenancePlan()
        {
            var equipment = GetEntityFactory<Equipment>().CreateList(3);

            _viewModel.Equipment = equipment.Where(e => e.Id > 1).Select(x => x.Id).ToArray();
            _vmTester.MapToEntity();

            Assert.AreEqual(2, _entity.Equipment.Count);
            Assert.AreEqual(2, _entity.Equipment[0].Id);
            Assert.AreEqual(3, _entity.Equipment[1].Id);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotAddEquipmentToMaintenancePlanIfAlreadyAdded()
        {
            var equipment = GetEntityFactory<Equipment>().CreateList(6);
            
            _viewModel.Equipment = equipment.Where(x => x.Id < 4).Select(x => x.Id).ToArray(); // Equipment 1, 2, and 3
            
            _vmTester.MapToEntity();
            
            _viewModel.Equipment = _viewModel.Equipment.Concat(
                equipment.Where(x => x.Id > 2).Select(x => x.Id)).ToArray(); // Equipment 3, 4, 5, 6

            _vmTester.MapToEntity();

            Assert.AreEqual(6, _entity.Equipment.Count);
            for (int i = 0; i < equipment.Count; ++i)
            {
                Assert.AreEqual(i + 1, _entity.Equipment[i].Id);
            }
        }
    }
}