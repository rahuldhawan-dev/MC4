using System;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MapCall.Common.MvcTest.Helpers
{
    [TestClass, DoNotParallelize]
    public class ActionBarHelperTest
    {
        #region Fields

        private ActionBarHelper<User> _target;
        private MvcApplicationTester<FakeMvcApplication> _app;
        private FakeMvcHttpHandler _request;
        private HtmlHelper<User> _htmlHelper;
        private Mock<IAuthenticationService<User>> _authServ;
        private Mock<IRoleService> _roleServ;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        #region Setup/Teardown

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);
            _container.Inject(_authServ.Object);
            _container.Inject<IAuthenticationService>(_authServ.Object);

            _roleServ = new Mock<IRoleService>();
            _container.Inject(_roleServ.Object);
            _roleServ.Setup(x =>
                          x.CanAccessRole(It.IsAny<RoleModules>(), It.IsAny<RoleActions>(),
                              It.IsAny<OperatingCenter>()))
                     .Returns(true);

            _app = new FakeMvcApplicationTester(_container);
            _app.ControllerFactory.RegisterController(new TestFacilityController());
            _app.ControllerFactory.RegisterController(new TestVisibilityController());

            var auth = _container.GetInstance<MvcAuthorizationFilter>();
            auth.Authorizors.Add(_container.GetInstance<RoleAuthorizer>());
            _app.Filters.GlobalFilters.Add(auth);
            _app.ViewEngine.ThrowIfViewIsNotRegistered = false;
            InitializeForRequest("~/TestFacility/");
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _app.Dispose();
        }

        #endregion

        private void InitializeForRequest(string virtualPath)
        {
            _request = _app.CreateRequestHandler(virtualPath);
            _request.UserIdentity.Setup(x => x.IsAuthenticated).Returns(true);
            var controller = _request.CreateAndInitializeController<TestFacilityController>();
            _htmlHelper = _request.CreateHtmlHelper<User>(controller);
            _target = _container.With(_htmlHelper).GetInstance<ActionBarHelper<User>>();
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestAutoGenerateCrudLinksIsTrueByDefault()
        {
            Assert.IsTrue(_container.With(_htmlHelper).GetInstance<ActionBarHelper<User>>().AutoGenerateCrudLinks);
        }

        #endregion

        #region ToHtmlString

        private void AssertHasHtml(string html)
        {
            var result = _target.ToHtmlString();
            if (!result.Contains(html))
            {
                Assert.Fail("Unable to find expected html '{0}' in \r\n\r\n {1}", html, result);
            }
        }

        private void AssertDoesNotHaveHtml(string html)
        {
            var result = _target.ToHtmlString();
            if (result.Contains(html))
            {
                Assert.Fail("Unexpectedly found the not-expected html '{0}' in \r\n\r\n {1}", html, result);
            }
        }

        private void AssertNothingRenders()
        {
            Assert.AreEqual(string.Empty, _target.ToHtmlString());
        }

        private void AssertHasSearchLink()
        {
            AssertHasHtml(
                "<a class=\"ab-search\" href=\"/TestFacility/Search\" title=\"Search for TestFacilities\">Search</a>");
        }

        private void AssertHasNewLink()
        {
            AssertHasHtml("<a class=\"ab-new\" href=\"/TestFacility/New\" title=\"Add new TestFacility\">Add</a>");
        }

        private void AssertDoesNotHaveNewLink()
        {
            AssertDoesNotHaveHtml(
                "<a class=\"ab-new\" href=\"/TestFacility/New\" title=\"Add new TestFacility\">Add</a>");
        }

        private void AssertHasHelpButton()
        {
            AssertHasHtml("<button class=\"ab-help\" type=\"button\">Help</button>");
        }

        private void AssertHasFormsButton()
        {
            AssertHasHtml("<button class=\"ab-forms\" type=\"button\">Forms</button>");
        }

        [TestMethod]
        public void TestHelpButtonGetsAddedIfAHelpPartialViewIsFound()
        {
            var mockView = new Mock<IView>();
            ((FakeViewEngine)_app.ViewEngine).Views.Add(ActionBarHelper.HELP_VIEW_NAME, mockView.Object);

            AssertHasHelpButton();
        }

        [TestMethod]
        public void TestToHtmlStringDoesNotRenderAnyAutoGeneratedCrudLinksIfAutoGenerateCrudLinksIsFalse()
        {
            _target.AutoGenerateCrudLinks = false;
            AssertNothingRenders();
        }

        [TestMethod]
        public void TestToHtmlStringDoesNotRenderForIndexViewIfUserIsNotAuthorized()
        {
            _roleServ.Setup(x => x.CanAccessRole(RoleModules.ProductionFacilities, It.IsAny<RoleActions>(),
                It.IsAny<OperatingCenter>())).Returns(false);
            AssertNothingRenders();
        }

        [TestMethod]
        public void TestToHtmlStringDoesNotRenderForSearchViewIfUserIsNotAuthorized()
        {
            InitializeForRequest("~/TestFacility/Search");
            _roleServ.Setup(x => x.CanAccessRole(RoleModules.ProductionFacilities, It.IsAny<RoleActions>(),
                It.IsAny<OperatingCenter>())).Returns(false);
            AssertNothingRenders();
        }

        [TestMethod]
        public void TestToHtmlStringDoesNotRenderForShowViewIfUserIsNotAuthorized()
        {
            InitializeForRequest("~/TestFacility/Search");
            _roleServ.Setup(x => x.CanAccessRole(RoleModules.ProductionFacilities, It.IsAny<RoleActions>(),
                It.IsAny<OperatingCenter>())).Returns(false);
            AssertNothingRenders();
        }

        [TestMethod]
        public void TestToHtmlStringOutputsExpectedLinksForIndexView()
        {
            AssertHasSearchLink();
            AssertHasNewLink();
        }

        [TestMethod]
        public void TestToHtmlStringOutputsExpectedLinksForSearchView()
        {
            InitializeForRequest("~/TestFacility/Search");
            AssertHasSearchLink();
            AssertHasNewLink();
        }

        [TestMethod]
        public void TestToHtmlStringOutputsExpectedLinksForShowView()
        {
            InitializeForRequest("~/TestFacility/Search");
            AssertHasSearchLink();
            AssertHasNewLink();
        }

        [TestMethod]
        public void TestToHtmlStringDoesNotRenderLinksForActionsThatHaveAnActionBarVisibleAttributeWithAFalseValue()
        {
            // Search should render a New link normally.
            _request = _app.CreateRequestHandler("~/TestVisibility/Search");
            _request.UserIdentity.Setup(x => x.IsAuthenticated).Returns(true);
            var controller = _request.CreateAndInitializeController<TestVisibilityController>();
            _htmlHelper = _request.CreateHtmlHelper<User>(controller);
            _target = _container.With(_htmlHelper).GetInstance<ActionBarHelper<User>>();
            AssertDoesNotHaveNewLink();
        }

        [TestMethod]
        public void TestFormsButtonGetsAddedIfAnActionBarShowFormsPartialViewIsFoundAndCurrentRequestIsShow()
        {
            InitializeForRequest("~/TestFacility/Show");
            var mockView = new Mock<IView>();
            ((FakeViewEngine)_app.ViewEngine).Views.Add(ActionBarHelper.SHOW_FORMS_VIEW_NAME, mockView.Object);

            AssertHasFormsButton();
        }

        [TestMethod]
        public void TestCanDisplayShowActionFormsReturnsTrueIfActionIsShowAndShowFormsPartialViewExists()
        {
            InitializeForRequest("~/TestFacility/Index");
            Assert.IsFalse(_target.CanDisplayShowActionForms, "Should be false since the action is not Show.");

            InitializeForRequest("~/TestFacility/Show");
            Assert.IsFalse(_target.CanDisplayShowActionForms,
                "Should be false because the action is Show and the view does not exist.");

            var mockView = new Mock<IView>();
            ((FakeViewEngine)_app.ViewEngine).Views.Add(ActionBarHelper.SHOW_FORMS_VIEW_NAME, mockView.Object);
            Assert.IsTrue(_target.CanDisplayShowActionForms,
                "Should be true because the action is Show and the view exists.");

            InitializeForRequest("~/TestFacility/Index");
            Assert.IsFalse(_target.CanDisplayShowActionForms,
                "Should be false since the action is not Show even though the view exists.");
        }

        [TestMethod]
        public void TestCanDisplayHelpReturnsTrueIfHelpPartialViewExists()
        {
            InitializeForRequest("~/TestFacility/Index");
            Assert.IsFalse(_target.CanDisplayHelp);

            var mockView = new Mock<IView>();
            ((FakeViewEngine)_app.ViewEngine).Views.Add(ActionBarHelper.HELP_VIEW_NAME, mockView.Object);
            Assert.IsTrue(_target.CanDisplayHelp);
        }

        #endregion

        #endregion

        #region Test class

        private class TestFacilityController : FakeCrudWithSearchController
        {
            [RequiresRole(RoleModules.ProductionFacilities)]
            public override ActionResult Index()
            {
                return base.Index();
            }

            [RequiresRole(RoleModules.ProductionFacilities)]
            public override ActionResult Search()
            {
                return base.Search();
            }

            [RequiresRole(RoleModules.ProductionFacilities)]
            public override ActionResult Show()
            {
                return base.Show();
            }

            [RequiresRole(RoleModules.ProductionFacilities)]
            public override ActionResult New()
            {
                return base.New();
            }

            [RequiresRole(RoleModules.ProductionFacilities)]
            public override ActionResult Edit()
            {
                return base.Edit();
            }

            [RequiresRole(RoleModules.ProductionFacilities)]
            public override ActionResult Delete()
            {
                return base.Delete();
            }
        }

        private class TestVisibilityController : FakeCrudWithSearchController
        {
            [ActionBarVisible(false)]
            public override ActionResult New()
            {
                return base.New();
            }
        }

        #endregion
    }
}
