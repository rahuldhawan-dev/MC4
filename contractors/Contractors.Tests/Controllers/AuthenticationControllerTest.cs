using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class AuthenticationControllerTest : ContractorControllerTestBase<AuthenticationController, ContractorUser, ContractorUserRepository>
    {
        #region Setup/Teardown

        protected override ContractorUser CreateUser()
        {
            return GetFactory<ContractorUserFactory>().Create(new { Email = "email@email.com" });
        }

        #endregion

        #region Private Methods

        // Have to do this since IsAjaxRequest is an extension method.
        private void SetIsAjaxRequest(bool wellIsIt)
        {
            if (wellIsIt)
            {
                Request.RequestHeaders["X-Requested-With"] = "XMLHttpRequest";
            }
        }

        private Tuple<ContractorUser, ContractorUserLogOn> CreateUserTuple(string email = "admin@site.com", string password = "some password#2")
        {
            var entity = GetFactory<ContractorUserFactory>().Create(new
            {
                Email = email,
                Password = password
            });

            var model =
                new ContractorUserLogOn(_container
                       .GetInstance<IAuthenticationRepository<ContractorUser>
                        >())
                    {Email = email, Password = password};
            return new Tuple<ContractorUser,ContractorUserLogOn>(entity, model);
        }

        #endregion

        #region Authentication

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.AllowsAnonymousAccess("~/Authentication/LogOn");
                a.AllowsAnonymousAccess("~/Authentication/LogOff");
            });

            // The general authorization testing doesn't work well when we have two actions with the
            // same name but with different http verbs. By default it picks whichever action has a single
            // parameter. 
            MyAssert.MethodHasAttribute<AllowAnonymousAttribute>(_target, "LogOn", typeof(ContractorUserLogOn));
        }

        #endregion

        #region LogOn

        [TestMethod]
        public void TestLogOnWithParametersIsHttpPostOnly()
        {
            MyAssert.MethodHasAttribute<HttpPostAttribute>(_target, "LogOn", typeof (ContractorUserLogOn));
        }

        [TestMethod]
        public void LogOn_Get_ReturnsViewWithModelAndNullProperties()
        {
            SetIsAjaxRequest(false);
            // Act
            var result = (ViewResult)_target.LogOn();
            var model = (ContractorUserLogOn)result.Model;

            // Assert
            MvcAssert.IsViewNamed(result, "LogOn");
            Assert.IsNull(model.Email);
            Assert.IsNull(model.Password);
            Assert.IsNull(model.ReturnUrl);
        }

        [TestMethod]
        public void TestLogOnGetSetsReturnUrlOnModelFromTempData()
        {
            // Arrange
            SetIsAjaxRequest(false);
            var expectedUrl = "some url";
            _target.TempData[ControllerExtensions.TempDataKeys.REDIRECT_URL] = expectedUrl;

            // Act
            var result = (ViewResult)_target.LogOn();
            var model = (ContractorUserLogOn)result.Model;

            // Assert
            MvcAssert.IsViewNamed(result, "LogOn");
            Assert.AreEqual(expectedUrl, model.ReturnUrl);
        }

        [TestMethod]
        public void TestLogOnGetReturnsNotFoundIfAjaxRequest()
        {
            SetIsAjaxRequest(true); 
            MvcAssert.IsStatusCode(404, _target.LogOn());
        }

        [TestMethod]
        public void LogOn_Post_ReturnsRedirectOnSuccess_WithoutReturnUrl()
        {
            // Arrange
            var user = CreateUserTuple();

            // Act
            var result = _target.LogOn(user.Item2);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            _authenticationService.Verify(x => x.SignIn(user.Item1.Id, false));
        }

        [TestMethod]
        public void LogOn_Post_ReturnsRedirectOnSuccess_WithLocalReturnUrl()
        {
            // Arrange
            var user = CreateUserTuple();
            user.Item2.ReturnUrl = "/someUrl";
            // Act
            var result = _target.LogOn(user.Item2);
            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            var redirectResult = (RedirectResult)result;
            Assert.AreEqual("/someUrl", redirectResult.Url);
            _authenticationService.Verify(x => x.SignIn(user.Item1.Id, false));
        }

        [TestMethod]
        public void LogOn_Post_SuccessRedirectsToHomeIfReturnUrlIsExternal()
        {
            // Arrange
            var user = CreateUserTuple();
            user.Item2.ReturnUrl = "http://malicious.example.net";
            SetIsAjaxRequest(false);
            // Act
            var result = _target.LogOn(user.Item2);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            _authenticationService.Verify(x => x.SignIn(user.Item1.Id, false));
        }

        [TestMethod]
        public void LogOn_Post_ReturnsViewIfValidateUserFails()
        {
            var user = _container.GetInstance<ContractorUserLogOn>();
            user.Email = "yeah";
            user.Password = "yeah";

            _target.RunModelValidation(user);
            var result = _target.LogOn(user);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void LogOn_Post_SetsCorrectErrorMessageForBadPassword()
        {
            // Arrange
            var user = CreateUserTuple();
            user.Item2.Password = "the wrong password";
            var expectedResult = UserLoginAttemptStatus.BadPassword;

            // Act
            _target.RunModelValidation(user.Item2);
            _target.LogOn(user.Item2);

            // Assert
            Assert.AreEqual("User does not exist or password is incorrect.", _target.ModelState["GenericLoginError"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void LogOn_Post_SetsCorrectErrorMessageForUnknownUser()
        {
            // Arrange
            // Don't use the factory here since this is supposed to be an unknown user.
            var model = _container.GetInstance<ContractorUserLogOn>();
            model.Email = "admin@site.com";
            model.Password = "goodPassword";
            var expectedResult = UserLoginAttemptStatus.UnknownUser;

            // Act
            _target.RunModelValidation(model);
            _target.LogOn(model);

            // Assert
            Assert.AreEqual("User does not exist or password is incorrect.", _target.ModelState["GenericLoginError"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void LogOn_Post_SetsCorrectErrorMessageForInvalidEmail()
        {
            // Arrange
            // Don't use the factory here since it'd throw an invalid email exception.
            var model = _viewModelFactory.BuildWithOverrides<ContractorUserLogOn>(new {
                Email = "admin23rf2233333333333333site.com",
                Password = "goodPassword"
            });
            var expectedResult = UserLoginAttemptStatus.InvalidEmail;

            // Act
            _target.RunModelValidation(model);
            _target.LogOn(model);

            // Assert
            Assert.AreEqual("The Email field must have a valid email address.", _target.ModelState["Email"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public void TestLogOnPostRedirectsToChangePasswordPageIfPasswordDoesNotMeetRequirements()
        {
            // Arrange
            var user = CreateUserTuple(password: "badpass");
            user.Item2.ReturnUrl = "/someUrl";
            // Act
            _target.RunModelValidation(user.Item2);
            var result = _target.LogOn(user.Item2);
            // Assert
            var redirectResult = (RedirectToRouteResult)result;
            MvcAssert.RedirectsToRoute(redirectResult, "User", "ChangePasswordPost", new { id = user.Item1.Id });
        }

        #endregion

        #region LogOff

        [TestMethod]
        public void TestLogOffDisablesOutputCache()
        {
            const string fail = "LogOff is an HttpGet, but has an effect on the client side's cookie. DO NOT CACHE or else the user won't be able to log off.";
            var meth = MyAssert.MethodHasAttribute<OutputCacheAttribute>(_target, "LogOff", Type.EmptyTypes);
            var attr = meth.GetCustomAttributes(true).OfType<OutputCacheAttribute>().Single();
            Assert.AreEqual(0, attr.Duration, fail);
            Assert.AreEqual(OutputCacheLocation.None, attr.Location, fail);
            Assert.IsTrue(attr.NoStore, fail);
        }

        [TestMethod]
        public void LogOff_LogsOutAndRedirectsToLoginScreen()
        {
            // Arrange
            _authenticationService.Setup(s => s.SignOut());

            // Act
            ActionResult result = _target.LogOff();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Authentication", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("LogOn", redirectResult.RouteValues["action"]);
        }

        #endregion

    }
}
