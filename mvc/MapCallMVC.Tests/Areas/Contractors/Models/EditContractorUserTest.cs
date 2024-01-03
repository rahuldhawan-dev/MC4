using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Contractors.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Contractors.Models
{
    [TestClass]
    public class EditContractorUserTest : MapCallMvcInMemoryDatabaseTestBase<ContractorUser, ContractorUserRepository>
    {
        #region Fields

        private ViewModelTester<EditContractorUser, ContractorUser> _vmTester;
        private EditContractorUser _viewModel;
        private ContractorUser _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<ContractorUser>().Create();
            _viewModel = _viewModelFactory.Build<EditContractorUser, ContractorUser>( _entity);
            // These values need to be set on the ViewModel for all the MapToEntiy tests to work. Null refs otherwise.
            _viewModel.Email = "some@address.com";
            _viewModel.Password = "Some pass";
            _viewModel.PasswordAnswer = "Some answer";
            _vmTester = new ViewModelTester<EditContractorUser, ContractorUser>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntityDoesNotResetPasswordSalt()
        {
            var expected = _entity.PasswordSalt;
            Assert.AreNotEqual(Guid.Empty, expected, "Sanity");
            _vmTester.MapToEntity();
            Assert.AreEqual(expected, _entity.PasswordSalt);
        }

        [TestMethod]
        public void TestMapToEntityOnlySetsPasswordWhenNewPasswordIsSetOnViewmodel()
        {
            var expected = _entity.Password;
            _viewModel.Password = null;
            _vmTester.MapToEntity();

            Assert.AreEqual(expected, _entity.Password);

            _viewModel.Password = "Some new password";
            _vmTester.MapToEntity();
            Assert.AreNotEqual(expected, _entity.Password);
        }

        [TestMethod]
        public void TestMapToEntityOnlySetsPasswordAnswerWhenNewPasswordAnswerIsSetOnViewmodel()
        {
            var expected = _entity.PasswordAnswer;
            _viewModel.PasswordAnswer = null;
            _vmTester.MapToEntity();

            Assert.AreEqual(expected, _entity.PasswordAnswer);

            _viewModel.PasswordAnswer = "Some new password answer";
            _vmTester.MapToEntity();
            Assert.AreNotEqual(expected, _entity.PasswordAnswer);
        }

        [TestMethod]
        public void TestMapToEntitySetsFailedLoginAttemptCountToZeroIfIsActiveIsTrue()
        {
            _entity.IsActive = false;
            _entity.FailedLoginAttemptCount = 123;
            _viewModel.IsActive = false;

            _vmTester.MapToEntity();
            Assert.IsFalse(_entity.IsActive);
            Assert.AreEqual(123, _entity.FailedLoginAttemptCount);

            _viewModel.IsActive = true;
            _vmTester.MapToEntity();
            Assert.IsTrue(_entity.IsActive);
            Assert.AreEqual(0, _entity.FailedLoginAttemptCount);
        }

        [TestMethod]
        public void TestSimpleMapping()
        {
            //IsActive, IsAdmin
            _vmTester.CanMapBothWays(x => x.IsActive);
            _vmTester.CanMapBothWays(x => x.IsAdmin);
        }

        [TestMethod]
        public void TestMapToEntityMapsContractor()
        {
            var contractor = GetFactory<ContractorFactory>().Create();
            _viewModel.Contractor = contractor.Id;
            _vmTester.MapToEntity();
            Assert.AreSame(contractor, _entity.Contractor);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Contractor);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsActive);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsAdmin);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PasswordQuestion);
        }

        [TestMethod]
        public void TestPasswordMustMeetContractorUserValidationRequirements()
        {
            _viewModel.Password = "yes";
            _viewModel.ConfirmPassword = _viewModel.Password;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.Password, "Password does not meet security requirements.");

            _viewModel.Password = "yesYES123";
            _viewModel.ConfirmPassword = _viewModel.Password;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Password);
        }

        [TestMethod]
        public void TestPasswordAndConfirmPasswordMustBeEqual()
        {
            _viewModel.Password = "yesYES123";
            _viewModel.ConfirmPassword = "noNO1234";
            ValidationAssert.ModelStateHasError(_viewModel, x => x.Password, "'Password' and 'ConfirmPassword' do not match.");
            ValidationAssert.ModelStateHasError(_viewModel, x => x.ConfirmPassword, "'ConfirmPassword' and 'Password' do not match.");

            _viewModel.ConfirmPassword = _viewModel.Password;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Password);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ConfirmPassword);
        }

        [TestMethod]
        public void TestPasswordIsOnlyRequiredWhenConfirmPasswordIsNotNull()
        {
            _viewModel.Password = null;
            _viewModel.ConfirmPassword = null;

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Password);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ConfirmPassword);

            _viewModel.ConfirmPassword = "fsafas12#";
            ValidationAssert.ModelStateHasError(_viewModel, x => x.Password, "The Password field is required.");

            _viewModel.Password = "fsafas12#";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Password);
        }

        [TestMethod]
        public void TestConfirmPasswordIsOnlyRequiredWhenPasswordIsNotNull()
        {
            _viewModel.Password = null;
            _viewModel.ConfirmPassword = null;

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Password);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ConfirmPassword);

            _viewModel.Password = "fsafas";
            ValidationAssert.ModelStateHasError(_viewModel, x => x.ConfirmPassword, "The ConfirmPassword field is required.");

            _viewModel.ConfirmPassword = "fsafas";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ConfirmPassword);
        }

        #endregion
    }
}
