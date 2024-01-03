using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Web.Mvc;
using System.Web.Routing;

// ReSharper disable Mvc.ActionNotResolved, Mvc.ControllerNotResolved, Mvc.AreaNotResolved

namespace MMSINC.Core.MvcTest.Utilities
{
    [TestClass]
    public class RouteContextTest
    {
        #region Fields

        private RouteContext _target;
        private FakeCrudController _fakeController;
        private FakeMvcApplicationTester _application;
        private FakeMvcHttpHandler _request;
        private FakeAuthorizeAttribute _authFilter;
        private Mock<IAuthenticationService<IAdministratedUser>> _authServ;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _authServ = new Mock<IAuthenticationService<IAdministratedUser>>();
            _container.Inject<IAuthenticationService>(_authServ.Object);

            _application = new FakeMvcApplicationTester(_container);
            _authFilter = new FakeAuthorizeAttribute(true);
            _application.Filters.GlobalFilters.Add(_authFilter);
            _fakeController = new FakeCrudController();
            _application.ControllerFactory.ActLikeDefaultControllerFactory = true;
            _application.ControllerFactory.RegisterControllerForNamespace("valid namespace", "Default",
                _fakeController);

            _request = _application.CreateRequestHandler("~/Default/Index");
            _request.RouteData.DataTokens["namespaces"] = new[] {"valid namespace"};

            _target = new RouteContext(_request.RequestContext);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _application.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorFailsIfRouteDataIsMissingControllerKeyValue()
        {
            _request.RouteData.Values.Remove("controller");
            MyAssert.Throws<InvalidOperationException>(() => new RouteContext(_request.RequestContext),
                "The RouteData must contain an item named 'Controller' with a non-empty string value.");
        }

        [TestMethod]
        public void TestConstructorFailsIfRouteDataIsMissingActionKeyValue()
        {
            _request.RouteData.Values.Remove("action");
            MyAssert.Throws<InvalidOperationException>(() => new RouteContext(_request.RequestContext),
                "The RouteData must contain an item named 'Action' with a non-empty string value.");
        }

        [TestMethod]
        public void TestConstructorAlwaysCreatesANewControllerContext()
        {
            var first = new RouteContext(_request.RequestContext);
            var second = new RouteContext(_request.RequestContext);

            Assert.IsNotNull(first.ControllerContext);
            Assert.IsNotNull(second.ControllerContext);
            Assert.AreNotSame(first.ControllerContext, second.ControllerContext);
        }

