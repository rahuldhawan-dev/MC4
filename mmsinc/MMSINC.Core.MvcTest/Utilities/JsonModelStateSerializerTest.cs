using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;

namespace MMSINC.Core.MvcTest.Utilities
{
    [TestClass]
    public class JsonModelStateSerializerTest
    {
        #region Fields

        private JsonModelStateSerializer _target;
        private ModelStateDictionary _modelState;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new JsonModelStateSerializer();
            _modelState = new ModelStateDictionary();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestCamelCaseKeysDefaultsToFalse()
        {
            Assert.IsFalse(new JsonModelStateSerializer().CamelCaseKeys);
        }

        [TestMethod]
        public void TestCamelCaseKeysPropertyGetsAndSets()
        {
            MyAssert.CanGetAndSetProperty(_target, () => _target.CamelCaseKeys);
        }

        [TestMethod]
        public void TestSerializeErrorsThrowsExceptionIfModelStateIsNotSet()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.SerializeErrors(null));
        }

        [TestMethod]
        public void TestSerializeErrorsOnlyReturnsModelStateErrors()
        {
            _target.CamelCaseKeys = false;
            var expectedError = "This is an error";
            _modelState.Add("Good", new ModelState());
            _modelState.AddModelError("Bad", expectedError);
            var result = _target.SerializeErrors(_modelState);
            Assert.IsFalse(result.ContainsKey("Good"), "ModelStates without errors should not be included");
            Assert.IsTrue(result.ContainsKey("Bad"));
            var errors = result["Bad"].ToArray();
            Assert.AreEqual(1, errors.Count());
            Assert.AreEqual(expectedError, errors.First());
        }

        [TestMethod]
        public void TestSerializeErrorsReturnsCamelCasedKeyIfCamelCaseKeysIsTrue()
        {
            _target.CamelCaseKeys = true;
            _modelState.AddModelError("CamelMyCase", "error");
            var result = _target.SerializeErrors(_modelState);
            Assert.IsFalse(result.ContainsKey("CamelMyCase"));
            Assert.IsTrue(result.ContainsKey("camelMyCase"));
        }

        [TestMethod]
        public void TestSerializeErrorsDoesNotReturnCamelCasedKeyIfCamelCaseKeysIsFalse()
        {
            _target.CamelCaseKeys = false;
            _modelState.AddModelError("CamelMyCase", "error");
            var result = _target.SerializeErrors(_modelState);
            Assert.IsFalse(result.ContainsKey("camelMyCase"));
            Assert.IsTrue(result.ContainsKey("CamelMyCase"));
        }

        #region SerializeKey

        private void TestSerializeKeys(bool camelCase, string toSerialize, string expected)
        {
            _target.CamelCaseKeys = camelCase;
            Assert.AreEqual(expected, _target.SerializeKey(toSerialize));
        }

        [TestMethod]
        public void TestSerializeKeyReturnsSameValuePassedInIfCamelCaseKeysIsFalse()
        {
            TestSerializeKeys(false, "Oh", "Oh");
        }

        [TestMethod]
        public void TestSerializeKeyCamelCasesSimplePropertyNameIfCamelCaseKeysIsTrue()
        {
            TestSerializeKeys(true, "PropertyWithWords", "propertyWithWords");
        }

        [TestMethod]
        public void TestSerializeKeyCamelCasesNestedPropertyNamesIfCamelCaseKeysIsTrue()
        {
            TestSerializeKeys(true, "Property.NestedProperty", "property.nestedProperty");
        }

        [TestMethod]
        public void TestSerializeKeyCamelCasesArrayIndexedPropertyNamesIfCamelCaseKeysIsTrue()
        {
            TestSerializeKeys(true, "ArrayProperty[32]", "arrayProperty[32]");
            TestSerializeKeys(true, "Property.ArrayProperty[32]", "property.arrayProperty[32]");
        }

        #endregion

        #endregion
    }
}
