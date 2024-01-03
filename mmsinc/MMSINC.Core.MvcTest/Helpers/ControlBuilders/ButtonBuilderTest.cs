using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;

namespace MMSINC.Core.MvcTest.Helpers.ControlBuilders
{
    [TestClass]
    public class ButtonBuilderTest
    {
        #region Fields

        private ButtonBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new ButtonBuilder();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestToHtmlStringReturnsExpectedValueForButtonType()
        {
            var expected = "<button type=\"button\"></button>";
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnsExpectedValueForResetType()
        {
            var expected = "<button class=\"reset\" type=\"button\"></button>";
            _target.AsType(ButtonType.Reset);
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnsExpectedValueForSubmitType()
        {
            var expected = "<button type=\"submit\"></button>";
            _target.AsType(ButtonType.Submit);
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringRendersWithText()
        {
            var expected = "<button type=\"button\">Hello I am text!</button>";
            _target.WithText("Hello I am text!");
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringRendersWithValueAttributeIfValueIsNotNull()
        {
            var expected = "<button type=\"button\" value=\"42\"></button>";
            _target.Value = 42;
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestDefaultTypeIsButtonType()
        {
            Assert.AreEqual(ButtonType.Button, new ButtonBuilder().Type);
        }

        [TestMethod]
        public void TestAsTypeSetsTypeAndReturnsSelf()
        {
            var result = _target.AsType(ButtonType.Submit);
            Assert.AreEqual(ButtonType.Submit, _target.Type);
            Assert.AreSame(_target, result);
        }

        [TestMethod]
        public void TestWithTextSetsTextPropertyAndReturnsSelf()
        {
            var result = _target.WithText("texty");
            Assert.AreEqual("texty", _target.Text);
            Assert.AreSame(_target, result);
        }

        [TestMethod]
        public void TestWithValueSetsValuePropertyAndReturnsSelf()
        {
            var expected = new object();
            var result = _target.WithValue(expected);
            Assert.AreSame(expected, _target.Value);
            Assert.AreSame(_target, result);
        }

        [TestMethod]
        public void TestWithAddsAnonymousObjectAsHtmlAttributesAndReturnsSelf()
        {
            var expected = new {
                mrs_dash = "yeah"
            };

            var result = _target.With(expected);
            Assert.AreSame(expected.mrs_dash, _target.HtmlAttributes["mrs-dash"]);
            Assert.AreSame(_target, result);
        }

        [TestMethod]
        public void TestWithAddsDictionaryAsHtmlAttributesAndReturnsSelf()
        {
            var expected = new Dictionary<string, object>();
            expected["something"] = "yay";

            var result = _target.With(expected);
            Assert.AreSame("yay", _target.HtmlAttributes["something"]);
            Assert.AreSame(_target, result);
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

        #endregion
    }
}
