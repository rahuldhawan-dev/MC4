using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using MMSINC.ClassExtensions;
using MMSINC.Helpers;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using Moq;
using StructureMap;

// ReSharper disable Mvc.ViewNotResolved, Mvc.ActionNotResolved

namespace MMSINC.Core.MvcTest.Helpers
{
    [TestClass]
    public class TabBuilderTest
    {
        #region Consts

        private const string PARTIAL_VIEW_NAME = "Partial",
                             PARTIAL_VIEW_CONTENT = "content of partial view";

        #endregion

        #region Fields

        private FakeMvcApplicationTester _application;
        private FakeMvcHttpHandler _request;
        private TabBuilder _target;
        private Mock<IView> _view;
        private HtmlHelper _helper;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _application = new FakeMvcApplicationTester(new Container());
            _request = _application.CreateRequestHandler();
            _view = InitPartialView(PARTIAL_VIEW_NAME, PARTIAL_VIEW_CONTENT);
            _helper = _request.CreateHtmlHelper<object>(null);
            _target = new TabBuilder(_helper);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _application.Dispose();
        }

        private Mock<IView> InitPartialView(string partialName, string content)
        {
            var view = new Mock<IView>();
            view.Setup(x => x.Render(It.IsAny<ViewContext>(), It.IsAny<TextWriter>()))
                .Callback<ViewContext, TextWriter>((vc, tw) => tw.Write(content));
            ((FakeViewEngine)_application.ViewEngine).PartialViews["Partial"] = view.Object;
            return view;
        }

        #endregion

        #region Private Methods

        private void AssertWrapperDivExists(string result)
        {
            Assert.IsTrue(
                result.StartsWith("<div class=\"tabs-container ui-tabs ui-widget ui-widget-content ui-corner-all\">"),
                result);
            Assert.IsTrue(result.EndsWith("</div>"), "Closing div not found. " + result);
        }

        private void AssertTabListWrapperExists(string result)
        {
            Assert.IsTrue(
                result.Contains(
                    "<ul class=\"ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all\">"),
                result);
            Assert.IsTrue(result.Contains("</ul>"), "Closing ul tag not found. " + result);
        }

        private void AssertTabLink(string tabName, string tabId, bool isVisible, string result)
        {
            const string expectedFormat =
                "<li class=\"ui-state-default ui-corner-top\" data-tab-text=\"{1}\"><a href=\"#{0}\">{1}</a></li>";
            var expected = string.Format(expectedFormat, tabId, tabName);
            if (isVisible)
            {
                Assert.IsTrue(result.Contains(expected), result);
            }
            else
            {
                Assert.IsFalse(result.Contains(expected), result);
            }
        }

        private void AssertTabBody(string tabId, string content, bool isVisible, string result, bool isAjaxTab,
            string updateTargetId)
        {
            const string expectedFormat =
                "<div class=\"tab-content ui-tabs-panel ui-widget-content ui-corner-bottom\" id=\"{0}\">{1}</div></div>";
            const string expectedAjaxTabFormat =
                "<div class=\"tab-content ui-tabs-panel ui-widget-content ui-corner-bottom\" data-ajax-tab=\"true\" id=\"{0}\">{1}</div></div>";
            const string expectedAjaxTabFormatWithUpdateTarget =
                "<div class=\"tab-content ui-tabs-panel ui-widget-content ui-corner-bottom\" data-ajax-tab=\"true\" data-ajax-update-target-id=\"{2}\" id=\"{0}\">{1}</div></div>";

            var format = (!isAjaxTab ? expectedFormat :
                updateTargetId == null ? expectedAjaxTabFormat : expectedAjaxTabFormatWithUpdateTarget);
            var expected = string.Format(format, tabId, content, updateTargetId);
            if (isVisible)
            {
                Assert.IsTrue(result.Contains(expected), result);
            }
            else
            {
                Assert.IsFalse(result.Contains(expected), result);
            }
        }

        private void AssertTabRendered(string tabName, string tabId, string content, bool isVisible, bool isAjaxTab,
            string updateTargetId)
        {
            var result = _target.ToString();
            AssertWrapperDivExists(result);
            AssertTabListWrapperExists(result);
            AssertTabLink(tabName, tabId, isVisible, result);
            AssertTabBody(tabId, content, isVisible, result, isAjaxTab, updateTargetId);
        }

