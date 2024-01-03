using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;

namespace MMSINC.Core.MvcTest.Helpers.ControlBuilders
{
    [TestClass]
    public class TextBoxBuilderTest
    {
        #region Fields

        private TextBoxBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new TextBoxBuilder();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestDefaultTypeIsText()
        {
            var target = new TextBoxBuilder();
            Assert.AreEqual(TextBoxType.Text, target.Type);
        }

        [TestMethod]
        public void TestToHtmlStringReturnsTextInputForTextType()
        {
            var expected = "<input type=\"text\" value=\"\" />";
            _target.Type = TextBoxType.Text;
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnsPasswordInputForPasswordType()
        {
            var expected = "<input type=\"password\" value=\"\" />";
            _target.Type = TextBoxType.Password;
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnsTextAreaForTextAreaType()
        {
            var expected = "<textarea></textarea>";
            _target.Type = TextBoxType.TextArea;
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnsTextInputWithValue()
        {
            var expected = "<input type=\"text\" value=\"OH I SEE!\" />";
            _target.Type = TextBoxType.Text;
            _target.Value = "OH I SEE!";
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnsTextInputWithNoValueIfValueIsNull()
        {
            var expected = "<input type=\"text\" value=\"\" />";
            _target.Type = TextBoxType.Text;
            _target.Value = null;
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnsPasswordInputWithValue()
        {
            var expected = "<input type=\"password\" value=\"OH I SEE!\" />";
            _target.Type = TextBoxType.Password;
            _target.Value = "OH I SEE!";
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnsTextAreaWithValue()
        {
            var expected = "<textarea>OH I SEE!</textarea>";
            _target.Type = TextBoxType.TextArea;
            _target.Value = "OH I SEE!";
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringEscapesHtmlValuesInTextAreaValue()
        {
            var expected = "<textarea>&lt;</textarea>";
            _target.Type = TextBoxType.TextArea;
            _target.Value = "<";
            Assert.AreEqual(expected, _target.ToHtmlString());
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
        public void TestWithAdds()
        {
            _target.With("neat", "cool");
            Assert.AreEqual("cool", _target.HtmlAttributes["neat"]);
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
        public void TestAsTypeSetsTypeAndReturnsSelf()
        {
            var result = _target.AsType(TextBoxType.TextArea);
            Assert.AreEqual(TextBoxType.TextArea, _target.Type);
            Assert.AreSame(_target, result);
        }

        #endregion
    }
}
