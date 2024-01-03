using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class LambdaComparerTest
    {
        #region Fields

        private LambdaComparer<Lambchop> _equalityTarget;
        private LambdaComparer<Lambchop> _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _equalityTarget = new LambdaComparer<Lambchop>((first, sec) => first.Id == sec.Id);
            _target = new LambdaComparer<Lambchop>(x => x.Id);
        }

        #endregion

        #region Tests

        #region Equals

        [TestMethod]
        public void TestEqualsReturnsTrueIfGivenSameInstance()
        {
            var lamb = new Lambchop();
            Assert.IsTrue(_equalityTarget.Equals(lamb, lamb));
        }

        [TestMethod]
        public void TestEqualsReturnsTrueIfDifferentInstancesHaveSamePropertyValue()
        {
            var firstLamb = new Lambchop {Id = 1};
            var secondLamb = new Lambchop {Id = 1};
            Assert.IsTrue(_equalityTarget.Equals(firstLamb, secondLamb));
        }

        [TestMethod]
        public void TestEqualsReturnsFalseIfDifferenceInstancesHaveDifferentPropertyValue()
        {
            var firstLamb = new Lambchop {Id = 1};
            var secondLamb = new Lambchop {Id = 2};
            Assert.IsFalse(_equalityTarget.Equals(firstLamb, secondLamb));
        }

        #endregion

        #region GetHashCode

        [TestMethod]
        public void TestGetHashCodeThrowsNullExceptionForNullObject()
        {
            MyAssert.Throws<NullReferenceException>(() => _equalityTarget.GetHashCode(null));
        }

        [TestMethod]
        public void TestGetHashCodeReturnsObjectsHashCode()
        {
            var lamb = new Lambchop();
            Assert.AreEqual(lamb.GetHashCode(), _equalityTarget.GetHashCode(lamb));
        }

        #endregion

        #region Compare

        [TestMethod]
        public void TestCompareReturnsZeroIfLeftAndRightAreEqual()
        {
            var left = new Lambchop {Id = 1};
            var right = new Lambchop {Id = 1};
            Assert.AreEqual(0, _target.Compare(left, right));
        }

        [TestMethod]
        public void TestCompareReturnsOneIfLeftIsGreaterThanRight()
        {
            var left = new Lambchop {Id = 2};
            var right = new Lambchop {Id = 1};
            Assert.AreEqual(1, _target.Compare(left, right));
        }

        [TestMethod]
        public void TestCompareReturnsNegativeOneIfLeftIsLessThanRight()
        {
            var left = new Lambchop {Id = 1};
            var right = new Lambchop {Id = 2};
            Assert.AreEqual(-1, _target.Compare(left, right));
        }

        #endregion

        #endregion

        #region Helper class

        private class Lambchop
        {
            public int Id { get; set; }
        }

        #endregion
    }
}
