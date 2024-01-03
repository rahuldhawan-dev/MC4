using System.Web.Mvc;
using MMSINC.Helpers;
using MMSINC.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MMSINC.Core.MvcTest.Helpers.RazorTable
{
    [TestClass]
    public class RazorTableFooterTest
    {
        #region Fields

        private FakeMvcHttpHandler _request;
        private TestRazorTableFooter _target;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _request = new FakeMvcHttpHandler(new Container());
            _target = new TestRazorTableFooter();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestRenderReturnsAnEmptyDivTag()
        {
            var result = _target.Render(_request.CreateHtmlHelper<object>(), 42);
            Assert.AreEqual("div", result.TagName);
            Assert.AreEqual("<div class=\"table-footer\"></div>", result.ToString(),
                "Inheritors are in charge of adding any additional tags to the div created by the public Render method.");
        }

        [TestMethod]
        public void TestProtectedRenderMethodIsCalledByPublicRenderMethod()
        {
            Assert.IsFalse(_target.ProtectedRenderCalled, "If this is true, this test isn't setup correctly.");
            _target.Render(_request.CreateHtmlHelper<object>(), 42);
            Assert.IsTrue(_target.ProtectedRenderCalled);
        }

        [TestMethod]
        public void TestRenderCallsAbstractRenderOverloadWithExpectedArguments()
        {
            var expectedHelper = _request.CreateHtmlHelper<object>();
            var expectedColSpan = 12;
            var resultTag = _target.Render(expectedHelper, expectedColSpan);

            Assert.AreSame(resultTag, _target.TagBuilderRendered);
            Assert.AreSame(expectedHelper, _target.HtmlHelperUsedInRendering);
            Assert.AreEqual(expectedColSpan, _target.ColSpanSet);
        }

        #endregion

        #region Helper classes

        private class TestRazorTableFooter : RazorTableFooter
        {
            public bool ProtectedRenderCalled { get; set; }
            public TagBuilder TagBuilderRendered { get; set; }
            public int ColSpanSet { get; set; }
            public HtmlHelper HtmlHelperUsedInRendering { get; set; }

            protected override void Render(HtmlHelper htmlHelper, TagBuilder footerTag, int colSpan)
            {
                ProtectedRenderCalled = true;
                HtmlHelperUsedInRendering = htmlHelper;
                ColSpanSet = colSpan;
                TagBuilderRendered = footerTag;
            }
        }

        #endregion
    }
}
