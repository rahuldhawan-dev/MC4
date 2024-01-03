using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;

namespace MMSINC.CoreTest.Data
{
    [TestClass]
    public class SearchSetTest
    {
        #region Tests

        [TestMethod]
        public void TestConstructorSetsResultsToEmptyCollection()
        {
            var target = new TestSearchSet();
            Assert.IsFalse(target.Results.Any());
        }

        [TestMethod]
        public void TestResultsSetterClearsTheInternalCollectionAndThenAddsNewItemsToTheSameInternalCollection()
        {
            var target = new TestSearchSet();
            var expectedFirst = new List<object>();
            expectedFirst.Add(new object());
            var expectedSecond = new List<object>();
            expectedSecond.Add(new object());

            // ReSharper disable PossibleMultipleEnumeration

            var expectedList = target.Results;
            target.Results = expectedFirst;
            Assert.AreSame(expectedList, target.Results);
            Assert.AreEqual(1, expectedList.Count());
            Assert.IsTrue(expectedList.Contains(expectedFirst.Single()));
            target.Results = expectedSecond;
            Assert.AreSame(expectedList, target.Results);
            Assert.AreEqual(1, expectedList.Count());
            Assert.IsTrue(expectedList.Contains(expectedSecond.Single()));

            // ReSharper restore PossibleMultipleEnumeration
        }

        #endregion

        #region Test class

        private class TestSearchSet : SearchSet<object> { }

        #endregion
    }
}
