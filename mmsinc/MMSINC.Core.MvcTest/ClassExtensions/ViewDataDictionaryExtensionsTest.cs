using System;
using System.Web.Mvc;
using MMSINC.ClassExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.ClassExtensions
{
    [TestClass]
    public class ViewDataDictionaryExtensionsTest
    {
        #region ViewDataDictionary.GetValueOrDefault

        [TestMethod]
        public void TestGetValueOrDefaultReturnsTheDefaultValueWhenViewDataDoesNotHaveAMatchingKey()
        {
            var target = new ViewDataDictionary();
            var key = "some key you are!";
            // Using an object instead of a valuetype
            // so we can use AreSame instead of AreEqual.
            var expectedDefault = new Object();
            Assert.AreSame(expectedDefault, target.GetValueOrDefault(key, expectedDefault));
        }

        [TestMethod]
        public void TestGetValueOrDefaultReturnsViewDataValueWhenViewDataDoesHaveMatchingKey()
        {
            var target = new ViewDataDictionary();
            var key = "some key you are!";
            // Using an object instead of a valuetype
            // so we can use AreSame instead of AreEqual.
            var expectedValue = new Object();
            var notExpectedDefault = new Object();
            target.Add(key, expectedValue);
            Assert.AreSame(expectedValue, target.GetValueOrDefault(key, notExpectedDefault));
        }

        [TestMethod]
        public void TestGetValueWorksWithGenericViewDataDictionaryToo()
        {
            var target = new ViewDataDictionary<object>();
            var key = "some key you are!";
            // Using an object instead of a valuetype
            // so we can use AreSame instead of AreEqual.
            var expectedValue = new Object();
            var notExpectedDefault = new Object();
            target.Add(key, expectedValue);
            Assert.AreSame(expectedValue, target.GetValueOrDefault(key, notExpectedDefault));
        }

        #endregion
    }
}
