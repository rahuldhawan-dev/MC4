using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.MSTest.TestExtensions;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class UserControllerTest : ContractorControllerTestBase<UserController, ContractorUser, ContractorUserRepository>
    {
        #region Fields

        private string _currentUserPassword;
        private Contractor _currentUserContractor;

        #endregion

        #region Setup/Teardown

        protected override ContractorUser CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _currentUserPassword = "the password";
            _currentUserContractor = _currentUser.Contractor;
            _container.Inject(Session);
            SetupAuthenticationService(_currentUser.Id + "; " + _currentUser.Email);
        }

        protected void SetupAuthenticationService(string identityId)
        {
            var identity = new GenericIdentity(identityId);
            var principal = new GenericPrincipal(identity, null);
            _container.Inject<IAuthenticationRepository<ContractorUser>>(
                _container.GetInstance<AuthenticationRepository>());
            _container.Inject<IAuthenticationCookieFactory>(
                _container.GetInstance<AuthenticationCookieFactory>());
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            // NOTE: ChangePasswod and ChangePasswordQA do not get tested here
            // for the HttpPost overrides because this authorization thing wasn't
            // setup with a way to handle actions with the same name and different http params.
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/User/Show");
                a.RequiresLoggedInUserOnly("~/User/ChangePassword");
                a.RequiresLoggedInUserOnly("~/User/ChangePasswordQA");
                a.RequiresLoggedInUserOnly("~/User/ChangePasswordPost");
                a.RequiresLoggedInUserOnly("~/User/ChangePasswordQAPost");
            });
        }

        #endregion

        #region ChangePassword

        [TestMethod]
        public void TestChangePasswordWithParametersIsHttpPostOnly()
        {
            MyAssert
               .MethodHasAttribute<HttpPostAttribute>(_target,
                    "ChangePasswordPost",
                    new[] {typeof(ChangePasswordContractorUser)});
        }

        [TestMethod]
        public void TestChangePasswordReturnsViewWithChangePasswordContractorUserModelForCurrentUser()
        {
            var someUser = GetFactory<ContractorUserFactory>().Create(new {
                Email = "oh@hey.com", Contractor = _currentUserContractor
            });
            _authenticationService.Setup(x => x.CurrentUser).Returns(someUser);
            var result = (ViewResult)_target.ChangePassword();

            Assert.IsTrue(result.Model is ChangePasswordContractorUser);
            Assert.AreEqual(someUser.Email, ((ChangePasswordContractorUser) result.Model).Email);
        }

        [TestMethod]
        public void TestChangePasswordReturnsViewWithChangePasswordContractorUserModelForFailures()
        {
            var currentPassword = "this is the current password";
            var someUser = GetFactory<ContractorUserFactory>().Create(new {
                Email = "oh@hey.com",
                Contractor = _currentUserContractor,
                Password = currentPassword
            });
            _authenticationService.Setup(x => x.CurrentUser).Returns(someUser);
            var model = _viewModelFactory.BuildWithOverrides<ChangePasswordContractorUser, ContractorUser>(someUser, new {
                CurrentPassword = currentPassword
            });

            _target.ModelState.AddModelError("", "some error");
            Assert.IsFalse(_target.ModelState.IsValid);

            _authenticationService.Setup(x => x.ValidateUser(someUser.Email, currentPassword)).Returns(UserLoginAttemptStatus.Success);
            var result = (ViewResult) _target.ChangePasswordPost(model);

            Assert.AreSame(model, result.Model);
        }

        [TestMethod]
        public void TestChangePasswordRedirectsToShowViewOnSuccessfulPasswordChange()
        {
            var currentPassword = "current password";
            var someUser = GetFactory<ContractorUserFactory>().Create(new {
                Email = "oh@hey.com",
                Contractor = _currentUserContractor,
                Password = currentPassword
            });
            _authenticationService.Setup(x => x.CurrentUser).Returns(someUser);
            var model = _viewModelFactory.BuildWithOverrides<ChangePasswordContractorUser, ContractorUser>(someUser, new {
                CurrentPassword = currentPassword, NewPassword = "some new password"
            });

            _authenticationService.Setup(x => x.ValidateUser(someUser.Email, currentPassword)).Returns(UserLoginAttemptStatus.Success);
            var result = (RedirectToRouteResult)_target.ChangePasswordPost(model);

            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestChangePasswordSetsUserIdInViewDataWhenPostbackFails()
        {
            _target.ModelState.AddModelError("", "error");

            var result = (ViewResult)_target.ChangePassword();
            Assert.AreEqual(_currentUser.Id,
                ((ChangePasswordContractorUser)result.Model)
               .Id);
        }

        [TestMethod]
        public void TestChangePasswordSetsSuccessMessageToTempDataOnSuccessfulPasswordChange()
        {
            var currentPassword = "this is the current password";
            var someUser = GetFactory<ContractorUserFactory>().Create(new {
                Email = "oh@hey.com",
                Contractor = _currentUserContractor,
                Password = currentPassword
            });
            _authenticationService.Setup(x => x.CurrentUser).Returns(someUser);
            var model = _viewModelFactory.BuildWithOverrides<ChangePasswordContractorUser, ContractorUser>(someUser, new {
                CurrentPassword = currentPassword, NewPassword = "the new password"
            });

            _authenticationService.Setup(x => x.ValidateUser(someUser.Email, currentPassword)).Returns(UserLoginAttemptStatus.Success);
            _target.ChangePasswordPost(model);

            Assert.AreEqual(UserController.Messages.PASSWORD_SUCCESSFULLY_CHANGED, ((List<string>)_target.TempData[MMSINC.Controllers.ControllerBase.SUCCESS_MESSAGE_KEY]).Single());
        }
        
        #endregion

        #region ChangePasswordQA

        [TestMethod]
        public void TestChangePasswordQAWithParametersIsHttpPostOnly()
        {
            MyAssert
                .MethodHasAttribute<HttpPostAttribute>(_target, "ChangePasswordQAPost",
                                                       new[] {typeof (ChangePasswordQuestionAndAnswerContractorUser)});
        }

        [TestMethod]
        public void TestChangePasswordQAReturnsViewWithChangePasswordQuestionAndAnswerContractorUserModelForCurrentUser()
        {
            var someUser = GetFactory<ContractorUserFactory>().Create(new {
                Email = "oh@hey.com", Contractor = _currentUserContractor
            });

            Console.WriteLine("Ok: " + someUser.GetHashCode());
            _authenticationService.Setup(x => x.CurrentUser).Returns(someUser);

            var result = (ViewResult)_target.ChangePasswordQA();
            var resultModel = (ChangePasswordQuestionAndAnswerContractorUser)result.Model;
            Assert.AreEqual(someUser.Email, resultModel.Email);
        }

        [TestMethod]
        public void TestChangePasswordQAReturnsViewWithChangePasswordContractorUserModelForFailures()
        {
            var model =
                _viewModelFactory
                   .BuildWithOverrides<
                        ChangePasswordQuestionAndAnswerContractorUser,
                        ContractorUser>(_currentUser, new {
                        Password = _currentUserPassword
                    });
            _target.ModelState.AddModelError("", "some error");
            Assert.IsFalse(_target.ModelState.IsValid);

            _authenticationService.Setup(x => x.ValidateUser(_currentUser.Email, _currentUserPassword)).Returns(UserLoginAttemptStatus.Success);
            var result = (ViewResult)_target.ChangePasswordQAPost(model);

            Assert.AreSame(model, result.Model);
        }

        [TestMethod]
        public void TestChangePasswordQARedirectsToShowViewOnSuccessfulPasswordChange()
        {
            var model =
                _viewModelFactory
                   .BuildWithOverrides<
                        ChangePasswordQuestionAndAnswerContractorUser,
                        ContractorUser>(_currentUser, new {
                        PasswordQuestion = "new question",
                        PasswordAnswer = "new answer",
                        Password = _currentUserPassword
                    });

            _authenticationService.Setup(x => x.ValidateUser(_currentUser.Email, _currentUserPassword))
                .Returns(UserLoginAttemptStatus.Success);

            var result = (RedirectToRouteResult)_target.
                ChangePasswordQAPost(model);

            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestChangePasswordQASetsCurrentUserIdInViewData()
        {
            var someUser = GetFactory<ContractorUserFactory>().Create(new {
                Email = "oh@hey.com", Contractor = _currentUserContractor
            });

            _authenticationService.Setup(x => x.CurrentUser).Returns(someUser);

            var result = (ViewResult)_target.ChangePasswordQA();
            Assert.AreEqual(someUser.Id,
                ((ChangePasswordQuestionAndAnswerContractorUser)result.Model)
               .Id);
        }

        [TestMethod]
        public void TestChangePasswordQASetsUserIdInViewDataWhenPostbackFails()
        {
            var someUser = GetFactory<ContractorUserFactory>().Create(new {
                Email = "oh@hey.com", Contractor = _currentUserContractor
            });
            _authenticationService.Setup(x => x.CurrentUser).Returns(someUser);
            _target.ModelState.AddModelError("", "error");

            var result = (ViewResult)_target.ChangePasswordQA();
            Assert.AreEqual(someUser.Id,
                ((ChangePasswordQuestionAndAnswerContractorUser)result.Model)
               .Id);
        }

        [TestMethod]
        public void TestChangePasswordQASetsSuccessMessageOnSuccessfulPasswordChange()
        {
            var model =
                _viewModelFactory
                   .BuildWithOverrides<
                        ChangePasswordQuestionAndAnswerContractorUser,
                        ContractorUser>(_currentUser, new {
                        Password = _currentUserPassword,
                        PasswordQuestion = "this is the new question",
                        PasswordAnswer = "this is the new answer"
                    });

            _authenticationService.Setup(x => x.ValidateUser(_currentUser.Email, _currentUserPassword))
                .Returns(UserLoginAttemptStatus.Success);

            _target.ChangePasswordQAPost(model);

            Assert.AreEqual(UserController.Messages.PASSWORD_QA_SUCCESSFULLY_CHANGED, ((List<string>)_target.TempData[MMSINC.Controllers.ControllerBase.SUCCESS_MESSAGE_KEY]).Single());
        }

        #endregion

        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // noop override: Show returns the current user only.
        }

        [TestMethod]
        public override void TestShowReturnsNotFoundIfRecordCanNotBeFound()
        {
            // noop override: Show can't return a 404 because it only returns the current user.
        }

        [TestMethod]
        public void TestShowReturnsViewWithCurrentUserAsModel()
        {
            var user = new ContractorUser();
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);

            var result = (ViewResult)_target.Show();

            Assert.AreSame(user, result.Model);
        }

        #endregion
    }

    public class TestContractorUser : ContractorUser
    {
        public void SetContractorUserID(int id)
        {
            Id = id;
        }
    }
}
