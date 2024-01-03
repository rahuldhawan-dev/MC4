using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using NHibernate;
using StructureMap;

namespace MMSINC.CoreTest.Authentication
{
    [TestClass]
    public class AuthenticationRepositoryBaseTest : InMemoryDatabaseTest<TestUser>
    {
        #region Constants

        public static readonly string[] INVALID_EMAIL_ADDRESSES = new[] {
            null, string.Empty, "a", "a@b", "a.b"
        };

        public const string NON_EXISTENT_USER_ADDRESS = "nonexistant@nosite.com";

        #endregion

        #region Private Members

        private TestAuthenticationRepository _target;
        private TestUser _testUser;
        private Mock<ICredentialPolicy> _credentialPolicy;

        // ReSharper disable once UnusedField.Compiler
        private System.Data.SQLite.SQLiteException _doNotUseThisException;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = _container.GetInstance<TestAuthenticationRepository>();
            _testUser = GetFactory<TestUserFactory>().Create();
            _credentialPolicy = new Mock<ICredentialPolicy>();
            _credentialPolicy.Setup(x => x.PasswordMeetsRequirement(It.IsAny<string>())).Returns(true);
            _target.SetPasswordRequirement(_credentialPolicy.Object);
        }

        #endregion

        #region Other Tests

        [TestMethod]
        public void TestAuthenticationRepositoryBaseHasDefaultCredentialPolicyByDefault()
        {
            // Create a new target since we're replacing the _target version's in TestInit.
            var target = _container.GetInstance<TestAuthenticationRepository>();
            Assert.IsInstanceOfType(target.CredentialPolicy, typeof(DefaultCredentialPolicy));
        }

        #endregion

        #region GetUser(string email)

        [TestMethod]
        public void TestGetUserReturnsUserWithSpecifiedEmailAddress()
        {
            Assert.AreEqual(_testUser, _target.GetUser(_testUser.Email));
        }

        [TestMethod]
        public void TestGetUserReturnsNullIfSpecifiedUserDoesNotExist()
        {
            Assert.IsNull(_target.GetUser(NON_EXISTENT_USER_ADDRESS));
        }

        [TestMethod]
        public void TestGetUserThrowsExceptionIfSpecifedEmailIsInvalid()
        {
            INVALID_EMAIL_ADDRESSES.Each(
                email =>
                    MyAssert.Throws<AuthenticationException>(
                        () => _target.GetUser(email)));
        }

        #endregion

        #region UserExists(string email)

        [TestMethod]
        public void TestUserExistsReturnsTrueIfSpecifiedUserExists()
        {
            Assert.IsTrue(_target.UserExists(_testUser.Email));
        }

        [TestMethod]
        public void TestUserExistsReturnsFalseIfSpecifiedUserDoesNotExist()
        {
            Assert.IsFalse(_target.UserExists(NON_EXISTENT_USER_ADDRESS));
        }

        [TestMethod]
        public void TestUserExistsThrowsExceptionIfSpecifedEmailIsInvalid()
        {
            INVALID_EMAIL_ADDRESSES.Each(
                email =>
                    MyAssert.Throws<AuthenticationException>(
                        () => _target.UserExists(email)));
        }

        #endregion

        #region ValidateUser(string email, string password)

        [TestMethod]
        public void TestValidateUserReturnsInvalidEmailIfEmailAddressIsNotValid()
        {
            INVALID_EMAIL_ADDRESSES.Each(
                email =>
                    Assert.AreEqual(UserLoginAttemptStatus.InvalidEmail,
                        _target.ValidateUser(email, null)));
        }

        [TestMethod]
        public void TestValidateUserReturnsUnknownUserIfUserDoesNotExist()
        {
            Assert.AreEqual(UserLoginAttemptStatus.UnknownUser,
                _target.ValidateUser(NON_EXISTENT_USER_ADDRESS, null));
        }

        [TestMethod]
        public void TestValidateUserReturnsBadPasswordIfPasswordDoesNotMatch()
        {
            Assert.AreEqual(UserLoginAttemptStatus.BadPassword,
                _target.ValidateUser(_testUser.Email, "bad password"));
        }

        [TestMethod]
        public void TestValidateUserReturnsOutResultFromExtraValidateMethodIfItReturnsFalse()
        {
            _target.FurtherValidateReturn = false;
            _target.ExtraStatusFn = (email, password, user) => {
                Assert.AreEqual(_testUser.Email, email);
                Assert.AreEqual(TestUserFactory.DEFAULT_PASSWORD, password);
                Assert.AreEqual(_testUser, user);
                return UserLoginAttemptStatus.AccessDisabled;
            };

            Assert.AreEqual(UserLoginAttemptStatus.AccessDisabled,
                _target.ValidateUser(_testUser.Email, TestUserFactory.DEFAULT_PASSWORD));
        }

