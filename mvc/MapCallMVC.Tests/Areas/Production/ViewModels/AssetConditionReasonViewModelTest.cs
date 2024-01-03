using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    public abstract class AssetConditionReasonViewModelTestBase<TViewModel> : ViewModelTestBase<AssetConditionReason, TViewModel> where TViewModel : AssetConditionReasonViewModel
    {
        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Code);
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.ConditionDescription, GetEntityFactory<ConditionDescription>().Create());
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.ConditionDescription, GetEntityFactory<ConditionDescription>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Code);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ConditionDescription);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Code, AssetConditionReason.StringLengths.CODE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Description, ReadOnlyEntityLookup.StringLengths.DESCRIPTION);
        }

        #endregion

        #endregion
    }

    [TestClass]
    public class CreateAssetConditionReasonTest : AssetConditionReasonViewModelTestBase<CreateAssetConditionReason> { }

    [TestClass]
    public class EditAssetConditionReasonTest : AssetConditionReasonViewModelTestBase<EditAssetConditionReason> { }
}
