using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities.StructureMap;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.Helpers.Forms
{
    [TestClass]
    public class FormBuilderTest
    {
        #region Fields

        private FormBuilder _target;
        private FakeMvcApplicationTester _appTester;
        private FakeMvcHttpHandler _request;
        private HtmlHelper<TestUser> _htmlHelper;
        private TextWriter _initialViewContextWriter;
        private Mock<IAuthenticationService<IAdministratedUser>> _authServ;
        private Mock<ITokenRepository<SecureFormToken, SecureFormDynamicValue>> _tokenRepo;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container(i => {
                i.For<ISecureFormTokenService>().Use<SecureFormTokenService<SecureFormToken, SecureFormDynamicValue>>();
                _authServ = i.For<IAuthenticationService<IAdministratedUser>>().Mock();
                i.For<IAuthenticationService>()
                 .Use(ctx => ctx.GetInstance<IAuthenticationService<IAdministratedUser>>());
                _tokenRepo = i.For<ITokenRepository<SecureFormToken, SecureFormDynamicValue>>().Mock();
            });

            // Make sure this is true for tests by default. Tests testing false
            // need to set it themselves.
            FormBuilder.SecureFormsEnabled = true;

            _authServ.Setup(x => x.CurrentUserId).Returns(12345);

            _appTester = _container.GetInstance<FakeMvcApplicationTester>();
            _appTester.RegisterArea("SomeArea");
            _appTester.ControllerFactory.RegisterController(new FormBuilderController());
            _request = _appTester.CreateRequestHandler();
            _htmlHelper = _request.CreateHtmlHelper<TestUser>();
            _initialViewContextWriter = _htmlHelper.ViewContext.Writer;

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));

            InitializeForm();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            // Ensure this is set back to true because if the last test that runs 
            // sets it to false then it causes other tests in other projects to fail.
            FormBuilder.SecureFormsEnabled = true;
            _appTester.Dispose();
        }

        private void InitializeForm()
        {
            _target = new FormBuilder<TestUser>(_htmlHelper);
            _target.Action = "PostAction";
            _target.Controller = "FormBuilder";
            _target.Area = string.Empty;
        }

        private void InitializeForSecureFormTests(Action<SecureFormToken> callBack = null)
        {
            callBack = callBack ?? (_ => { });
            _target.Action = "SecureAction";
            _tokenRepo
               .Setup(x => x.Save(It.IsAny<SecureFormToken>()))
               .Callback(callBack)
               .Returns((SecureFormToken t) => t);
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorSetsNewFormContextOnViewContext()
        {
            // This test needs a fresh request/htmlhelper/target to accurately test this.

            var request = _appTester.CreateRequestHandler();
            var htmlHelper = request.CreateHtmlHelper<object>();

            // The CreateHtmlHelper method adds a FakeFormContext for whatever
            // reason that I don't remember. We need to null it out to get at
            // the "default" FormContext that ViewContext returns.
            htmlHelper.ViewContext.FormContext = null;

            var previousFormContext = htmlHelper.ViewContext.FormContext;
            var target = new FormBuilder(htmlHelper);
            Assert.IsNotNull(htmlHelper.ViewContext.FormContext);
            Assert.AreNotSame(previousFormContext, htmlHelper.ViewContext.FormContext);
        }

        #endregion

        #region Rendering

        [TestMethod]
        public void TestFormRendersWithFormMethodIfFormMethodIsExplicitlySet()
        {
            var expected = @"<form action=""/FormBuilder/PostAction"" method=""get""></form>";
            _target.Method = FormMethod.Get;
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestFormMethodCanBeAutomaticallyFoundIfNotSet()
        {
            var expected = @"<form action=""/FormBuilder/PostAction"" method=""post""></form>";
            _target.Action = "PostAction";
            _target.Method = null;
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFormRendersWithExpectedUrlForActionAndController()
        {
            var expected = @"<form action=""/FormBuilder/PostAction"" method=""post""></form>";
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFormRendersWithExpectedUrlForActionControllerAndArea()
        {
            var expected = @"<form action=""/SomeArea/FormBuilder/PostAction"" method=""post""></form>";
            _target.Area = "SomeArea";
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFormRendersExpectedUrlWithAreaIfCurrentRequestIsInAreaAndAreaPropertyIsNull()
        {
            var expected = @"<form action=""/SomeArea/FormBuilder/PostAction"" method=""post""></form>";
            var request = _appTester.CreateRequestHandler("~/SomeArea/FormBuilder/SomeOtherAction");
            var htmlHelper = request.CreateHtmlHelper<object>();
            var target = new FormBuilder(htmlHelper);
            target.Area = null;
            target.Controller = "FormBuilder";
            target.Action = "PostAction";
            var result = target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFormRendersWithExpectedRouteFormatIfRouteNameIsProvided()
        {
            var expected = @"<form action=""/FormBuilder/PostAction/Jinkies!/43"" method=""post""></form>";

            _appTester.Routes.MapRoute("SomeMagicalRoute", "{controller}/{action}/Jinkies!/{id}",
                new {id = (int?)null});
            _target.RouteData["id"] = 43;
            _target.Controller = "FormBuilder";
            _target.RouteName = "SomeMagicalRoute";
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFormRendersWithAdditionalCssClasses()
        {
            var expected = @"<form action=""/FormBuilder/PostAction"" class=""two one"" method=""post""></form>";
            _target.AddCssClass("one");
            _target.AddCssClass("two");
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFormRendersWithHttpOverrideMethodThingy()
        {
            var expected =
                @"<form action=""/FormBuilder/DeleteAction"" method=""post""><input name=""X-HTTP-Method-Override"" type=""hidden"" value=""DELETE"" /></form>";
            _target.Action = "DeleteAction";
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFormRendersWithViewContextWritersContent()
        {
            var expected = @"<form action=""/FormBuilder/PostAction"" method=""post"">I am some content.</form>";
            _htmlHelper.ViewContext.Writer.Write("I am some content.");
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFormRendersWithStuffWrittenUsingTheWriteMethod()
        {
            var expected = @"<form action=""/FormBuilder/PostAction"" method=""post"">I am some content.</form>";
            _target.Write("I am some content.");
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFormRendersWithFormStateCssClassIfActionIsIndex()
        {
            var expected =
                @"<form action=""/FormBuilder"" class=""has-form-state"" id=""formState_0"" method=""get""></form>";
            _target.Action = "Index";
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFormRendersWithIncreasingGeneratedIdForFormStateForms()
        {
            _target.Action = "Index";
            var result = _target.ToHtmlString();
            Assert.AreEqual(
                @"<form action=""/FormBuilder"" class=""has-form-state"" id=""formState_0"" method=""get""></form>",
                result);

            _target = new FormBuilder(_htmlHelper);
            _target.Action = "Index";
            _target.Controller = "FormBuilder";

            result = _target.ToHtmlString();
            Assert.AreEqual(
                @"<form action=""/FormBuilder"" class=""has-form-state"" id=""formState_1"" method=""get""></form>",
                result);
        }

        [TestMethod]
        public void TestFormRendersAsSecureFormIfActionRequiresSecureForm()
        {
            var expected = @"<form action=""/FormBuilder/SecureAction"" class=""no-double-submit"" method=""get"">" +
                           @"<input class=""no-form-state"" name=""__SECUREFORM"" type=""hidden"" value=""00000000-0000-0000-0000-000000000000"" /></form>";
            InitializeForSecureFormTests();
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFormRendersWithAjaxAttributesWhenAjaxOptionsAreSet()
        {
            var expected =
                @"<form action=""/FormBuilder/PostAction"" data-ajax=""true"" data-ajax-method=""POST"" method=""post""></form>";
            _target.Ajax = new System.Web.Mvc.Ajax.AjaxOptions {
                HttpMethod = "post"
            };
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestAjaxFormRendersWithSpecifiedHttpMethodIfOneIsExplicitlySetOnAjaxOptions()
        {
            var expected =
                @"<form action=""/FormBuilder/PostAction"" data-ajax=""true"" data-ajax-method=""WHAT"" method=""post""></form>";
            _target.Ajax = new System.Web.Mvc.Ajax.AjaxOptions {
                HttpMethod = "what"
            };
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestAjaxFormRendersWithSameHttpMethodIfOneIsNotSetInAjaxOptions()
        {
            var expected =
                @"<form action=""/FormBuilder/PostAction"" data-ajax=""true"" data-ajax-method=""POST"" method=""post""></form>";
            _target.Ajax = new AjaxOptions();
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);

            InitializeForm();
            _target.Ajax = new AjaxOptions();
            expected =
                @"<form action=""/FormBuilder/GetAction"" data-ajax=""true"" data-ajax-method=""GET"" method=""get""></form>";
            _target.Action = "GetAction";
            result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestAjaxFormRendersWithPreloadAttributeIfIsAjaxContentPreloadedIsTrue()
        {
            var expected =
                @"<form action=""/FormBuilder/PostAction"" data-ajax=""true"" data-ajax-method=""POST"" data-ajax-tab-preloaded=""true"" method=""post""></form>";
            _target.Ajax = new AjaxOptions();
            _target.IsAjaxContentPreloaded = true;
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);

            InitializeForm();
            expected =
                @"<form action=""/FormBuilder/PostAction"" data-ajax=""true"" data-ajax-method=""POST"" method=""post""></form>";
            _target.Ajax = new AjaxOptions();
            _target.IsAjaxContentPreloaded = false;
            result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestSecureFormRenderingSavesToken()
        {
            InitializeForSecureFormTests(token => {
                Assert.AreEqual(_target.Action, token.Action);
                Assert.AreEqual(_target.Controller, token.Controller);
                Assert.AreEqual(_target.Area, token.Area);
                Assert.AreEqual(12345, token.UserId);
            });

            _target.Area = "SomeArea";

            _target.ToHtmlString();
        }

        [TestMethod]
        public void TestSecureFormRenderingSavesTokenWhenUsingRouteName()
        {
            InitializeForSecureFormTests(token => {
                Assert.AreEqual("SecureAction", token.Action);
                Assert.AreEqual("FormBuilder", token.Controller);
                Assert.AreEqual(12345, token.UserId);
            });

            _appTester.Routes.MapRoute("SomeMagicalRoute", "{controller}/{action}/Jinkies!/{id}",
                new {id = (int?)null});
            _target.RouteData["id"] = 43;
            _target.RouteName = "SomeMagicalRoute";

            _target.ToHtmlString();
        }

        [TestMethod]
        public void TestSecureFormRendersWithTokenGuidSetByRepository()
        {
            var guid = Guid.NewGuid();
            var expectedFormat =
                @"<form action=""/FormBuilder/SecureAction"" class=""no-double-submit"" method=""get"">" +
                @"<input class=""no-form-state"" name=""__SECUREFORM"" type=""hidden"" value=""{0}"" /></form>";
            var expected = string.Format(expectedFormat, guid);

            InitializeForSecureFormTests(token => { token.Token = guid; });

            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFormRendersWithConfirmationMessage()
        {
            var expected =
                @"<form action=""/FormBuilder/PostAction"" data-confirm=""Say what now?"" method=""post""></form>";
            _target.Confirmation = "Say what now?";
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        #endregion

        #region Disposing/Ending/Closing

        [TestMethod]
        public void TestDisposeWritesToViewContextsOriginalWriter()
        {
            var expected = @"<form action=""/FormBuilder/PostAction"" method=""post""></form>";
            _target.Dispose();
            Assert.AreEqual(expected, _initialViewContextWriter.ToString());
        }

        [TestMethod]
        public void TestDisposeDoesNotWriteToViewContextMoreThanOneTime()
        {
            var expected = @"<form action=""/FormBuilder/PostAction"" method=""post""></form>";
            _target.Dispose();
            _target.Dispose();
            _target.Dispose();
            _target.Dispose();
            _target.Dispose();
            _target.Dispose();
            Assert.AreEqual(expected, _initialViewContextWriter.ToString());
        }

        [TestMethod]
        public void TestDisposeDoesNotWriteOutputToViewContextWriterIfToStringIsCalledFirst()
        {
            _target.ToString();
            _target.Dispose();
            Assert.AreEqual(string.Empty, _initialViewContextWriter.ToString());
        }

        [TestMethod]
        public void TestDisposeDoesNotWriteOutputToViewContextWriterIfToHtmlStringIsCalledFirst()
        {
            _target.ToHtmlString();
            _target.Dispose();
            Assert.AreEqual(string.Empty, _initialViewContextWriter.ToString());
        }

        private void AssertDisposeReturnsFormContextBackToOriginalInstance(Action<FormBuilder> disposer)
        {
            var request = _appTester.CreateRequestHandler();
            var htmlHelper = request.CreateHtmlHelper<object>();

            // The CreateHtmlHelper method adds a FakeFormContext for whatever
            // reason that I don't remember. We need to null it out to get at
            // the "default" FormContext that ViewContext returns.
            htmlHelper.ViewContext.FormContext = null;

            var previousFormContext = htmlHelper.ViewContext.FormContext;
            var target = new FormBuilder(htmlHelper);
            target.Action = "PostAction";
            target.Controller = "FormBuilder";
            disposer(target);
            Assert.AreSame(previousFormContext, htmlHelper.ViewContext.FormContext);
        }

        [TestMethod]
        public void TestDisposeReturnFormContextBackToOriginalInstance()
        {
            AssertDisposeReturnsFormContextBackToOriginalInstance(f => f.Dispose());
        }

        [TestMethod]
        public void TestToStringReturnFormContextBackToOriginalInstance()
        {
            AssertDisposeReturnsFormContextBackToOriginalInstance(f => f.ToString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnFormContextBackToOriginalInstance()
        {
            AssertDisposeReturnsFormContextBackToOriginalInstance(f => f.ToHtmlString());
        }

        [TestMethod]
        public void TestToStringDoesNotRenderFormToViewContextsOriginalWriter()
        {
            _htmlHelper.ViewContext.Writer.Write("stuff");
            var s = _target.ToString();
            Assert.AreEqual("", _initialViewContextWriter.ToString());
        }

        [TestMethod]
        public void TestToHtmlStringDoesNotRenderFormToViewContextsOriginalWriter()
        {
            _htmlHelper.ViewContext.Writer.Write("stuff");
            var s = _target.ToHtmlString();
            Assert.AreEqual("", _initialViewContextWriter.ToString());
        }

        [TestMethod]
        public void TestToStringAndToHtmlStringReturnTheExactSameThing()
        {
            Assert.AreEqual(_target.ToString(), _target.ToHtmlString());
        }

        #endregion

        #region MergeRouteValues

        [TestMethod]
        public void TestMergeRouteValuesMergesRouteValues()
        {
            var rvd = new RouteValueDictionary();
            rvd["add me"] = "I better be there";
            _target.MergeRouteValues(rvd);
            Assert.AreEqual("I better be there", _target.RouteData["add me"]);
        }

        #endregion

        #region RequiresSecureForm

        [TestMethod]
        public void TestRequiresSecureFormReturns_TRUE_IfActionHasRequiresSecureFormAttributeWithTrueValue()
        {
            var request = _appTester.CreateRequestHandler("~/FormBuilder/GetSecureFormRequired");
            Assert.IsTrue(FormBuilder.RequiresSecureForm(request.RouteContext));
        }

        [TestMethod]
        public void
            TestRequiresSecureFormReturns_FALSE_IfActionHasRequiresSecureFormAttributeWithTrueValueButSecureFormsEnabledIsSetToFalse()
        {
            FormBuilder.SecureFormsEnabled = false;
            var request = _appTester.CreateRequestHandler("~/FormBuilder/GetSecureFormRequired");
            Assert.IsFalse(FormBuilder.RequiresSecureForm(request.RouteContext));
        }

        [TestMethod]
        public void TestRequiresSecureFormReturns_FALSE_IfActionHasRequiresSecureFormAttributeWithFalseValue()
        {
            var request = _appTester.CreateRequestHandler("~/FormBuilder/PostNoSecureFormRequired");
            Assert.IsFalse(FormBuilder.RequiresSecureForm(request.RouteContext));
        }

        [TestMethod]
        public void TestRequiresSecureFormReturns_TRUE_IfActionIsPOSTlikeAndDoesNOTHaveRequiresSecureFormAttribute()
        {
            var request = _appTester.CreateRequestHandler("~/FormBuilder/PostNoAttribute");
            Assert.IsTrue(FormBuilder.RequiresSecureForm(request.RouteContext));
        }

        [TestMethod]
        public void
            TestRequiresSecureFormReturns_FALSE_IfActionIsPOSTlikeAndDoesNOTHaveRequiresSecureFormAttributeButSecureFormsEnabledIsSetToFalse()
        {
            FormBuilder.SecureFormsEnabled = false;
            var request = _appTester.CreateRequestHandler("~/FormBuilder/PostNoAttribute");
            Assert.IsFalse(FormBuilder.RequiresSecureForm(request.RouteContext));
        }

        [TestMethod]
        public void TestRequiresSecureFormReturns_FALSE_IfActionIsGETlikeAndDoesNOTHaveRequiresSecureFormAttribute()
        {
            var request = _appTester.CreateRequestHandler("~/FormBuilder/GetNoAttribute");
            Assert.IsFalse(FormBuilder.RequiresSecureForm(request.RouteContext));
        }

        #endregion

        #endregion

        #region Classes

        private class FormBuilderController : Controller
        {
            [HttpGet]
            public ActionResult GetAction()
            {
                return null;
            }

            [HttpPost,
             RequiresSecureForm(false)] // Used in most of the form rendering tests. Makes it easier to check for stuff.
            public ActionResult PostAction()
            {
                return null;
            }

            [HttpDelete,
             RequiresSecureForm(false)] // Used in most of the form rendering tests. Makes it easier to check for stuff.
            public ActionResult DeleteAction()
            {
                return null;
            }

            [RequiresSecureForm]
            public ActionResult SecureAction()
            {
                return null;
            }

            public ActionResult Index()
            {
                return null;
            }

            [HttpPost, RequiresSecureForm(false)]
            public ActionResult PostNoSecureFormRequired()
            {
                return null;
            }

            [HttpGet, RequiresSecureForm(true)]
            public ActionResult GetSecureFormRequired()
            {
                return null;
            }

            [HttpPost]
            public ActionResult PostNoAttribute()
            {
                return null;
            }

            [HttpGet]
            public ActionResult GetNoAttribute()
            {
                return null;
            }
        }

        #endregion
    }
}
