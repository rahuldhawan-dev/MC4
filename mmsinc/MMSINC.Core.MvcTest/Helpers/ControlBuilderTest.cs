using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;

namespace MMSINC.Core.MvcTest.Helpers
{
    [TestClass]
    public class ControlBuilderTest
    {
        #region Fields

        private TestControlBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new TestControlBuilder();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestHtmlAttributesDoesNotReturnNullDictionary()
        {
            var target = new TestControlBuilder();
            Assert.IsNotNull(target.HtmlAttributes);
        }

        [TestMethod]
        public void TestAddCssClassAddsClassAttributeAndValueToHtmlAttributes()
        {
            Assert.IsFalse(_target.HtmlAttributes.ContainsKey("class"));
            _target.AddCssClass("some-class");

            Assert.AreEqual("some-class", _target.HtmlAttributes["class"]);
        }

        [TestMethod]
        public void TestAddCssClassPutsSpaceBetweenMultipleClassNames()
        {
            _target.AddCssClass("some-class");
            _target.AddCssClass("some-other-class");
            Assert.AreEqual("some-class some-other-class", _target.HtmlAttributes["class"]);
        }

        [TestMethod]
        public void TestCreateTagBuilderReturnsTagBuilderWithHtmlAttributes()
        {
            _target.HtmlAttributes["sweet"] = "merciful crap";
            var tb = _target.GetTagBuilder();
            Assert.AreEqual("merciful crap", tb.Attributes["sweet"]);
        }

        [TestMethod]
        public void TestIdPropertyReturnsIdValueFromHtmlAttributes()
        {
            _target.HtmlAttributes["id"] = "yup";
            Assert.AreEqual("yup", _target.Id);
        }

        [TestMethod]
        public void TestIdPropertyReturnsNullIfIdKeyIsNotPresentInHtmlAttributes()
        {
            Assert.IsNull(_target.Id);
        }

        [TestMethod]
        public void TestNamePropertyReturnsNameValueFromHtmlAttributes()
        {
            _target.HtmlAttributes["name"] = "yup";
            Assert.AreEqual("yup", _target.Name);
        }

        [TestMethod]
        public void TestNamePropertyReturnsNullIfNameKeyIsNotPresentInHtmlAttributes()
        {
            Assert.IsNull(_target.Name);
        }

        [TestMethod]
        public void TestDoWithAddsAnonymousObjectAsHtmlAttributes()
        {
            var expected = new {
                mrs_dash = "yeah"
            };

            _target.DoWith(expected);
            Assert.AreSame(expected.mrs_dash, _target.HtmlAttributes["mrs-dash"]);
        }

        [TestMethod]
        public void TestDoWithAddsDictionaryAsHtmlAttributes()
        {
            var expected = new Dictionary<string, object>();
            expected["something"] = "yay";

            _target.DoWith(expected);
            Assert.AreSame("yay", _target.HtmlAttributes["something"]);
        }

        [TestMethod]
        public void TestDoWithAdds()
        {
            _target.DoWith("neat", "cool");
            Assert.AreEqual("cool", _target.HtmlAttributes["neat"]);
        }

        [TestMethod]
        public void TestDoWithCssClassAddsCssClass()
        {
            Assert.IsFalse(_target.HtmlAttributes.ContainsKey("class"));
            _target.DoWithCssClass("some-class");

            Assert.AreEqual("some-class", _target.HtmlAttributes["class"]);
        }

        #endregion

        #region Test class

        private class TestControlBuilder : ControlBuilder<TestControlBuilder>
        {
            protected override string CreateHtmlString()
            {
                throw new NotImplementedException();
            }

            public TagBuilder GetTagBuilder()
            {
                return CreateTagBuilder("div");
            }

            public new void DoWith(object htmlAttributes)
            {
                base.DoWith(htmlAttributes);
            }

            public new void DoWith(string key, object value)
            {
                base.DoWith(key, value);
            }

            public new void DoWithCssClass(string cssClass)
            {
                base.DoWithCssClass(cssClass);
            }
        }

        #endregion
    }
}
