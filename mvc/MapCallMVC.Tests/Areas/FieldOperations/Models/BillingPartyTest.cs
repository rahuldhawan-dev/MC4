using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class CreateBillingPartyTest : MapCallMvcInMemoryDatabaseTestBase<BillingParty>
    {
        #region Fields

        private ViewModelTester<CreateBillingParty, BillingParty> _vmTester;
        private CreateBillingParty _viewModel;
        private BillingParty _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateBillingParty(_container);
            _entity = new BillingParty();
            _vmTester = new ViewModelTester<CreateBillingParty, BillingParty>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.EstimatedHourlyRate);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
        }

        #endregion
    }

    [TestClass]
    public class EditBillingPartyTest : InMemoryDatabaseTest<BillingParty>
    {
        #region Fields

        private ViewModelTester<EditBillingParty, BillingParty> _vmTester;
        private EditBillingParty _viewModel;
        private BillingParty _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditBillingParty(_container);
            _entity = new BillingParty();
            _vmTester = new ViewModelTester<EditBillingParty, BillingParty>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.EstimatedHourlyRate);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
        }

        #endregion
    }
}
