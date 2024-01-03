using MapCallMVC.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using System;
using System.Web.Mvc;
using StructureMap;

namespace MapCallMVC.Tests.Views
{
    [TestClass, DoNotParallelize]
    public class ViewBaseTest
    {
        #region Fields

        private MapCallMvcApplicationTester _app;
        private FakeMvcHttpHandler _request;
        private TestViewBase<object> _target;
        private FakeCrudController _controller;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = MapCallMvcApplicationTester.InitializeDummyObjectFactory();
            _app = _container.With(true).GetInstance<MapCallMvcApplicationTester>();
            _request = _app.CreateRequestHandler();
            _target = _container.GetInstance<TestViewBase<object>>();
            _target.Html = _request.CreateHtmlHelper<object>();
            _controller = _request.CreateAndInitializeController<FakeCrudController>();
            _target.ViewContext = new ViewContext
            {
                Controller = _controller,
            };
            _target.Html.ViewContext.Controller = _controller;
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _app.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestActionBarHelperPropertyCreatesNewInstanceIfOneIsNotFoundInControllersViewData()
        {
            const string sharedKey = TestViewBase<object>.SHARED_VIEWDATA_KEY;
            Assert.IsFalse(_controller.ViewData.ContainsKey(sharedKey), "The shared VDD should not have been created yet, so there'd be no ActionBarHelper instance.");
            var result = _target.ActionBarHelper;
            Assert.IsNotNull(result);

            var shared = (ViewDataDictionary)_controller.ViewData[TestViewBase<object>.SHARED_VIEWDATA_KEY];
            Assert.IsNotNull(shared["ActionBarHelper"]);
            Assert.AreSame(result, shared["ActionBarHelper"]);
        }

        [TestMethod]
        public void TestViewBasesBornOfTheSameControllerShareTheSameInstanceOfActionBarHelper()
        {
            var anotherView = _container.GetInstance<TestViewBase<string>>();
            anotherView.ViewContext = new ViewContext {Controller = _controller};

            Assert.AreSame(_target.ActionBarHelper, anotherView.ActionBarHelper);

            var anotherBadCreation = _container.GetInstance<TestViewBase<string>>();
            anotherBadCreation.Html = _request.CreateHtmlHelper<string>();
            anotherBadCreation.ViewContext =
                new ViewContext {Controller = _request.CreateAndInitializeController<FakeCrudController>()};
            anotherBadCreation.Html.ViewContext.Controller = anotherBadCreation.ViewContext.Controller;
            Assert.AreNotSame(_target.ActionBarHelper, anotherBadCreation.ActionBarHelper);
            Assert.AreNotSame(anotherView.ActionBarHelper, anotherBadCreation.ActionBarHelper);
        }

        #endregion

        #region Test class

        private class TestViewBase<T> : ViewBase<T>
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }

            public TestViewBase() : base() { }
        }

        #endregion

    }
}
