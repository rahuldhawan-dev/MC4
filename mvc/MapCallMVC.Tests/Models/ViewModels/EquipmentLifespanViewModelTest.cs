using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class EquipmentLifespanViewModelTest : ViewModelTestBase<EquipmentLifespan, EquipmentLifespanViewModel>
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ExtendedLifeMajor);
            _vmTester.CanMapBothWays(x => x.ExtendedLifeMinor);
            _vmTester.CanMapBothWays(x => x.EstimatedLifespan);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsNotRequired(x => x.ExtendedLifeMajor);
            ValidationAssert.PropertyIsNotRequired(x => x.ExtendedLifeMinor);
            ValidationAssert.PropertyIsNotRequired(x => x.EstimatedLifespan);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // N/A
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // N/A
        }

        #endregion
    }
}
