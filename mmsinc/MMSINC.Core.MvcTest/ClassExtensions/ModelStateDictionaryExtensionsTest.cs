using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.ClassExtensions
{
    [TestClass]
    public class ModelStateDictionaryExtensionsTest
    {
        #region Fields

        private ModelStateDictionary _target;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new ModelStateDictionary();
        }

        #endregion

        #region Private Methods

        private void AddKeyValue(string key, object value)
        {
            var vpr = new ValueProviderResult(value, value.ToString(), CultureInfo.CurrentCulture);
            _target.Add(key, new ModelState {Value = vpr});
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestToRouteValueDictionaryAddsAllKeysAndAttemptedValues()
        {
            AddKeyValue("SomeKey", "SomeValue");
            AddKeyValue("Child.Key", "AnotherValue");
            AddKeyValue("Child.Key2", "A2Value");
            AddKeyValue("Boolean", true);

            var result = _target.ToRouteValueDictionary();

            Assert.AreEqual("SomeValue", result["SomeKey"]);
            Assert.AreEqual("AnotherValue", result["Child.Key"]);
            Assert.AreEqual("A2Value", result["Child.Key2"]);
            Assert.AreEqual("True", result["Boolean"]);
        }

        [TestMethod]
        public void TestToRouteValueDictionaryReturnsEmptyDictionaryIfModelStateIsEmpty()
        {
            var result = _target.ToRouteValueDictionary();
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TestToDictionaryOfErrors()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("One", "Single error message.");
            modelState.AddModelError("Two", "First error for Two.");
            modelState.AddModelError("Two", "Second error for Two.");
            modelState.Add("NoError", new ModelState());

            var result = modelState.ToDictionaryOfErrors();
            Assert.AreEqual(2, result.Count, "There should only be two keys because NoError has no errors.");
            Assert.AreEqual("Single error message.", result["One"]);
            Assert.AreEqual("First error for Two. Second error for Two.", result["Two"],
                "Two had two error messages that should have been combined into a single string.");
        }

        #endregion
    }
}
