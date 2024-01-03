using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class CreateChemicalDeliveryTest : MapCallMvcInMemoryDatabaseTestBase<ChemicalDelivery>
    {
        #region Fields

        private ViewModelTester<CreateChemicalDelivery, ChemicalDelivery> _vmTester;
        private CreateChemicalDelivery _viewModel;
        private ChemicalDelivery _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<CreateChemicalDelivery>();
            _entity = new ChemicalDelivery();
            _vmTester = new ViewModelTester<CreateChemicalDelivery, ChemicalDelivery>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.DateOrdered);
            _vmTester.CanMapBothWays(x => x.ScheduledDeliveryDate);
            _vmTester.CanMapBothWays(x => x.ActualDeliveryDate);
            _vmTester.CanMapBothWays(x => x.ConfirmationInformation);
            _vmTester.CanMapBothWays(x => x.ReceiptNumberJde);
            _vmTester.CanMapBothWays(x => x.BatchNumberJde);
            _vmTester.CanMapBothWays(x => x.EstimatedDeliveryQuantityGallons);
            _vmTester.CanMapBothWays(x => x.ActualDeliveryQuantityGallons);
            _vmTester.CanMapBothWays(x => x.EstimatedDeliveryQuantityPounds);
            _vmTester.CanMapBothWays(x => x.ActualDeliveryQuantityPounds);
            _vmTester.CanMapBothWays(x => x.DeliveryTicketNumber);
            _vmTester.CanMapBothWays(x => x.DeliveryInstructions);
            _vmTester.CanMapBothWays(x => x.SplitFacilityDelivery);
            _vmTester.CanMapBothWays(x => x.SecurityInformation);
        }

        [TestMethod]
        public void TestRequiredProperties()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Storage);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Chemical);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Chemical, GetEntityFactory<Chemical>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Storage, GetEntityFactory<ChemicalStorage>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.UnitCost, GetEntityFactory<ChemicalUnitCost>().Create());
        }

        #endregion
    }

    [TestClass]
    public class EditChemicalDeliveryTest : MapCallMvcInMemoryDatabaseTestBase<ChemicalDelivery>
    {
        #region Fields

        private ViewModelTester<EditChemicalDelivery, ChemicalDelivery> _vmTester;
        private EditChemicalDelivery _viewModel;
        private ChemicalDelivery _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<EditChemicalDelivery>();
            _entity = new ChemicalDelivery();
            _vmTester = new ViewModelTester<EditChemicalDelivery, ChemicalDelivery>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.DateOrdered);
            _vmTester.CanMapBothWays(x => x.ScheduledDeliveryDate);
            _vmTester.CanMapBothWays(x => x.ActualDeliveryDate);
            _vmTester.CanMapBothWays(x => x.ConfirmationInformation);
            _vmTester.CanMapBothWays(x => x.ReceiptNumberJde);
            _vmTester.CanMapBothWays(x => x.BatchNumberJde);
            _vmTester.CanMapBothWays(x => x.EstimatedDeliveryQuantityGallons);
            _vmTester.CanMapBothWays(x => x.ActualDeliveryQuantityGallons);
            _vmTester.CanMapBothWays(x => x.EstimatedDeliveryQuantityPounds);
            _vmTester.CanMapBothWays(x => x.ActualDeliveryQuantityPounds);
            _vmTester.CanMapBothWays(x => x.DeliveryTicketNumber);
            _vmTester.CanMapBothWays(x => x.DeliveryInstructions);
            _vmTester.CanMapBothWays(x => x.SplitFacilityDelivery);
            _vmTester.CanMapBothWays(x => x.SecurityInformation);
        }

        [TestMethod]
        public void TestRequiredProperties()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Storage);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Chemical);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Chemical, GetEntityFactory<Chemical>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Storage, GetEntityFactory<ChemicalStorage>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.UnitCost, GetEntityFactory<ChemicalUnitCost>().Create());
        }

        #endregion
    }
}
