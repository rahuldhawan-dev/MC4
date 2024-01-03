using MapCallMVC.Models.ViewModels.OperatorLicenses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Models.ViewModels.OperatorLicenses
{
    [TestClass]
    public class CreateOperatorLicenseTest : OperatorLicenseViewModelTestBase<CreateOperatorLicense>
    {
        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();

            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Employee);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.State);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();

            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.Employee);
            _vmTester.CanMapBothWays(x => x.State);
        }
    }
}
