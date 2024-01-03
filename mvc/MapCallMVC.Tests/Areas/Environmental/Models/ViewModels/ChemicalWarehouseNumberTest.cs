using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class CreateChemicalWarehouseNumberTest : MapCallMvcInMemoryDatabaseTestBase<ChemicalWarehouseNumber>
    {
        #region Fields

        private ViewModelTester<CreateChemicalWarehouseNumber, ChemicalWarehouseNumber> _vmTester;
        private CreateChemicalWarehouseNumber _viewModel;
        private ChemicalWarehouseNumber _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<CreateChemicalWarehouseNumber>();
            _entity = new ChemicalWarehouseNumber();
            _vmTester = new ViewModelTester<CreateChemicalWarehouseNumber, ChemicalWarehouseNumber>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.WarehouseNumber);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WarehouseNumber);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetFactory<UniqueOperatingCenterFactory>().Create());
        }

        #endregion
    }

    [TestClass]
    public class EditChemicalWarehouseNumberTest : MapCallMvcInMemoryDatabaseTestBase<ChemicalWarehouseNumber>
    {
        #region Fields

        private ViewModelTester<EditChemicalWarehouseNumber, ChemicalWarehouseNumber> _vmTester;
        private EditChemicalWarehouseNumber _viewModel;
        private ChemicalWarehouseNumber _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<EditChemicalWarehouseNumber>();
            _entity = new ChemicalWarehouseNumber();
            _vmTester = new ViewModelTester<EditChemicalWarehouseNumber, ChemicalWarehouseNumber>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.WarehouseNumber);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WarehouseNumber);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetFactory<UniqueOperatingCenterFactory>().Create());
        }

        #endregion
    }
}
