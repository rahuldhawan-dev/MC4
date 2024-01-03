using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.ClassExtensions
{
    [TestClass]
    public class IEnumerableExtensionsTest
    {
        #region Fields

        private string[] _target = new[] {"duuuuuuh", null, string.Empty, "Hello!"};

        #endregion

        #region Each

        [TestMethod]
        public void TestEachReturnsSourceObjectAtTheEnd()
        {
            var result = _target.Each(s => { /* noop. chuck testa */
            });
            Assert.AreSame(_target, result);
        }

        [TestMethod]
        public void TestEachPerformsActionOnAllItemsInCollection()
        {
            var actionItems = new List<string>();
            _target.Each(actionItems.Add);

            foreach (var item in _target)
            {
                Assert.IsTrue(actionItems.Contains(item));
            }
        }

        [TestMethod]
        public void TestEachThrowsExceptionForNullAction()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.Each(null));
        }

        #endregion

        #region Range

        [TestMethod]
        public void TestRangeReturnsAnEnumerableThatIncludesTheStartAndEndValuesAndAllValuesInBetween()
        {
            var result = IEnumerableExtensions.Range(10, 15);
            // ReSharper disable PossibleMultipleEnumeration
            Assert.AreEqual(10, result.First());
            Assert.AreEqual(15, result.Last());

            var arr = result.ToArray();
            Assert.AreEqual(10, arr[0]);
            Assert.AreEqual(11, arr[1]);
            Assert.AreEqual(12, arr[2]);
            Assert.AreEqual(13, arr[3]);
            Assert.AreEqual(14, arr[4]);
            Assert.AreEqual(15, arr[5]);
            // ReSharper restore PossibleMultipleEnumeration
        }

        #endregion
    }
}
