using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Facilities.Models.ViewModels
{
    public abstract class TankInspectionViewModelTest<TViewModel> : ViewModelTestBase<TankInspection, TViewModel>
        where TViewModel : TankInspectionViewModel
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            GetEntityFactory<TankInspectionQuestion>().Create();
            _viewModel.ZipCode = "123456";
        }

        #endregion

        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.TankAddress);
            _vmTester.CanMapBothWays(x => x.ZipCode);
            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            _vmTester.CanMapBothWays(x => x.ProductionWorkOrder, GetEntityFactory<ProductionWorkOrder>().Create());
            _vmTester.CanMapBothWays(x => x.Facility, GetEntityFactory<Facility>().Create());
            _vmTester.CanMapBothWays(x => x.Equipment, GetEntityFactory<Equipment>().Create());
            _vmTester.CanMapBothWays(x => x.TankObservedBy, GetEntityFactory<Employee>().Create());
            _vmTester.CanMapBothWays(x => x.PublicWaterSupply, GetEntityFactory<PublicWaterSupply>().Create());
            _vmTester.CanMapBothWays(x => x.Town, GetEntityFactory<Town>().Create());
            _vmTester.CanMapBothWays(x => x.Coordinate, GetEntityFactory<Coordinate>().Create());
            _vmTester.CanMapBothWays(x => x.TankInspectionType, GetEntityFactory<TankInspectionType>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Coordinate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TankInspectionType);
        }

        [TestMethod]
        public void TestNotRequiredFields()
        {
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.TankAddress);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.ZipCode);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.TankAddress, TankInspection.StringLengths.GENERAL_TAB_LENGTH);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ZipCode, TankInspection.StringLengths.ZIP_CODE);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Town, GetEntityFactory<Town>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Coordinate, GetEntityFactory<Coordinate>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Equipment, GetEntityFactory<Equipment>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.TankObservedBy, GetEntityFactory<Employee>().Create());
        }

        [TestMethod]
        public void TestMapToEntitySetsCoordinateToFacilityAndTankInspection()
        {
            var expected = GetEntityFactory<Coordinate>().Create();
            var unexpected = GetEntityFactory<Coordinate>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { Coordinate = unexpected });
            _viewModel.Coordinate = expected.Id;
            _viewModel.Facility = facility.Id;
            _entity.Coordinate = null;

            _vmTester.MapToEntity();

            Assert.AreSame(expected, _entity.Coordinate);
            Assert.AreSame(expected, facility.Coordinate);
        }

        [TestMethod]
        public void TestMapToEntitySetsCapacityToEquipmentAndTankInspection()
        {
            decimal expected = 2.020202M;
            decimal unexpected = 4.444444M;
            var eq = GetEntityFactory<Equipment>().Create();
            var eqF = GetEntityFactory<EquipmentCharacteristicField>().Create();
            var eqChar = GetEntityFactory<EquipmentCharacteristic>().Create(new { Field = eqF, Equipment = eq, Value = Convert.ToString(unexpected) });

            eqChar.Field.FieldName = "TNK_VOLUME";
            eqChar.Field.Description = "TNK_VOLUME";
            eq.Characteristics.Add(eqChar);
            _viewModel.Equipment = eq.Id;
            _viewModel.TankCapacity = expected;
            _entity.Equipment = eq;
            _entity.Coordinate = null;

            _vmTester.MapToEntity();

            Assert.AreEqual(expected, _entity.TankCapacity);
            Assert.AreEqual(Convert.ToString(expected), _entity.Equipment.Characteristics[0].Value);
            Assert.AreEqual(Convert.ToString(expected), eq.Characteristics[0].Value);
        }

        #endregion
    }

    [TestClass]
    public class CreateTankInspectionTest : TankInspectionViewModelTest<CreateTankInspection>
    {
        [TestMethod]
        public void TestSetDefaultsSetsTankInspectionQuestionsAndInspectionTypeValues()
        {
            var questions = GetEntityFactory<TankInspectionQuestionType>().CreateList(4);
            _viewModel.SetDefaults();

            Assert.AreEqual(TankInspectionType.Indices.ROUTINE, _viewModel.TankInspectionType);

            foreach (var question in questions)
            {
                Assert.IsTrue(_viewModel.TankInspectionQuestions.Any(x => x.TankInspectionQuestionType == question.Id));
            }
        }
    }

    [TestClass]
      public class EditTankInspectionTest : TankInspectionViewModelTest<EditTankInspection> { }
}