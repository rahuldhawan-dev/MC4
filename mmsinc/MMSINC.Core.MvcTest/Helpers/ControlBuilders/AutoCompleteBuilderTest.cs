using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.Core.MvcTest.Helpers.ControlBuilders
{
    [TestClass]
    public class AutoCompleteBuilderTest
    {
        #region Fields

        private AutoCompleteBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new AutoCompleteBuilder();
            _target.Id = "SomeId";
            _target.Name = "SomeName";
            _target.Url = "/SomeUrl";
            _target.ActionParameterName = "SomeParam";
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestToHtmlStringThrowsInvalidOperationExceptionIfIdIsNullEmptyOrWhiteSpace()
        {
            _target.Id = null;
            MyAssert.Throws<InvalidOperationException>(() => _target.ToHtmlString());

            _target.Id = string.Empty;
            MyAssert.Throws<InvalidOperationException>(() => _target.ToHtmlString());

            _target.Id = "       ";
            MyAssert.Throws<InvalidOperationException>(() => _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringThrowsInvalidOperationExceptionIfUrlIsNullEmptyOrWhiteSpace()
        {
            _target.Url = null;
            MyAssert.Throws<InvalidOperationException>(() => _target.ToHtmlString());

            _target.Url = string.Empty;
            MyAssert.Throws<InvalidOperationException>(() => _target.ToHtmlString());

            _target.Url = "       ";
            MyAssert.Throws<InvalidOperationException>(() => _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringThrowsInvalidOperationExceptionIActionParameterNamelIsNullEmptyOrWhiteSpace()
        {
            _target.ActionParameterName = null;
            MyAssert.Throws<InvalidOperationException>(() => _target.ToHtmlString());

            _target.ActionParameterName = string.Empty;
            MyAssert.Throws<InvalidOperationException>(() => _target.ToHtmlString());

            _target.ActionParameterName = "       ";
            MyAssert.Throws<InvalidOperationException>(() => _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnsExpectedOutput()
        {
            var expected =
                @"<input class=""autocomplete-faux-hidden"" id=""SomeId"" name=""SomeName"" tabindex=""-1"" type=""text"" value=""Some prefilled value"" />" +
                @"<input autocomplete=""off"" class=""autocomplete"" data-autocomplete-action=""/SomeUrl"" data-autocomplete-actionparam=""SomeParam"" data-autocomplete-dependent=""SomeName"" id=""SomeId_AutoComplete"" type=""text"" value=""Some prefilled value"" />";
            _target.Value = "Some prefilled value";
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestToHtmlStringReturnsStringWithDisplayTextSetInTextBoxWhenDisplayTextHasValue()
        {
            var expected =
                @"<input class=""autocomplete-faux-hidden"" id=""SomeId"" name=""SomeName"" tabindex=""-1"" type=""text"" value=""Some prefilled value"" />" +
                @"<input autocomplete=""off"" class=""autocomplete"" data-autocomplete-action=""/SomeUrl"" data-autocomplete-actionparam=""SomeParam"" data-autocomplete-dependent=""SomeName"" id=""SomeId_AutoComplete"" type=""text"" value=""Some display text"" />";
            _target.Value = "Some prefilled value";
            _target.DisplayText = "Some display text";
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void
            TestToHtmlStringAppliesHtmlAttributesPropertyToTheHiddenTextBoxBecauseThatIsWhereUnobtrusiveValidatorsGo()
        {
            _target.With("an-attribute", "yay");
            StringAssert.StartsWith(_target.ToHtmlString(),
                @"<input an-attribute=""yay"" class=""autocomplete-faux-hidden"" id=""SomeId"" name=""SomeName"" tabindex=""-1"" type=""text"" value="""" />");
        }

        [TestMethod]
        public void TestWithDisplayTextSetsDisplayTextProperty()
        {
            _target.WithDisplayText("blah");
            Assert.AreEqual("blah", _target.DisplayText);
        }

        [TestMethod]
        public void TestWithDisplayTextReturnsSelf()
        {
            Assert.AreSame(_target, _target.WithDisplayText("some text"));
        }

        [TestMethod]
        public void TestWithValueSetsValueProperty()
        {
            var expected = new object();
            _target.WithValue(expected);
            Assert.AreSame(expected, _target.Value);
        }

        [TestMethod]
        public void TestWithValueReturnsSelf()
        {
            Assert.AreSame(_target, _target.WithValue(null));
        }

        [TestMethod]
        public void TestWithAddsAnonymousObjectAsHtmlAttributes()
        {
            var expected = new {
                mrs_dash = "yeah"
            };

            _target.With(expected);
            Assert.AreSame(expected.mrs_dash, _target.HtmlAttributes["mrs-dash"]);
        }

        [TestMethod]
        public void TestWithAddsDictionaryAsHtmlAttributes()
        {
            var expected = new Dictionary<string, object>();
            expected["something"] = "yay";

            _target.With(expected);
            Assert.AreSame("yay", _target.HtmlAttributes["something"]);
        }

        [TestMethod]
        public void TestWithAddsAndReturnsSelf()
        {
            var result = _target.With("neat", "cool");
            Assert.AreEqual("cool", _target.HtmlAttributes["neat"]);
            Assert.AreSame(_target, result);
        }

        [TestMethod]
        public void TestWithIdSetsIdAndReturnsSelf()
        {
            var result = _target.WithId("Id");
            Assert.AreEqual("Id", _target.Id);
            Assert.AreSame(_target, result);
        }

        [TestMethod]
        public void TestWithNameSetsNameAndReturnsSelf()
        {
            var result = _target.WithName("Name");
            Assert.AreEqual("Name", _target.Name);
            Assert.AreSame(_target, result);
        }

        [TestMethod]
        public void TestDependsOnIncludedInHTMLString()
        {
            var expected =
                @"<input class=""autocomplete-faux-hidden"" id=""SomeId"" name=""SomeName"" tabindex=""-1"" type=""text"" value="""" />" +
                @"<input autocomplete=""off"" class=""autocomplete"" data-autocomplete-action=""/SomeUrl"" data-autocomplete-actionparam=""SomeParam"" data-autocomplete-dependent=""SomeName"" data-autocomplete-dependson=""#Town"" id=""SomeId_AutoComplete"" type=""text"" value="""" />";
            _target.DependsOn = "Town";
            var result = _target.ToHtmlString();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestPlaceHolderIncludedInHTMLString()
        {
            var expected =
                @"<input class=""autocomplete-faux-hidden"" id=""SomeId"" name=""SomeName"" tabindex=""-1"" type=""text"" value="""" />" +
                @"<input autocomplete=""off"" class=""autocomplete"" data-autocomplete-action=""/SomeUrl"" data-autocomplete-actionparam=""SomeParam"" data-autocomplete-dependent=""SomeName"" id=""SomeId_AutoComplete"" placeholder=""Fancytext"" type=""text"" value="""" />";

            _target.PlaceHolder = "Fancytext";
            var result = _target.ToHtmlString();

            Assert.AreEqual(expected, result);
        }

        #endregion
    }
}