        [TestMethod]
        public void TestConstructorSetsRequestContextPropertyToRequestContextParameter()
        {
            Assert.AreSame(_request.RequestContext, _target.RequestContext);
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionIfRequestContextParameterIsNull()
        {
            MyAssert.Throws(() => new RouteContext((RequestContext)null));
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionWhenControllerIsMissing()
        {
            _request.RouteData.Values["Controller"] = "Somethingelse";
            MyAssert.Throws<InvalidOperationException>(() => new RouteContext(_request.RequestContext));
        }

        [TestMethod]
        public void TestConstructorSetsControllerDescriptorCorrectlyIfItNeedsToTrimTheControllerName()
        {
            _request.RouteData.Values["Controller"] = "    Default ";
            var wootles = new RouteContext(_request.RequestContext);
            Assert.IsNotNull(wootles.ControllerDescriptor);
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionWhenActionDescriptorIsNotFound()
        {
            _request.RouteData.Values["Action"] = "i'm not here";
            MyAssert.Throws(() => new RouteContext(_request.RequestContext));
        }

        [TestMethod]
        public void TestConstructorWithControllerContextParameterUsesControllerContextsRequestContext()
        {
            var controller = new FakeController();
            var controllerContext = _request.CreateControllerContext(controller);
            var target = new RouteContext(controllerContext);
            Assert.AreSame(_request.RequestContext, target.RequestContext);
        }

        [TestMethod]
        public void TestConstructorWithControllerContextParameterUsesControllerContextsRouteData()
        {
            var controller = new FakeController();
            var controllerContext = _request.CreateControllerContext(controller);
            var target = new RouteContext(controllerContext);
            Assert.AreSame(_request.RequestContext.RouteData, target.RouteData);
        }

        [TestMethod]
        public void TestConstructorCanFindControllerWhenOriginalRequestComesWithRouteThatDoesNotAllowForItsNamespace()
        {
            var badNamespaces = new[] {"bad namespace"};
            _request.RouteData.DataTokens["namespaces"] = badNamespaces;
            foreach (Route route in _request.RouteCollection)
            {
                route.DataTokens.Add("namespaces", new string[] {"valid namespace"});
            }

            var badController = new FakeController();
            _application.ControllerFactory.RegisterControllerForNamespace("bad namespace", "bad", badController);
            _application.ControllerFactory.RegisterControllerForNamespace("valid namespace", "FakeCrud",
                _fakeController);

            var result = new RouteContext(_request.RequestContext, "FakeCrud", "Index", null);
            Assert.AreEqual("FakeCrud", result.RouteControllerName);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreSame(_fakeController, result.ControllerContext.Controller);
        }

        [TestMethod]
        public void TestConstructorCanDealWithApplicationsRunningInAVirtualDirectory()
        {
            _application.ControllerFactory.RegisterController("FakeCrud", _fakeController);
            _request = _application.CreateRequestHandler("~/", hostName: "www.somecoolsite.com",
                appPath: "/subdirectory/", urlProtocol: "http");

            var result = new RouteContext(_request.RequestContext, "FakeCrud", "Index", null);
            Assert.AreEqual(result.RouteControllerName, "FakeCrud");
            Assert.AreEqual(result.ActionName, "Index");
            Assert.AreSame(result.ControllerContext.Controller, _fakeController);
        }

        [TestMethod]
        public void TestConstructorCanDealWithApplicationsThatRequireHttps()
        {
            _application.ControllerFactory.RegisterController("FakeCrud", _fakeController);
            _application.Filters.GlobalFilters.Add(new RequireHttpsAttribute());
            _request = _application.CreateRequestHandler("~/", hostName: "www.somecoolsite.com",
                appPath: "/subdirectory/", urlProtocol: "http");
            var result = new RouteContext(_request.RequestContext, "FakeCrud", "Index", null);
            MyAssert.DoesNotThrow(() => result.IsAuthorized());
        }

        [TestMethod]
        public void TestConstructorThatOnlyTakesRequestContextReturnsTheRequestContextAndRouteDataPassedToIt()
        {
            var expectedRequestContext = _request.RequestContext;
            var expectedRouteData = _request.RequestContext.RouteData;
            var result = new RouteContext(expectedRequestContext);
            Assert.AreSame(expectedRequestContext, result.RequestContext);
            Assert.AreSame(expectedRouteData, result.RouteData);
        }

        [TestMethod]
        public void
            TestConstructorThatTakesARequestContextAndControllerAndActionNamesCreatesANewRequestContextAndRouteData()
        {
            var unexpectedRequestContext = _request.RequestContext;
            var unexpectedRouteData = _request.RequestContext.RouteData;

            _application.ControllerFactory.RegisterController("FakeCrud", _fakeController);
            var result = new RouteContext(_request.RequestContext, "FakeCrud", "Index", null);

            Assert.AreNotSame(unexpectedRequestContext, result.RequestContext);
            Assert.AreNotSame(unexpectedRouteData, result.RouteData);
            Assert.AreNotSame(unexpectedRequestContext.HttpContext.Request, result.RequestContext.HttpContext.Request);
        }

        [TestMethod]
        public void TestContructorKnowsHowToGenerateUrlsWhenCustomRoutesAreInvolvedAndUrlsMightIncludeQueryStrings()
        {
            _application.ControllerFactory.RegisterController("FakeCrud", _fakeController);
            _application.Routes.MapRoute(name: "SomeRouteName",
                url: "{controller}/Show/{id}/{action}",
                defaults: new {action = "Index"});

            // Index is acting as a child action for the DefaultShowChild route.
            // This test is to ensure that the action name doesn't get parsed as "Index?Query=String" 
            // because that is wrong and I just spent two hours tracking it down.
            var result = new RouteContext(_request.RequestContext, "/FakeCrud/Show/42/Index?Query=String");
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("FakeCrud", result.ControllerName);
        }

        [TestMethod]
        public void TestConstructorKnowsHowToGenerateUrlWhenAreasAreInvolved()
        {
            _application.RegisterArea("SomeArea", (context) => {
                context.MapRoute(
                    name: "SomeArea_Default",
                    url: "SomeArea/{controller}/{action}",
                    defaults: new {area = "SomeArea", action = "Index"},
                    namespaces: new[] {"SomeAreaNamespace"});
            });
            var expectedController = new FakeCrudController();
            _application.ControllerFactory.RegisterControllerForNamespace("SomeAreaNamespace", "AnAreaController",
                expectedController);
            var result = new RouteContext(_request.RequestContext, "AnArea", "Index", "SomeArea");
            Assert.AreEqual("/SomeArea/AnArea", result.RequestContext.HttpContext.Request.RawUrl);
            Assert.AreSame(expectedController, result.ControllerContext.Controller);

            result = new RouteContext(_request.RequestContext, "AnArea", "Show", "SomeArea");
            Assert.AreEqual("/SomeArea/AnArea/Show", result.RequestContext.HttpContext.Request.RawUrl);
            Assert.AreSame(expectedController, result.ControllerContext.Controller);
        }

        [TestMethod]
        public void
            TestConstructorUsesCurrentRequestsAreaWhenGeneratingANewRequestContextWhenTheAreaNameParameterIsNULL()
        {
            _application.RegisterArea("SomeArea", (context) => {
                context.MapRoute(
                    name: "SomeArea_Default",
                    url: "SomeArea/{controller}/{action}",
                    defaults: new {area = "SomeArea", action = "Index"},
                    namespaces: new[] {"SomeAreaNamespace"});
            });
            var expectedController = new FakeCrudController();
            _application.ControllerFactory.RegisterControllerForNamespace("SomeAreaNamespace", "AnAreaController",
                expectedController);
            _request.RouteData.DataTokens["area"] = "SomeArea";

            var result = new RouteContext(_request.RequestContext, "AnArea", "Index", null);
            Assert.AreEqual("/SomeArea/AnArea", result.RequestContext.HttpContext.Request.RawUrl);
            Assert.AreSame(expectedController, result.ControllerContext.Controller);

            result = new RouteContext(_request.RequestContext, "AnArea", "Show", null);
            Assert.AreEqual("/SomeArea/AnArea/Show", result.RequestContext.HttpContext.Request.RawUrl);
            Assert.AreSame(expectedController, result.ControllerContext.Controller);
        }

        [TestMethod]
        public void TestConstructorSetsAreaNamePropertyBasedOnRouteData()
        {
            _application.RegisterArea("SomeArea", (context) => {
                context.MapRoute(
                    name: "SomeArea_Default",
                    url: "SomeArea/{controller}/{action}",
                    defaults: new {area = "SomeArea", action = "Index"},
                    namespaces: new[] {"SomeAreaNamespace"});
            });

            var expectedController = new FakeCrudController();
            _application.ControllerFactory.RegisterControllerForNamespace("SomeAreaNamespace", "AnAreaController",
                expectedController);
            var request = _application.CreateRequestHandler("~/SomeArea/AnArea/Show/");

            var result = new RouteContext(request.RequestContext);
            Assert.AreEqual("SomeArea", result.AreaName);
            Assert.AreEqual("SomeArea", result.RouteData.DataTokens["area"]);
        }

        [TestMethod]
        public void TestConstructorThatAcceptsControllerContextSetsItAndItsRequestContextAsProperties()
        {
            var request = _application.CreateRequestHandler("~/FakeCrud/Index/");
            var controller = request.CreateAndInitializeController<FakeCrudController>();
            var expectedControllerContext = controller.ControllerContext;
            var expectedRequestContext = expectedControllerContext.RequestContext;
            var expectedRouteData = expectedRequestContext.RouteData;

            var result = new RouteContext(controller.ControllerContext);
            // None of these properties should end up being overwritten on the original object.
            Assert.AreSame(expectedControllerContext, result.ControllerContext);
            Assert.AreSame(expectedRequestContext, result.RequestContext);
            Assert.AreSame(expectedRouteData, result.RouteData);
            Assert.AreEqual("FakeCrud", result.RouteControllerName);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void TestControllerDescriptorReturnsDescriptorThatMatchesControllersType()
        {
            Assert.AreSame(typeof(FakeCrudController), _target.ControllerDescriptor.ControllerType);
        }

        [TestMethod]
        public void TestControllerNameReturnsNameFromControllerDescriptor()
        {
            Assert.AreEqual("FakeCrud", _target.ControllerDescriptor.ControllerName);
        }

        [TestMethod]
        public void TestRouteDataPropertyReturnsRequestContextsRouteData()
        {
            Assert.AreSame(_request.RequestContext.RouteData, _target.RouteData);
        }

        [TestMethod]
        public void
            TestRouteDataPropertyIsNotTheSameAsRequestContextWhenUsingConstructorWithControllerAndActionNameParameters()
        {
            _application.ControllerFactory.RegisterController("FakeCrud", _fakeController);
            var target = new RouteContext(_request.RequestContext, "FakeCrud", "Show", null);
            Assert.AreNotSame(_request.RouteData, target.RouteData);
            Assert.IsNotNull(target.RouteData);
        }

        [TestMethod]
        public void TestActionDescriptorMatchesActionFromRouteData()
        {
            var expected = _target.ControllerDescriptor.FindAction(_target.ControllerContext, "Index");

            Assert.AreEqual(expected.ActionName, _target.ActionDescriptor.ActionName);
            Assert.AreSame(expected.ControllerDescriptor, _target.ActionDescriptor.ControllerDescriptor);
        }

        [TestMethod]
        public void TestActionDescriptorIsFoundWhenTheActionNameNeedsTrimming()
        {
            _request.RouteData.Values["Action"] = "   Show   ";
            _request.RouteData.Values["id"] = 432;
            var rc = new RouteContext(_request.RequestContext);
            Assert.IsNotNull(rc.ControllerDescriptor.FindAction(rc.ControllerContext, "Show"));
            Assert.IsNotNull(rc.ActionDescriptor);
            Assert.AreEqual("Show", rc.ActionName);
        }

        [TestMethod]
        public void TestActionDescriptorIsFoundRegardlessOfRequestsHttpVerb()
        {
            _request = _application.CreateRequestHandler("~/Crud/Show/432", httpMethod: "POST");
            var rc = new RouteContext(_request.RequestContext);
            Assert.IsNull(rc.ControllerDescriptor.FindAction(rc.ControllerContext, "Show"),
                "Normally this would end up being null cause of different verbs.");
            Assert.IsNotNull(rc.ActionDescriptor);
            Assert.AreEqual("Show", rc.ActionName);
        }

        [TestMethod]
        public void
            TestActionDescriptorIsFoundRegardlessOfRequestsHttpVerbWhenANewHttpRequestIsCreatedInTheConstructor()
        {
            _application.ControllerFactory.RegisterController("FakeCrud", _fakeController);
            _request = _application.CreateRequestHandler("~/Crud/Show/432", httpMethod: "POST");
            var rc = new RouteContext(_request.RequestContext, "FakeCrud", "Show", "");
            Assert.AreNotSame(_request.Request.Object, rc.RequestContext.HttpContext.Request,
                "This test isn't setup correctly");

#pragma warning disable 219
            object dontThrow = null;
            MyAssert.DoesNotThrow(() => dontThrow = rc.RequestContext.HttpContext.Request.Form);
            MyAssert.DoesNotThrow(() => dontThrow = rc.RequestContext.HttpContext.Request.Headers);
            MyAssert.DoesNotThrow(() => dontThrow = rc.RequestContext.HttpContext.Request.QueryString);
#pragma warning restore 219

            Assert.IsNull(rc.ControllerDescriptor.FindAction(rc.ControllerContext, "Show"),
                "Normally this would end up being null cause of different verbs.");
            Assert.IsNotNull(rc.ActionDescriptor);
            Assert.AreEqual("Show", rc.ActionName);
        }

        [TestMethod]
        public void TestInternalHttpContextReturnsSameUserAsPassedInHttpContext()
        {
            var expectedController = new FakeController();
            _application.ControllerFactory.RegisterController("Default", expectedController);
            _target = new RouteContext(_request.RequestContext, "Default", "AdminAction", null);
            Assert.AreNotSame(_request.HttpContext.Object, _target.RequestContext.HttpContext,
                "A new HttpContext should have been created");
            Assert.AreSame(_request.User.Object, _target.RequestContext.HttpContext.User);
        }

        [TestMethod]
        public void TestInternalHttpContextReturnsNewResponseObject()
        {
            var expectedController = new FakeController();
            _application.ControllerFactory.RegisterController("Default", expectedController);
            _target = new RouteContext(_request.RequestContext, "Default", "AdminAction", null);
            Assert.AreNotSame(_request.Response.Object, _target.RequestContext.HttpContext.Response);
        }

        #region IsAuthorized

        [TestMethod]
        public void TestIsAuthorizedReturnsValueBasedOnAuthorizationFiltersAndTheCurrentRoute()
        {
            foreach (var outcome in new[] {true, false})
            {
                _authFilter.IsAuthorized = outcome;
                Assert.AreEqual(outcome, _target.IsAuthorized());
            }
        }

        [TestMethod]
        public void TestIsAuthorizedWorksWhenUsingInternalHttpContext()
        {
            // This test is here because LogonAuthorizeAttribute(or something it uses)
            // uses the HttpContext. There are a lot of unimplemented properties on
            // InternalHttpContext so we wanna ensure that calling IsAuthorized does not
            // throw an exception.
            var authFilter = _container.GetInstance<MvcAuthorizationFilter>();
            GlobalFilters.Filters.Add(authFilter);

            var expectedController = new FakeController();
            _application.ControllerFactory.RegisterController("Default", expectedController);
            _target = new RouteContext(_request.RequestContext, "Default", "AdminAction", null);

            // ObjectWriter.WriteAllProperties(_pipeline.Request.Object, "Original request");
            //  ObjectWriter.WriteAllProperties(_target.RequestContext.HttpContext.Request, "New request");
            _target.IsAuthorized();
            MyAssert.DoesNotThrow(() => _target.IsAuthorized());
        }

        #endregion

        #endregion

        #region Helper class

        private class FakeController : ControllerBase
        {
            protected override void ExecuteCore()
            {
                throw new NotImplementedException("This shouldn't matter in RouteContext tests.");
            }

            public ActionResult Index()
            {
                throw new NotImplementedException("This shouldn't matter in RouteContext tests.");
            }

            [RequiresAdmin]
            public ActionResult AdminAction()
            {
                throw new NotImplementedException("This shouldn't matter in RouteContext tests.");
            }
        }

        #endregion
    }
}
