using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.Common.Model.Entities;
using MapCallMVC.Models.ViewModels.Streets;

namespace MapCallMVC.Tests.Models.ViewModels.Streets
{
    [TestClass]
    public abstract class BaseStreetViewModelTest<TViewModel> : ViewModelTestBase<Street, TViewModel>
        where TViewModel : StreetViewModel
    {
        #region Tests

        #region Mapping
        
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.IsActive);
            _vmTester.CanMapBothWays(x => x.Name);
            _vmTester.CanMapBothWays(x => x.Prefix);
            _vmTester.CanMapBothWays(x => x.Suffix);
        }
        
        [TestMethod]
        public void TestMapToEntitySetsFullStNameFromPrefixNameAndSuffix()
        {
            var prefix = GetEntityFactory<StreetPrefix>().Create();
            var suffix = GetEntityFactory<StreetSuffix>().Create();
            _viewModel.Suffix = suffix.Id;
            _viewModel.Prefix = prefix.Id;
            _viewModel.Name = "Foo";

            _vmTester.MapToEntity();

            Assert.AreEqual($"{prefix.Description} Foo {suffix.Description}", _entity.FullStName);
        }

        [TestMethod]
        public void TestMapToEntityTrimsTheFullStreetNameWhenItIsMissingPrefixOrSuffix()
        {
            _viewModel.Suffix = null;
            _viewModel.Prefix = null;
            _viewModel.Name = "Something";

            _vmTester.MapToEntity();
            Assert.AreEqual("Something", _entity.FullStName);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.Name);
        }
        
        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.Prefix, GetEntityFactory<StreetPrefix>().Create());
            ValidationAssert.EntityMustExist(x => x.Suffix, GetEntityFactory<StreetSuffix>().Create());
        }
        
        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.Name, Street.StringLengths.NAME);
        }
  
        #endregion

        #endregion
    }
}
