using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    public class EndOfPipeExceedanceViewModelTest<TViewModel> : ViewModelTestBase<EndOfPipeExceedance, TViewModel> where TViewModel : EndOfPipeExceedanceViewModel
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity.ConsentOrder = false;
            _entity.NewAcquisition = false;
        }

        #endregion

        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.State, GetEntityFactory<State>().Create());
            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            _vmTester.CanMapBothWays(x => x.WasteWaterSystem, GetEntityFactory<WasteWaterSystem>().Create());
            _vmTester.CanMapBothWays(x => x.Facility, GetEntityFactory<Facility>().Create());
            _vmTester.CanMapBothWays(x => x.EventDate);
            _vmTester.CanMapBothWays(x => x.EndOfPipeExceedanceType, GetEntityFactory<EndOfPipeExceedanceType>().Create());
            _vmTester.CanMapBothWays(x => x.LimitationType, GetEntityFactory<LimitationType>().Create());
            _vmTester.CanMapBothWays(x => x.EndOfPipeExceedanceTypeOtherReason);
            _vmTester.CanMapBothWays(x => x.EndOfPipeExceedanceRootCause, GetEntityFactory<EndOfPipeExceedanceRootCause>().Create());
            _vmTester.CanMapBothWays(x => x.EndOfPipeExceedanceRootCauseOtherReason);
            _vmTester.CanMapBothWays(x => x.ConsentOrder);
            _vmTester.CanMapBothWays(x => x.NewAcquisition);
            _vmTester.CanMapBothWays(x => x.BriefDescription);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.State);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WasteWaterSystem);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EventDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EndOfPipeExceedanceType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LimitationType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EndOfPipeExceedanceRootCause);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ConsentOrder);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.NewAcquisition);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.BriefDescription);

            //Required When
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.EndOfPipeExceedanceTypeOtherReason, "I Tried To Make Ramen In The Coffee Pot And I Broke Everything", x => x.EndOfPipeExceedanceType, EndOfPipeExceedanceType.Indices.OTHER, 2);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.EndOfPipeExceedanceRootCauseOtherReason, "I Tried To Make Ramen In The Coffee Pot And I Broke Everything", x => x.EndOfPipeExceedanceRootCause, EndOfPipeExceedanceRootCause.Indices.OTHER, 2);
        }

        [TestMethod]
        public void TestNotRequiredFields()
        {
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.Facility);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.EndOfPipeExceedanceTypeOtherReason, EndOfPipeExceedance.StringLengths.EndOfPipeExceedanceTypeOtherReason);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.EndOfPipeExceedanceRootCauseOtherReason, EndOfPipeExceedance.StringLengths.EndOfPipeExceedanceRootCauseOtherReason);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.State, GetEntityFactory<State>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.WasteWaterSystem, GetEntityFactory<WasteWaterSystem>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Facility, GetEntityFactory<Facility>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.EndOfPipeExceedanceType, GetEntityFactory<EndOfPipeExceedanceType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.LimitationType, GetEntityFactory<LimitationType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.EndOfPipeExceedanceRootCause, GetEntityFactory<EndOfPipeExceedanceRootCause>().Create());
        }

        #endregion
    }

    [TestClass]
    public class CreateEndOfPipeExceedanceTest : EndOfPipeExceedanceViewModelTest<CreateEndOfPipeExceedance> { }

    [TestClass]
    public class EditEndOfPipeExceedanceTest : EndOfPipeExceedanceViewModelTest<EditEndOfPipeExceedance> { }
}

