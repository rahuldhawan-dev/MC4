using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;
using MMSINC.Testing;
using MMSINC.Utilities;

namespace MMSINC.Core.MvcTest.Helpers
{
    [TestClass]
    public class CrumbBuilderTest
    {
        #region Fields

        private CrumbBuilder _target;
        private RouteContext _routeContext;
        private FakeMvcApplicationTester _application;
        private FakeMvcHttpHandler _pipeline;
        private HtmlHelper _htmlHelper;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _application = new FakeMvcApplicationTester(new StructureMap.Container());
            _pipeline = _application.CreateRequestHandler("~/FakeCrud/Index");
            var controller = new FakeCrudController();
            _application.ControllerFactory.RegisterController("FakeCrud", controller);

            _routeContext = new RouteContext(_pipeline.RequestContext);
            _target = new CrumbBuilder(_routeContext);
            _htmlHelper = _pipeline.CreateHtmlHelper<object>(null);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _application.Dispose();
        }

        #endregion

        #region Private Methods

        private string Render()
        {
            return _target.ToHtmlString(_htmlHelper).ToString();
        }

        private RouteContext GetInternalRouteContext(CrumbBuilder target)
        {
            // ReSharper disable PossibleNullReferenceException
            return (RouteContext)typeof(CrumbBuilder)
                                .GetField("_routeContext",
                                     BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                .GetValue(target);
            // ReSharper restore PossibleNullReferenceException
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorWithRequestContextCreatesNewRouteContext()
        {
            var target = new CrumbBuilder(_pipeline.RequestContext);
            var routeContext = GetInternalRouteContext(target);
            Assert.AreSame(_pipeline.RequestContext, routeContext.RequestContext);
        }

        [TestMethod]
        public void TestConstructorWithControllerContextCreatesNewRouteContextFromControllersRequestContext()
        {
            var controller = new FakeCrudController();
            var controllerContext = _pipeline.CreateControllerContext(controller);
            var target = new CrumbBuilder(controllerContext);
            var routeContext = GetInternalRouteContext(target);
            Assert.AreSame(_pipeline.RequestContext, routeContext.RequestContext);
        }

        [TestMethod]
        public void TestConstructorSetsModel()
        {
            var model = new Object();
            var target = new CrumbBuilder(_routeContext, model);
            Assert.AreSame(model, target.Model);
        }

        [TestMethod]
        public void TestSeparatorDefaultsToNull()
        {
            Assert.IsNull(new CrumbBuilder(_routeContext).Separator);
        }

        [TestMethod]
        public void TestTextCrumbsRenderWithHtmlEncodedValues()
        {
            _target.WithTextCrumb("R & D");
            Assert.AreEqual("<span>R &amp; D</span>", Render());
        }

        [TestMethod]
        public void TestTextCrumbsRenderWithWrapperSpan()
        {
            _target.WithTextCrumb("Blurp");
            Assert.AreEqual("<span>Blurp</span>", Render());
        }

        [TestMethod]
        public void TestHtmlAttributesAreCustomizable()
        {
            _target.WithTextCrumb("Firsties!", new {@class = "first-class"});
            Assert.AreEqual("<span class=\"first-class\">Firsties!</span>", Render());
        }

        [TestMethod]
        public void TestHtmlAttributesCanDealWithDictionaryHtmlAttributes()
        {
            var html = new Dictionary<string, object>();
            html.Add("class", "first-class");
            _target.WithTextCrumb("Firsties!", html);
            Assert.AreEqual("<span class=\"first-class\">Firsties!</span>", Render());
        }

        [TestMethod]
        public void TestCrumbDividerDoesNotShowUpAfterLastCrumb()
        {
            _target.WithTextCrumb("First");
            _target.WithTextCrumb("Second");
            _target.Separator = "DIVVY";
            Assert.AreEqual("<span>First</span>DIVVY<span>Second</span>", Render());
        }

        [TestMethod]
        public void TestCanRenderEverythingAsStraightUpTextForTheTitlebar()
        {
            _target.TextOnlySeparator = " > ";
            _target.WithTextCrumb("First");
            _target.WithLinkCrumb("Link text", "controller name", "action");
            Assert.AreEqual("First &gt; Link text", _target.ToTextOnlyHtmlString(_htmlHelper).ToString());
        }

        [TestMethod]
        public void TestWithCrumbReturnsSelf()
        {
            Assert.AreSame(_target, _target.WithTextCrumb("afaefa"));
            Assert.AreSame(_target, _target.WithTextCrumb("again"));
            Assert.AreSame(_target, _target.WithLinkCrumb("controller", "action", "linkText"));
        }

        [TestMethod]
        public void TestRendersEmptyStringWhenNoCrumbsAreCreated()
        {
            _pipeline.RouteData.Values["Action"] = "NonExistantAction";
            _target = new CrumbBuilder((RouteContext)null);

            Assert.AreEqual(string.Empty, Render());
        }

        #region Default CRUD tests

        [TestMethod]
        public void TestCorrectRouteDataIsAttachedToLinks()
        {
            _target.WithLinkCrumb("this is a link", "SomeController", "SomeAction", new {id = 3});
            Assert.AreEqual("<a href=\"/SomeController/SomeAction/3\">this is a link</a>", Render());
        }

        [TestMethod]
        public void TestDisplayNameAttributeIsUsedForControllerName()
        {
            _application.ControllerFactory.RegisterController("ControllerWithDisplayName",
                new ControllerWithDisplayName());
            _pipeline = _application.CreateRequestHandler("~/ControllerWithDisplayName/Index");
            // Need a new target since we need a new RouteContext.
            _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
            Assert.AreEqual("<span>This is my display name</span>", Render());
        }

        [TestMethod]
        public void TestCrumbAttributeUsageOnAction()
        {
            _application.ControllerFactory.RegisterController("ControllerWithCrumbAttributeAction",
                new ControllerWithCrumbAttributeAction());
            _pipeline = _application.CreateRequestHandler("~/ControllerWithCrumbAttributeAction/NewThing");
            // Need a new target since we need a new RouteContext.
            _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
            Assert.AreEqual("<span>Controller With Crumb Attribute Actions</span><span>Creating</span>", Render());
        }

        [TestMethod]
        public void TestControllerNameIsPluralizedIfNoDisplayNameAttributeIsFound()
        {
            Assert.AreEqual("<span>Fake Cruds</span>", Render(), "Should be plural and not 'Fake Crud'");
        }

        [TestMethod]
        public void TestCrudForSearchAction()
        {
            _application.ControllerFactory.RegisterController("FakeCrudWithSearch", new FakeCrudWithSearchController());
            var actionVariations = new[] {"Search", "SEARCH", "search", "  Search "};
            foreach (var action in actionVariations)
            {
                var pipeline = _application.CreateRequestHandler("~/FakeCrudWithSearch/" + action);
                _target = new CrumbBuilder(new RouteContext(pipeline.RequestContext));
                Assert.AreEqual("<span>Fake Crud With Searches</span>", Render(), "Should only be a text crumb.");
            }
        }

        [TestMethod]
        public void TestCrudForIndexAction()
        {
            var actionVariations = new[] {"Index", "INDEX", "index", "  Index "};
            foreach (var action in actionVariations)
            {
                _pipeline.RouteData.Values["Action"] = action;
                _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
                Assert.AreEqual("<span>Fake Cruds</span>", Render(), "Should only be a text crumb.");
            }
        }

        [TestMethod]
        public void TestCrudForIndexActionReturnsLinkToSearchPageIfControllerHasSearchAction()
        {
            _application.ControllerFactory.RegisterController("Controller", new FakeCrudWithSearchController());
            _pipeline = _application.CreateRequestHandler("~/Controller/Index");
            _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
            Assert.AreEqual("<a href=\"/Controller/Search\">Fake Crud With Searches</a>", Render());
        }

        [TestMethod]
        public void TestCrudForIndexReturnsTextCrumbIfThereIsNoIndexActionForAController()
        {
            _application.ControllerFactory.RegisterController("NoIndex", new IndexlessController());
            _pipeline = _application.CreateRequestHandler("~/NoIndex/Show");
            _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
            // Indexlesses is apparently plural for Indexless.
            Assert.AreEqual("<span>Indexlesses</span><span>Show</span>", Render());
        }

        [TestMethod]
        public void TestCrudForShowAction()
        {
            var actionVariations = new[] {"Show", "SHOW", "show", "   Show "};

            foreach (var action in actionVariations)
            {
                _pipeline.RouteData.Values["Action"] = action;
                _pipeline.RouteData.Values["id"] = 14;
                // Need a new target since we need a new RouteContext.
                _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));

                Assert.AreEqual("<a href=\"/FakeCrud\">Fake Cruds</a><span>Show</span>", Render());
            }
        }

