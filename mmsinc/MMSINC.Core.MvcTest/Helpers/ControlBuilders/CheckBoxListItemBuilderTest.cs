using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;

namespace MMSINC.Core.MvcTest.Helpers.ControlBuilders
{
    [TestClass]
    public class CheckBoxListItemBuilderTest
    {
        #region Fields

        private CheckBoxListItemBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new CheckBoxListItemBuilder();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestToHtmlStringRendersWithCheckedAttributeIfCheckedPropertyIsTrue()
        {
            _target.Checked = true;
            Assert.AreEqual("<mc-checkboxlistitem checked=\"\"></mc-checkboxlistitem>", _target.ToHtmlString());

            _target.Checked = false;
            Assert.AreEqual("<mc-checkboxlistitem></mc-checkboxlistitem>", _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringAddsTextAttributeIfValueIsNotNull()
        {
            _target.Text = null;
            Assert.AreEqual("<mc-checkboxlistitem></mc-checkboxlistitem>", _target.ToHtmlString());

            _target.Text = "Neato";
            Assert.AreEqual("<mc-checkboxlistitem text=\"Neato\"></mc-checkboxlistitem>", _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringAddsValueAttributeIfValueIsNotNull()
        {
            _target.Value = null;
            Assert.AreEqual("<mc-checkboxlistitem></mc-checkboxlistitem>", _target.ToHtmlString());

            _target.Value = 431;
            Assert.AreEqual("<mc-checkboxlistitem value=\"431\"></mc-checkboxlistitem>", _target.ToHtmlString());
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
        public void TestWithTextReturnsSelf()
        {
            Assert.AreSame(_target, _target.WithText(null));
        }

        [TestMethod]
        public void TestWithValueSetsTextProperty()
        {
            _target.WithText("Neat");
            Assert.AreSame("Neat", _target.Text);
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
