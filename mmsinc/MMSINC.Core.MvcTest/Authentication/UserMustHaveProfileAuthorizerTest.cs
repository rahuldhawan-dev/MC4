using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AuthorizeNet;
using AuthorizeNet.APICore;
using AuthorizeNet.Utility.NotProvided;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.Core.MvcTest.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using NHibernate;
using StructureMap;

namespace MMSINC.Core.MvcTest.Authentication
{
    [TestClass]
    public class UserMustHaveProfileAuthorizerTest
    {
        #region Private Members

        private UserMustHaveProfileAuthorizer _target;
        private Mock<IAuthenticationService<TestUser>> _mockAuthenticationService;
        private Mock<IDateTimeProvider> _mockDateTimeProvider;
        private Mock<IExtendedCustomerGateway> _mockCustomerGateway;
        private Mock<IRepository<IUserWithProfile>> _mockUserRepository;
        private MvcApplicationTester<FakeMvcApplication> _appTester;
        private IContainer _container;

        #endregion

        #region Init

        [TestInitialize]
        public void TestInitialize()
        {
            _mockCustomerGateway = new Mock<IExtendedCustomerGateway>();
            _mockUserRepository = new Mock<IRepository<IUserWithProfile>>();
            _mockAuthenticationService = new Mock<IAuthenticationService<TestUser>>();
            _container = new Container(i => {
                i.For<ISession>().Use(new Mock<ISession>().Object);
                i.For<IAuthenticationService<TestUser>>().Use(_mockAuthenticationService.Object);
                i.For<IAuthenticationService<IUserWithProfile>>().Use(_mockAuthenticationService.Object);
                i.For<IAuthenticationService>().Use(_mockAuthenticationService.Object);
                i.For<IDateTimeProvider>().Use((_mockDateTimeProvider = new Mock<IDateTimeProvider>()).Object);
                i.For<ICustomerGateway>().Use(_mockCustomerGateway.Object);
                i.For<IExtendedCustomerGateway>().Use(_mockCustomerGateway.Object);
                // i.For<IRepository<TestUser>>().Use(_mockUserRepository.Object);
                i.For<IRepository<IUserWithProfile>>().Use(_mockUserRepository.Object);
            });
            _target = _container.GetInstance<UserMustHaveProfileAuthorizer>();
            _appTester = new MvcApplicationTester<FakeMvcApplication>(_container);
            _appTester.ControllerFactory.RegisterController(new TestController());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _appTester.Dispose();
            _mockUserRepository.VerifyAll();
        }

        #endregion

        #region Tests - Authorize Core

        [TestMethod]
        public void TestAuthorizeIsNotAuthorizedWhenUserDoesNotHaveProfile()
        {
            var req = _appTester.CreateRequestHandler("~/Test/Action");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);

            var now = DateTime.Now;
            var customerProfileId = 123;
            var customer = new Customer {
                PaymentProfiles = new List<PaymentProfile>()
            };
            var userId = 666;

            _mockAuthenticationService
               .SetupGet(x => x.CurrentUser.Id)
               .Returns(userId);
            _mockDateTimeProvider
               .Setup(x => x.GetCurrentDate())
               .Returns(now);
            _mockAuthenticationService
               .SetupGet(x => x.CurrentUser.ProfileLastVerified)
               .Returns(now.AddDays(-1).AddSeconds(-1));

            _target.Authorize(args);

            var result = authContext.Result as ViewResult;

            MvcAssert.IsViewNamed(result, "~/Views/Error/Forbidden.cshtml");
            Assert.AreEqual(UserMustHaveProfileAuthorizer.MUST_HAVE_PROFILE, result.ViewData["Message"]);
        }

        [TestMethod]
        public void
            TestAuthorizeuthorizeIsNotAuthorizedWhenUserIsRegularUserHasNotHadProfileVerifiedRecentlyAndHasNoPaymentProfiles()
        {
            var req = _appTester.CreateRequestHandler("~/Test/Action");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);

            var now = DateTime.Now;
            var customerProfileId = 123;
            var customer = new Customer {
                PaymentProfiles = new List<PaymentProfile>()
            };
            var userId = 666;

            _mockAuthenticationService
               .SetupGet(x => x.CurrentUser.Id)
               .Returns(userId);
            _mockDateTimeProvider
               .Setup(x => x.GetCurrentDate())
               .Returns(now);
            _mockAuthenticationService
               .SetupGet(x => x.CurrentUser.ProfileLastVerified)
               .Returns(now.AddDays(-1).AddSeconds(-1));
            _mockAuthenticationService
               .SetupGet(x => x.CurrentUser.CustomerProfileId)
               .Returns(customerProfileId);
            _mockCustomerGateway
               .Setup(x => x.GetCustomer(customerProfileId.ToString()))
               .Returns(customer);

            _target.Authorize(args);
            var result = authContext.Result as ViewResult;

            MvcAssert.IsViewNamed(result, "~/Views/Error/Forbidden.cshtml");
        }

        [TestMethod]
        public void TestAuthorizeCoreUpdatesUserVerificationAndDoesNotSetResultIfUsersProfileIsValid()
        {
            var req = _appTester.CreateRequestHandler("~/Test/Action");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);

            var now = DateTime.Now;
            var user = new Mock<TestUser>();
            var customer = new Mock<Customer>();
            var profileId = 123;
            var profiles = new List<PaymentProfile> {
                new PaymentProfile(new customerPaymentProfileMaskedType()) {
                    CardNumber = "XXXX0002",
                    CardExpiration = "XX/XX"
                }
            };

            _mockDateTimeProvider
               .Setup(x => x.GetCurrentDate())
               .Returns(now);
            _mockAuthenticationService
               .SetupGet(x => x.CurrentUser)
               .Returns(user.Object);
            user
               .SetupGet(x => x.ProfileLastVerified)
               .Returns((DateTime?)null);
            user
               .SetupGet(x => x.CustomerProfileId)
               .Returns(profileId);
            _mockCustomerGateway
               .Setup(x => x.GetCustomer(profileId.ToString()))
               .Returns(customer.Object);
            customer
               .SetupGet(x => x.PaymentProfiles)
               .Returns(profiles);
            _mockAuthenticationService
               .SetupSet(x => x.CurrentUser.ProfileLastVerified = now);
            _mockUserRepository
               .Setup(x => x.Save(user.Object));

            _target.Authorize(args);
            Assert.IsNull(authContext.Result);
        }

        #endregion

        private class TestController : Controller
        {
            [UserMustHaveProfile]
            public ActionResult Action()
            {
                return null;
            }
        }

        public class TestUser : IUserWithProfile
        {
            public virtual int Id { get; set; }
            public virtual string UniqueName { get; set; }

            public virtual bool IsAdmin { get; set; }

            public virtual bool HasAccess { get; set; }

            public virtual string Email { get; set; }

            public virtual string Password { get; set; }

            public virtual Guid PasswordSalt { get; set; }

            public virtual int? CustomerProfileId { get; set; }
            public virtual DateTime? ProfileLastVerified { get; set; }
            public virtual DateTime? LastLoggedInAt { get; set; }
        }
    }
}