        [TestMethod]
        public void TestValidateUserDoesNotSkipPasswordValidationIfFurtherValidateReturnsFalseAndSuccess()
        {
            _target.FurtherValidateReturn = false;
            _target.ExtraStatusFn = (email, password, user) => {
                Assert.AreEqual(_testUser.Email, email);
                Assert.AreEqual("bad password", password);
                Assert.AreEqual(_testUser, user);
                return UserLoginAttemptStatus.Success;
            };

            Assert.AreEqual(UserLoginAttemptStatus.BadPassword,
                _target.ValidateUser(_testUser.Email, "bad password"));
        }

        [TestMethod]
        public void TestValidateUserReturnsSuccessIfEverythingIsOk()
        {
            Assert.AreEqual(UserLoginAttemptStatus.Success,
                _target.ValidateUser(_testUser.Email, TestUserFactory.DEFAULT_PASSWORD));
        }

        [TestMethod]
        public void TestValidateReturnsAccessDisabledIfUserHasAccessIsFalse()
        {
            _testUser.HasAccess = false;
            Assert.AreEqual(UserLoginAttemptStatus.AccessDisabled,
                _target.ValidateUser(_testUser.Email, TestUserFactory.DEFAULT_PASSWORD));
        }

        [TestMethod]
        public void
            TestValidateReturnsSuccessPasswordChangedRequiredIfLoginIsSuccessfulButDoesNotMeetPasswordRequirements()
        {
            _credentialPolicy.Setup(x => x.PasswordMeetsRequirement(It.IsAny<string>())).Returns(false);
            Assert.AreEqual(UserLoginAttemptStatus.SuccessRequiresPasswordChange,
                _target.ValidateUser(_testUser.Email, TestUserFactory.DEFAULT_PASSWORD));
        }

        [TestMethod]
        public void TestValidateUserCallsOnInvalidPasswordIfPasswordIsInvalid()
        {
            Assert.IsFalse(_target.OnInvalidPasswordCalled);
            Assert.IsFalse(_target.OnValidPasswordCalled);
            _target.ValidateUser(_testUser.Email, "bad pass");
            Assert.IsTrue(_target.OnInvalidPasswordCalled);
            Assert.IsFalse(_target.OnValidPasswordCalled, "Sanity. OnValid should not have been called.");
        }

        [TestMethod]
        public void TestValidateUserCallsOnValidPasswordIfPasswordIsValid()
        {
            Assert.IsFalse(_target.OnInvalidPasswordCalled);
            Assert.IsFalse(_target.OnValidPasswordCalled);
            _target.ValidateUser(_testUser.Email, TestUserFactory.DEFAULT_PASSWORD);
            Assert.IsFalse(_target.OnInvalidPasswordCalled, "Sanity. OnInvalid should not have been called.");
            Assert.IsTrue(_target.OnValidPasswordCalled);
        }

        #endregion

        #region ValidatePasswordQuestionAnswer(string email, string answer)

        [TestMethod]
        public void TestValidatePasswordQuestionAnswerReturnsTrueIfAnswerMatches()
        {
            Assert.IsTrue(
                _target.ValidatePasswordQuestionAnswer(
                    _testUser.Email,
                    TestUserFactory.DEFAULT_PASSWORD_ANSWER));
        }

        [TestMethod]
        public void TestValidatePasswordQuestionAnswerReturnsFalseIfAnswerDoesNotMatch()
        {
            Assert.IsFalse(
                _target.ValidatePasswordQuestionAnswer(
                    _testUser.Email,
                    "some other answer"));
        }

        [TestMethod]
        public void TestValidatePasswordQuestionAnswerReturnsFalseIfUserDoesNotExist()
        {
            Assert.IsFalse(
                _target.ValidatePasswordQuestionAnswer(
                    NON_EXISTENT_USER_ADDRESS, "does not matter"));
        }

        #endregion
    }

    internal class TestAuthenticationRepository : AuthenticationRepositoryBase<TestUser>
    {
        #region Fields

        private ICredentialPolicy _credentialPolicy;

        #endregion

        #region Properties

        public Func<string, string, TestUser, UserLoginAttemptStatus> ExtraStatusFn { get; set; }
        public bool FurtherValidateReturn { get; set; }

        public bool OnInvalidPasswordCalled { get; set; }
        public bool OnValidPasswordCalled { get; set; }

        public override ICredentialPolicy CredentialPolicy
        {
            get { return _credentialPolicy ?? base.CredentialPolicy; }
        }

        #endregion

        #region Constructors

        public TestAuthenticationRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public override bool FurtherValidate(string email, string password, TestUser user,
            ref UserLoginAttemptStatus status)
        {
            if (ExtraStatusFn != null)
            {
                status = ExtraStatusFn(email, password, user);
            }

            return FurtherValidateReturn;
        }

        public override void OnInvalidPassword(TestUser user)
        {
            OnInvalidPasswordCalled = true;
        }

        public override void OnValidPassword(TestUser user)
        {
            OnValidPasswordCalled = true;
        }

        public void SetPasswordRequirement(ICredentialPolicy passReq)
        {
            _credentialPolicy = passReq;
        }

        #endregion
    }
}
