using System;
using System.Collections.Generic;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Utilities
{
    /// <summary>
    /// Summary description for QueryStringHelperTest
    /// </summary>
    [TestClass]
    public class QueryStringHelperTest
    {
        #region BuildFromDictionary

        [TestMethod]
        public void TestJoinUrlWithQueryStringThrowsForNullOrWhiteSpaceBaseUrl()
        {
            MyAssert.Throws(
                () => QueryStringHelper.JoinUrlWithQueryString(null, null));

            MyAssert.Throws(
                () => QueryStringHelper.JoinUrlWithQueryString(string.Empty, null));

            MyAssert.Throws(
                () => QueryStringHelper.JoinUrlWithQueryString("        ", null));
        }

        [TestMethod]
        public void TestBuildFromDictionaryThrowsExceptionIfPassedNullArgument()
        {
            MyAssert.Throws<ArgumentNullException>(
                () => QueryStringHelper.BuildFromDictionary(null));
        }

        [TestMethod]
        public void TestBuildFromDictionaryDoesNotThrowWhenKeyValuePairHasNullValue()
        {
            var target = new Dictionary<string, object>();
            target["I'mnull"] = null;

            QueryStringHelper.BuildFromDictionary(target);
        }

        [TestMethod]
        public void TestBuildFromDictionaryReturnsExpectedFormattedValue()
        {
            var target = new Dictionary<string, object>();
            target["key1"] = "value";
            target["key2"] = "anotherValue";

            var expected = "key1=value&key2=anotherValue";
            var result = QueryStringHelper.BuildFromDictionary(target);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestBuildFromDictionaryDoesNotAttachQuestionMarkToBeginningOfResult()
        {
            var target = new Dictionary<string, object>();
            target["key1"] = "value";
            target["key2"] = "anotherValue";

            var result = QueryStringHelper.BuildFromDictionary(target);

            Assert.IsFalse(result.StartsWith("?"),
                "The QueryStringHelper isn't supposed to append a ? to the beginning of a statement.");
        }

        [TestMethod]
        public void TestBuildFromDictionaryOverloadAttachesQueryStringToUrl()
        {
            var url = "http://www.blahblah.com/blah";
            var target = new Dictionary<string, object>();
            target["key1"] = "value";
            target["key2"] = "anotherValue";

            var expected =
                "http://www.blahblah.com/blah?key1=value&key2=anotherValue";

            var result = QueryStringHelper.BuildFromDictionary(url, target);

            Assert.AreEqual(expected, result);
        }

        #endregion

        #region BuildFromKeyValuePair

        [TestMethod]
        public void TestBuildFromKeyValuePairThrowsForNullOrWhiteSpaceKey()
        {
            MyAssert.Throws<InvalidOperationException>(
                () => QueryStringHelper.BuildFromKeyValuePair(null, null));

            MyAssert.Throws<InvalidOperationException>(
                () => QueryStringHelper.BuildFromKeyValuePair(string.Empty, null));

            MyAssert.Throws<InvalidOperationException>(
                () => QueryStringHelper.BuildFromKeyValuePair("         ", null));
        }

        [TestMethod]
        public void TestBuildFromKeyValuePairReturnsExpectedFormattedValue()
        {
            var expectedKey = "key";
            var expectedValue = "value";
            var expectedOutput = "key=value";

            var result = QueryStringHelper.BuildFromKeyValuePair(expectedKey,
                expectedValue);

            Assert.AreEqual(expectedOutput, result);
        }

        #endregion
    }
}