        [TestMethod]
        public void TestCrudForShowActionReturnsDefaultPropertyValueOfModelIfToStringIsNotOverriddenOnModel()
        {
            _pipeline.RouteData.Values["Action"] = "Show";
            _pipeline.RouteData.Values["id"] = 14;
            var model = new ModelWithDefaultProperty {
                DefaultProperty = "DefVal"
            };
            _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext), model);
            _target.DefaultPropertyForShowText = "DefaultProperty";
            _htmlHelper = _pipeline.CreateHtmlHelper(model);

            Assert.AreEqual("<a href=\"/FakeCrud\">Fake Cruds</a><span>DefVal</span>", Render());
        }

        [TestMethod]
        public void TestCrudForShowActionReturnsDefaultLinkTextIfNothingElseWorksOutInTermsOfGettingAUniqueValue()
        {
            _pipeline.RouteData.Values["Action"] = "Show";
            _pipeline.RouteData.Values["id"] = 14;
            var model = new ModelWithDefaultProperty {
                DefaultProperty = ""
            };
            _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext), model);
            _target.DefaultPropertyForShowText = "DefaultProperty";
            _htmlHelper = _pipeline.CreateHtmlHelper(model);

            Assert.AreEqual("<a href=\"/FakeCrud\">Fake Cruds</a><span>Show</span>", Render());
        }

        [TestMethod]
        public void TestCrudForNewAction()
        {
            var actionVariations = new[] {"New", "NEW", "new", "  New "};
            foreach (var action in actionVariations)
            {
                _pipeline.RouteData.Values["Action"] = action;
                // Need a new target since we need a new RouteContext.
                _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
                Assert.AreEqual("<a href=\"/FakeCrud\">Fake Cruds</a><span>Creating</span>", Render());
            }
        }

        [TestMethod]
        public void TestCrudForCreateAction()
        {
            var actionVariations = new[] {"Create", "CREATE", "create", "  Create "};
            foreach (var action in actionVariations)
            {
                _pipeline.Request.Setup(x => x.HttpMethod).Returns("POST");
                _pipeline.RouteData.Values["Action"] = action;
                // Need a new target since we need a new RouteContext.
                _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
                Assert.AreEqual("<a href=\"/FakeCrud\">Fake Cruds</a><span>Creating</span>", Render());
            }
        }

        [TestMethod]
        public void TestCrudForCreateAndNewActionReturnsSameExactThing()
        {
            _pipeline.RouteData.Values["Action"] = "New";
            // Need a new target since we need a new RouteContext.
            _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
            var newResult = Render();

            _pipeline.Request.Setup(x => x.HttpMethod).Returns("POST");
            _pipeline.RouteData.Values["Action"] = "Create";
            // Need a new target since we need a new RouteContext.
            _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
            var createResult = Render();

            Assert.AreEqual(newResult, createResult);
        }

        [TestMethod]
        public void TestCrudForEditAction()
        {
            var actionVariations = new[] {"Edit", "EDIT", "edit", "  Edit "};
            foreach (var action in actionVariations)
            {
                _pipeline.RouteData.Values["Action"] = action;
                _pipeline.RouteData.Values["id"] = 14;
                // Need a new target since we need a new RouteContext.
                _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
                Assert.AreEqual(
                    "<a href=\"/FakeCrud\">Fake Cruds</a><a href=\"/FakeCrud/Show/14\">Show</a><span>Editing</span>",
                    Render());
            }
        }

        [TestMethod]
        public void TestCrudForUpdateAction()
        {
            var actionVariations = new[] {"Update", "UPDATe", "update", "  Update "};
            foreach (var action in actionVariations)
            {
                _pipeline.Request.Setup(x => x.HttpMethod).Returns("POST");
                _pipeline.RouteData.Values["Action"] = action;
                _pipeline.RouteData.Values["id"] = 14;
                // Need a new target since we need a new RouteContext.
                _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
                Assert.AreEqual(
                    "<a href=\"/FakeCrud\">Fake Cruds</a><a href=\"/FakeCrud/Show/14\">Show</a><span>Editing</span>",
                    Render());
            }
        }

        [TestMethod]
        public void TestCrudForEditAndUpdateReturnTheSameExactThing()
        {
            _pipeline.RouteData.Values["Action"] = "Edit";
            _pipeline.RouteData.Values["id"] = 14;
            // Need a new target since we need a new RouteContext.
            _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
            var editResult = Render();

            _pipeline.Request.Setup(x => x.HttpMethod).Returns("POST");
            _pipeline.RouteData.Values["Action"] = "Update";
            _pipeline.RouteData.Values["id"] = 14;
            // Need a new target since we need a new RouteContext.
            _target = new CrumbBuilder(new RouteContext(_pipeline.RequestContext));
            var updateResult = Render();

            Assert.AreEqual(editResult, updateResult);
        }

        #endregion

        #endregion

        #region Helper classes

        [DisplayName("This is my display name")]
        private class ControllerWithDisplayName : Controller
        {
            public ActionResult Index()
            {
                return null;
            }
        }

        private class ControllerWithCrumbAttributeAction : Controller
        {
            public ActionResult New()
            {
                return null;
            }

            [Crumb(Action = "New")]
            public ActionResult NewThing()
            {
                return null;
            }
        }

        private class IndexlessController : Controller
        {
            public ActionResult Show()
            {
                return null;
            }
        }

        private class ModelWithDefaultProperty
        {
            public bool ReturnBaseToString { get; set; }
            public string DefaultProperty { get; set; }
            public string ToStringValue { get; set; }

            public override string ToString()
            {
                return ReturnBaseToString ? base.ToString() : ToStringValue;
            }
        }

        #endregion
    }
}
