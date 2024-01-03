using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Contractors.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Contractors.Models
{
    [TestClass]
    public class CreateContractorUserTest : MapCallMvcInMemoryDatabaseTestBase<ContractorUser, ContractorUserRepository>
    {
        #region Fields

        private ViewModelTester<CreateContractorUser, ContractorUser> _vmTester;
        private CreateContractorUser _viewModel;
        private ContractorUser _entity;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IContractorUserRepository>().Use(_ => Repository);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<ContractorUser>().Create();
            _viewModel = _viewModelFactory.Build<CreateContractorUser, ContractorUser>( _entity);
            // These values need to be set on the ViewModel for all the MapToEntiy tests to work. Null refs otherwise.
            _viewModel.Email = "some@address.com";
            _viewModel.Password = "Some pass";
            _viewModel.PasswordAnswer = "Some answer";
            _vmTester = new ViewModelTester<CreateContractorUser, ContractorUser>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMappingCreatesNewPasswordSalt()
        {
            _vmTester.MapToEntity();
            var firstSalt = _entity.PasswordSalt;
            _vmTester.MapToEntity();
            var secondSalt = _entity.PasswordSalt;
            Assert.AreNotEqual(firstSalt, secondSalt);
        }

        [TestMethod]
        public void TestMappingSaltsPassword()
        {
            _viewModel.Password = "this is my pass";
            _vmTester.MapToEntity();
            var expected = _viewModel.Password.Salt(_entity.PasswordSalt);
            Assert.AreEqual(expected, _entity.Password);
        }

        [TestMethod]
        public void TestMappingSaltsPasswordAnswer()
        {
            _viewModel.PasswordAnswer = "this is my answer";
            _vmTester.MapToEntity();
            var expected = _viewModel.PasswordAnswer.Salt(_entity.PasswordSalt);
            Assert.AreEqual(expected, _entity.PasswordAnswer);
        }

        [TestMethod]
        public void TestMappingMapsContractor()
        {
            var contractor = GetFactory<ContractorFactory>().Create();
            _viewModel.Contractor = contractor.Id;
            _vmTester.MapToEntity();
            Assert.AreSame(contractor, _entity.Contractor);
        }

        [TestMethod]
        public void TestMapToEntitySanitizesEmailAddress()
        {
            _viewModel.Email = "   some@email.com ";
            _vmTester.MapToEntity();
            Assert.AreEqual("some@email.com", _entity.Email);

            _viewModel.Email = "SOME@ALLCAPSSCREAMING.COM";
            _vmTester.MapToEntity();
            Assert.AreEqual("some@allcapsscreaming.com", _entity.Email);
        }

        [TestMethod]
        public void TestSimpleMappedFields()
        {
            // IsAdmin, ISActive, Email 
            _vmTester.CanMapBothWays(x => x.IsActive);
            _vmTester.CanMapBothWays(x => x.IsAdmin);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ConfirmPassword);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Contractor);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Email);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsActive);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsAdmin);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Password);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PasswordAnswer);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PasswordQuestion);
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

        #endregion
    }
}
