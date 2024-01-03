using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Models.ViewModels
{
    [TestClass]
    public class CreateConfinedSpaceFormAtmosphericTestTest : ViewModelTestBase<ConfinedSpaceFormAtmosphericTest, CreateConfinedSpaceFormAtmosphericTest>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        private Employee _currentUserEmployee;
        private User _currentUser;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _authServ = new Mock<IAuthenticationService<User>>();
            _container.Inject(_authServ.Object);
            _currentUserEmployee = GetEntityFactory<Employee>().Create();
            _currentUser = GetEntityFactory<User>().Create(new{ Employee = _currentUserEmployee });
            _authServ.Setup(x => x.CurrentUser).Returns(_currentUser);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.CarbonMonoxidePartsPerMillionBottom);
            _vmTester.CanMapBothWays(x => x.CarbonMonoxidePartsPerMillionMiddle);
            _vmTester.CanMapBothWays(x => x.CarbonMonoxidePartsPerMillionTop);
            _vmTester.CanMapBothWays(x => x.HydrogenSulfidePartsPerMillionBottom);
            _vmTester.CanMapBothWays(x => x.HydrogenSulfidePartsPerMillionMiddle);
            _vmTester.CanMapBothWays(x => x.HydrogenSulfidePartsPerMillionTop);
            _vmTester.CanMapBothWays(x => x.LowerExplosiveLimitPercentageBottom);
            _vmTester.CanMapBothWays(x => x.LowerExplosiveLimitPercentageMiddle);
            _vmTester.CanMapBothWays(x => x.LowerExplosiveLimitPercentageTop);
            _vmTester.CanMapBothWays(x => x.OxygenPercentageBottom);
            _vmTester.CanMapBothWays(x => x.OxygenPercentageMiddle);
            _vmTester.CanMapBothWays(x => x.OxygenPercentageTop);
            _vmTester.CanMapBothWays(x => x.TestedAt);
            _vmTester.CanMapBothWays(x => x.ConfinedSpaceFormReadingCaptureTime);
        }

        [TestMethod]
        public void TestMapToEntitySetsTestedByToCurrentUserEmployee()
        {
            // Mock setup is already done in test init since it's needed for other tests.
            _entity.TestedBy = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_currentUserEmployee, _entity.TestedBy);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CarbonMonoxidePartsPerMillionBottom);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CarbonMonoxidePartsPerMillionMiddle);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CarbonMonoxidePartsPerMillionTop);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HydrogenSulfidePartsPerMillionBottom);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HydrogenSulfidePartsPerMillionMiddle);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HydrogenSulfidePartsPerMillionTop);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LowerExplosiveLimitPercentageBottom);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LowerExplosiveLimitPercentageMiddle);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LowerExplosiveLimitPercentageTop);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OxygenPercentageBottom);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OxygenPercentageMiddle);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OxygenPercentageTop);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TestedAt);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ConfinedSpaceFormReadingCaptureTime);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no string length props
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // no entity must exist props
        }

        [TestMethod]
        public void TestMinValidationOnAllResultFields()
        {
            const decimal THESE_ALL_ARE_ZERO = decimal.Zero;
            const int THESE_ARE_ALSO_ALL_ZERO = 0;
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.CarbonMonoxidePartsPerMillionBottom, THESE_ARE_ALSO_ALL_ZERO);
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.CarbonMonoxidePartsPerMillionMiddle, THESE_ARE_ALSO_ALL_ZERO);
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.CarbonMonoxidePartsPerMillionTop, THESE_ARE_ALSO_ALL_ZERO);
            
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.HydrogenSulfidePartsPerMillionBottom, THESE_ARE_ALSO_ALL_ZERO);
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.HydrogenSulfidePartsPerMillionMiddle, THESE_ARE_ALSO_ALL_ZERO);
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.HydrogenSulfidePartsPerMillionTop, THESE_ARE_ALSO_ALL_ZERO);

            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.LowerExplosiveLimitPercentageBottom, THESE_ALL_ARE_ZERO);
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.LowerExplosiveLimitPercentageMiddle, THESE_ALL_ARE_ZERO);
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.LowerExplosiveLimitPercentageTop, THESE_ALL_ARE_ZERO);
            
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.OxygenPercentageBottom, THESE_ALL_ARE_ZERO);
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.OxygenPercentageMiddle, THESE_ALL_ARE_ZERO);
            ValidationAssert.PropertyHasMinValueRequirement(_viewModel, x => x.OxygenPercentageTop, THESE_ALL_ARE_ZERO);
        }

        [TestMethod]
        public void TestAcknowledgedValuesAreOutOfRangeIsRequiredWhenAnyOfTheReadingFieldsAreOutsideOfAcceptableRanges()
        {
            // This is an annoying test due to the billion RequiredWhens. The parent values need to be
            // reset at the end of each test because they otherwise cause the next test to fail.
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.CarbonMonoxidePartsPerMillionBottom, 100, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.CO_MAX, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.CarbonMonoxidePartsPerMillionBottom = null;
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.CarbonMonoxidePartsPerMillionMiddle, 100, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.CO_MAX, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.CarbonMonoxidePartsPerMillionMiddle = null;
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.CarbonMonoxidePartsPerMillionTop, 100, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.CO_MAX, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.CarbonMonoxidePartsPerMillionTop = null;

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.HydrogenSulfidePartsPerMillionBottom, 100, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.H2S_MAX, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.HydrogenSulfidePartsPerMillionBottom = null;
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.HydrogenSulfidePartsPerMillionMiddle, 100, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.H2S_MAX, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.HydrogenSulfidePartsPerMillionMiddle = null;
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.HydrogenSulfidePartsPerMillionTop, 100, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.H2S_MAX, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.HydrogenSulfidePartsPerMillionTop = null;

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.LowerExplosiveLimitPercentageBottom, 100m, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.LEL_MAX, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.LowerExplosiveLimitPercentageBottom = null;
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.LowerExplosiveLimitPercentageMiddle, 100m, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.LEL_MAX, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.LowerExplosiveLimitPercentageMiddle = null;
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.LowerExplosiveLimitPercentageTop, 100m, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.LEL_MAX, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.LowerExplosiveLimitPercentageTop = null;

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.OxygenPercentageBottom, 100m, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.OxygenPercentageBottom = null;
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.OxygenPercentageBottom, 0m, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MIN, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.OxygenPercentageBottom = null;
            
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.OxygenPercentageMiddle, 100m, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.OxygenPercentageMiddle = null;
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.OxygenPercentageMiddle, 0m, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MIN, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.OxygenPercentageMiddle = null;
            
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.OxygenPercentageTop, 100m, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MAX, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.OxygenPercentageTop = null;
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AcknowledgedValuesAreOutOfRange, true, x => x.OxygenPercentageTop, 0m, ConfinedSpaceFormAtmosphericTest.AcceptableConcentrations.OXYGEN_MIN, expectedRequiredErrorMessage: CreateConfinedSpaceFormAtmosphericTest.ERROR_CONFIRM_READINGS);
            _viewModel.OxygenPercentageTop = null;
        }

        #endregion

        #endregion
    }
}
