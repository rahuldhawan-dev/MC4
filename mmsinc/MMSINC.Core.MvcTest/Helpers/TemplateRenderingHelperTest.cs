using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Helpers;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.Core.MvcTest.Helpers
{
    [TestClass]
    public class TemplateRenderingHelperTest
    {
        #region Constants

        private const string VIEW_TEMPLATE_BEGINNING = "This is the beginning.",
                             VIEW_TEMPLATE_END = "This is the end.",
                             VIEW_TEMPLATE_ALL = VIEW_TEMPLATE_BEGINNING + TemplateRenderingHelper.CONTENT_PLACEHOLDER +
                                                 VIEW_TEMPLATE_END;

        #endregion

        #region Fields

        private TestTemplateRenderingHelper _target;
        private ViewContext _viewContext;
        private MockController _controller;
        private ControllerContext _controllerContext;
        private MockViewEngine _viewEngine;
        private StringWriter _writer;
        private StringBuilder _writerBuilder;

        // Need to make sure this is cleaned up properly
        private IEnumerable<IViewEngine> _originalViewEngines;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _originalViewEngines = ViewEngines.Engines.ToArray();
            ViewEngines.Engines.Clear();
            _viewEngine = new MockViewEngine {TestViewTemplate = VIEW_TEMPLATE_ALL};
            ViewEngines.Engines.Add(_viewEngine);

            _controller = new MockController();
            _controllerContext = new ControllerContext {Controller = _controller};
            _controller.ControllerContext = _controllerContext;

            _writerBuilder = new StringBuilder();
            _writer = new StringWriter(_writerBuilder);
            _viewContext = new ViewContext {Controller = _controller, Writer = _writer};
            _target = new TestTemplateRenderingHelper(_viewContext);
            _target.View = "SomeView";
        }

        [TestCleanup]
        public void Cleanup()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.AddRange(_originalViewEngines);
            _writer.Dispose();
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorThrowsForNullViewContext()
        {
            // Test both overloads, even though the one overload directly calls the second.
            MyAssert.Throws<ArgumentNullException>(() => new TestTemplateRenderingHelper(null));
            MyAssert.Throws<ArgumentNullException>(() => new TestTemplateRenderingHelper(null, null));
        }

        [TestMethod]
        public void TestConstructorCopiesViewData()
        {
            var vdd = new ViewDataDictionary();
            vdd.Add("Key", "value");
            var target = new TestTemplateRenderingHelper(_viewContext, vdd);
            Assert.AreNotSame(vdd, target.ViewData);
            Assert.AreEqual(vdd["Key"], target.ViewData["Key"]);
        }

        [TestMethod]
        public void TestConstructorSetsViewDataToNewInstance()
        {
            var target = new TestTemplateRenderingHelper(_viewContext);
            Assert.IsNotNull(target.ViewData);
            Assert.AreNotSame(_viewContext.ViewData, target.ViewData,
                "Not set up to copy the viewdata from the viewcontext.");
        }

        [TestMethod]
        public void TestConstructorSetsContentHelperResult()
        {
            var expected = new HelperResult(writer => { });
            var target = new TestTemplateRenderingHelper(_viewContext, null, expected);
            Assert.AreSame(expected, target.ContentHelperResult);
        }

        [TestMethod]
        public void TestConstructorSetsVisibleToTrueByDefault()
        {
            Assert.IsTrue(new TestTemplateRenderingHelper(_viewContext).Visible);
        }

        #endregion

        #region BeginRender

        [TestMethod]
        public void TestBeginRenderWritesOpeningPartOfTemplateToViewContextsWriter()
        {
            _target.BeginRender();
            Assert.AreEqual(VIEW_TEMPLATE_BEGINNING, _writerBuilder.ToString());
        }

        [TestMethod]
        public void TestBeginRenderOnlyWritesOpeningPartOnce()
        {
            _target.BeginRender();
            _target.BeginRender();
            Assert.AreEqual(VIEW_TEMPLATE_BEGINNING, _writerBuilder.ToString(),
                "The template should not be written more than once.");
        }

        [TestMethod]
        public void TestBeginRenderDoesNotRenderAnythingIfVisibleIsFalse()
        {
            _target.Visible = false;
            _target.BeginRender();
            Assert.AreEqual(string.Empty, _writerBuilder.ToString());
        }

        #endregion

        #region Dispose

        [TestMethod]
        public void TestDisposeCallsEndRender()
        {
            _target.BeginRender();
            _target.Dispose();
            Assert.IsTrue(_writerBuilder.ToString().EndsWith(VIEW_TEMPLATE_END));
        }

        #endregion

        #region EndRender

        [TestMethod]
        public void TestEndRenderWritesClosingPartOfTemplateToViewContextsWriter()
        {
            _target.BeginRender();
            _target.EndRender();
            Assert.IsTrue(_writerBuilder.ToString().EndsWith(VIEW_TEMPLATE_END));
        }

        [TestMethod]
        public void TestEndRenderOnlyWritesClosingPartOnce()
        {
            _target.BeginRender();
            _target.EndRender();
            _target.EndRender();
            var result = _writerBuilder.ToString();
            if (result.IndexOf(VIEW_TEMPLATE_END, StringComparison.InvariantCulture) !=
                result.LastIndexOf(VIEW_TEMPLATE_END, StringComparison.InvariantCulture))
            {
                Assert.Fail("The end template is rendered more than once");
            }
        }

        [TestMethod]
        public void TestEndRenderDoesNotRenderAnythingIfVisibleIsFalse()
        {
            _target.Visible = false;
            _target.BeginRender();
            _target.EndRender();
            Assert.AreEqual(string.Empty, _writerBuilder.ToString());
        }

        [TestMethod]
        public void TestEverythingHasBeenWrittenAfterEndRenderIsCalled()
        {
            var expectedContent = "This is content";
            var expectedResult = VIEW_TEMPLATE_BEGINNING + expectedContent + VIEW_TEMPLATE_END;
            _target.BeginRender();
            _writer.Write(expectedContent);
            _target.EndRender();
            Assert.AreEqual(expectedResult, _writerBuilder.ToString());
        }

        #endregion

        #region GetViewTemplate

        [TestMethod]
        public void TestGetViewTemplateReturnsTemplate()
        {
            Assert.AreEqual(VIEW_TEMPLATE_ALL, _target.GetViewTemplate());
        }

        #endregion

        #region ToHelperResult

        [TestMethod]
        public void TestToHelperResultReturnsAHelperResultThatRendersTheWholeThing()
        {
            var content = "I am content.";
            var expected = VIEW_TEMPLATE_BEGINNING + content + VIEW_TEMPLATE_END;
            _target.ContentHelperResult = new HelperResult(writer => writer.Write(content));
            _target.ToHelperResult().WriteTo(_writer);
            Assert.AreEqual(expected, _writerBuilder.ToString());
        }

        [TestMethod]
        public void TestToHelperResultRendersEmptyStringIfVisibleIsSetToFalse()
        {
            _target.Visible = false;
            _target.ContentHelperResult = new HelperResult(writer => writer.Write("meh"));
            _target.ToHelperResult().WriteTo(_writer);
            Assert.AreEqual(string.Empty, _writerBuilder.ToString());
        }

        [TestMethod]
        public void TestToHelperResultDisposesRenderingHelperWhenHelperResultExecutes()
        {
            _target.ContentHelperResult = new HelperResult(writer => writer.Write("meh"));
            _target.ToHelperResult().WriteTo(_writer);
            Assert.IsTrue(_target.IsDisposed);
        }

        [TestMethod]
        public void TestToHelperResultDoesNotDisposeWhenItCreatesTheHelperResult()
        {
            _target.ContentHelperResult = new HelperResult(writer => writer.Write("meh"));
            _target.ToHelperResult();
            Assert.IsFalse(_target.IsDisposed);
        }

        [TestMethod]
        public void
            TestToHelperResultThrowsObjectDisposeExceptionIfSomeJerkDisposedTheRenderingHelperBeforeTheTemplateExecuted()
        {
            _target.ContentHelperResult = new HelperResult(writer => writer.Write("meh"));
            var helperResult = _target.ToHelperResult(); // need to grab that before disposing.
            _target.Dispose();
            MyAssert.Throws<ObjectDisposedException>(() => helperResult.WriteTo(_writer));
        }

        [TestMethod]
        public void TestToHelperResultThrowsArgumentNullExceptionIfContentHelperResultIsNull()
        {
            _target.ContentHelperResult = null;
            MyAssert.Throws<ArgumentNullException>(() => _target.ToHelperResult());
        }

        #endregion

        #endregion

        #region Test class

        private class TestTemplateRenderingHelper : TemplateRenderingHelper
        {
            public bool IsDisposed;

            public TestTemplateRenderingHelper(ViewContext viewContext) : base(viewContext) { }

            public TestTemplateRenderingHelper(ViewContext viewContext, ViewDataDictionary viewData) : base(viewContext,
                viewData) { }

            public TestTemplateRenderingHelper(ViewContext viewContext, ViewDataDictionary viewData,
                HelperResult content) : base(viewContext, viewData, content) { }

            public override void Dispose()
            {
                base.Dispose();
                IsDisposed = true;
            }
        }

        private class MockController : Controller { }

        private class MockViewEngine : RazorViewEngine
        {
            public string TestViewTemplate { get; set; }

            public override ViewEngineResult FindPartialView(ControllerContext controllerContext,
                string partialViewName, bool useCache)
            {
                return new ViewEngineResult(new MockView(TestViewTemplate), this);
            }

            private class MockView : IView
            {
                private string _viewToRender;

                public MockView(string viewToRender)
                {
                    _viewToRender = viewToRender;
                }

                public void Render(ViewContext viewContext, TextWriter writer)
                {
                    writer.Write(_viewToRender);
                }
            }
        }

        #endregion
    }
}
