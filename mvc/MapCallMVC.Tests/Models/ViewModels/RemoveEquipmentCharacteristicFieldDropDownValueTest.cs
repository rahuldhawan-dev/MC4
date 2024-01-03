using Historian.Data.Client.Repositories;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCallMVC.Tests.Models.ViewModels 
{
    [TestClass]
    public class RemoveEquipmentCharacteristicFieldDropDownValueTest : ViewModelTestBase<EquipmentCharacteristicField, RemoveEquipmentCharacteristicFieldDropDownValue>
    {
        #region Fields

        private Mock<IEquipmentRepository> _mockEquipmentRepo;
        
        #endregion

        #region Setup/Teardown
        
        [TestInitialize]
        public void TestInitialize()
        {
            _mockEquipmentRepo = new Mock<IEquipmentRepository>();
            _container.Inject(_mockEquipmentRepo.Object);
        }
        
        #endregion
        
        #region Tests
        
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // noop
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            // noop
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // noop
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop
        }

        [TestMethod]
        public void TestMapToEntityRemovesDropDownValuesFromEntity()
        {
            var dropdownValue = new EquipmentCharacteristicDropDownValue {
                Field = _entity,
                Value = "Test1"
            };
            
            Session.Save(dropdownValue);
            
            _entity.DropDownValues.Add(dropdownValue);
            
            Session.Flush();
            
            _viewModel.SelectedDropDownValue = 1;
            _vmTester.MapToEntity();
            
            Assert.AreEqual(0, _entity.DropDownValues.Count);
        }

        [TestMethod]
        public void TestValidationFailsIfIsSAPCharacteristic()
        {
            _mockEquipmentRepo
               .Setup(x => x.IsCharacteristicDropDownValueCurrentlyInUse(
                    It.IsAny<EquipmentCharacteristicField>(), 
                    It.IsAny<int>()))
               .Returns(false);
            
            var field = GetFactory<EquipmentCharacteristicFieldFactory>().Create(new { IsSAPCharacteristic = true });
            _viewModel.Id = field.Id;

            _viewModel.SelectedDropDownValue = 1;
            
            ValidationAssert.ModelStateHasNonPropertySpecificError(RemoveEquipmentCharacteristicFieldDropDownValue.ErrorMessages.REMOVE_SAP_DROPDOWN_VALUES);
        }
        
        [TestMethod]
        public void TestValidationFailsIfDropDownValueIsAlreadyInUse()
        {
            _mockEquipmentRepo
               .Setup(x => x.IsCharacteristicDropDownValueCurrentlyInUse(
                    It.IsAny<EquipmentCharacteristicField>(), 
                    It.IsAny<int>()))
               .Returns(true);
            
            _viewModel.SelectedDropDownValue = 1;
            
            ValidationAssert.ModelStateHasNonPropertySpecificError(RemoveEquipmentCharacteristicFieldDropDownValue.ErrorMessages.ALREADY_IN_USE);
        }
        
        #endregion
    }
}
