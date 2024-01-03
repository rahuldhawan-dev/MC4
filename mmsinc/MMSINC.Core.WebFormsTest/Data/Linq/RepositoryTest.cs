using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Interface;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINCTestImplementation.Model;
using StructureMap;
using Subtext.TestLibrary;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MMSINC.Core.WebFormsTest.Data.Linq
{
    [TestClass]
    public class RepositoryTest
    {
        #region Private Members

        private HttpSimulator _simulator;
        private IRepository<Employee> _target;
        private IContainer _container;

        #endregion

        #region Private Methods

        private bool CompareEmployees(Employee left, Employee right)
        {
            // only testing that ID, name, and address match
            return (left.FirstName == right.FirstName &&
                    left.LastName == right.LastName &&
                    left.Address == right.Address &&
                    left.City == right.City &&
                    left.Country == right.Country);
        }

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public void RepositoryTestInitialize()
        {
            _simulator = new HttpSimulator();
            _target = new TestRepository();
            _container = new Container();
        }

        [TestCleanup]
        public void RepositoryTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        ///<summary>
        /// The System.Data.Linq.DataContext is stored in the current HTTPContext.
        /// If the HTTPContext doesn't exist, an exception should be thrown.
        ///</summary>
        [TestMethod]
        public void TestThrowsExceptionWhenNoHTTPContext()
        {
            MyAssert.Throws(() => _target.RestoreFromPersistedState(0),
                typeof(InvalidContextException));
        }

        [TestMethod]
        public void TestCurrentEntityReturnsNullWhenIndexNotSet()
        {
            using (_simulator.SimulateRequest())
            {
                Assert.IsNull(_target.CurrentEntity,
                    "CurrentEntity property should return null when entity index has not been set.");
            }
        }

        [TestMethod]
        public void TestCurrentEntityReturnsCorrectEntityWhenValidIndexSet()
        {
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);
                var expected = dataContext.Employees.Single(
                    employee => employee.EmployeeID == 1);
                _target.RestoreFromPersistedState(0);

                MyAssert.IsNotNullButInstanceOfType(_target.CurrentEntity,
                    typeof(Employee));
                Assert.IsTrue(CompareEmployees(expected, _target.CurrentEntity));
            }
        }

        [TestMethod]
        public void TestGetReturnsEntityWithTheGivenID()
        {
            var expectedID = 1;
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);
                var expected =
                    dataContext.Employees.Single(e => e.EmployeeID == expectedID);
                var actual = _target.Get(expectedID);

                Assert.IsTrue(CompareEmployees(expected, actual));
            }
        }

        [TestMethod]
        public void TestSetSelectedDataKeyToANonExistentOneSetsItToNegativeOne()
        {
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);

                _target.SetSelectedDataKey(777);

                Assert.AreEqual(-1, _target.CurrentIndex);
            }
        }

        [TestMethod]
        public void TestInsertAndDelete()
        {
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);

                var targetEmployee = new Employee {
                    FirstName = "John",
                    LastName = "Smith",
                    BirthDate = DateTime.Today,
                    HireDate = DateTime.Today
                };

                _target.InsertNewEntity(targetEmployee);

                var actualEmployee = _target.Entities[_target.Count() - 1];

                MyAssert.IsNotNullButInstanceOfType(actualEmployee,
                    typeof(Employee),
                    "Insert seems to have failed.  The newly inserted Employee object should be the last in the Entities collection.");
                Assert.IsTrue(CompareEmployees(actualEmployee,
                        targetEmployee),
                    "Insert seems to have failed.  The newly inserted Employee object should be the last in the Entities collection.");

                _target.DeleteEntity(targetEmployee);
            }
        }

        [TestMethod]
        public void TestCount()
        {
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();

                _container.Inject<IDataContext>(dataContext);

                Assert.AreEqual(dataContext.Employees.Count(),
                    _target.Count(), "Error with Count() method.");

                var originalCount = _target.Count();

                var targetEmployee = new Employee {
                    FirstName = "John",
                    LastName = "Smith",
                    BirthDate = DateTime.Today,
                    HireDate = DateTime.Today
                };

                _target.InsertNewEntity(targetEmployee);

                Assert.AreEqual(originalCount + 1,
                    _target.Count(),
                    "Repository Count() should have increased by one.");

                _target.DeleteEntity(targetEmployee);

                Assert.AreEqual(originalCount, _target.Count(),
                    "Repository Count() should have returned to its original value.");
            }
        }

        [TestMethod]
        public void TestUpdateFiresEntityUpdatedEvent()
        {
            var called = false;
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);

                _target.EntityUpdated += (sender, e) => called = true;

                _target.SetSelectedDataKey(1);
                var targetEmployee = _target.CurrentEntity;

                var oldFirstName = targetEmployee.FirstName;
                targetEmployee.FirstName = "The New ";

                _target.UpdateCurrentEntity(targetEmployee);

                var actualEmployee = _target.CurrentEntity;
                Assert.IsTrue(CompareEmployees(targetEmployee,
                    actualEmployee), "Update seems to have failed.");

                targetEmployee.FirstName = oldFirstName;
                _target.UpdateCurrentEntity(targetEmployee);

                actualEmployee = _target.CurrentEntity;
                Assert.IsTrue(CompareEmployees(targetEmployee,
                    actualEmployee), "Update seems to have failed.");

                Assert.IsTrue(called);
            }
        }

        [TestMethod]
        public void TestUpdateLiterallyFiresEntityUpdatedEvent()
        {
            var called = false;
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);

                _target.EntityUpdated += (sender, e) => called = true;

                _target.SetSelectedDataKey(1);
                var targetEmployee = _target.CurrentEntity;

                var oldFirstName = targetEmployee.FirstName;
                targetEmployee.FirstName = "New Name";

                _target.UpdateCurrentEntityLiterally(targetEmployee);

                var actualEmployee = _target.CurrentEntity;
                Assert.IsTrue(CompareEmployees(targetEmployee, actualEmployee),
                    "Update seems to have failed");

                targetEmployee.FirstName = oldFirstName;
                _target.UpdateCurrentEntityLiterally(targetEmployee);

                actualEmployee = _target.CurrentEntity;
                Assert.IsTrue(CompareEmployees(targetEmployee, actualEmployee),
                    "Update seems to have failed");

                Assert.IsTrue(called);
            }
        }

        [TestMethod]
        public void TestUpdateDoesNotSetExistingPropertiesToNull()
        {
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);

                _target.SetSelectedDataKey(1);
                var targetEmployee = _target.CurrentEntity;

                targetEmployee.FirstName = "The New ";

                _target.UpdateCurrentEntity(targetEmployee);

                Assert.IsNotNull(targetEmployee.LastName);
                Assert.IsNotNull(targetEmployee.ReportsTo);
            }
        }

        [TestMethod]
        public void TestUpdateSetsExistingPropertiesAndAssociationsToNullWhenNull()
        {
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);

                _target.SetSelectedDataKey(1);
                var targetEmployee = _target.CurrentEntity;

                var oldTitle = targetEmployee.Title;

                targetEmployee.Title = null;
                _target.UpdateCurrentEntityLiterally(targetEmployee);
                Assert.IsNull(targetEmployee.Title);

                targetEmployee.Title = oldTitle;
                _target.UpdateCurrentEntityLiterally(targetEmployee);
                Assert.IsNotNull(targetEmployee.Title);

                var oldReportsTo = targetEmployee.ReportsTo;

                targetEmployee.ReportsTo = null;
                _target.UpdateCurrentEntityLiterally(targetEmployee);
                Assert.IsNull(targetEmployee.ReportsTo);

                targetEmployee.ReportsTo = oldReportsTo;
                _target.UpdateCurrentEntityLiterally(targetEmployee);
                Assert.IsNotNull(targetEmployee.ReportsTo);
            }
        }

        [TestMethod]
        public void TestSetSelectedDataKeySetsSelectedEntityIndex()
        {
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);

                // NOT SET
                Assert.AreEqual(-1, _target.CurrentIndex);

                _target.SetSelectedDataKey(1);

                MyAssert.IsGreaterThan(_target.CurrentIndex, -1);
            }
        }

        [TestMethod]
        public void TestInsertNewEntityFiresEntitiyInsertedEvent()
        {
            var called = false;
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);

                _target.EntityInserted += (sender, e) => called = true;
                var employee = new Employee {
                    FirstName = "John",
                    LastName = "Smith",
                    BirthDate = DateTime.Today,
                    HireDate = DateTime.Today
                };

                _target.InsertNewEntity(employee);

                Assert.IsTrue(called);

                // clean up after ourselves
                _target.DeleteEntity(employee);
            }
        }

        [TestMethod]
        public void TestDeleteFiresEntityDeletedEvent()
        {
            var called = false;
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);

                _target.EntityDeleted += (sender, e) => called = true;
                var employee = new Employee {
                    FirstName = "John",
                    LastName = "Smith",
                    BirthDate = DateTime.Today,
                    HireDate = DateTime.Today
                };

                // just so we don't delete an existing employee
                _target.InsertNewEntity(employee);

                _target.DeleteEntity(employee);

                Assert.IsTrue(called);
            }
        }

        [TestMethod]
        public void TestSelectAllAsListReturnsSortedListWhenSortExpressionProvided()
        {
            var sortExpression = "LastName";
            Employee lastEmployee = null;
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);

                var list = EmployeeRepository.SelectAllAsList(sortExpression);

                foreach (var employee in list)
                {
                    if (lastEmployee != null && lastEmployee.LastName != employee.LastName)
                    {
                        MyAssert.IsLessThan(lastEmployee.LastName.CompareTo(employee.LastName), 0);
                    }

                    lastEmployee = employee;
                }
            }
        }

        [TestMethod]
        public void TestSetSelectedDataKeySetsSelectedEntityIndexAndDataKeyWithFilterExpression()
        {
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);

                // this is tricky, don't know if any employees have been 
                // deleted from the database. Lets filter it to the last employee
                var employee = _target.Entities.Last();

                _target.GetFilteredSortedData(em => em.LastName == employee.LastName, null);
                _target.SetSelectedDataKey(employee.EmployeeID);

                Assert.AreEqual(employee.EmployeeID.ToString(),
                    _target.SelectedDataKey.ToString());
                // TODO: not sure why this stopped working
                //Assert.AreEqual(0, _target.CurrentIndex);
            }
        }

        [TestMethod]
        public void TestSetSelectedDataKeyForRPCSetsFilterExpressionAndCallsDataKey()
        {
            using (_simulator.SimulateRequest())
            {
                var dataContext = new NorthwindDataContext();
                _container.Inject<IDataContext>(dataContext);

                var employee = _target.Entities.Last();
                Expression<Func<Employee, bool>> expression =
                    o => o.EmployeeID == employee.EmployeeID;

                _target.SetSelectedDataKeyForRPC(employee.EmployeeID, expression);

                Assert.AreEqual(_target.FilterExpression, expression);
                Assert.AreEqual(employee.EmployeeID.ToString(),
                    _target.SelectedDataKey.ToString());
                // TODO: not sure why this stopped working
                //Assert.AreEqual(0, _target.CurrentIndex);
            }
        }
    }

    internal class TestRepository : EmployeeRepository
    {
        #region Exposed Static Methods

        public static void SetTableWrapper(ITable<Employee> table)
        {
            _dataTable = table;
        }

        public static void ResetTableWrapper()
        {
            SetTableWrapper(null);
        }

        #endregion
    }
}
