using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;

namespace MMSINC.Core.MvcTest.Helpers.ControlBuilders
{
    [TestClass]
    public class CheckBoxBuilderTest
    {
        #region Fields

        private CheckBoxBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new CheckBoxBuilder();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestToHtmlStringRendersWithCheckedAttributeIfCheckedPropertyIsTrue()
        {
            _target.Checked = true;
            Assert.AreEqual("<input checked=\"checked\" type=\"checkbox\" />", _target.ToHtmlString());

            _target = new CheckBoxBuilder();
            _target.Checked = false;
            Assert.AreEqual("<input type=\"checkbox\" />", _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringAddsValueAttributeIfValueIsNotNull()
        {
            _target.Value = null;
            Assert.AreEqual("<input type=\"checkbox\" />", _target.ToHtmlString());

            _target = new CheckBoxBuilder();
            _target.Value = 431;
            Assert.AreEqual("<input type=\"checkbox\" value=\"431\" />", _target.ToHtmlString());
        }

        [TestMethod]
        public void TestIsCheckedReturnsSelf()
        {
            Assert.AreSame(_target, _target.IsChecked(true));
            Assert.AreSame(_target, _target.IsChecked(false));
        }

        [TestMethod]
        public void TestIsCheckedSetsCheckedProperty()
        {
            _target.IsChecked(true);
            Assert.IsTrue(_target.Checked);
            _target.IsChecked(false);
            Assert.IsFalse(_target.Checked);
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
        public void TestWithValueReturnsSelf()
        {
            Assert.AreSame(_target, _target.WithValue(null));
        }

        [TestMethod]
        public void TestWithValueSetsValueProperty()
        {
            _target.WithValue("Neat");
            Assert.AreSame("Neat", _target.Value);
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
