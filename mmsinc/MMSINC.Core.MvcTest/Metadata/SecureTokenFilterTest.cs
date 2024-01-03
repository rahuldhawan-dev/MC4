using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using MMSINC.Helpers;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class SecureTokenFilterTest : InMemoryDatabaseTest<SecureFormToken>
    {
        #region Fields

        private SecureTokenFilter<SecureFormToken, SecureFormDynamicValue> _target;
        private SecureFormToken _token;
        private FakeMvcApplicationTester _tester;
        private FakeMvcHttpHandler _request;
        private Mock<IAuthenticationService<IAdministratedUser>> _authServ;
        private Mock<IAdministratedUser> _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private ActionExecutingContext _executingContext;
        private Mock<ITokenRepository<SecureFormToken, SecureFormDynamicValue>> _tokenRepository;
        private DateTime _nowDate;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _authServ = new Mock<IAuthenticationService<IAdministratedUser>>();
            _user = new Mock<IAdministratedUser>();
            _user.Setup(x => x.UniqueName).Returns("some user");
            _user.Setup(x => x.Id).Returns(42);
            _authServ.Setup(x => x.CurrentUser).Returns(_user.Object);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);
            _container.Inject(_authServ.Object);
            _container.Inject<IAuthenticationService>(_authServ.Object);
            _tokenRepository = new Mock<ITokenRepository<SecureFormToken, SecureFormDynamicValue>>();
            _container.Inject(_tokenRepository.Object);

            _nowDate = DateTime.Now;
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_nowDate);
            _container.Inject(_dateTimeProvider.Object);

            var controller = new CrudController();
            _tester = new FakeMvcApplicationTester(_container);
            _tester.ControllerFactory.RegisterController(controller);

            _request = _tester.CreateRequestHandler("~/Crud/SecureAction/");
            var rc = new RouteContext(_request.RequestContext);
            _executingContext = new ActionExecutingContext(_request.CreateControllerContext(controller),
                rc.ActionDescriptor, new Dictionary<string, object>());
            _token = new SecureFormToken {
                Action = "SecureAction", Controller = "Crud", UserId = _user.Object.Id, CreatedAt = DateTime.Now,
                Token = Guid.NewGuid()
            };
            //_token = _tokenRepository.Save(_token);
            _tokenRepository.Setup(x => x.FindByToken(_token.Token)).Returns(_token);

            _target = _container.GetInstance<SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>>();
            AddTokenToForm();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _tester.Dispose();
        }

        #endregion

        #region Private Methods

        private void AddTokenToQuerystring()
        {
            AddTokenToQuerystring(_token.Token.ToString());
        }

        private void AddTokenToQuerystring(string token)
        {
            _request.RequestForm.Remove(SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.FORM_KEY);
            _request.RequestQueryString[SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.FORM_KEY] = token;
        }

        private void AddTokenToForm()
        {
            AddTokenToForm(_token.Token.ToString());
        }

        private void AddTokenToForm(string token)
        {
            _request.RequestQueryString.Remove(SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.FORM_KEY);
            _request.RequestForm[SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.FORM_KEY] = token;
        }

        private void AddRouteValue(string key, string value)
        {
            _request.RouteData.Values[key] = value;
        }

        private void ClearTokenFromForm()
        {
            _request.RequestForm.Remove(SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.FORM_KEY);
        }

        private void OnActionExecuting()
        {
            _target.OnActionExecuting(_executingContext);
        }

        private void AssertIsNotValid(string message = null)
        {
            OnActionExecuting();
            var result =
                (SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.InvalidSecureFormTokenViewResult)
                _executingContext.Result;
            Assert.AreEqual(message, result.ViewResult.ViewData["Message"]);
        }

        private void AssertIsValid()
        {
            OnActionExecuting();
            Assert.IsNull(_executingContext.Result);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestOnActionExecutingReturnsAnActionResultThatSetsStatusCodeTo400WhenExecuted()
        {
            const int expectedStatusCode = (int)HttpStatusCode.BadRequest;

            AddTokenToForm("this is not a guid.");
            _target.OnActionExecuting(_executingContext);

            // Replace the ViewResult with one that doesn't throw an exception.
            var mockView = new Mock<ViewResultBase>();
            var result =
                (SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.InvalidSecureFormTokenViewResult)
                _executingContext.Result;
            result.ViewResult = mockView.Object;

            _executingContext.Result.ExecuteResult(_executingContext);
            _request.Response.VerifySet(x => x.StatusCode = expectedStatusCode,
                "Status code should have been set to 400.");
            _request.Response.VerifySet(x => x.TrySkipIisCustomErrors = true,
                "These need to be skipped or else they'll never see the custom error.");
        }

        [TestMethod]
        public void TestOnActionExecutingDoesNotReplaceCurrentResultIfResultIsAlreadySet()
        {
            var expected = new ViewResult();
            _executingContext.Result = expected;

            OnActionExecuting();

            Assert.AreSame(expected, _executingContext.Result, "The result should not have changed.");
        }

        [TestMethod]
        public void
            TestOnActionExecutingReturnsErrorView_IfSecureFormIsRequiredAndUserIsAuthenticatedButTokenIsMissing()
        {
            ClearTokenFromForm();

            AssertIsNotValid(SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.ErrorMessages
               .INVALID_SECURITY_TOKEN);
        }

        [TestMethod]
        public void TestOnActionExecutingReturnsErrorView_IfSecureFormIsRequiredAndTokenIsNotRecognized()
        {
            AddTokenToForm(Guid.NewGuid().ToString());

            AssertIsNotValid(SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.ErrorMessages
               .INVALID_SECURITY_TOKEN);
        }

        [TestMethod]
        public void TestOnActionExecutingReturnsErrorView_IfSecureFormIsRequiredAndTokenIsUnparsable()
        {
            AddTokenToForm("this is not a guid.");

            AssertIsNotValid(SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.ErrorMessages
               .INVALID_SECURITY_TOKEN);
        }

        [TestMethod]
        public void TestOnActionExecutingReturnsErrorView_IfSecureFormIsRequiredAndTokenDoesNotMatchCurrentUser()
        {
            _user.Setup(x => x.Id).Returns(666);

            AssertIsNotValid(SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.ErrorMessages
               .TOKEN_DOES_NOT_MATCH_USER);
        }

        [TestMethod]
        public void TestOnActionExecutingReturnsErrorView_IfSecureFormIsRequiredAndTokenIsExpired()
        {
            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(_nowDate.AddMinutes(SecureFormToken.EXPIRATION_MINUTES + 1));

            AssertIsNotValid(SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.ErrorMessages.TOKEN_IS_EXPIRED);
        }

        [TestMethod]
        public void TestOnActionExecutingReturnsErrorView_IfTokenRouteDoesNotMatchRequestRoute()
        {
            _token.Action = "Something unrelated";

            AddTokenToForm();

            AssertIsNotValid(SecureTokenFilter<SecureFormToken, SecureFormDynamicValue>.ErrorMessages
               .TOKEN_DOES_NOT_MATCH_ROUTE);
        }

        [TestMethod]
        public void TestOnActionExecutingSetsNullResult_IfTokenSecuredValueIsInRouteValues()
        {
            var key = "Id";
            var value = "1234";
            _token.DynamicValues.Add(new SecureFormDynamicValue {SecureFormToken = _token, Key = key, Value = value});

            AddRouteValue(key, value);

            AssertIsValid();
        }

        [TestMethod]
        public void TestOnActionExecutingDoesNotReturnResultIfTokenIsValid()
        {
            AddTokenToForm();

            AssertIsValid();
        }

        [TestMethod]
        public void TestOnActionExecutingDoesNotReturnResult_IfActionDoesNotRequireSecureForm()
        {
            _request = _tester.CreateRequestHandler("~/Crud/InsecureAction/");
            var rc = new RouteContext(_request.RequestContext);
            _executingContext = new ActionExecutingContext(_request.CreateControllerContext(new CrudController()),
                rc.ActionDescriptor, new Dictionary<string, object>());
            ClearTokenFromForm();
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(false);

            AssertIsValid();
        }

        [TestMethod]
        public void TestOnActionExecutingAlsoFindsTokenFromQuerystring()
        {
            AddTokenToQuerystring();

            AssertIsValid();
        }

        [TestMethod]
        public void TestOnActionExecutingDeletesTheToken()
        {
            Assert.IsTrue(FormBuilder.SecureFormsEnabled);
            AssertIsValid();

            _target.OnActionExecuting(_executingContext);
            _tokenRepository.Verify(x => x.Delete(_token));
        }

        [TestMethod]
        public void TestOnActionExecutingAllowsForNullValues()
        {
            var key = "Id";

            _token.DynamicValues.Add(new SecureFormDynamicValue {SecureFormToken = _token, Key = key, Value = null});
            AddRouteValue(key, String.Empty);

            AssertIsValid();
        }

        #endregion

        #region Test classes

        private class CrudController : Controller
        {
            [RequiresSecureForm]
            public ActionResult SecureAction()
            {
                return null;
            }

            public ActionResult InsecureAction()
            {
                return null;
            }

            [FakeAuthorize(false)]
            public ActionResult Unauthorized()
            {
                return null;
            }
        }

        #endregion
    }
}
