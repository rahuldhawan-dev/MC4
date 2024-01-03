using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Facilities.Models.ViewModels
{
    [TestClass]
    public abstract class EmergencyResponsePlanTest<TViewModel> : ViewModelTestBase<EmergencyResponsePlan, TViewModel> where TViewModel : EmergencyResponsePlanViewModel
    {
        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.State, GetEntityFactory<State>().Create());
            _vmTester.CanMapBothWays(x => x.Title);
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetFactory<UniqueOperatingCenterFactory>().Create());
            _vmTester.CanMapBothWays(x => x.Facility, GetEntityFactory<Facility>().Create());
            _vmTester.CanMapBothWays(x => x.EmergencyPlanCategory, GetEntityFactory<EmergencyPlanCategory>().Create());
            _vmTester.CanMapBothWays(x => x.ReviewFrequency, GetEntityFactory<ReviewFrequency>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.State);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EmergencyPlanCategory);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Title);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
        }

        [TestMethod]
        public void TestNotRequiredFields()
        {
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.Facility);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.ReviewFrequency);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Title, EmergencyResponsePlan.StringLengths.TITLE);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation() { }

        #endregion
    }

    [TestClass]
    public class EditEmergencyResponsePlanTest : EmergencyResponsePlanTest<EditEmergencyResponsePlan> { }

    [TestClass]
    public class CreateEmergencyResponsePlan1Test : EmergencyResponsePlanTest<CreateEmergencyResponsePlan> { }
}

