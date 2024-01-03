using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MapCall.Common.Testing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Facilities.Models.ViewModels.PlanReviews;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Facilities.Models.ViewModels.PlanReviews
{
    [TestClass]
    public class PlanReviewViewModelTest<TViewModel> : ViewModelTestBase<PlanReview, TViewModel> where TViewModel : PlanReviewViewModel
    {
        #region Fields

        protected User _user;
        protected DateTime _now;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        protected Mock<IAuthenticationService<User>> _authService;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            _authService = e.For<IAuthenticationService<User>>().Mock();
        }

        [TestInitialize]
        public void PlanReviewViewModelTestInitialize()
        {
            _now = DateTime.Now;
            _user = GetEntityFactory<User>().Create(new {
                Employee = GetEntityFactory<Employee>().Create()
            });

            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);
            _authService.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ReviewDate);
            _vmTester.CanMapBothWays(x => x.NextReviewDate);
            _vmTester.CanMapBothWays(x => x.ReviewChangeNotes);
            _vmTester.CanMapBothWays(x => x.Plan, GetEntityFactory<EmergencyResponsePlan>().Create());
            _vmTester.CanMapBothWays(x => x.ReviewedBy, GetEntityFactory<Employee>().Create());
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ReviewedBy);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Plan);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ReviewDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.NextReviewDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ReviewChangeNotes);
        }

        [TestMethod]
        public override void TestStringLengthValidation() { }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.ReviewedBy, GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Plan, GetEntityFactory<EmergencyResponsePlan>().Create());
        }

        #endregion

        #endregion
    }
}

