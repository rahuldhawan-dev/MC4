using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels.OperatorLicenses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Models.ViewModels.OperatorLicenses
{
    public abstract class OperatorLicenseViewModelTestBase<TViewModel> : 
        ViewModelTestBase<OperatorLicense, TViewModel> where TViewModel : OperatorLicenseViewModel
    {
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.OperatorLicenseType, GetEntityFactory<OperatorLicenseType>().Create());
            _vmTester.CanMapBothWays(x => x.ExpirationDate);
            _vmTester.CanMapBothWays(x => x.ValidationDate);
            _vmTester.CanMapBothWays(x => x.LicenseLevel);
            _vmTester.CanMapBothWays(x => x.LicenseSubLevel);
            _vmTester.CanMapBothWays(x => x.LicenseNumber);
            _vmTester.CanMapBothWays(x => x.LicensedOperatorOfRecord);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatorLicenseType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LicenseLevel);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LicenseNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ValidationDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ExpirationDate);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LicenseLevel, OperatorLicense.StringLengths.LICENSE_LEVEL);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LicenseSubLevel, OperatorLicense.StringLengths.LICENSE_SUB_LEVEL);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LicenseNumber, OperatorLicense.StringLengths.LICENSE_NUMBER);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatorLicenseType, GetEntityFactory<OperatorLicenseType>().Create());
        }
    }
}