        private void AssertRendered(string tabName, string tabId, string content, bool isVisible = true)
        {
            AssertTabRendered(tabName, tabId, content, isVisible, isAjaxTab: false, updateTargetId: null);
        }

        private void AssertAjaxTabRendered(string tabName, string tabId, string content, bool isVisible = true,
            string updateTargetId = null)
        {
            AssertTabRendered(tabName, tabId, content, isVisible, isAjaxTab: true, updateTargetId: updateTargetId);
        }

        private Func<object, HelperResult> CreateHelperResult(string content)
        {
            Action<TextWriter> writeAction = (tw) => tw.Write(content);
            Func<object, HelperResult> helper = (o) => { return new HelperResult(writeAction); };
            return helper;
        }

        #endregion

        #region General rendering

        [TestMethod]
        public void TestDivWrapperHasAdditionalHtmlAttributes()
        {
            _target.HtmlAttributes = new {hyphenated_property = "neat"};
            var result = _target.ToString();
            Assert.IsTrue(
                result.StartsWith(
                    "<div class=\"tabs-container ui-tabs ui-widget ui-widget-content ui-corner-all\" hyphenated-property=\"neat\">"),
                result);
        }

        [TestMethod]
        public void TestTabBodyHasCustomHtmlAttributesRendered()
        {
            _target.WithTab("Some Tab", CreateHelperResult("blargh"), bodyHtmlAttributes: new {@class = "neato"});
            var result = _target.ToString();
            Assert.IsTrue(
                result.Contains(
                    "<div class=\"tab-content ui-tabs-panel ui-widget-content ui-corner-bottom neato\" id=\"SomeTabTab\">blargh</div>"),
                result);
        }

        [TestMethod]
        public void TestTabHasCustomHtmlAttributesRendered()
        {
            _target.WithTab("Some Tab", CreateHelperResult("blargh"), tabHtmlAttributes: new {data_bind = "neato"});
            var result = _target.ToString();
            Assert.IsTrue(
                result.Contains(
                    "<li class=\"ui-state-default ui-corner-top\" data-bind=\"neato\" data-tab-text=\"Some Tab\">"),
                result);
        }

        [TestMethod]
        public void TestTabCustomHtmlAttributesDoNotOverwriteCssClasses()
        {
            _target.WithTab("Some Tab", CreateHelperResult("blargh"), tabHtmlAttributes: new {@class = "neato"});
            var result = _target.ToString();
            Assert.IsTrue(
                result.Contains("<li class=\"ui-state-default ui-corner-top neato\" data-tab-text=\"Some Tab\">"),
                result);
        }

        [TestMethod]
        public void TestTabRendersWithDataTabTextThatDoesNotIncludeItemCount()
        {
            _target.WithTab("Some Tab", CreateHelperResult("blargh"), itemCount: 42);
            var result = _target.ToString();
            Assert.IsTrue(result.Contains("<li class=\"ui-state-default ui-corner-top\" data-tab-text=\"Some Tab\">"),
                result);
            Assert.IsTrue(result.Contains("Some Tab (42)"));
        }

        [TestMethod]
        public void
            TestSelectedCssIsUsedForTabBodyIfThereIsOnlyOneTabRegardlessOfTabsIsSelectedPropertyValue_ForHelperResults()
        {
            // ReSharper disable RedundantArgumentDefaultValue
            _target.WithTab("Some Tab", CreateHelperResult("blargh"), isSelected: false);
            // ReSharper restore RedundantArgumentDefaultValue
            var result = _target.ToString();
            Assert.IsTrue(
                result.Contains(
                    "<div class=\"tab-content ui-tabs-panel ui-widget-content ui-corner-bottom\" id=\"SomeTabTab\">blargh</div>"),
                result);
        }

        [TestMethod]
        public void
            TestSelectedCssIsUsedForTabBodyIfThereIsOnlyOneTabRegardlessOfTabsIsSelectedPropertyValue_ForPartials()
        {
            // ReSharper disable RedundantArgumentDefaultValue
            _target.WithTab("Some Tab", "SomeTab", "Partial", null, isVisible: true, isSelected: false);
            // ReSharper restore RedundantArgumentDefaultValue
            var result = _target.ToString();
            Assert.IsTrue(
                result.Contains(
                    "<div class=\"tab-content ui-tabs-panel ui-widget-content ui-corner-bottom\" id=\"SomeTab\">content of partial view</div>"),
                result);
        }

