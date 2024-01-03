using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities.StructureMap;
using Moq;
using StructureMap;

// ReSharper disable Mvc.ActionNotResolved, Mvc.ControllerNotResolved, Mvc.AreaNotResolved

namespace MMSINC.Core.MvcTest.Helpers
{
    [TestClass]
    public class SecureUrlHelperTest
    {
        #region Private Members

        private SecureUrlHelper _target;
        private FakeMvcApplicationTester _appTester;
        private FakeMvcHttpHandler _request;
        private HtmlHelper<TestUser> _htmlHelper;
        private Mock<IAuthenticationService<IAdministratedUser>> _authServ;
        private Mock<ITokenRepository<SecureFormToken, SecureFormDynamicValue>> _tokenRepo;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(i => {
                i.For<ISecureFormTokenService>().Use<SecureFormTokenService<SecureFormToken, SecureFormDynamicValue>>();
            });

            FormBuilder.SecureFormsEnabled = true;

            _appTester = new FakeMvcApplicationTester(_container);
            _appTester.ControllerFactory.RegisterController(new SecuredController());
            _appTester.ControllerFactory.RegisterController(new UnsecuredController());
            _request = _appTester.CreateRequestHandler();
            _htmlHelper = _request.CreateHtmlHelper<TestUser>();
            _tokenRepo = new Mock<ITokenRepository<SecureFormToken, SecureFormDynamicValue>>();
            _container.Inject(_tokenRepo.Object);
            _authServ = new Mock<IAuthenticationService<IAdministratedUser>>();
            _container.Inject(_authServ.Object);
            _authServ.Setup(x => x.CurrentUserId).Returns(12345);

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));

            _target = new SecureUrlHelper(_request.RequestContext);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _tokenRepo.VerifyAll();
        }

        #endregion

        #region Nested Type: Model

        private class Model
        {
            #region Properties

            [Secured]
            public int Secured { get; set; }

            [Required, Secured]
            public int RequiredSecured { get; set; }

            public int Unsecured { get; set; }

            #endregion
        }

        #endregion

        #region Nested Type: SecuredController

        private class SecuredController : Controller
        {
            #region Exposed Methods

            [RequiresSecureForm]
            public ActionResult Secured()
            {
                return null;
            }

            [RequiresSecureForm]
            public ActionResult SecuredWithModel(Model model)
            {
                return null;
            }

            #endregion
        }

        #endregion

        #region Nested Type: UnsecuredController

        private class UnsecuredController : Controller
        {
            #region Exposed Methods

            public ActionResult Unsecured(Model model)
            {
                return null;
            }

            #endregion
        }

        #endregion

        #region Private Methods

        protected void SetupForSecured(Action<SecureFormToken> callBack = null)
        {
            _tokenRepo
               .Setup(x => x.Save(It.IsAny<SecureFormToken>()))
               .Callback(callBack ?? (t => { }))
               .Returns<SecureFormToken>(x => x);
        }

        #endregion

        [TestMethod]
        public void TestActionReturnsSecuredUrl()
        {
            SetupForSecured();
            var result = _target.Action("Secured", "Secured", new RouteValueDictionary());

            Assert.AreEqual("/Secured/Secured?__SECUREFORM=00000000-0000-0000-0000-000000000000", result);
        }

        [TestMethod]
        public void TestActionReturnsUrlUnchangedIfActionDoesNotRequireSecureForm()
        {
            var result = _target.Action("Unsecured", "Unsecured", new RouteValueDictionary());

            Assert.AreEqual("/Unsecured/Unsecured", result);
        }

        [TestMethod]
        public void TestActionReturnsUrlUnchangedIfSecureFormsAreDisabled()
        {
            FormBuilder.SecureFormsEnabled = false;

            var result = _target.Action("Secured", "Secured", new RouteValueDictionary());

            Assert.AreEqual("/Secured/Secured", result);
        }

        [TestMethod]
        public void TestThrowsWhenValueForRequiredSecuredPropertyNotProvided()
        {
            MyAssert.Throws<InvalidOperationException>(() =>
                _target.Action("SecuredWithModel", "Secured", new RouteValueDictionary {
                    {"Secured", 1},
                    {"Unsecured", 3}
                }));
        }

        [TestMethod]
        public void TestTokenHasCorrectRouteValues()
        {
            SetupForSecured(t => {
                Assert.AreEqual("Secured", t.Controller);
                Assert.AreEqual("Secured", t.Action);
                Assert.AreEqual(12345, t.UserId);
            });
            _target.Action("Secured", "Secured", new RouteValueDictionary());
        }

        [TestMethod]
        public void TestTokenHasSecuredValues()
        {
            SetupForSecured(t => {
                Assert.IsTrue(t.DynamicValues.Any(v => v.Key == "Secured" &&
                                                       Convert.ToInt32(v.DeserializedValue) == 1));
                Assert.IsTrue(t.DynamicValues.Any(v => v.Key == "RequiredSecured" &&
                                                       Convert.ToInt32(v.DeserializedValue) == 2));
                Assert.IsFalse(t.DynamicValues.Any(v => v.Key == "Unsecured"));
            });
            _target.Action("SecuredWithModel", "Secured", new RouteValueDictionary {
                {"Secured", 1}, {"RequiredSecured", 2}, {"Unsecured", 3}
            });
        }
    }
}
