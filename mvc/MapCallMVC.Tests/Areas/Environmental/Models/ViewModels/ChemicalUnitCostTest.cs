using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class CreateChemicalUnitCostTest : MapCallMvcInMemoryDatabaseTestBase<ChemicalUnitCost>
    {
        #region Fields

        private ViewModelTester<CreateChemicalUnitCost, ChemicalUnitCost> _vmTester;
        private CreateChemicalUnitCost _viewModel;
        private ChemicalUnitCost _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<CreateChemicalUnitCost>();
            _entity = new ChemicalUnitCost();
            _vmTester = new ViewModelTester<CreateChemicalUnitCost, ChemicalUnitCost>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.StartDate);
            _vmTester.CanMapBothWays(x => x.EndDate);
            _vmTester.CanMapBothWays(x => x.PricePerPoundWet);
            _vmTester.CanMapBothWays(x => x.PoNumber);
            _vmTester.CanMapBothWays(x => x.ChemicalLeadTimeDays);
            _vmTester.CanMapBothWays(x => x.ChemicalOrderingProcess);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Chemical);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Chemical, GetEntityFactory<Chemical>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.WarehouseNumber, GetEntityFactory<ChemicalWarehouseNumber>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Vendor, GetEntityFactory<ChemicalVendor>().Create());
        }

        #endregion
    }

    [TestClass]
    public class EditChemicalUnitCostTest : MapCallMvcInMemoryDatabaseTestBase<ChemicalUnitCost>
    {
        #region Fields

        private ViewModelTester<EditChemicalUnitCost, ChemicalUnitCost> _vmTester;
        private EditChemicalUnitCost _viewModel;
        private ChemicalUnitCost _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<EditChemicalUnitCost>();
            _entity = new ChemicalUnitCost();
            _vmTester = new ViewModelTester<EditChemicalUnitCost, ChemicalUnitCost>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.StartDate);
            _vmTester.CanMapBothWays(x => x.EndDate);
            _vmTester.CanMapBothWays(x => x.PricePerPoundWet);
            _vmTester.CanMapBothWays(x => x.PoNumber);
            _vmTester.CanMapBothWays(x => x.ChemicalLeadTimeDays);
            _vmTester.CanMapBothWays(x => x.ChemicalOrderingProcess);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Chemical);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Chemical, GetEntityFactory<Chemical>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.WarehouseNumber, GetEntityFactory<ChemicalWarehouseNumber>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Vendor, GetEntityFactory<ChemicalVendor>().Create());
        }
        #endregion
    }
}