        [TestMethod]
        public void TestUnselectedTabHasDisplayNoneStyleInlined()
        {
            _target.WithTab("Some Tab", "SomeTab", "Partial", null, isVisible: true, isSelected: false);
            _target.WithTab("Some Other Tab", "SomeOtherTab", "Partial", null, isVisible: true, isSelected: true);
            // ReSharper restore RedundantArgumentDefaultValue
            var result = _target.ToString();
            Assert.IsTrue(
                result.Contains(
                    "<div class=\"tab-content ui-tabs-panel ui-widget-content ui-corner-bottom\" id=\"SomeTab\" style=\"display:none;\">content of partial view</div>"),
                result);
        }

        [TestMethod]
        public void TestArgumentNullExceptionIsThrownDuringRenderingWhenHtmlHelperPropertyIsNull()
        {
            _target.HtmlHelper = null;
            MyAssert.Throws<ArgumentNullException>(() => _target.ToHtmlString());
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            MyAssert.Throws<ArgumentNullException>(() => _target.ToString());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [TestMethod]
        public void
            TestRenderingFromAHelperResultDoesntScrewUpAndLeaveOutValidationAttributesBecauseRossPutsOneLineOfTestCodeInAndForgetsToRemoveIt()
        {
            var helper = _request.CreateHtmlHelper<ValidationModel>();

            //
            // NOTE NOTE NOTE NOTE NOTE NOTE NOTE
            //
            // For this test to test *correctly*, the BeginForm call needs to be made from outside of
            // the HelperResult func. Otherwise, a new form context will be made each time the Func is
            // executed, and then the test will not be able to test that a form element might be getting
            // duplicated and mess up the validation attributes. 
            using (helper.BeginForm())
            {
                _target = new TabBuilder(helper);

                Func<object, HelperResult> helperResult = (obj) => {
                    return new HelperResult(writer => { writer.Write(helper.TextBoxFor(x => x.Prop)); });
                };

                _target.WithTab("Some Tab", helperResult);

                var result = _target.ToString();
                //  Assert.AreEqual("", result);
                Assert.IsTrue(
                    result.Contains(
                        "<input data-val=\"true\" data-val-required=\"The Prop field is required.\" id=\"Prop\" name=\"Prop\" type=\"text\" value=\"\" />"),
                    result);
            }
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestToHtmlStringReturnsTheExactSameThingAsToString()
        {
            Assert.AreEqual(_target.ToString(), _target.ToHtmlString());
        }

        #region WithTab overload with HelperResult param and no Id param

        [TestMethod]
        public void TestWithTabWithHelperResultParamAndNoIdParameterGeneratesAnId()
        {
            var helper = CreateHelperResult(PARTIAL_VIEW_CONTENT);
            _target.WithTab("Tab Name", helper);
            AssertRendered("Tab Name", "TabNameTab", PARTIAL_VIEW_CONTENT);
        }

        [TestMethod]
        public void TestWithTabWithHelperResultParamAndNoIdParameterDoesNotRenderIfIsVisibleParamIsFalse()
        {
            var helper = CreateHelperResult(PARTIAL_VIEW_CONTENT);
            _target.WithTab("Tab Name", helper, isVisible: false);
            AssertRendered("Tab Name", "TabName", PARTIAL_VIEW_CONTENT, false);
        }

        #endregion

        #region WithTab overload with HelperResult param and Id param

        [TestMethod]
        public void TestWithTabWithHelperResultParamAndIdParameterRendersWithIdParameter()
        {
            var helper = CreateHelperResult(PARTIAL_VIEW_CONTENT);
            _target.WithTab("Tab Name", "SomeTabId", helper);
            AssertRendered("Tab Name", "SomeTabId", PARTIAL_VIEW_CONTENT);
        }

        [TestMethod]
        public void TestWithTabWithHelperResultParamAndIdParameterDoesNotRenderIfIsVisibleParamIsFalse()
        {
            var helper = CreateHelperResult(PARTIAL_VIEW_CONTENT);
            _target.WithTab("Tab Name", "SomeTabId", helper, isVisible: false);
            AssertRendered("Tab Name", "SomeTabId", PARTIAL_VIEW_CONTENT, false);
        }

        #endregion

        #region WithTab overload with partialView param and no Id param

        [TestMethod]
        public void TestWithTabWithPartialWithoutIdParameterGeneratesAnId()
        {
            _target.WithTab("Tab Name", "Partial");
            AssertRendered("Tab Name", "TabNameTab", PARTIAL_VIEW_CONTENT);
        }

        [TestMethod]
        public void TestWithTabWithPartialWithoutIdParameterDoesNotRenderIfIsVisibleParamIsFalse()
        {
            _target.WithTab("Tab Name", "Partial", isVisible: false);
            AssertRendered("Tab Name", "TabName", PARTIAL_VIEW_CONTENT, false);
        }

        #endregion

        #region WithTab overload with partialView param and Id param

        [TestMethod]
        public void TestWithTabWithPartialUsesIdParameter()
        {
            _target.WithTab("Tab Name", "SomeTabId", "Partial");
            AssertRendered("Tab Name", "SomeTabId", PARTIAL_VIEW_CONTENT);
        }

        [TestMethod]
        public void TestWithTabWithPartialWithIdParameterDoesNotRenderIfIsVisibleParamIsFalse()
        {
            _target.WithTab("Tab Name", "SomeTabId", "Partial", isVisible: false);
            AssertRendered("Tab Name", "SomeTabId", PARTIAL_VIEW_CONTENT, false);
        }

        #endregion

        #region WithTab without partialModel parameter

        [TestMethod]
        public void TestThatTheViewDataPassedToThePartialViewIsDifferentInstanceButHasParentViewDatasKeys()
        {
            // Basically, this test is ensuring the HtmlHelper.Partial("partial", model, ViewDataDictionary)
            // is used, which does this all properly for us.

            ViewContext resultViewContext = null;
            _view.Setup(x => x.Render(It.IsAny<ViewContext>(), It.IsAny<TextWriter>()))
                 .Callback<ViewContext, TextWriter>((vc, tw) => resultViewContext = vc);

            _helper.ViewData["someKey"] = "someValue";
            _target.WithTab("Tab Name", "Partial");
            _target.ToHtmlString();

            Assert.AreNotSame(_helper.ViewContext, resultViewContext, "A new ViewContext instance must be created.");
            Assert.IsTrue(resultViewContext.ViewData.ContainsKey("someKey"),
                "The new ViewContext's ViewData must include keys from the parent ViewContext.");
            Assert.AreEqual("someValue", resultViewContext.ViewData["someKey"], "And those values should be the same");
        }

        [TestMethod]
        public void TestThatTheParentModelIsUsedWhenTheNonPartialModelOverloadIsused()
        {
            _target.WithTab("Tab Name", "Partial");

            var expectedModel = new object();
            _helper.ViewContext.ViewData.Model = expectedModel;

            ViewContext resultViewContext = null;
            _view.Setup(x => x.Render(It.IsAny<ViewContext>(), It.IsAny<TextWriter>()))
                 .Callback<ViewContext, TextWriter>((vc, tw) => resultViewContext = vc);
            _target.ToHtmlString();

            Assert.AreSame(expectedModel, resultViewContext.ViewData.Model);
        }

        [TestMethod]
        public void TestThatTheParentModelIsNotUsedWhenThePartialModelOverloadIsUsed()
        {
            var expectedModel = new object();
            var unexpectedParentModel = new object();
            _helper.ViewContext.ViewData.Model = unexpectedParentModel;
            // ReSharper disable RedundantArgumentName
            _target.WithTab("Tab Name", "Partial", partialModel: expectedModel);
            // ReSharper restore RedundantArgumentName

            ViewContext resultViewContext = null;
            _view.Setup(x => x.Render(It.IsAny<ViewContext>(), It.IsAny<TextWriter>()))
                 .Callback<ViewContext, TextWriter>((vc, tw) => resultViewContext = vc);
            _target.ToHtmlString();

            Assert.AreSame(expectedModel, resultViewContext.ViewData.Model);
        }

        [TestMethod]
        public void
            TestWithTab_WithExplicitModel_CreatesCopyOfParentViewDataDictionaryWhenModelIsNullAndAlsoSetsModelOnVDDToNull()
        {
            var expectedParentModel = new object();
            _helper.ViewContext.ViewData.Model = expectedParentModel;
            _helper.ViewContext.ViewData["expected"] = "value";
            _target.WithTab("Tab Name", "Partial", partialModel: null);

            ViewContext resultViewContext = null;
            _view.Setup(x => x.Render(It.IsAny<ViewContext>(), It.IsAny<TextWriter>()))
                 .Callback<ViewContext, TextWriter>((vc, tw) => resultViewContext = vc);
            _target.ToHtmlString();

            Assert.IsNull(resultViewContext.ViewData.Model,
                "Model must be set to null or else the RazorEngine will cry.");
            Assert.AreNotSame(_helper.ViewContext.ViewData, resultViewContext.ViewData,
                "A new ViewDataDictionary should have been created.");
            Assert.AreEqual("value", resultViewContext.ViewData["expected"]);
        }

        #endregion

        #region WithAjaxTab overload with HelperResult param and no Id param

        [TestMethod]
        public void TestWithAjaxTabWithHelperResultParamAndNoIdParameterGeneratesAnId()
        {
            var helper = CreateHelperResult(PARTIAL_VIEW_CONTENT);
            _target.WithAjaxTab("Tab Name", helper);
            AssertAjaxTabRendered("Tab Name", "TabNameTab", PARTIAL_VIEW_CONTENT);
        }

        [TestMethod]
        public void TestWithAjaxTabWithHelperResultParamAndNoIdParameterDoesNotRenderIfIsVisibleParamIsFalse()
        {
            var helper = CreateHelperResult(PARTIAL_VIEW_CONTENT);
            _target.WithAjaxTab("Tab Name", helper, isVisible: false);
            AssertAjaxTabRendered("Tab Name", "TabName", PARTIAL_VIEW_CONTENT, false);
        }

        #endregion

        #region WithAjaxTab overload with HelperResult param and Id param

        [TestMethod]
        public void TestWithAjaxTabWithHelperResultParamAndIdParameterRendersWithIdParameter()
        {
            var helper = CreateHelperResult(PARTIAL_VIEW_CONTENT);
            _target.WithAjaxTab("Tab Name", "SomeTabId", helper);
            AssertAjaxTabRendered("Tab Name", "SomeTabId", PARTIAL_VIEW_CONTENT);
        }

        [TestMethod]
        public void TestWithAjaxTabWithHelperResultParamAndIdParameterDoesNotRenderIfIsVisibleParamIsFalse()
        {
            var helper = CreateHelperResult(PARTIAL_VIEW_CONTENT);
            _target.WithAjaxTab("Tab Name", "SomeTabId", helper, isVisible: false);
            AssertAjaxTabRendered("Tab Name", "SomeTabId", PARTIAL_VIEW_CONTENT, false);
        }

        [TestMethod]
        public void TestWithAjaxTabWithHelperResultParamAndIdParameterRendersWithAdditionalTarget()
        {
            var helper = CreateHelperResult(PARTIAL_VIEW_CONTENT);
            _target.WithAjaxTab("Tab Name", "SomeTabId", helper, updateTargetId: "imatarget");
            AssertAjaxTabRendered("Tab Name", "SomeTabId", PARTIAL_VIEW_CONTENT, updateTargetId: "imatarget");
        }

        #endregion

        #region WithAjaxTab overload with partialView param and no Id param

        [TestMethod]
        public void TestWithAjaxTabWithPartialWithoutIdParameterGeneratesAnId()
        {
            _target.WithAjaxTab("Tab Name", "Partial");
            AssertAjaxTabRendered("Tab Name", "TabNameTab", PARTIAL_VIEW_CONTENT);
        }

        [TestMethod]
        public void TestWithAjaxTabWithPartialWithoutIdParameterDoesNotRenderIfIsVisibleParamIsFalse()
        {
            _target.WithAjaxTab("Tab Name", "Partial", isVisible: false);
            AssertAjaxTabRendered("Tab Name", "TabName", PARTIAL_VIEW_CONTENT, false);
        }

        #region WithAjaxTab overload with partialView param and Id param

        [TestMethod]
        public void TestWithAjaxTabWithPartialUsesIdParameter()
        {
            _target.WithTab("Tab Name", "SomeTabId", "Partial");
            AssertRendered("Tab Name", "SomeTabId", PARTIAL_VIEW_CONTENT);
        }

        [TestMethod]
        public void TestWithAjaxTabWithPartialWithIdParameterDoesNotRenderIfIsVisibleParamIsFalse()
        {
            _target.WithTab("Tab Name", "SomeTabId", "Partial", isVisible: false);
            AssertRendered("Tab Name", "SomeTabId", PARTIAL_VIEW_CONTENT, false);
        }

        #endregion

        #region WithAjaxTab without partialModel parameter

        [TestMethod]
        public void TestWithAjaxTabThatTheViewDataPassedToThePartialViewIsDifferentInstanceButHasParentViewDatasKeys()
        {
            // Basically, this test is ensuring the HtmlHelper.Partial("partial", model, ViewDataDictionary)
            // is used, which does this all properly for us.

            ViewContext resultViewContext = null;
            _view.Setup(x => x.Render(It.IsAny<ViewContext>(), It.IsAny<TextWriter>()))
                 .Callback<ViewContext, TextWriter>((vc, tw) => resultViewContext = vc);

            _helper.ViewData["someKey"] = "someValue";
            _target.WithAjaxTab("Tab Name", "Partial");
            _target.ToHtmlString();

            Assert.AreNotSame(_helper.ViewContext, resultViewContext, "A new ViewContext instance must be created.");
            Assert.IsTrue(resultViewContext.ViewData.ContainsKey("someKey"),
                "The new ViewContext's ViewData must include keys from the parent ViewContext.");
            Assert.AreEqual("someValue", resultViewContext.ViewData["someKey"], "And those values should be the same");
        }

        [TestMethod]
        public void TestWithAjaxTabThatTheParentModelIsUsedWhenTheNonPartialModelOverloadIsused()
        {
            _target.WithAjaxTab("Tab Name", "Partial");

            var expectedModel = new object();
            _helper.ViewContext.ViewData.Model = expectedModel;

            ViewContext resultViewContext = null;
            _view.Setup(x => x.Render(It.IsAny<ViewContext>(), It.IsAny<TextWriter>()))
                 .Callback<ViewContext, TextWriter>((vc, tw) => resultViewContext = vc);
            _target.ToHtmlString();

            Assert.AreSame(expectedModel, resultViewContext.ViewData.Model);
        }

        [TestMethod]
        public void TestWithAjaxTabThatTheParentModelIsNotUsedWhenThePartialModelOverloadIsUsed()
        {
            var expectedModel = new object();
            var unexpectedParentModel = new object();
            _helper.ViewContext.ViewData.Model = unexpectedParentModel;
            // ReSharper disable RedundantArgumentName
            _target.WithAjaxTab("Tab Name", "Partial", partialModel: expectedModel);
            // ReSharper restore RedundantArgumentName

            ViewContext resultViewContext = null;
            _view.Setup(x => x.Render(It.IsAny<ViewContext>(), It.IsAny<TextWriter>()))
                 .Callback<ViewContext, TextWriter>((vc, tw) => resultViewContext = vc);
            _target.ToHtmlString();

            Assert.AreSame(expectedModel, resultViewContext.ViewData.Model);
        }

        [TestMethod]
        public void
            TestWithAjaxTab_WithExplicitModel_CreatesCopyOfParentViewDataDictionaryWhenModelIsNullAndAlsoSetsModelOnVDDToNull()
        {
            var expectedParentModel = new object();
            _helper.ViewContext.ViewData.Model = expectedParentModel;
            _helper.ViewContext.ViewData["expected"] = "value";
            _target.WithAjaxTab("Tab Name", "Partial", partialModel: null);

            ViewContext resultViewContext = null;
            _view.Setup(x => x.Render(It.IsAny<ViewContext>(), It.IsAny<TextWriter>()))
                 .Callback<ViewContext, TextWriter>((vc, tw) => resultViewContext = vc);
            _target.ToHtmlString();

            Assert.IsNull(resultViewContext.ViewData.Model,
                "Model must be set to null or else the RazorEngine will cry.");
            Assert.AreNotSame(_helper.ViewContext.ViewData, resultViewContext.ViewData,
                "A new ViewDataDictionary should have been created.");
            Assert.AreEqual("value", resultViewContext.ViewData["expected"]);
        }

        #endregion

        #endregion

        #endregion

        #region Test class

        private class ValidationModel
        {
            [Required]
            public string Prop { get; set; }
        }

        #endregion
    }
}
