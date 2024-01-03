using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Facilities.Models.ViewModels.PlanReviews;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Facilities.Models.ViewModels.PlanReviews
{
    [TestClass]
    public class NewPlanReviewTest : ViewModelTestBase<PlanReview, NewPlanReview>
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Plan, GetEntityFactory<EmergencyResponsePlan>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Plan);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Plan, GetEntityFactory<EmergencyResponsePlan>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation() { }

        #endregion
    }
}
