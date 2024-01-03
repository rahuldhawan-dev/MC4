using System;
using System.Collections.Specialized;
using MMSINC.Common;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Common
{
    /// <summary>
    /// Summary description for QueryStringWrapperTest.
    /// </summary>
    [TestClass]
    public class QueryStringWrapperTest
    {
        #region Private Members

        private NameValueCollection _innerQueryString;
        private TestQueryStringWrapper _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void QueryStringWrapperTestInitialize()
        {
            _innerQueryString = new NameValueCollection();
            _target = new TestQueryStringWrapperBuilder(_innerQueryString);
        }

        #endregion

        [TestMethod]
        public void TestGetValueReturnsValueFromInnerCollectionWithSpecifiedKey()
        {
            string key = "key", value = "value";
            _innerQueryString.Add(key, value);

            Assert.AreEqual(value, _target.GetValue(key));
        }

        [TestMethod]
        public void TestGetValueReturnsNullWhenInnerCollectionDoesNotHaveSpecifiedKey()
        {
            var key = "DO NOT WANT";

            Assert.IsNull(_target.GetValue(key));
        }

        [TestMethod]
        public void TestGetValueTypedReturnsValueWithSpecifiedKeyCastToSpecifiedType()
        {
            var key = "key";
            object value = 1;
            _innerQueryString.Add(key, value.ToString());

            Assert.AreEqual((int)value, _target.GetValue<Int32>(key));

            value = (float)3.14159;
            _innerQueryString[key] = value.ToString();

            Assert.AreEqual((float)value, _target.GetValue<float>(key));

            value = DateTime.Now;
            _innerQueryString[key] = value.ToString();

            MyAssert.AreClose((DateTime)value, _target.GetValue<DateTime>(key));
        }

        [TestMethod]
        public void TestGetValueTypedReturnsNullWhenNoValueForSpecifiedKeyAndSpecifiedTypeIsNullable()
        {
            var key = "key";

            Assert.IsNull(_target.GetValue<int?>(key));
            Assert.IsNull(_target.GetValue<DateTime?>(key));
            Assert.IsNull(_target.GetValue<object>(key));
        }

        [TestMethod]
        public void TestGetValueTypedReturnsValueWithSpecifiedKeyCastToSpecifiedTypeUsingProvidedTransform()
        {
            var called = false;
            var key = "key";
            var value = 1;
            _innerQueryString.Add(key, value.ToString());

            Assert.AreEqual(value, _target.GetValue(key, val => {
                called = true;
                return Int32.Parse(val);
            }));
            Assert.IsTrue(called);
        }
    }

    internal class TestQueryStringWrapperBuilder : TestDataBuilder<TestQueryStringWrapper>
    {
        #region Private Members

        private readonly NameValueCollection _innerQueryString;

        #endregion

        #region Constructors

        public TestQueryStringWrapperBuilder(NameValueCollection queryString)
        {
            _innerQueryString = queryString;
        }

        #endregion

        #region Exposed Methods

        public override TestQueryStringWrapper Build()
        {
            var obj = new TestQueryStringWrapper(_innerQueryString);
            return obj;
        }

        #endregion
    }

    internal class TestQueryStringWrapper : QueryStringWrapper
    {
        #region Constructors

        public TestQueryStringWrapper(NameValueCollection queryString) : base(queryString) { }

        #endregion
    }
}
