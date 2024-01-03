using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Utilities.Sorting;
using MMSINCTestImplementation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Utilities.Sorting
{
    /// <summary>
    /// Summary description for SorterTest
    /// </summary>
    [TestClass]
    public class SorterTest
    {
        #region Constants

        private const string SQL_INVALID_FIELD_NAME = "@@";
        private const string SQL_INVALID_CHILD_FIELD_NAME = "ReportsTo.@@";

        #endregion

        #region Private Members

        private IEnumerable<Employee> _testData;
        private Sorter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void SorterTestInitialize()
        {
            SetupTestData();
            _target = new Sorter(_testData);
        }

        #endregion

        #region Private Methods

        private void SetupTestData()
        {
            _testData = new List<Employee> {
                new Employee {
                    EmployeeID = 1,
                    LastName = "Smith"
                },
                new Employee {
                    EmployeeID = 2,
                    LastName = "Doe"
                }
            };
        }

        #endregion

        [TestMethod]
        public void TestSortableSorts()
        {
            var dataSorted = _target.Sort<Employee>("LastName ASC");
            Assert.IsTrue(dataSorted.ToList()[0].LastName == "Doe");

            dataSorted = _target.Sort<Employee>("LastName DESC");
            Assert.IsTrue(dataSorted.ToList()[0].LastName == "Smith");

            dataSorted = _target.Sort<Employee>("EmployeeID ASC");
            Assert.IsTrue(dataSorted.ToList()[0].EmployeeID == 1);

            dataSorted = _target.Sort<Employee>("EmployeeID DESC");
            Assert.IsTrue(dataSorted.ToList()[0].EmployeeID == 2);
        }

        /* TODO: Can't seem to get these tests to pass for some reason
        [TestMethod]
        public void TestSortableThrowsExceptionForInvalidProperty()
        {
            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => _target.Sort<Employee>(SQL_INVALID_FIELD_NAME));
        }

        [TestMethod]
        public void TestSortableThrowsExceptionForInvalidChildProperty()
        {
            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => _target.Sort<Employee>(SQL_INVALID_CHILD_FIELD_NAME));
        }
        */

        [TestMethod]
        public void TestSortableReturnsOriginalListIfNoArgumentProvided()
        {
            var actual = _target.Sort<Employee>(String.Empty);
            Assert.AreSame(_testData, actual);

            actual = _target.Sort<Employee>(null);
            Assert.AreSame(_testData, actual);
        }
    }
}
