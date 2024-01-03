using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class SecureFormValueProviderTest
    {
        #region Fields

        private SecureFormToken _token;
        private SecureFormValueProvider<SecureFormToken, SecureFormDynamicValue> _target;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _token = new SecureFormToken();
            _target = new SecureFormValueProvider<SecureFormToken, SecureFormDynamicValue>(_token);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestContainsPrefixReturnsTrueIfTokenContainsDynamicValueWithPrefixAsKey()
        {
            _token.DynamicValues.Add(new SecureFormDynamicValue
                {SecureFormToken = _token, Key = "Blah", Value = "sup"});
            Assert.IsTrue(_target.ContainsPrefix("Blah"));
        }

        [TestMethod]
        public void TestContainsPrefixReturnsFalseIfTokenDoesNotContainDynamicValueWithPrefixAsKey()
        {
            Assert.IsFalse(_target.ContainsPrefix("I am not here"));
        }

        [TestMethod]
        public void TestGetValueReturnsExpectedValueProviderResult()
        {
            _token.DynamicValues.Add(
                new SecureFormDynamicValue {SecureFormToken = _token, Key = "TheKey", Value = true});

            var result = _target.GetValue("TheKey");

            Assert.AreEqual(true, result.RawValue);
            Assert.AreEqual("True", result.AttemptedValue);
        }

        [TestMethod]
        public void TestGetValueReturnsNullIfContainsPrefixIsFalseForSomeKey()
        {
            var unexpectedKey = "oh";

            Assert.IsFalse(_target.ContainsPrefix(unexpectedKey));
            Assert.IsNull(_target.GetValue(unexpectedKey));
        }

        [TestMethod]
        public void TestContainsPrefixAndGetValueWorkRegardlessOfCase()
        {
            var key1 = "key1";
            var value1 = 1;
            var key2 = "KEY2";
            var value2 = false;
            _token.DynamicValues.Add(new SecureFormDynamicValue {SecureFormToken = _token, Key = key1, Value = value1});
            _token.DynamicValues.Add(new SecureFormDynamicValue {SecureFormToken = _token, Key = key2, Value = value2});

            Action<string, object> testKeyIsValid = (key, value) => {
                Assert.IsTrue(_target.ContainsPrefix(key));
                Assert.AreEqual(value, _target.GetValue(key).RawValue);
            };

            testKeyIsValid("key1", value1);
            testKeyIsValid("Key1", value1);
            testKeyIsValid("KEY1", value1);
            testKeyIsValid("key2", value2);
            testKeyIsValid("Key2", value2);
            testKeyIsValid("KEY2", value2);
        }

        #endregion
    }
}
