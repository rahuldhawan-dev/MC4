
using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class AuthenticationRepositoryTest : ContractorsControllerTestBase<ContractorUser>
    {
        #region Private Members

        private IAuthenticationRepository<ContractorUser> _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void AuthenticationRepositoryTestInitialize()
        {
            _target = _container.GetInstance<AuthenticationRepository>();
        }

        #endregion

        #region UserExists

        [TestMethod]
        public void TestUserExistsThrowsContractorUserExceptionForInvalidUser()
        {
            ContractorUserRepositoryTest.BAD_STRING_VALUES.Each(s => MyAssert.Throws<AuthenticationException>(() => _target.UserExists(s)));
        }

        [TestMethod]
        public void TestUserExistsReturnsTrueIfValidUserExists()
        {
            var expectedEmail = "burt@reynolds.com";
            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail});
         
            Assert.IsTrue(_target.UserExists(expectedEmail));
        }

        [TestMethod]
        public void TestUserExistsReturnsFalseIfValidUserDoesNotExist()
        {
            Assert.IsFalse(_target.UserExists("some@guy.com"));
        }

        #endregion

        #region ValidateUser

        [TestMethod]
        public void TestValidateUserReturnsInvalidEmailForBadEmail()
        {
            Assert.AreEqual(UserLoginAttemptStatus.InvalidEmail, _target.ValidateUser(null, null));
        }

        [TestMethod]
        public void TestValidateUserReturnsUnknownUserIfUserDoesNotExist()
        {
            // Add some whitespace
            var result = _target.ValidateUser("craig@t.nelson.com", "egfagea");

            Assert.AreEqual(UserLoginAttemptStatus.UnknownUser, result);
        }

        [TestMethod]
        public void TestValidateUserReturnsBadPasswordForMismatchedPassword()
        {
            var expectedEmail = "burt@reynolds.com";
            var expectedPassword = "uuuuuuuuuuuuh got any gum?";
            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail, Password = expectedPassword});

            // Add some whitespace
            var result = _target.ValidateUser(expectedEmail, "egfagea");
            Assert.AreEqual(UserLoginAttemptStatus.BadPassword, result);
        }

        [TestMethod]
        public void TestValidateUserReturnsSuccessIfUserExistsAndPasswordMatches()
        {
            var expectedEmail = "burt@reynolds.com";
            var expectedPassword = "Uuuuuuuuuuuuh got any gum?";
            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail, Password = expectedPassword});

            // Add some whitespace
            var result = _target.ValidateUser(expectedEmail, expectedPassword);
            Assert.AreEqual(UserLoginAttemptStatus.Success, result);
        }

        [TestMethod]
        public void TestValidateUserReturnsContractorAccessDisabled()
        {
            var expectedEmail = "burt@reynolds.com";
            var expectedPassword = "uuuuuuuuuuuuh got any gum?";
            var user = GetFactory<ContractorUserFactory>().Create(new { Email = expectedEmail, Password = expectedPassword });
            user.Contractor.ContractorsAccess = false;
            
            // Add some whitespace
            var result = _target.ValidateUser(expectedEmail, expectedPassword);
            Assert.AreEqual(UserLoginAttemptStatus.AccessDisabled, result);
        }

        [TestMethod]
        public void TestValidateUserIncreasesFailedLoginAttemptsCountIfUserSuppliesWrongPassword()
        {
            var expectedEmail = "burt@reynolds.com";
            var expectedPassword = "SomePass1";
            var user = GetFactory<ContractorUserFactory>().Create(new { Email = expectedEmail, Password = expectedPassword });

            Assert.AreEqual(0, user.FailedLoginAttemptCount);

            var result = _target.ValidateUser(expectedEmail, "wrong password");

            Assert.AreEqual(1, user.FailedLoginAttemptCount);
            Assert.AreEqual(UserLoginAttemptStatus.BadPassword, result);
        }

        [TestMethod]
        public void TestValidateUserSetsIsActiveToFalseWhenFailedLoginAttemptCountReachesMaximumAllowedFails()
        {
            var expectedEmail = "burt@reynolds.com";
            var expectedValidPassword = "SomePass1";
            var expectedMaxFailedLoginAttemptCount = 6; // bug 4077 says it's supposed to be 6.
            var user = GetFactory<ContractorUserFactory>().Create(new { Email = expectedEmail, Password = expectedValidPassword, FailedLoginAttemptCount = expectedMaxFailedLoginAttemptCount - 1 });

            // The user should be active and able to login when they have not yet reached the max failed login attempt count.
            Assert.AreEqual(expectedMaxFailedLoginAttemptCount - 1, user.FailedLoginAttemptCount);
            Assert.IsTrue(user.HasAccess);
            Assert.IsTrue(user.IsActive);
            
            // The user should have their access removed if they reach the max failed login attempt count. 
            var result = _target.ValidateUser(expectedEmail, "wrong password");
            Assert.AreEqual(expectedMaxFailedLoginAttemptCount, user.FailedLoginAttemptCount);
            Assert.IsFalse(user.IsActive);
            Assert.IsFalse(user.HasAccess);
            Assert.AreEqual(UserLoginAttemptStatus.BadPassword, result);

            // Then after reaching lockout, we should get AccessDisabled even if they use the correct password. 
            // The fail count should not increase at this point as the code shouldn't be reaching the point where
            // the password is being checked.
            result = _target.ValidateUser(expectedEmail, expectedValidPassword);
            Assert.AreEqual(expectedMaxFailedLoginAttemptCount, user.FailedLoginAttemptCount);
            Assert.IsFalse(user.IsActive);
            Assert.IsFalse(user.HasAccess);
            Assert.AreEqual(UserLoginAttemptStatus.AccessDisabled, result);
        }

        [TestMethod]
        public void TestValidateUserResetsFailedLoginAttemptCountIfUserSignsInWithCorrectPasswordPriorToReachingMaximumAllowedFails()
        {
            var expectedEmail = "burt@reynolds.com";
            var expectedValidPassword = "SomePass1";
            var expectedMaxFailedLoginAttemptCount = 6; // bug 4077 says it's supposed to be 6.
            var user = GetFactory<ContractorUserFactory>().Create(new { Email = expectedEmail, Password = expectedValidPassword, FailedLoginAttemptCount = expectedMaxFailedLoginAttemptCount - 1 });

            // The user should be active and able to login when they have not yet reached the max failed login attempt count.
            Assert.AreEqual(expectedMaxFailedLoginAttemptCount - 1, user.FailedLoginAttemptCount);
            Assert.IsTrue(user.HasAccess);
            Assert.IsTrue(user.IsActive);

            // The user should be able to successfully login and their fail count should be reset. 
            var result = _target.ValidateUser(expectedEmail, expectedValidPassword);
            Assert.AreEqual(0, user.FailedLoginAttemptCount);
            Assert.IsTrue(user.IsActive);
            Assert.IsTrue(user.HasAccess);
            Assert.AreEqual(UserLoginAttemptStatus.Success, result);
        }

        [TestMethod]
        public void TestValidateUserShouldNotSetIsActiveToTrueIfTheUserTriesToLoginSuccessfullyBUtDoesNotHaveAccess()
        {
            var expectedEmail = "burt@reynolds.com";
            var expectedValidPassword = "SomePass1";
            var expectedMaxFailedLoginAttemptCount = 6; // bug 4077 says it's supposed to be 6.
            var user = GetFactory<ContractorUserFactory>().Create(new { Email = expectedEmail, Password = expectedValidPassword, IsActive = false });

            // The user should be active and able to login when they have not yet reached the max failed login attempt count.
            Assert.IsFalse(user.HasAccess);
            Assert.IsFalse(user.IsActive);

            // The user should not be able to login and their active status should not be set to true when they use otherwise good credentials.
            var result = _target.ValidateUser(expectedEmail, expectedValidPassword);
            Assert.IsFalse(user.IsActive);
            Assert.IsFalse(user.HasAccess);
            Assert.AreEqual(UserLoginAttemptStatus.AccessDisabled, result);
        }

        #endregion

        #region GetUser

        [TestMethod]
        public void TestGetUserReturnsMatchingRecord()
        {
            var expectedEmail = "burt@reynolds.com";
            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail});

            var result = _target.GetUser(expectedEmail);
            Assert.AreEqual(expectedEmail, result.Email);
        }

        [TestMethod]
        public void TestGetUserReturnsNullIfNoMatchingRecord()
        {
            var expectedEmail = "burt@reynolds.com";

            var result = _target.GetUser(expectedEmail);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetUserTrimsEmailAddress()
        {
            var expectedEmail = "burt@reynolds.com";
            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail});

            var result = _target.GetUser("    " + expectedEmail);
            Assert.AreEqual(expectedEmail, result.Email);
        }

        [TestMethod]
        public void TestGetUserLowercasesEmailAddress()
        {
            var expectedEmail = "burt@reynolds.com";
            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail});

            var result = _target.GetUser(expectedEmail.ToUpper());
            Assert.AreEqual(expectedEmail, result.Email);
        }

        #endregion
    }
}
