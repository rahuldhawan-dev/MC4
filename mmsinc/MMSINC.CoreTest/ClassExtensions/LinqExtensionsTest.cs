using System.Collections.Generic;
using System.Data.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Utilities.Sorting;
using MMSINCTestImplementation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINC.CoreTest.ClassExtensions
{
    [TestClass]
    public class LinqExtensionsTest
    {
        #region IEnumerable Extensions

        [TestMethod]
        public void TestMaxOrDefaultReturnsMinimumValueFromASet()
        {
            int expected = 5, actual;
            var set = new List<int> {
                1, 2, 3, 4, expected, 4, 3, 2, 1
            };

            actual = set.MaxOrDefault();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMaxOrDefaultReturnsDefaultValueOfSetTypeIfSetIsEmpty()
        {
            var intSet = new List<int>();
            var objectSet = new List<object>();

            Assert.AreEqual(default(int), intSet.MaxOrDefault());
            Assert.AreEqual(default(object), objectSet.MaxOrDefault());
        }

        [TestMethod]
        public void TestSortingReturnsSortableObject()
        {
            var data = new List<Employee>();
            var target = data.Sorting();
            Assert.IsInstanceOfType(target, typeof(ISorter));
        }

        [TestMethod]
        public void TestSettingSortableFactoryChangesResultOfCallingSorting()
        {
            var mocks = new MockRepository();
            var sorting = mocks.DynamicMock<ISorter>();
            mocks.ReplayAll();
            IEnumerableExtensions.SetSortingFactory(set => sorting);

            Assert.AreSame(sorting, new List<object>().Sorting());

            IEnumerableExtensions.ResetSortingFactory();
            mocks.VerifyAll();
        }

        [TestMethod]
        public void TestResetSortingFactory()
        {
            IEnumerableExtensions.SetSortingFactory(set => null);
            IEnumerableExtensions.ResetSortingFactory();

            var result = new List<object>().Sorting();
            Assert.IsInstanceOfType(result, typeof(ISorter));

            // and because we want a concrete class here:
            Assert.IsInstanceOfType(result, typeof(Sorter));
        }

        #endregion
    }
}
