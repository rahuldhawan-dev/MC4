using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MMSINC.Helpers;
using MMSINC.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MMSINC.Core.MvcTest.Helpers
{
    [TestClass]
    public class ViewTemplateHelperTest
    {
        #region Fields

        private FakeMvcApplicationTester _app;
        private MockView _view;
        private ViewDataDictionary _viewData;
        private ViewTemplateHelper _target;
        private ModelMetadata _metadata;

        #endregion

        #region Setup

        [TestInitialize]
        public void InitializeTest()
        {
            _app = new FakeMvcApplicationTester(new Container());
            _view = new MockView();
            _viewData = new ViewDataDictionary();
            _view.SetTestViewData(_viewData);
            _metadata = _app.ModelMetadataProvider.GetMetadataForType(null, typeof(MockViewModel));
            _viewData.ModelMetadata = _metadata;

            _view.ViewContext = new ViewContext();
            _target = new ViewTemplateHelper(_view);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _app.Dispose();
        }

        #endregion

        #region DisplayName

        [TestMethod]
        public void TestDisplayNameReturnsValueFromViewDataIfItExists()
        {
            var expected = "Some name to display";
            _viewData[ViewTemplateHelper.ViewDataKeys.DISPLAY_NAME] = expected;
            Assert.AreEqual(expected, _target.DisplayName);
        }

        [TestMethod]
        public void TestDisplayNameReturnsEmptyStringIfViewDataDoesNotContainValue()
        {
            Assert.IsNull(_target.DisplayName);
        }

        [TestMethod]
        public void TestDisplayNameReturnsEmptyStringIfEmptyString()
        {
            var expected = "";
            _viewData[ViewTemplateHelper.ViewDataKeys.DISPLAY_NAME] = expected;
            Assert.AreEqual(string.Empty, _target.DisplayName);
        }

        #endregion

        #region FieldPairWrapperClass

        [TestMethod]
        public void TestFieldPairWrapperClassFieldPairClassConstant()
        {
            Assert.AreEqual(ViewTemplateHelper.FIELD_PAIR_CLASS, _target.FieldPairWrapperClass);
        }

        [TestMethod]
        public void TestFieldPairWrapperClassIncludesCustomClassFromHtmlAttributesIfOneExists()
        {
            var expected = ViewTemplateHelper.FIELD_PAIR_CLASS + " super class";
            _viewData[ViewTemplateHelper.ViewDataKeys.CUSTOM_CSS_CLASS] = "super class";
            _target = new ViewTemplateHelper(_view);
            Assert.AreEqual(expected, _target.FieldPairWrapperClass);
        }

        #endregion

        #region HtmlAttributes

        [TestMethod]
        public void TestAnonymousHtmlAttributesGetParsedCorrectly()
        {
            var html = new {attribute = "value"};
            _viewData["html"] = html;
            _target = new ViewTemplateHelper(_view);
            Assert.AreEqual(html.attribute, _target.HtmlAttributes["attribute"]);
        }

        #endregion

        #region Helper classes

        private class MockView : WebViewPage
        {
            public override void Execute()
            {
                throw new NotImplementedException();
            }

            public void SetTestViewData(ViewDataDictionary vdd)
            {
                SetViewData(vdd);
            }
        }

        internal class MockViewModel
        {
            [StringLength(50)]
            public string StringProp { get; set; }
        }

        #endregion
    }
}
