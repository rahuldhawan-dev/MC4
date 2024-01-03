using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class CreateChemicalInventoryTransactionTest : MapCallMvcInMemoryDatabaseTestBase<ChemicalInventoryTransaction>
    {
        #region Fields

        private ViewModelTester<CreateChemicalInventoryTransaction, ChemicalInventoryTransaction> _vmTester;
        private CreateChemicalInventoryTransaction _viewModel;
        private ChemicalInventoryTransaction _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<CreateChemicalInventoryTransaction>();
            _entity = new ChemicalInventoryTransaction();
            _vmTester = new ViewModelTester<CreateChemicalInventoryTransaction, ChemicalInventoryTransaction>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Date);
            _vmTester.CanMapBothWays(x => x.QuantityGallons);
            _vmTester.CanMapBothWays(x => x.QuantityPounds);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Storage);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Delivery, GetEntityFactory<ChemicalDelivery>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Storage, GetEntityFactory<ChemicalStorage>().Create());
        }

        #endregion
    }

    [TestClass]
    public class EditChemicalInventoryTransactionTest : MapCallMvcInMemoryDatabaseTestBase<ChemicalInventoryTransaction>
    {
        #region Fields

        private ViewModelTester<EditChemicalInventoryTransaction, ChemicalInventoryTransaction> _vmTester;
        private EditChemicalInventoryTransaction _viewModel;
        private ChemicalInventoryTransaction _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<EditChemicalInventoryTransaction>();
            _entity = new ChemicalInventoryTransaction();
            _vmTester = new ViewModelTester<EditChemicalInventoryTransaction, ChemicalInventoryTransaction>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Date);
            _vmTester.CanMapBothWays(x => x.QuantityGallons);
            _vmTester.CanMapBothWays(x => x.QuantityPounds);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Storage);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Delivery, GetEntityFactory<ChemicalDelivery>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Storage, GetEntityFactory<ChemicalStorage>().Create());
        }

        #endregion
    }
}
