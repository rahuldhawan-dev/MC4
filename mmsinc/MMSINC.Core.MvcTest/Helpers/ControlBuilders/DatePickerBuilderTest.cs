using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;

namespace MMSINC.Core.MvcTest.Helpers.ControlBuilders
{
    [TestClass]
    public class DatePickerBuilderTest
    {
        #region Fields

        private DatePickerBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new DatePickerBuilder();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestIncludeTimePickerIsFalseByDefault()
        {
            var target = new DatePickerBuilder();
            Assert.IsFalse(target.IncludeTimePicker);
        }

        [TestMethod]
        public void TestToHtmlStringIncludesTimePickerCssClassWhenIncludeTimePickerIsTrue()
        {
            var expected =
                "<input autocomplete=\"off\" class=\"date-time date\" type=\"text\" value=\"\" /> <button class=\"date-picker-trigger\" type=\"button\"></button>";
            _target.IncludeTimePicker = true;
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnsTextInputWithValue()
        {
            var expected =
                "<input autocomplete=\"off\" class=\"date\" type=\"text\" value=\"OH I SEE!\" /> <button class=\"date-picker-trigger\" type=\"button\"></button>";
            _target.Value = "OH I SEE!";
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnsTextInputWithNoValueIfValueIsNull()
        {
            var expected =
                "<input autocomplete=\"off\" class=\"date\" type=\"text\" value=\"\" /> <button class=\"date-picker-trigger\" type=\"button\"></button>";
            _target.Value = null;
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
        public void TestWithTimePickerSetsIncludesTimePickerAndReturnsSelf()
        {
            var result = _target.WithTimePicker(true);
            Assert.IsTrue(result.IncludeTimePicker);
            Assert.AreSame(_target, result);
        }

        #endregion
    }
}
