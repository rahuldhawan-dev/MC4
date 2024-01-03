using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class RedTagPermitAuthorizationViewModelTestBase : ViewModelTestBase<ProductionWorkOrder, RedTagPermitAuthorizationViewModel>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authService;
        public User _user;
        public Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _authService = new Mock<IAuthenticationService<User>>();
            _user = GetEntityFactory<User>().Create();
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authService.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authService.Object);
            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.NeedsRedTagPermitAuthorization);
        }

        [TestMethod]
        public void TestMapSetsAuditProperties()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);

            _viewModel.NeedsRedTagPermitAuthorization = true;
            _vmTester.MapToEntity();

            Assert.IsNotNull(_entity.NeedsRedTagPermitAuthorizedOn);
            Assert.IsNotNull(_entity.NeedsRedTagPermitAuthorizedBy);

            Assert.AreEqual(expectedDate, _entity.NeedsRedTagPermitAuthorizedOn.Value);
            Assert.AreEqual(_user.Employee, _entity.NeedsRedTagPermitAuthorizedBy);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.NeedsRedTagPermitAuthorizedBy, _user.Employee);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.NeedsRedTagPermitAuthorization);
        }

        [TestMethod]
        public void TestValidationFailsIfUserDoesNotHaveAssociatedEmployee()
        {
            _user.Employee = null;
            _viewModel.NeedsRedTagPermitAuthorization = true;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.NeedsRedTagPermitAuthorization, RedTagPermitAuthorizationViewModel.USER_MUST_HAVE_EMPLOYEE_RECORD);

            _user.Employee = GetEntityFactory<Employee>().Create();

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.NeedsRedTagPermitAuthorization);
        }

        #endregion

        #endregion
    }
}
