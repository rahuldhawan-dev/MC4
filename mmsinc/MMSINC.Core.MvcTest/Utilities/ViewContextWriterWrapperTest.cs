using System.IO;
using System.Web.Mvc;
using System.Web.WebPages;
using MMSINC.Testing;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MMSINC.Core.MvcTest.Utilities
{
    [TestClass]
    public class ViewContextWriterWrapperTest
    {
        #region Fields

        private ViewContext _viewContext;
        private StringWriter _originalWriter;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _originalWriter = new StringWriter();
            _viewContext = new ViewContext {
                Writer = _originalWriter
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorSwapsOutViewContextWriterWithItsOwn()
        {
            var viewContext = new ViewContext {Writer = _originalWriter};
            var target = new ViewContextWriterWrapper(viewContext);
            Assert.AreNotSame(_originalWriter, viewContext.Writer);
            Assert.AreSame(target.Writer, viewContext.Writer);
        }

        [TestMethod]
        public void TestDisposeSetsViewContextWriterBackToOriginalWriter()
        {
            var target = new ViewContextWriterWrapper(_viewContext);
            target.Dispose();
            Assert.AreSame(_originalWriter, _viewContext.Writer);
        }

        [TestMethod]
        public void TestConstructorAndDisposeProperlyPushAndPopReplacementWriterWhenViewIsNotNull()
        {
            var view = new FakeView();
            view.ViewContext = _viewContext;
            view.Initialize();

            // We can't actually test against _originalWriter here. When a WebViewPage 
            // pushes a new context, it actually creates a new temporary writer and pushes
            // that to the output stack. The original writer is written to later when the
            // the page context is popped from the stack.
            var originalTempWriter = view.OutputStack.Peek();
            Assert.AreSame(originalTempWriter, view.OutputStack.Peek(), "Yuh huh");

            var target = new ViewContextWriterWrapper(_viewContext, view);
            var result = view.OutputStack.Peek();
            Assert.AreSame(target.Writer, result);

            target.Dispose();
            Assert.AreNotSame(target.Writer, view.OutputStack.Peek());
            Assert.AreSame(originalTempWriter, view.OutputStack.Peek());
        }

        [TestMethod]
        public void TestDisposeAddsContentAddedToOriginalWriterIfTheOverrideWriterIsEmpty()
        {
            // Write something to the original writer.
            _viewContext.Writer.Write("This is here before starting.");
            var target = new ViewContextWriterWrapper(_viewContext);

            // And now write some other stuff to the original writer.
            target.OriginalWriter.Write("New stuff.");

            target.Dispose();
            Assert.AreEqual("This is here before starting.", target.OriginalWriter.ToString());
            Assert.AreEqual("New stuff.", target.Writer.ToString());
        }

        #endregion

        #region Test classes

        private class FakeView : WebViewPage
        {
            public FakeView()
            {
                var handler = new FakeMvcHttpHandler(new Container());
                Context = handler.HttpContext.Object;
            }

            public void Initialize()
            {
                // Assert.IsNotNull(ViewContext);
                // Assert.IsNotNull(base.Output, "Whooooa");

                var pageContext = new WebPageContext();
                PushContext(pageContext, ViewContext.Writer);
                ExecutePageHierarchy();
            }

            public override void Execute() { }
        }

        #endregion
    }
}
