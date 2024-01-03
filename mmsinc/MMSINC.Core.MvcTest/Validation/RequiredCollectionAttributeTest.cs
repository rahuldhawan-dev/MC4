using System;
using System.Collections.Generic;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Validation
{
    [TestClass]
    public class RequiredCollectionAttributeTest
    {
        #region Fields

        private RequiredCollectionAttribute _target;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new RequiredCollectionAttribute();
        }

        #endregion

        #region Tests

        #region Min property

        [TestMethod]
        public void TestMinPropertyDefaultsToOne()
        {
            // Creating a new instance just so the constructor
            // isn't messed up in the test initialize.
            var target = new RequiredCollectionAttribute();
            Assert.AreEqual(1, target.Min);
        }

        [TestMethod]
        public void TestMinPropertySetsAndGets()
        {
            var expected = 424;
            _target.Min = expected;
            Assert.AreEqual(expected, _target.Min);
        }

        [TestMethod]
        public void TestMinPropertyThrowsArgumentOutOfRangeExceptionIfValueIsLessThanZero()
        {
            MyAssert.Throws<ArgumentOutOfRangeException>(() => _target.Min = -1);
        }

        #endregion

        #region IsValid method

        [TestMethod]
        public void TestIsValidReturnsFalseIfObjectIsNull()
        {
            Assert.IsFalse(_target.IsValid(null));
        }

        [TestMethod]
        public void TestIsValidThrowsInvalidCastExceptionIfObjectIsString()
        {
            MyAssert.Throws<InvalidCastException>(() => _target.IsValid("string"));
        }

        [TestMethod]
        public void TestIsValidThrowsInvalidCastExceptionIfObjectIsNotIEnumerable()
        {
            MyAssert.Throws<InvalidCastException>(() => _target.IsValid(new object()));
        }

        [TestMethod]
        public void TestIsValidReturnsFalseIfTheAmountOfItemsIsLessThanMin()
        {
            var items = new List<object>();
            items.Add("wow");
            _target.Min = 2;
            Assert.IsFalse(_target.IsValid(items));
        }

        [TestMethod]
        public void TestIsValidReturnsTrueIfTheAmountOfItemsIsEqualToMin()
        {
            var items = new List<object>();
            items.Add("wow");
            _target.Min = 1;
            Assert.IsTrue(_target.IsValid(items));
        }

        [TestMethod]
        public void TestIsValidReturnsTrueIfTheAmountOfItemsIsGreaterThanMin()
        {
            var items = new List<object>();
            items.Add("wow");
            items.Add("another item");
            _target.Min = 1;
            Assert.IsTrue(_target.IsValid(items));
        }

        #endregion

        #endregion
    }
}
