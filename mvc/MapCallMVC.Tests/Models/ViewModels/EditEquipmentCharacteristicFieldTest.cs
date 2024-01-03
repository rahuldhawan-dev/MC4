using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels 
{
    [TestClass]
    public class EditEquipmentCharacteristicFieldTest : ViewModelTestBase<EquipmentCharacteristicField, EditEquipmentCharacteristicField>
    {
        #region Tests
        
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.IsActive, false, true);
            _vmTester.CanMapBothWays(x => x.Order);
            _vmTester.CanMapBothWays(x => x.Description);
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
            ValidationAssert.PropertyHasMaxStringLength(x => x.Description, 100);
        }

        [TestMethod]
        public void TestMapToEntityAddsDropDownValuesToEntity()
        {
            _viewModel.DropDownValues = new[] {
                "test1",
                "test2"
            };

            // Sanity check
            Assert.AreEqual(0, _entity.DropDownValues.Count);
            
            _vmTester.MapToEntity();
            
            Assert.AreEqual(2, _entity.DropDownValues.Count);
        }

        [TestMethod]
        public void TestValidationFailsIfIsSAPCharacteristic()
        {
            var field = GetFactory<EquipmentCharacteristicFieldFactory>().Create(new { IsSAPCharacteristic = true });
            _viewModel.Id = field.Id;
            
            _viewModel.DropDownValues = new[] {
                "test1", 
                "test2"
            };
            
            ValidationAssert.ModelStateHasNonPropertySpecificError(EditEquipmentCharacteristicField.ErrorMessages.EDIT_SAP_DROPDOWN_VALUES);
        }
        
        #endregion
    }
}
