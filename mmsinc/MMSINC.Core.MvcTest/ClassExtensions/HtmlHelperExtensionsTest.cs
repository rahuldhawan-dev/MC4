using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;
using MMSINC.Utilities.StructureMap;

// ReSharper disable Mvc.ActionNotResolved, Mvc.ControllerNotResolved, Mvc.AreaNotResolved

namespace MMSINC.Core.MvcTest.ClassExtensions
{
    [TestClass]
    public class HtmlHelperExtensionsTest
    {
        #region Fields

        private FakeMvcApplicationTester _application;
        private TestUser _model;
        private HtmlHelper _target;
        private ViewDataDictionary _viewData;
        private FakeCrudController _controller;
        private FakeMvcHttpHandler _request;

        private Mock<IAuthenticationService<TestUser>> _authServ;
        private Mock<TestUser> _user;
        private AuthorizationContext _authContext;
        private ActionExecutingContext _executingContext;
        private Mock<ITokenRepository<SecureFormToken, SecureFormDynamicValue>> _tokenRepository;
        private DateTime _nowDate;
        private IContainer _container;

        #endregion

        #region Setup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container(i => {
                i.For<ISecureFormTokenService>().Use<SecureFormTokenService<SecureFormToken, SecureFormDynamicValue>>();
            });

            _authServ = new Mock<IAuthenticationService<TestUser>>();
            _user = new Mock<TestUser>();
            _user.Setup(x => x.UniqueName).Returns("some user");
            _user.Setup(x => x.Id).Returns(42);
            _authServ.Setup(x => x.CurrentUser).Returns(_user.Object);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);
            _container.Inject(_authServ.Object);
            _tokenRepository = new Mock<ITokenRepository<SecureFormToken, SecureFormDynamicValue>>();
            _container.Inject(_tokenRepository.Object);

            _application = new FakeMvcApplicationTester(_container);
            _application.ModelFormatterProvider.Clear();
            _request = _application.CreateRequestHandler();
            _controller = _request.CreateAndInitializeController<FakeCrudController>();
            _viewData = _controller.ViewData;
            _model = new TestUser {Email = "foo@bar.com"};

            // Setting these manually instead of through the overload constructor
            // because otherwise you have to pass in a bunch of other stuff that it
            // does null ref checks for.

            _target = _request.CreateHtmlHelper<object>();
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _application.Dispose();
        }

        protected void MockMetadata<TObj, TProp>(TObj model, string propertyName)
        {
            var provider = new Mock<ModelMetadataProvider>();
            _viewData.ModelMetadata = new ModelMetadata(provider.Object, typeof(TObj),
                () => model.GetPropertyValueByName(propertyName), typeof(TProp), propertyName);
        }

        #endregion

        #region SecureActionLink

        [TestMethod]
        public void TestSecureActionLinkGeneratesActionLinkAndSavesToken()
        {
            var routeValues = new { };
            var htmlAttributes = new { };
            var guid = Guid.NewGuid();
            var expected = String.Format("<a href=\"/FakeCrud?__SECUREFORM={0}\">link text</a>", guid);
            var token = new SecureFormToken {
                Action = "Index", Controller = "FakeCrud", UserId = _user.Object.Id, CreatedAt = DateTime.Now,
                Token = guid
            };
            _tokenRepository.Setup(x => x.Save(It.IsAny<SecureFormToken>())).Returns(token);

            var result =
                _target.SecureActionLink<TestUser>("link text", "Index", "FakeCrud", routeValues, htmlAttributes);

            Assert.AreEqual(expected, result.ToString());
        }

        [TestMethod]
        public void TestSecureActionLinkGeneratesActionLinkAndAddsTokenToRouteValues()
        {
            var routeValues = new Dictionary<string, object> { };
            var htmlAttributes = new { };
            var guid = Guid.NewGuid();
            var token = new SecureFormToken {
                Action = "Index", Controller = "FakeCrud", UserId = _user.Object.Id, CreatedAt = DateTime.Now,
                Token = guid
            };
            _tokenRepository.Setup(x => x.Save(It.IsAny<SecureFormToken>())).Returns(token);

            var result =
                _target.SecureActionLink<TestUser>("link text", "Index", "FakeCrud", routeValues, htmlAttributes);

            Assert.AreEqual(routeValues[FormBuilder.SECURE_FORM_HIDDEN_FIELD_NAME], token.Token);
        }

        [TestMethod]
        public void TestSecureActionLinkGeneratesActionLinkAndAddsCorrectDynamicValues()
        {
            var routeValues = new Dictionary<string, object> {
                {"Foo", "Bar"},
                {"area", "blergh"}
            };
            var htmlAttributes = new { };
            var guid = Guid.NewGuid();
            var expected = String.Format("<a href=\"/FakeCrud?__SECUREFORM={0}\">link text</a>", guid);
            var token = new SecureFormToken {
                Action = "Index", Controller = "FakeCrud", UserId = _user.Object.Id, CreatedAt = DateTime.Now,
                Token = guid
            };
            _tokenRepository.Setup(x => x.Save(It.IsAny<SecureFormToken>()))
                            .Callback((SecureFormToken x) => { token = x; }).Returns(() => token);

            var result =
                _target.SecureActionLink<TestUser>("link text", "Index", "FakeCrud", routeValues, htmlAttributes);

            Assert.AreEqual(1, token.DynamicValues.Count);
            Assert.AreEqual("Bar", token.DynamicValues[0].DeserializedValue);
        }

        #endregion

        #region ActionRouteLink

        [TestMethod]
        public void TestActionRouteLinkUsesNamedRouteToGenerateUrlForLink()
        {
            _application.Routes.MapRoute(
                name: "SomeNamedRoute",
                url: "SomeJunk/{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: null
            );

            var result = _target.ActionRouteLink("link text", "SomeNamedRoute", "SomeAction", "SomeController")
                                .ToString();
            Assert.AreEqual("<a href=\"/SomeJunk/SomeController/SomeAction\">link text</a>", result);
        }

        [TestMethod]
        public void TestActionRouteLinkWithoutRouteValuesOrHtmlAttributesDoesNotAddEither()
        {
            _application.Routes.MapRoute(
                name: "SomeNamedRoute",
                url: "SomeJunk/{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: null
            );

            var result = _target.ActionRouteLink("link text", "SomeNamedRoute", "SomeAction", "SomeController")
                                .ToString();
            Assert.AreEqual("<a href=\"/SomeJunk/SomeController/SomeAction\">link text</a>", result);
        }

        [TestMethod]
        public void TestActionRouteLinkWithRouteValuesButNotHtmlAttributesAddsRouteValues()
        {
            _application.Routes.MapRoute(
                name: "SomeNamedRoute",
                url: "SomeJunk/{controller}/{action}/{someParam}",
                defaults: new {controller = "Home", action = "Index"},
                namespaces: null
            );

            var routeValues = new {someParam = "neat"};

            var result = _target
                        .ActionRouteLink("link text", "SomeNamedRoute", "SomeAction", "SomeController", routeValues)
                        .ToString();
            Assert.AreEqual("<a href=\"/SomeJunk/SomeController/SomeAction/neat\">link text</a>", result);

            var rvd = new RouteValueDictionary();
            rvd["someParam"] = "alsoNeat";
            result = _target.ActionRouteLink("link text", "SomeNamedRoute", "SomeAction", "SomeController", rvd)
                            .ToString();
            Assert.AreEqual("<a href=\"/SomeJunk/SomeController/SomeAction/alsoNeat\">link text</a>", result);
        }

        [TestMethod]
        public void TestActionRouteLinkWithRouteValuesAndHtmlAttributesRendersBoth()
        {
            _application.Routes.MapRoute(
                name: "SomeNamedRoute",
                url: "SomeJunk/{controller}/{action}/{someParam}",
                defaults: new {controller = "Home", action = "Index"},
                namespaces: null
            );

            var routeValues = new {someParam = "neat"};
            var html = new {attr = "value"};

            var result = _target
                        .ActionRouteLink("link text", "SomeNamedRoute", "SomeAction", "SomeController", routeValues,
                             html).ToString();
            Assert.AreEqual("<a attr=\"value\" href=\"/SomeJunk/SomeController/SomeAction/neat\">link text</a>",
                result);

            var htmlDict = new RouteValueDictionary();
            htmlDict["attr"] = "value";
            result = _target.ActionRouteLink("link text", "SomeNamedRoute", "SomeAction", "SomeController", routeValues,
                htmlDict).ToString();
            Assert.AreEqual("<a attr=\"value\" href=\"/SomeJunk/SomeController/SomeAction/neat\">link text</a>",
                result);
        }

        #endregion

        #region AuthorizedActionLink

        private void TestAuthorizedActionLink(string actionName, string expectedMarkup, object routeValues = null,
            string linkText = "link")
        {
            _application.ControllerFactory.RegisterController("ActionAuthorization",
                new ActionAuthorizationController());
            var helper = _request.CreateHtmlHelper<object>(null);
            var result =
                helper.AuthorizedActionLink(linkText, actionName, "ActionAuthorization", routeValues: routeValues);
            Assert.AreEqual(expectedMarkup, result.ToString());
        }

        [TestMethod]
        public void TestAuthorizedActionLinkDoesNotRenderIfUserIsNotAuthorizedToAccessControllerAction()
        {
            TestAuthorizedActionLink(ActionAuthorizationController.UnauthorizedActionMemberInfo.Name, string.Empty);
        }

        [TestMethod]
        public void TestAuthorizedActionLinkDoesRenderIfUserIsAuthorizedToAccessControllerAction()
        {
            TestAuthorizedActionLink(ActionAuthorizationController.AuthorizedActionMemberInfo.Name,
                "<a href=\"/ActionAuthorization/AuthorizedAction\">link</a>");
        }

        [TestMethod]
        public void TestAuthorizedActionLinkIncludesRouteData()
        {
            TestAuthorizedActionLink(ActionAuthorizationController.AuthorizedActionMemberInfo.Name,
                "<a href=\"/ActionAuthorization/AuthorizedAction/431\">link</a>", new {id = 431});
        }

        [TestMethod]
        public void TestAuthorizedActionLinkHtmlEscapesLinkText()
        {
            TestAuthorizedActionLink(ActionAuthorizationController.AuthorizedActionMemberInfo.Name,
                "<a href=\"/ActionAuthorization/AuthorizedAction\">R &amp; D</a>", linkText: "R & D");
        }

        [TestMethod]
        public void TestAuthorizedActionLinkHandlesDifferingHttpVerbs()
        {
            // TODO: Make this work.
            _request.Request.Setup(x => x.HttpMethod).Returns("POST");
            _request.RequestHeaders["X-HTTP-Method-Override"] = "POST";

            _target = _request.CreateHtmlHelper<object>();

            _application.ControllerFactory.RegisterController("ActionAuthorization",
                new ActionAuthorizationController());

            var result = _target.AuthorizedActionLink("link",
                ActionAuthorizationController.HttpGetAuthorizedActionMemberInfo.Name, "ActionAuthorization");
            Assert.AreEqual("<a href=\"/ActionAuthorization/HttpGetAuthorizedAction\">link</a>", result.ToString());

            result = _target.AuthorizedActionLink("link",
                ActionAuthorizationController.HttpPostAuthorizedActionMemberInfo.Name, "ActionAuthorization");
            Assert.AreEqual("<a href=\"/ActionAuthorization/HttpPostAuthorizedAction\">link</a>", result.ToString());
        }

        #endregion

        #region AuthorizedActionLinkOrText

        private void TestAuthorizedActionLinkOrText(string actionName, string expectedMarkup, object routeValues = null,
            string linkText = "link")
        {
            _application.ControllerFactory.RegisterController("ActionAuthorization",
                new ActionAuthorizationController());
            var helper = _request.CreateHtmlHelper<object>(null);
            var result = helper.AuthorizedActionLinkOrText(linkText, actionName, "ActionAuthorization",
                routeValues: routeValues);
            Assert.AreEqual(expectedMarkup, result.ToString());
        }

        [TestMethod]
        public void TestAuthorizedActionLinkOrTextRendersLinkTextOnlyIfUserIsNotAuthorizedToAccessControllerAction()
        {
            TestAuthorizedActionLinkOrText(ActionAuthorizationController.UnauthorizedActionMemberInfo.Name, "link");
            _application.ControllerFactory.RegisterController("ActionAuthorization",
                new ActionAuthorizationController());
            var helper = _request.CreateHtmlHelper<object>(null);
            var result = helper.AuthorizedActionLinkOrText("link",
                ActionAuthorizationController.UnauthorizedActionMemberInfo.Name, "ActionAuthorization");
            Assert.AreEqual("link", result.ToString());
        }

        [TestMethod]
        public void TestAuthorizedActionLinkOrTextRendersLinkIfUserIsAuthorizedToAccessControllerAction()
        {
            TestAuthorizedActionLinkOrText(ActionAuthorizationController.AuthorizedActionMemberInfo.Name,
                "<a href=\"/ActionAuthorization/AuthorizedAction\">link</a>");
        }

        [TestMethod]
        public void TestAuthorizedActionLinkOrTextIncludesRouteData()
        {
            TestAuthorizedActionLinkOrText(ActionAuthorizationController.AuthorizedActionMemberInfo.Name,
                "<a href=\"/ActionAuthorization/AuthorizedAction/431\">link</a>", new {id = 431});
        }

        [TestMethod]
        public void TestAuthorizedActionLinkOrTextHtmlEscapesLinkText()
        {
            TestAuthorizedActionLinkOrText(ActionAuthorizationController.AuthorizedActionMemberInfo.Name,
                "<a href=\"/ActionAuthorization/AuthorizedAction\">R &amp; D</a>", linkText: "R & D");
        }

        [TestMethod]
        public void TestAuthorizedActionLinkOrTextHandlesDifferingHttpVerbs()
        {
            // TODO: Make this work.
            _request.Request.Setup(x => x.HttpMethod).Returns("POST");
            _request.RequestHeaders["X-HTTP-Method-Override"] = "POST";

            _target = _request.CreateHtmlHelper<object>();

            _application.ControllerFactory.RegisterController("ActionAuthorization",
                new ActionAuthorizationController());

            var result = _target.AuthorizedActionLinkOrText("link",
                ActionAuthorizationController.HttpGetAuthorizedActionMemberInfo.Name, "ActionAuthorization");
            Assert.AreEqual("<a href=\"/ActionAuthorization/HttpGetAuthorizedAction\">link</a>", result.ToString());

            result = _target.AuthorizedActionLinkOrText("link",
                ActionAuthorizationController.HttpPostAuthorizedActionMemberInfo.Name, "ActionAuthorization");
            Assert.AreEqual("<a href=\"/ActionAuthorization/HttpPostAuthorizedAction\">link</a>", result.ToString());
        }

        #endregion

        #region ButtonGroup

        [TestMethod]
        public void TestButtonGroupRendersWithoutTheLabelJunk()
        {
            var expectedInnards = new HtmlString("<button>I is a button</button>");
            const string expectedFormat =
                "<div class=\"field-pair fp-display fp-buttons\"><div class=\"field\"><div>{0}</div></div></div>";
            var expected = string.Format(expectedFormat, expectedInnards);

            // Test that the overload that accepts an IHtmlString works as expected.
            var result = _target.ButtonGroup(expectedInnards);
            Assert.AreEqual(expected, result.ToHtmlString());

            // And test that the overload that takes a HelperResult works the same way.
            result = _target.ButtonGroup(expectedInnards.ToHelperResult());
            Assert.AreEqual(expected, result.ToHtmlString());
        }

        #endregion

        #region ConvertToRouteValueDictionary

        [TestMethod]
        public void TestConvertToRouteValueDictionaryReturnsSameInstanceWhenInstanceIsAlreadyRouteValueDictionary()
        {
            var expected = new RouteValueDictionary();
            var result = HtmlHelperExtensions.ConvertToRouteValueDictionary(expected);
            Assert.AreSame(expected, result);
        }

        [TestMethod]
        public void
            TestConvertToRouteValueDictionaryReturnsNewRouteValueDictionaryIfObjectIsNotIDictionaryStringObject()
        {
            var expected = new {
                someKey = "someValue"
            };
            var result = HtmlHelperExtensions.ConvertToRouteValueDictionary(expected);
            Assert.AreNotSame(expected, result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(result["someKey"], "someValue");
        }

        [TestMethod]
        public void
            TestConvertToRouteValueDictionaryReturnsNewRouteValueDictionaryCorrectlyForIDictionaryInstancesThatArePassedInAsAnonymousObjects()
        {
            var expected = new Dictionary<string, object>();
            expected["someKey"] = "someValue";
            // ReSharper disable once RedundantCast
            var result = HtmlHelperExtensions.ConvertToRouteValueDictionary((object)expected);
            Assert.AreNotSame(expected, result);
            Assert.AreNotSame(1, result.Count);
            Assert.AreNotSame(result["someKey"], "someValue");
        }

        #endregion

        #region CurrentUserCanDo

        [TestMethod]
        public void TestCurrentUserCanDoReturnsTrueWhenCurrentUserCanDoThat()
        {
            _application.ControllerFactory.RegisterController("ActionAuthorization",
                new ActionAuthorizationController());
            var helper = _request.CreateHtmlHelper<object>(null);
            var result = helper.CurrentUserCanDo("AuthorizedAction", "ActionAuthorization");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCurrentUserCanDoReturnsFalseWhenCurrentUserCannotDoThat()
        {
            _application.ControllerFactory.RegisterController("ActionAuthorization",
                new ActionAuthorizationController());
            var helper = _request.CreateHtmlHelper<object>(null);
            var result = helper.CurrentUserCanDo("UnauthorizedAction", "ActionAuthorization");

            Assert.IsFalse(result);
        }

        #endregion

        #region DefaultActionLink

        [TestMethod]
        public void TestDefaultActionLinkUsesDefaultNamedRouteToGenerateUrlForLink()
        {
            _application.Routes.Clear();
            _application.Routes.MapRoute(
                name: HtmlHelperExtensions.DEFAULT_ROUTE_NAME,
                url: "SomeJunk/{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: null
            );

            var result = _target.DefaultActionLink("link text", "SomeAction", "SomeController", "").ToString();
            Assert.AreEqual("<a href=\"/SomeJunk/SomeController/SomeAction\">link text</a>", result);
        }

        [TestMethod]
        public void TestDefaultActionLinkWithoutRouteValuesOrHtmlAttributesDoesNotAddEither()
        {
            _application.Routes.Clear();
            _application.Routes.MapRoute(
                name: HtmlHelperExtensions.DEFAULT_ROUTE_NAME,
                url: "SomeJunk/{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: null
            );

            var result = _target.DefaultActionLink("link text", "SomeAction", "SomeController", "").ToString();
            Assert.AreEqual("<a href=\"/SomeJunk/SomeController/SomeAction\">link text</a>", result);
        }

        [TestMethod]
        public void TestDefaultActionLinkGetsTheDefaultRouteForAnAreaWhenAreaIsSet()
        {
            _application.Routes.Clear();
            _application.Routes.MapRoute(
                name: "SomeArea_default",
                url: "SomeArea/{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: null
            );

            var result = _target.DefaultActionLink("link text", "SomeAction", "SomeController", "SomeArea").ToString();
            Assert.AreEqual("<a href=\"/SomeArea/SomeController/SomeAction\">link text</a>", result);
        }

        [TestMethod]
        public void TestDefaultActionLinkWithRouteValuesButNotHtmlAttributesAddsRouteValues()
        {
            _application.Routes.Clear();
            _application.Routes.MapRoute(
                name: HtmlHelperExtensions.DEFAULT_ROUTE_NAME,
                url: "SomeJunk/{controller}/{action}/{someParam}",
                defaults: new {controller = "Home", action = "Index"},
                namespaces: null
            );

            var routeValues = new {someParam = "neat"};
            var result = _target.DefaultActionLink("link text", "SomeAction", "SomeController", "", routeValues)
                                .ToString();
            Assert.AreEqual("<a href=\"/SomeJunk/SomeController/SomeAction/neat\">link text</a>", result);

            var rvd = new RouteValueDictionary();
            rvd["someParam"] = "alsoNeat";
            result = _target.DefaultActionLink("link text", "SomeAction", "SomeController", "", rvd).ToString();
            Assert.AreEqual("<a href=\"/SomeJunk/SomeController/SomeAction/alsoNeat\">link text</a>", result);
        }

        [TestMethod]
        public void TestDefaultActionLinkWithRouteValuesAndHtmlAttributesRendersBoth()
        {
            _application.Routes.Clear();
            _application.Routes.MapRoute(
                name: HtmlHelperExtensions.DEFAULT_ROUTE_NAME,
                url: "SomeJunk/{controller}/{action}/{someParam}",
                defaults: new {controller = "Home", action = "Index"},
                namespaces: null
            );

            var routeValues = new {someParam = "neat"};
            var html = new {attr = "value"};
            var result = _target.DefaultActionLink("link text", "SomeAction", "SomeController", "", routeValues, html)
                                .ToString();
            Assert.AreEqual("<a attr=\"value\" href=\"/SomeJunk/SomeController/SomeAction/neat\">link text</a>",
                result);

            var htmlDict = new RouteValueDictionary();
            htmlDict["attr"] = "value";
            result = _target.DefaultActionLink("link text", "SomeAction", "SomeController", "", routeValues, htmlDict)
                            .ToString();
            Assert.AreEqual("<a attr=\"value\" href=\"/SomeJunk/SomeController/SomeAction/neat\">link text</a>",
                result);
        }

        #endregion

        #region DisplayCrumbs

        [TestMethod]
        public void TestCreateCrumbBuilderReturnsANewCrumbBuilderInstance()
        {
            _application.ControllerFactory.RegisterController("Controller", _controller);
            _request = _application.CreateRequestHandler("~/Controller/Show/");
            var helper = _request.CreateHtmlHelper<object>();
            var first = helper.CreateCrumbBuilder();
            var second = helper.CreateCrumbBuilder();
            Assert.IsNotNull(first);
            Assert.IsNotNull(second);
            Assert.AreNotSame(first, second);
        }

        [TestMethod]
        public void TestDisplayCrumbsReturnsRenderingFromCrumbBuilder()
        {
            _application.ControllerFactory.RegisterController("FakeCrud", _controller);
            _request = _application.CreateRequestHandler("~/FakeCrud/Show/");
            var helper = _request.CreateHtmlHelper<object>();
            var result = helper.DisplayCrumbs().ToString();
            Assert.AreEqual("<a href=\"/FakeCrud\">Fake Cruds</a><span>Show</span>", result);
        }

        [TestMethod]
        public void TestDisplayCrumbsPassesSeperatorParameterToCrumbBuilder()
        {
            _application.ControllerFactory.RegisterController("FakeCrud", _controller);
            _request = _application.CreateRequestHandler("~/FakeCrud/Show/");
            _request.InitializeController(_controller);
            var helper = _request.CreateHtmlHelper<object>();
            var result = helper.DisplayCrumbs(separator: "seppy").ToString();
            Assert.AreEqual("<a href=\"/FakeCrud\">Fake Cruds</a>seppy<span>Show</span>", result);
        }

        [TestMethod]
        public void TestDisplayTitleCrumbsSetsTextOnlySeparator()
        {
            _application.ControllerFactory.RegisterController("FakeCrud", _controller);
            _request = _application.CreateRequestHandler("~/FakeCrud/Show/");
            var helper = _request.CreateHtmlHelper<object>();
            var result = helper.DisplayTitleCrumbs(separator: " : ").ToString();
            Assert.AreEqual("Fake Cruds : Show", result);
        }

        #endregion

        #region DisplayValueFor

        [TestMethod]
        public void TestDisplayValueForReturnsFormattedValueFromDisplayFormatAttribute()
        {
            var expectedDate = new DateTime(1984, 4, 24, 4, 0, 4);
            var model = new DisplayValueForModel {
                PropWithFormat = expectedDate
            };
            var target = _request.CreateHtmlHelper(model);
            var result = target.DisplayValueFor(x => x.PropWithFormat);
            Assert.AreEqual("4/24/1984", result.ToString());

            result = target.DisplayValue("PropWithFormat");
            Assert.AreEqual("4/24/1984", result.ToString());
        }

        [TestMethod]
        public void TestDisplayValueForReturnsFormattedValueFromFormatStringParameter()
        {
            var expectedDate = new DateTime(1984, 4, 24, 4, 4, 0);
            var model = new DisplayValueForModel {
                PropNoFormat = expectedDate
            };
            var target = _request.CreateHtmlHelper(model);
            var result = target.DisplayValueFor(x => x.PropNoFormat, "{0:d}");
            Assert.AreEqual("4/24/1984", result.ToString());

            result = target.DisplayValue("PropNoFormat", "{0:d}");
            Assert.AreEqual("4/24/1984", result.ToString());
        }

        [TestMethod]
        public void TestDisplayValueForReturnsFormattedValueFromDisplayFormatAttributeIfFormatStringParameterIsNull()
        {
            var expectedDate = new DateTime(1984, 4, 24, 4, 4, 0);
            var model = new DisplayValueForModel {
                PropWithFormat = expectedDate
            };
            var target = _request.CreateHtmlHelper(model);
            var result = target.DisplayValueFor(x => x.PropWithFormat, null);
            Assert.AreEqual("4/24/1984", result.ToString());

            result = target.DisplayValueFor(x => x.PropWithFormat, CommonStringFormats.DATETIME_WITHOUT_SECONDS);
            Assert.AreEqual("4/24/1984 4:04 AM", result.ToString());

            result = target.DisplayValue("PropWithFormat", null);
            Assert.AreEqual("4/24/1984", result.ToString());

            result = target.DisplayValue("PropWithFormat", CommonStringFormats.DATETIME_WITHOUT_SECONDS);
            Assert.AreEqual("4/24/1984 4:04 AM", result.ToString());
        }

        [TestMethod]
        public void TestDisplayValueForUsesFormatAttributeIfOneExistsForTheMetadata()
        {
            var model = new DisplayValueForModel {
                BoolFormatProp = true
            };
            var target = _request.CreateHtmlHelper(model);
            var result = target.DisplayValueFor(x => x.BoolFormatProp);
            Assert.AreEqual("Sure", result.ToString());

            result = target.DisplayValue("BoolFormatProp");
            Assert.AreEqual("Sure", result.ToString());
        }

        [TestMethod]
        public void TestDisplayValueForUsesRegisteredDefaultFormatterIfOneExistsForTheModelType()
        {
            _application.ModelFormatterProvider.RegisterFormatter(typeof(DateTime), new TestModelFormatterAttribute());
            var model = new DisplayValueForModel();
            var target = _request.CreateHtmlHelper(model);

            var result = target.DisplayValueFor(x => x.PropNoFormat);
            Assert.AreEqual("I FORMATTED!", result.ToString());

            result = target.DisplayValue("PropNoFormat");
            Assert.AreEqual("I FORMATTED!", result.ToString());
        }

        #endregion

        #region EditorWithoutWrapperFor

        [TestMethod]
        public void TestEditorWithoutWrapperForReturnsEditorWithoutWrapperHtml()
        {
            Assert.Inconclusive(
                "This isn't testable because the views this is used with aren't part of MMSINC.Core.Mvc.");
        }

        #endregion

        #region FindView

        [TestMethod]
        public void TestFindViewReturnsTrueIfAViewExists()
        {
            _application.ViewEngine.ThrowIfViewIsNotRegistered = false;
            Assert.IsFalse(_target.ViewExists("Blah"));

            ((FakeViewEngine)_application.ViewEngine).Views.Add("Blah", new Mock<IView>().Object);
            Assert.IsTrue(_target.ViewExists("Blah"));
        }

        #endregion

        #region LinkButton

        [TestMethod]
        public void TestLinkButtonReturnsAnchorTagWithTextWrappedInSpan()
        {
            var result = _target.LinkButton("Blah", "action", "controller", new {id = 3}).ToString();
            Assert.AreEqual("<a class=\"link-button\" href=\"/controller/action/3\"><span>Blah</span></a>", result);
        }

        [TestMethod]
        public void TestLinkButtonAddsLinkButtonCssClass()
        {
            var result = _target.LinkButton("Blah", "action", "controller", new {id = 3}).ToString();
            Assert.AreEqual("<a class=\"link-button\" href=\"/controller/action/3\"><span>Blah</span></a>", result);
        }

        [TestMethod]
        public void TestLinkButtonMergesCssClasses()
        {
            var result = _target.LinkButton("Blah", "action", "controller", new {id = 3}, new {@class = "some-css"})
                                .ToString();
            Assert.AreEqual("<a class=\"some-css link-button\" href=\"/controller/action/3\"><span>Blah</span></a>",
                result);
        }

        [TestMethod]
        public void TestLinkButtonDoesNotRenderDictionaryPropertiesAsAttributes()
        {
            var rvd = new RouteValueDictionary();
            rvd["shouldexist"] = "indeed";
            var result = _target.LinkButton("Some Link Text", "/SomeUrl", rvd).ToString();
            Assert.AreEqual(
                "<a class=\"link-button\" href=\"/SomeUrl\" shouldexist=\"indeed\"><span>Some Link Text</span></a>",
                result);
        }

        #endregion

        #region PrettyText

        [TestMethod]
        public void TestPrettyTextWordifiesStripsIDFromTheEndOfTheInputString()
        {
            new[] {"ID", "Id"}
               .Each(s => Assert.AreEqual("Foo Bar Baz", HtmlHelperExtensions.PrettyText("FooBarBaz" + s)));
        }

        [TestMethod]
        public void TestPrettyTextWithViewDataGetsPropertyNameFromModelMetadata()
        {
            MockMetadata<TestUser, string>(_model, "Email");

            Assert.AreEqual("Email", HtmlHelperExtensions.PrettyText(_viewData));
        }

        #endregion

        #region RenderDisplayTemplate

        [TestMethod]
        public void TestRenderDisplayTemplateDoesNotMessUpCallsToHtmlBeginFormInsideTheHelperResult()
        {
            Func<object, HelperResult> helperFunc = (obj) => {
                return new HelperResult((writer) => {
                    using (_target.BeginForm())
                    {
                        writer.Write("I should be inside this form");
                    }
                });
            };

            var result = _target.RenderDisplayTemplate((IHtmlString)null, helperFunc).ToString();
            Assert.AreEqual(
                "<div class=\"field-pair fp-display\"><div class=\"field\"><div><form action=\"~/\" method=\"post\">I should be inside this form</form></div></div></div>",
                result);
        }

        #endregion

        #region RenderEditorTemplate

        [TestMethod]
        public void TestRenderEditorTemplateDoesNotMessUpCallsToHtmlBeginFormInsideTheHelperResult()
        {
            Func<object, HelperResult> helperFunc = (obj) => {
                return new HelperResult((writer) => {
                    using (_target.BeginForm())
                    {
                        writer.Write("I should be inside this form");
                    }
                });
            };

            var result = _target.RenderEditorTemplate((IHtmlString)null, helperFunc).ToString();
            Assert.AreEqual(
                "<div class=\"field-pair fp-edit\"><div class=\"field\"><div><form action=\"~/\" method=\"post\">I should be inside this form</form></div></div></div>",
                result);
        }

        #endregion

        #region ResourceRegistry

        [TestMethod]
        public void TestResourceRegistryDoesNotReturnNull()
        {
            _target.ViewContext.Controller = new FakeController();
            Assert.IsNotNull(_target.ResourceRegistry());
        }

        [TestMethod]
        public void TestResourceRegistryReturnsTheSameInstanceForAControllerEachTime()
        {
            _target.ViewContext.Controller = new FakeController();
            Assert.AreSame(_target.ResourceRegistry(), _target.ResourceRegistry());
        }

        [TestMethod]
        public void TestResourceRegistryReturnsTheSameInstanceForDifferentHtmlHelpersUsingTheSameControllerInstance()
        {
            _target.ViewContext.Controller = _controller;
            var otherHelper = _request.CreateHtmlHelper<object>();
            otherHelper.ViewContext.Controller = _controller;
            Assert.AreSame(_target.ResourceRegistry(),
                otherHelper.ResourceRegistry());
        }

        [TestMethod]
        public void TestResourceRegistryReturnsDifferentInstanceForDifferentInstanceOfController()
        {
            _target.ViewContext.Controller = _controller;
            var otherHelper = _request.CreateHtmlHelper<object>();
            otherHelper.ViewContext.Controller = new FakeController();
            Assert.AreNotSame(_target.ResourceRegistry(),
                otherHelper.ResourceRegistry());
        }

        #endregion

        #region SharedViewData

        [TestMethod]
        public void TestSharedViewDataReturnsSameViewDataObjectAsController()
        {
            _target.ViewContext.Controller = new FakeController {ViewData = _viewData};
            Assert.AreSame(_viewData, _target.SharedViewData());
        }

        #endregion

        #region TableFor

        [TestMethod]
        public void TestTableForWhenUsingPropertyOfModel()
        {
            var model = new ModelWithItems();
            model.Items = new[] {new Model {Prop = "Some Value"}};

            var target = _request.CreateHtmlHelper(_controller, model);
            var table = target.TableFor(x => x.Items).ColumnFor(x => x.Prop);
            var result = table.ToString();

            Assert.AreEqual(
                "<div class=\"table-wrapper\"><table><thead><tr><th data-property=\"Prop\">Prop</th></tr></thead><tbody><tr><td>Some Value</td></tr></tbody></table></div>",
                result);
        }

        [TestMethod]
        public void TestTableForWhenNoExpressionParameterIsUsed()
        {
            var model = new[] {new Model {Prop = "Some Value"}};

            var target = _request.CreateHtmlHelper<IEnumerable<Model>>(_controller, model);
            var table = target.TableFor().ColumnFor(x => x.Prop);
            var result = table.ToString();

            Assert.AreEqual(
                "<div class=\"table-wrapper\"><table><thead><tr><th data-property=\"Prop\">Prop</th></tr></thead><tbody><tr><td>Some Value</td></tr></tbody></table></div>",
                result);
        }

        #endregion

        #region Tabs

        [TestMethod]
        public void TestTabsReturnsNewTabBuilder()
        {
            var first = _target.Tabs();
            var second = _target.Tabs();
            Assert.IsNotNull(first);
            Assert.IsNotNull(second);
            Assert.AreNotSame(first, second);
        }

        [TestMethod]
        public void TestTabsSetsHtmlAttributesPropertyOnTabBuilder()
        {
            var expected = new {bluh = "bluh"};
            var result = _target.Tabs(expected).HtmlAttributes;
            Assert.AreSame(expected, result);
        }

        #endregion

        #region Link

        [TestMethod]
        public void TestLinkReturnsLinkForGivenUrl()
        {
            var theLink = "this is the link";

            Assert.AreEqual(String.Format("<a href=\"{0}\">{0}</a>", theLink),
                _target.Link(theLink).ToString());
        }

        [TestMethod]
        public void TestLinkReturnsLinkForGivenUrlWithTextIfProvided()
        {
            var theLink = "this is the link";
            var theTexts = "these are the texts";

            Assert.AreEqual(
                String.Format("<a href=\"{0}\">{1}</a>", theLink, theTexts),
                _target.Link(theLink, theTexts).ToString());
        }

        [TestMethod]
        public void TestLinkReturnsLinkForGivenUrlWithGivenTextAndGivenHtmlAttributesIfProvided()
        {
            var theLink = "this is the link";
            var theTexts = "these are the texts";
            var theAttributes = new {
                foo = "bar",
                foobar = "baz"
            };

            Assert.AreEqual(
                String.Format(
                    "<a foo=\"bar\" foobar=\"baz\" href=\"{0}\">{1}</a>", theLink, theTexts),
                _target.Link(theLink, theTexts, theAttributes).ToString());
        }

        #endregion

        #region Helper classes

        private class ModelWithItems
        {
            public IEnumerable<Model> Items { get; set; }
        }

        private class Model
        {
            public string Prop { get; set; }
        }

        private class BoolModel
        {
            [BoolFormat(True = "Sure", False = "Nah", Null = "Uh uh")]
            public bool? PropWithFormat { get; set; }

            public bool? PropNoFormat { get; set; }
        }

        private class DisplayValueForModel
        {
            [DisplayFormat(DataFormatString = "{0:d}")]
            public DateTime PropWithFormat { get; set; }

            public DateTime PropNoFormat { get; set; }

            [BoolFormat("Sure", "Nah", "Uh uh")]
            public bool? BoolFormatProp { get; set; }
        }

        private class TestModelFormatterAttribute : ModelFormatterAttribute
        {
            public override string FormatValue(object value)
            {
                return "I FORMATTED!";
            }
        }

        private class RangePickerModel
        {
            public DateRange DateRange { get; set; }
        }

        private class DateRangeModel
        {
            public DateRange DateRange { get; set; }
        }

        #endregion
    }
}
