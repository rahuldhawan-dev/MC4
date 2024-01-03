using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using MMSINC.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MMSINC.Core.MvcTest.Helpers.ControlBuilders
{
    [TestClass]
    public class HiddenInputBuilderTest
    {
        #region Fields

        private HiddenInputBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new HiddenInputBuilder();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestToHtmlStringRendersValue()
        {
            _target.Value = 12345;
            Assert.AreEqual("<input type=\"hidden\" value=\"12345\" />", _target.ToHtmlString());
        }

        [TestMethod]
        public void TestWithValueSetsValueProperty()
        {
            var expected = new object();
            _target.WithValue(expected);
            Assert.AreEqual(expected, _target.Value);
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
        public void TestExcludeClientSideValidationRemovesClientSideValidation()
        {
            _target.HtmlAttributes["data-val-required"] = "this property is required";
            // sanity check
            Assert.AreEqual("<input data-val-required=\"this property is required\" type=\"hidden\" value=\"\" />", _target.ToHtmlString());

            var result = _target.ExcludeClientSideValidation();

            Assert.AreEqual("<input type=\"hidden\" value=\"\" />", _target.ToHtmlString());
        }
        
        #endregion
    }
}
