using System;
using System.Collections;
using System.Security.Principal;
using System.Web;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MMSINC.CoreTest.Authentication
{
    [TestClass]
    public class AuthenticationServiceBaseTest
    {
        #region Private Members

        private const string TEST_COOKIE_VALUE = "Look at all your different color hats!",
                             AUTH_COOKIE_NAME = "This would usually be .ASPXAUTH at runtime";

        private Mock<IPrincipal> _mockPrincipal;
        private Mock<IIdentity> _mockIdentity;
        private Mock<IAuthenticationRepository<TestUser>> _mockRepository;
        private Mock<IAuthenticationLogRepository<TestAuthenticationLog, TestUser>> _mockLogRepo;
        private Mock<IFormsAuthenticator> _formsAuthenticator;
        private Mock<IAuthenticationCookie> _authCookie;
        private Mock<HttpContextBase> _context;
        private Mock<IDateTimeProvider> _dateProvider;
        private string _userEmail;
        private TestAuthenticationService _target;
        private TestUser _user;
        private Hashtable _contextItems; // Hashtable is what HttpContext uses internally.
        private TestAuthenticationLog _authLog;
        private HttpCookieCollection _responseCookies;
        private IContainer _container;

        #endregion

        #region Private Methods

        private void InitializeTarget()
        {
            _target = _container.GetInstance<TestAuthenticationService>();
            _target.FormsAuthenticator = _formsAuthenticator.Object;
            _target.AuthenticationCookieTest = _authCookie.Object;
        }

        private void InitializeTargetForSignedInUser()
        {
            _mockIdentity.Setup(x => x.Name).Returns("some user name it doesnt matter");
            _mockIdentity.Setup(x => x.IsAuthenticated).Returns(true);
            _authCookie = new Mock<IAuthenticationCookie>();
            _authCookie.Setup(x => x.Id).Returns(_user.Id);
            _authCookie.Setup(x => x.UserName).Returns(_user.Email);
            _authCookie.Setup(x => x.IsValidlyFormatted).Returns(true);

            _contextItems[AUTH_COOKIE_NAME] = TEST_COOKIE_VALUE;
            _formsAuthenticator.Setup(x => x.GetCurrentRequestAuthenticationCookie())
                               .Returns(() => (string)_contextItems[AUTH_COOKIE_NAME]);

            _authLog = new TestAuthenticationLog();
            _authLog.IpAddress = "Some IP";
            _authLog.LoggedInAt = DateTime.Now;
            _authLog.ExpiresAt = DateTime.Now.AddHours(2);
            _mockLogRepo.Setup(x => x.FindActiveLogByCookie(TEST_COOKIE_VALUE)).Returns(_authLog);

            InitializeTarget();
        }

        private void InitializeTargetForSignedOutUser()
        {
            _mockIdentity.Setup(x => x.Name).Returns(string.Empty);
            _mockIdentity.Setup(x => x.IsAuthenticated).Returns(false);
            _authCookie = new Mock<IAuthenticationCookie>();
            _authCookie.Setup(x => x.Id).Returns((int?)null);
            _authCookie.Setup(x => x.UserName).Returns((string)null);
            _authCookie.Setup(x => x.IsValidlyFormatted).Returns(false);

            InitializeTarget();
        }

        private TestUser CreateTestUser(int id, string email)
        {
            var user = new TestUser {Email = email, HasAccess = true};
            user.SetPropertyValueByName("Id", id);
            _mockRepository.Setup(x => x.Find(id)).Returns(user);
            return user;
        }

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void InitializeTest()
        {
            _userEmail = "userEmail@email.com";

            _dateProvider = new Mock<IDateTimeProvider>();
            _formsAuthenticator = new Mock<IFormsAuthenticator>();
            _formsAuthenticator.Setup(x => x.FormsCookieName).Returns(AUTH_COOKIE_NAME);
            _mockIdentity = new Mock<IIdentity>();
            _mockIdentity.Setup(x => x.IsAuthenticated).Returns(true);
            _mockPrincipal = new Mock<IPrincipal>();
            _mockPrincipal.Setup(x => x.Identity).Returns(_mockIdentity.Object);
            _mockRepository = new Mock<IAuthenticationRepository<TestUser>>();
            _mockLogRepo = new Mock<IAuthenticationLogRepository<TestAuthenticationLog, TestUser>>();
            _user = CreateTestUser(42, _userEmail);
            _context = new Mock<HttpContextBase>();
            _contextItems = new Hashtable();
            _context.Setup(x => x.Items).Returns(_contextItems);
            _responseCookies = new HttpCookieCollection();
            _context.Setup(x => x.Request.UserHostAddress).Returns("Some IP");
            _context.Setup(x => x.Response.Cookies).Returns(_responseCookies);

            _container = new Container(e => {
                e.For<IPrincipal>().Use(_mockPrincipal.Object);
                e.For<IDateTimeProvider>().Use(_dateProvider.Object);
                e.For<IAuthenticationCookieFactory>().Use<AuthenticationCookieFactory>();
                e.For<IAuthenticationRepository<TestUser>>().Use(_mockRepository.Object);
                e.For<IAuthenticationLogRepository<TestAuthenticationLog, TestUser>>().Use(_mockLogRepo.Object);
                e.For<HttpContextBase>().Use(_context.Object);
            });

            InitializeTargetForSignedInUser();
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorCreatesFormsAuthenticator()
        {
            var target = _container.GetInstance<TestAuthenticationService>();
            Assert.IsNotNull(target.FormsAuthenticator);
        }

        [TestMethod]
        public void TestConstructorCreatesAuthenticationCookieFromIPrincipalParameter()
        {
            var expectedId = 13579;
            var expectedUsername = "jack@kabletown.com";
            _mockIdentity.Setup(x => x.Name)
                         .Returns(string.Format(AuthenticationCookie.FORMAT, expectedId, expectedUsername));
            var target = _container.GetInstance<TestAuthenticationService>();
            Assert.IsNotNull(target.AuthenticationCookieTest);
            Assert.AreEqual(expectedId, target.AuthenticationCookieTest.Id);
            Assert.AreEqual(expectedUsername, target.AuthenticationCookieTest.UserName);
            Assert.IsTrue(target.AuthenticationCookieTest.IsValidlyFormatted);
        }

        #endregion

        #region CurrentUser

        [TestMethod]
        public void TestCurrentUserThrowsInvalidOperationExceptionIfCurrentUserIsNotAuthenticated()
        {
            InitializeTargetForSignedOutUser();

            TestUser u = null;
            MyAssert.Throws<InvalidOperationException>(() => u = _target.CurrentUser);
        }

        [TestMethod]
        public void TestCurrentUserLoadsCurrentUserIfUserIsAuthenticated()
        {
            _mockRepository.Setup(x => x.Find(_user.Id)).Returns(_user);
            Assert.IsTrue(_target.CurrentUserIsAuthenticated);
            Assert.AreSame(_user, _target.CurrentUser);
        }

        #endregion

        #region CurrentUserIdentifier

        [TestMethod]
        public void TestCurrentUserIdentifierThrowsInvalidOperationExceptionIfCurrentUserIsNotAuthenticated()
        {
            InitializeTargetForSignedOutUser();

            string u = null;
            MyAssert.Throws<InvalidOperationException>(() => u = _target.CurrentUserIdentifier);
        }

        [TestMethod]
        public void TestCurrentUserIdentifierReturnsCurrentUserUniqueNameIfUserIsAuthenticated()
        {
            InitializeTargetForSignedInUser();
            _mockRepository.Setup(x => x.Find(_user.Id)).Returns(_user);
            _target.SignIn(_user.Id, false);
            Assert.IsTrue(_target.CurrentUserIsAuthenticated);
            Assert.AreEqual(_user.UniqueName, _target.CurrentUserIdentifier);
        }

        #endregion

        #region CurrentUserIsAdmin

        [TestMethod]
        public void TestCurrentUserIsAdminReturnsTrueIfCurrentUserIsAdminAndCurrentUserIsAuthenticated()
        {
            _mockIdentity.Setup(x => x.IsAuthenticated).Returns(true);
            _user.IsAdmin = true;
            _target.SignIn(_user.Id, false);
            Assert.IsTrue(_target.CurrentUserIsAdmin);
        }

        [TestMethod]
        public void TestCurrentUserIsAdminReturnsFalseIfCurrentUserIsNotAuthenticated()
        {
            InitializeTargetForSignedOutUser();
            _user.IsAdmin = true;

            Assert.IsFalse(_target.CurrentUserIsAdmin);
        }

        [TestMethod]
        public void TestCurrentUserIsAdminReturnsFalseIfCurrentUserIsNotAdmin()
        {
            _mockIdentity.Setup(x => x.IsAuthenticated).Returns(true);
            _user.IsAdmin = false;
            _target.SignIn(_user.Id, false);
            Assert.IsFalse(_target.CurrentUserIsAdmin);
        }

        #endregion

        #region CurrentUserIsAuthenticated

        [TestMethod]
        public void
            TestCurrentUserIsAuthenticatedReturnsTrueWhenIsAuthenticatedIsTrueAndUserExistsIsTrueAndUserHasAccess()
        {
            _user.HasAccess = true;
            _mockIdentity.Setup(x => x.IsAuthenticated).Returns(true);
            _mockRepository.Setup(x => x.Find(_user.Id)).Returns(_user);

            Assert.IsTrue(_target.CurrentUserIsAuthenticated);

            _mockRepository.Verify(x => x.Find(_user.Id));
        }

        [TestMethod]
        public void TestCurrentUserIsAuthenticatedReturnsFalseWhenIsAuthenticatedIsTrueButUserHasAccessIsFalse()
        {
            _user.HasAccess = false;
            _mockIdentity.Setup(x => x.IsAuthenticated).Returns(true);
            _mockRepository.Setup(x => x.Find(_user.Id)).Returns(_user);

            Assert.IsFalse(_target.CurrentUserIsAuthenticated);

            _mockRepository.Verify(x => x.Find(_user.Id));
        }

        [TestMethod]
        public void TestCurrentUserIsAuthenticatedReturnsFalseWhenIsAuthenticatedIsTrueAndUserExistsIsFalse()
        {
            InitializeTargetForSignedInUser();
            _authCookie.Setup(x => x.Id).Returns(93311);
            _mockIdentity.Setup(x => x.IsAuthenticated).Returns(true);
            _mockRepository.Setup(x => x.Find(93311)).Returns((TestUser)null);

            Assert.IsFalse(_target.CurrentUserIsAuthenticated);
        }

        [TestMethod]
        public void TestCurrentUserIsAuthenticatedReturnsFalseWhenIsAuthenticatedIsFalse()
        {
            // Ensure that all the other parameters for a user being considered signed in
            // are all set, then setting IsAuthenticated to false should negate that.
            InitializeTargetForSignedInUser();
            _mockIdentity.Setup(x => x.IsAuthenticated).Returns(false);
            // reinitialize since the identity value is set through the constructor.
            InitializeTarget();
            Assert.IsTrue(_authCookie.Object.IsValidlyFormatted, "Ensure this test didn't get screwed up.");

            Assert.IsFalse(_target.CurrentUserIsAuthenticated);
        }

        [TestMethod]
        public void TestCurrentUserIsAuthenticatedReturnsTrueImmediatelyAfterCallingSignIn()
        {
            _authCookie.Setup(x => x.IsValidlyFormatted).Returns(false);
            _mockIdentity.Setup(x => x.IsAuthenticated).Returns(false);
            var user = new TestUser {Email = _userEmail, HasAccess = true};
            user.SetPropertyValueByName("Id", 8352); // Something not 0.
            _mockRepository.Setup(x => x.Find(user.Id)).Returns(user);

            Assert.IsFalse(_target.CurrentUserIsAuthenticated);

            _target.SignIn(user.Id, false);

            Assert.IsTrue(_target.CurrentUserIsAuthenticated);
            Assert.AreSame(_target.CurrentUser, user);
        }

        [TestMethod]
        public void TestCurrentUserIsAuthenticatedReturnsFalseImmediatelyAfterCallingSignOut()
        {
            InitializeTargetForSignedInUser();
            Assert.IsTrue(_target.CurrentUserIsAuthenticated);

            _target.SignOut();
            Assert.IsFalse(_target.CurrentUserIsAuthenticated);
        }

        [TestMethod]
        public void
            TestCurrentUserIsAuthenticatedReturnsTrueWhenIdentityIsAuthenticatedIsTrueAndHasAValidlyFormattedAuthenticationCookieForAValidUser()
        {
            InitializeTargetForSignedInUser();
            Assert.IsTrue(_authCookie.Object.IsValidlyFormatted);
            Assert.IsTrue(_mockIdentity.Object.IsAuthenticated);
            Assert.IsTrue(_target.CurrentUserIsAuthenticated);
        }

        [TestMethod]
        public void TestCurrentUserIsAuthenticatedReturnsFalseIfEmailAddressDoesNotMatchAuthenticationCookieUsername()
        {
            InitializeTargetForSignedInUser();
            Assert.IsTrue(_target.CurrentUserIsAuthenticated);
            _authCookie.Setup(x => x.UserName).Returns("something that isnt the user's email");
            Assert.IsFalse(_target.CurrentUserIsAuthenticated);
        }

        [TestMethod]
        public void
            TestCurrentUserIsAuthenticatedReturnsFalseIfAuthenticationCookieIdIsLessThanOrEqualToZeroOrIdIsNull()
        {
            var ids = new int?[] {0, -1, null};
            ids.Each(id => {
                InitializeTargetForSignedInUser();
                Assert.IsTrue(_target.CurrentUserIsAuthenticated, "Ensure this is setup correctly.");
                _authCookie.Setup(x => x.Id).Returns(0);
                Assert.IsFalse(_target.CurrentUserIsAuthenticated,
                    "User must not be authenticated if their user id is {0}", id);
            });
        }

        [TestMethod]
        public void TestCurrentUserIsAuthenticatedReturnsFalseIfExistingAuthenticationLogCanNotBeFound()
        {
            InitializeTargetForSignedInUser();
            Assert.IsTrue(_target.CurrentUserIsAuthenticated,
                "Sanity check to ensure that all variables are set to sign in validly.");

            InitializeTarget();
            _mockLogRepo.Setup(x => x.FindActiveLogByCookie(TEST_COOKIE_VALUE)).Returns((TestAuthenticationLog)null);
            Assert.IsFalse(_target.CurrentUserIsAuthenticated, "User should no longer be authenticated.");
        }

        [TestMethod]
        public void TestCurrentUserIsAuthenticatedReturnsFalseIfHttpContextItemsDoesNotIncludeRequestCookie()
        {
            InitializeTargetForSignedInUser();
            Assert.IsTrue(_target.CurrentUserIsAuthenticated,
                "Sanity check to ensure that all variables are set to sign in validly.");

            InitializeTarget(); // Need to reinitialize for this to work since AuthServ caches values and stuff.
            _contextItems.Clear();
            Assert.IsFalse(_target.CurrentUserIsAuthenticated, "User should no longer be authenticated.");
        }

        [TestMethod]
        public void TestCurrentUserIsAuthenticatedReturnsFalseIfCurrentIPDoesNotMatchLogIP()
        {
            InitializeTargetForSignedInUser();
            Assert.IsTrue(_target.CurrentUserIsAuthenticated,
                "Sanity check to ensure that all variables are set to sign in validly.");

            InitializeTarget(); // Need to reinitialize for this to work since AuthServ caches values and stuff.
            _context.Setup(x => x.Request.UserHostAddress).Returns("A different ip");
            Assert.IsFalse(_target.CurrentUserIsAuthenticated, "User should no longer be authenticated.");
        }

        [TestMethod]
        public void TestCurrentUserIsAuthenticatedReturnsFalseIfAuthenticationLogIsExpired()
        {
            InitializeTargetForSignedInUser();
            Assert.IsTrue(_target.CurrentUserIsAuthenticated,
                "Sanity check to ensure that all variables are set to sign in validly.");

            InitializeTarget(); // Need to reinitialize for this to work since AuthServ caches values and stuff.

            _dateProvider.Setup(x => x.GetCurrentDate()).Returns(_authLog.ExpiresAt.AddSeconds(1));
            // Tickets constantly compare their expiration values against DateTime.NowUtc. This test runs too
            // fast for making Expired return true, so we need a really short delay after creation and before
            // processing to make sure the test can pass.
            //  _formsAuthenticator.Setup(x => x.Decrypt(TEST_COOKIE_VALUE)).Returns(ticket);
            Assert.IsFalse(_target.CurrentUserIsAuthenticated, "User should no longer be authenticated.");
        }

        [TestMethod]
        public void TestCurrentUserIsAuthenticatedReturnsFalseIfSuppliedCookieHasLoggedOutAtValue()
        {
            InitializeTargetForSignedInUser();
            Assert.IsTrue(_target.CurrentUserIsAuthenticated,
                "Sanity check to ensure that all variables are set to sign in validly.");

            InitializeTarget(); // Need to reinitialize for this to work since AuthServ caches values and stuff.
            _authLog.LoggedOutAt = DateTime.Now;
            Assert.IsFalse(_target.CurrentUserIsAuthenticated, "User should no longer be authenticated.");
        }

        #endregion

        #region SignIn

        [TestMethod]
        public void TestSignInSetsAuthenticationCookieToNewValidInstance()
        {
            InitializeTargetForSignedOutUser();
            var expectedUser = CreateTestUser(99, "bilbo@baggins.com");

            // Need this to ensure the cookie changes.
            var curCookie = _target.AuthenticationCookieTest;

            _target.SignIn(expectedUser.Id, false);
            var result = _target.AuthenticationCookieTest;
            Assert.AreNotSame(curCookie, result);
            Assert.AreEqual(expectedUser.Id, result.Id);
            Assert.AreEqual(expectedUser.Email, result.UserName);
            Assert.IsTrue(result.IsValidlyFormatted);
        }

        [TestMethod]
        public void TestSignInPassesAuthenticationCookieToFormsAuthenticator()
        {
            InitializeTargetForSignedOutUser();
            var expectedUser = CreateTestUser(99, "bilbo@baggins.com");

            IAuthenticationCookie result = null;
            _formsAuthenticator.Setup(x => x.SetAuthCookie(It.IsAny<IAuthenticationCookie>()))
                               .Callback((IAuthenticationCookie x) => { result = x; });

            _target.SignIn(expectedUser.Id, false);
            Assert.IsNotNull(result);
            Assert.AreSame(result, _target.AuthenticationCookieTest);
        }

        private TestAuthenticationLog SignInAndGetLog()
        {
            InitializeTargetForSignedOutUser();
            var expectedUser = CreateTestUser(99, "bilbo@baggins.com");

            _formsAuthenticator.Setup(x => x.SetAuthCookie(It.IsAny<IAuthenticationCookie>()))
                               .Returns(TEST_COOKIE_VALUE);

            TestAuthenticationLog result = null;
            _mockLogRepo.Setup(x => x.Save(It.IsAny<TestAuthenticationLog>(), TEST_COOKIE_VALUE))
                        .Callback((TestAuthenticationLog log, string cookieString) => { result = log; });
            _target.SignIn(expectedUser.Id, false);

            Assert.IsNotNull(result, "Log was not saved.");
            return result;
        }

        [TestMethod]
        public void TestSignInCreatesANewAuthenticationLogInstanceForTheNewCookie()
        {
            var result = SignInAndGetLog();
            Assert.IsNotNull(result, "Log was not saved.");
        }

        [TestMethod]
        public void TestSignInSetsCurrentRequestUserHostAddressAsIpAddressOnNewAuthenticationLogInstance()
        {
            _context.Setup(x => x.Request.UserHostAddress).Returns("This is my IP");
            var result = SignInAndGetLog();
            Assert.AreEqual("This is my IP", result.IpAddress);
        }

        [TestMethod]
        public void TestSignInSetsLoggedInAtToNow()
        {
            var expected = DateTime.Now.AddHours(12);
            _dateProvider.Setup(x => x.GetCurrentDate()).Returns(expected);
            var result = SignInAndGetLog();
            Assert.AreEqual(expected, result.LoggedInAt);
        }

        [TestMethod]
        public void TestSignInCorrectlyAuthenticatesWithTokenValidationAttributeStuff()
        {
            InitializeTargetForSignedOutUser();
            var expectedUser = CreateTestUser(99, "bilbo@baggins.com");

            _formsAuthenticator.Setup(x => x.SetAuthCookie(It.IsAny<IAuthenticationCookie>()))
                               .Returns(TEST_COOKIE_VALUE);

            Assert.IsFalse(_target.CurrentUserIsAuthenticated, "Sanity check");
            _target.SignIn(expectedUser.Id, true);
            Assert.IsTrue(_target.CurrentUserIsAuthenticated);
        }

        [TestMethod]
        public void TestSignInSetsUsersLastLoggedInAt()
        {
            var expectedUser = CreateTestUser(99, "bilbo@baggins.com");
            var currentDateTime = DateTime.Now.AddHours(1);
            _dateProvider.Setup(x => x.GetCurrentDate()).Returns(currentDateTime);

            _target.SignIn(expectedUser.Id, false);

            _mockRepository.Verify(x => x.Save(It.Is<TestUser>(u => u.LastLoggedInAt == currentDateTime)));
            Assert.AreEqual(currentDateTime, expectedUser.LastLoggedInAt);
        }

        #endregion

        #region SignOut

        [TestMethod]
        public void TestSignOutInvalidatesTheCurrentUsersAuthentication()
        {
            _user.IsAdmin = true;
            // Ensure all this stuff is correct.
            InitializeTargetForSignedInUser();
            var curCookie = _target.AuthenticationCookieTest;
            Assert.IsTrue(_target.CurrentUserIsAuthenticated);
            Assert.IsTrue(_target.CurrentUserIsAdmin);
            Assert.AreSame(_user, _target.CurrentUser);

            _target.SignOut();
            Assert.IsFalse(_target.CurrentUserIsAuthenticated);
            MyAssert.Throws<InvalidOperationException>(() => _user = _target.CurrentUser);
            Assert.AreNotSame(curCookie, _target.AuthenticationCookieTest);
        }

        [TestMethod]
        public void TestSignOutAlsoSignsOutWithFormAuthenticator()
        {
            InitializeTargetForSignedInUser();
            _target.SignOut();
            _formsAuthenticator.Verify(x => x.SignOut());
        }

        [TestMethod]
        public void TestSignOutSetsLoggedOutTimeOnExistingLog()
        {
            InitializeTargetForSignedInUser();

            var expected = DateTime.Now.AddMinutes(43);
            _dateProvider.Setup(x => x.GetCurrentDate()).Returns(expected);

            Assert.IsNull(_authLog.LoggedOutAt);

            _target.SignOut();

            Assert.AreEqual(expected, _authLog.LoggedOutAt);
            _mockLogRepo.Verify(x => x.Save(_authLog));
        }

        [TestMethod]
        public void TestSignOutDoesNotExplodeDueToEmptyStringCookieValue()
        {
            InitializeTargetForSignedInUser();
            _contextItems[AUTH_COOKIE_NAME] = string.Empty;

            _target.SignOut();

            _mockLogRepo.Verify(x => x.FindActiveLogByCookie(string.Empty), Times.Never);
        }

        #endregion

        #endregion
    }

    public class TestAuthenticationService : AuthenticationServiceBase<TestUser, TestAuthenticationLog>
    {
        #region Properties

        public IAuthenticationCookie AuthenticationCookieTest
        {
            get { return AuthenticationCookie; }
            set { AuthenticationCookie = value; }
        }

        #endregion

        #region Constructors

        public TestAuthenticationService(IContainer container, IPrincipal principal,
            IAuthenticationCookieFactory cookieFactory) : base(container, principal, cookieFactory) { }

        #endregion
    }
}
