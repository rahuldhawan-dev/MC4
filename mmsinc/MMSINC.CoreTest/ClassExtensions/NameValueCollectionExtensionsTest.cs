using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.CoreTest.ClassExtensions
{
    [TestClass]
    public class NameValueCollectionExtensionsTest
    {
        #region Fields

        private NameValueCollection _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new NameValueCollection();
        }

        #endregion

        #region GetValue

        [TestMethod]
        public void TestGetValueAsReturnsExpectedValueConvertedToTypeWhenNameValueCollectionContainsMatchingKey()
        {
            var valueAsString = "5";
            var expectedValueAsDecimal = (decimal)5;
            _target.Add("key", valueAsString);
            Assert.AreEqual(expectedValueAsDecimal, _target.GetValueAs<decimal>("key").Value);
        }

        [TestMethod]
        public void TestGetValueAsReturnsNullForTypeIfKeyDoesNotExist()
        {
            Assert.IsNull(_target.GetValueAs<decimal>("key"));
        }

        [TestMethod]
        public void TestGetValueAsThrowsExceptionIfItCantConvertValueToSuppliedType()
        {
            _target.Add("key", "some value that would never be a decimal");
            MyAssert.Throws(() => _target.GetValueAs<decimal>("key"));
        }

        #region GetValue with enums

        public enum Oh
        {
            Yeah = 32
        }

        [TestMethod]
        public void TestGetValueForEnumWorksWithTheNamedRepresentationOfAnEnumValue()
        {
            _target.Add("key", Oh.Yeah.ToString());
            Assert.AreEqual(Oh.Yeah, _target.GetValueAs<Oh>("key").Value);
        }

        [TestMethod]
        public void TestGetValueForEnumWorksWithNumericalRepresentationOfAnEnumValue()
        {
            _target.Add("key", ((int)Oh.Yeah).ToString());
            Assert.AreEqual(Oh.Yeah, _target.GetValueAs<Oh>("key").Value);
        }

        #endregion

        [TestMethod]
        public void TestGetValueWorksWithDates()
        {
            var valueAsString = "4/24/1984 4:04AM";
            var expectedValue = new DateTime(1984, 4, 24, 4, 4, 0);
            _target.Add("key", valueAsString);
            Assert.AreEqual(expectedValue, _target.GetValueAs<DateTime>("key").Value);

            valueAsString = "today";
            expectedValue = DateTime.Now;
            expectedValue = expectedValue.AddSeconds(-expectedValue.Second);
            _target["key"] = valueAsString;
            MyAssert.AreClose(expectedValue, _target.GetValueAs<DateTime>("key").Value, TimeSpan.FromMinutes(1));
        }

        #endregion

        #region FindKeys

        [TestMethod]
        public void TestFindKeysFindsKeysByStringPart()
        {
            var expected = new[] {
                "work order", "planning work order", "scheduling work order"
            };
            var extra = new[] {
                "work", "order", "planning", "scheduling", "markout"
            };

            new[] {expected, extra}.Each(a => a.Each(s => _target.Add(s, string.Empty)));

            var actual = _target.FindKeys("work order").ToArray();

            Assert.AreEqual(expected.Length, actual.Length);
            actual.Each(s => Assert.IsTrue(expected.Contains(s)));
        }

        #endregion

        #region EnsureValue

        [TestMethod]
        public void TestEnsureValueThrowsKeyNotFoundExceptionIfCollectionDoesNotContainGivenKey()
        {
            MyAssert.Throws<KeyNotFoundException>(() => _target.EnsureValue("some fake key"));
        }

        [TestMethod]
        public void TestEnsureValueReturnsValueIfCollectionContainsTheGivenKey()
        {
            var key = "the key";
            var value = "the value";
            _target.Add(key, value);

            MyAssert.DoesNotThrow(() => Assert.AreEqual(value, _target.EnsureValue(key)));
        }

        #endregion

        #region Each

        [TestMethod]
        public void TestEachDoesNotThrowExceptionForNullKey()
        {
            var nvc = new NameValueCollection {{"password", "foo"}, {"foobar", "baz"}, {null, "foo"}};
            var dict = new Dictionary<string, string>();

            MyAssert.DoesNotThrow(() => nvc.Each(dict.Add));
        }

        #endregion

        #region ToQueryString()

        [TestMethod]
        public void TestToQueryStringConvertsCollectionToAQueryString()
        {
            var coll = new NameValueCollection {{"foo", "bar"}, {"foo", "baz"}, {"foobar", "baz"}};

            Assert.AreEqual("?foo=bar&foo=baz&foobar=baz", coll.ToQueryString());
        }

        [TestMethod]
        public void TestToQueryStringReturnsEmptyStringIfCollectionIsNull()
        {
            Assert.AreEqual(string.Empty, ((NameValueCollection)null).ToQueryString());
        }

        #endregion
    }
}
