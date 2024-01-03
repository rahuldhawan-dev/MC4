using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class RemoveEnvironmentalPermitFeeTest : MapCallMvcInMemoryDatabaseTestBase<EnvironmentalPermit>
    {
        #region Fields

        private ViewModelTester<RemoveEnvironmentalPermitFee, EnvironmentalPermit> _vmTester;
        private RemoveEnvironmentalPermitFee _viewModel;
        private EnvironmentalPermit _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<RemoveEnvironmentalPermitFee>();
            _entity = new EnvironmentalPermit();
            _vmTester = new ViewModelTester<RemoveEnvironmentalPermitFee, EnvironmentalPermit>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestMapToEntityRemovesFeeFromPermit()
        {
            var fee = GetEntityFactory<EnvironmentalPermitFee>().Create();
            _entity.Fees.Add(fee);
            _viewModel.EnvironmentalPermitFeeId = fee.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_entity.Fees.Contains(fee));
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EnvironmentalPermitFeeId);
        }

        [TestMethod]
        public void TestValidationFailsIfFeeDoesNotExistForPermit()
        {
            var fee = GetEntityFactory<EnvironmentalPermitFee>().Create();
            _viewModel.Id = fee.EnvironmentalPermit.Id;
            _viewModel.EnvironmentalPermitFeeId = fee.Id;
            fee.EnvironmentalPermit.Fees.Remove(fee);

            ValidationAssert.ModelStateHasError(_viewModel, x => x.EnvironmentalPermitFeeId, $"Fee#{fee.Id} does not exist for permit#{_viewModel.Id}.");

            fee.EnvironmentalPermit.Fees.Add(fee);
            ValidationAssert.ModelStateIsValid(_viewModel);
        }


        #endregion

        #endregion
    }

}
