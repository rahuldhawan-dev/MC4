using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Models.ViewModels
{
    [TestClass]
    public class ConfinedSpaceFormHazardViewModelTest : ViewModelTestBase<ConfinedSpaceFormHazard, ConfinedSpaceFormHazardViewModel>
    {
        #region Fields

        private ConfinedSpaceFormHazardType _hazardType1, _hazardType2;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {

            _hazardType1 = GetEntityFactory<ConfinedSpaceFormHazardType>().Create(new { Description = "Hazard One"});
            _hazardType2 = GetEntityFactory<ConfinedSpaceFormHazardType>().Create(new { Description = "Hazard Two" });
            _viewModel.SetDefaults();
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // NOTE: Mapping is strange for this view model because it's used as a child collection for
            // ConfinedSpaceFormViewModel. The mapping to the view model is handled manually by the parent
            // and is tested in the parent's test class. The mapping to the entity is also done by the parent
            // manually calling MapToEntity on this view model.
            _vmTester.CanMapBothWays(x => x.HazardType, GetEntityFactory<ConfinedSpaceFormHazardType>().Create());
            _vmTester.CanMapBothWays(x => x.Notes);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.HazardType);
        }

        [TestMethod]
        public void TestRequiredWhenValidation()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.Notes, "Neat", x => x.IsChecked, true);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Notes, ConfinedSpaceFormHazard.StringLengths.NOTES);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.HazardType, GetEntityFactory<ConfinedSpaceFormHazardType>().Create());
        }

        #endregion

        #endregion
    }
}
