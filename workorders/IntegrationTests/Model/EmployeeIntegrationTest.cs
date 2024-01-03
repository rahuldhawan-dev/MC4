using System;
using System.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for EmployeeTestTest
    /// </summary>
    [TestClass]
    public class EmployeeIntegrationTest : WorkOrdersTestClass<Employee>
    {
        #region Constants

        public const int MIN_EXPECTED_COUNT = 617;
        public const int REFERENCE_EMPLOYEE_ID = 397;
        public const string REFERENCE_EMPLOYEE_NAME = "MapCall Developer";

        #endregion

        #region Private Methods

        protected override Employee GetValidObjectFromDatabase()
        {
            return GetValidEmployee();
        }

        protected override Employee GetValidObject()
        {
            return GetValidObjectFromDatabase();
        }

        protected override void DeleteObject(Employee entity)
        {
            DeleteEmployee(entity);
        }

        #endregion

        #region Exposed Static Methods

        public static Employee GetValidEmployee()
        {
            return EmployeeRepository.GetEntity(REFERENCE_EMPLOYEE_ID);
        }

        public static void DeleteEmployee(Employee entity)
        {
            throw new DomainLogicException("Cannot delete Employee objects in this context.");
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void EmployeeIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void EmployeeIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestSampleData()
        {
            const string disclaimer = "This may have undesirable consequences on some automated tests.  Please refer to the MMSINC Wiki for " +
                                      "proper setup instructions for this project.";

            using (_simulator.SimulateRequest())
            {
                MyAssert.IsGreaterThanOrEqualTo(new EmployeeRepository().Count(), MIN_EXPECTED_COUNT,
                                                "Your test database has less than the expected number of records in tblPermissions.  " +
                                                disclaimer);

                var target =
                    EmployeeRepository.GetEntity(
                        REFERENCE_EMPLOYEE_ID);
                Assert.IsNotNull(target);
                
                Assert.AreEqual("mcUser", target.UserName);
            }
        }

        [TestMethod]
        public void TestRepositorySelectAllAsListReturnsEmployeesAlphabeticallyByName()
        {
            string last = null;
            using (_simulator.SimulateRequest())
            {
                foreach (var employee in EmployeeRepository.SelectAllAsList())
                {
                    // The \t is being ignored because someone is importing employees somewhere on the SAP side with a tab between first and last name.
                    // Also sql server and C# seem to have a difference in opinion about the order of names when dashes or spaces are involved.
                    if (!String.IsNullOrEmpty(last) && !last.Contains("'") && !last.Contains("\t") && !last.Contains("-") && !last.Contains(" "))
                        Assert.IsTrue(employee.FullName.ToUpper().CompareTo(last.ToUpper()) >= 0, String.Format("current: {0} : last: {1}", employee.FullName, last));
                    last = employee.FullName;
                }
            }
        }

        [TestMethod]
        public void TestRepositorySelectByNamePart()
        {
            const string namePart = "joh";

            using (_simulator.SimulateRequest())
            {
                var results = EmployeeRepository.SelectByNamePart(namePart);

                foreach (var employee in results)
                    Assert.IsTrue(employee.FullName.ToLower().Contains(namePart));
            }
        }

        [TestMethod]
        public void TestRepositorySelectByNamePartReturnsEmployeesAlphabeticallyByName()
        {
            string last = null;
            const string namePart = "joh";
            using (_simulator.SimulateRequest())
            {
                // filter out the irish, sql and .net seem to disagree
                // on how they should be sorted.
                var list = EmployeeRepository
                          .SelectByNamePart(namePart)
                          .Where(e => !e.FullName.Contains("'"));
                foreach (var employee in list)
                {
                    var current = employee.FullName.Trim();

                    if (!string.IsNullOrEmpty(last))
                    {
                        Assert.IsTrue(string.Compare(current, last, StringComparison.Ordinal) >= 0,
                            $"Name '{last}' seems to come after '{employee.FullName}' for some reason.");
                    }

                    last = current.Trim();
                }
            }
        }

        [TestMethod]
        public void TestRepositorySelectAllAsListLimitedToACount()
        {
            var count = 20;

            using (_simulator.SimulateRequest())
                Assert.AreEqual(count,
                                EmployeeRepository.SelectAllAsList(count).Count);
        }

        [TestMethod]
        public void TestRepositorySelectAllAsListLimitedToACountReturnsAllResultsWhenCountIsZeroOrLess()
        {
            using (_simulator.SimulateRequest())
            {
                Assert.AreNotEqual(0,
                                   EmployeeRepository.SelectAllAsList(0).Count);

                Assert.AreNotEqual(0, EmployeeRepository.SelectAllAsList(-1).Count);
            }
        }

        [TestMethod]
        public void TestRepositorySelectByNamePartLimitedToACount()
        {
            var count = 15;
            const string namePart = "joh";

            using (_simulator.SimulateRequest())
                Assert.AreEqual(count,
                                EmployeeRepository.SelectByNamePart(namePart, count).Count);
        }

        [TestMethod]
        public void TestRepositorySelectByNamePartLimitedToACountReturnsAllResultsWhenCountIsZeroOrLess()
        {
            const string namePart = "joh";

            using (_simulator.SimulateRequest())
            {
                Assert.AreNotEqual(0,
                                   EmployeeRepository.SelectByNamePart(namePart, 0).Count);

                Assert.AreNotEqual(0,
                                   EmployeeRepository.SelectByNamePart(namePart, -1).Count);
            }
        }

        [TestMethod]
        public void TestRepositorySelectByNamePartReturnsAllResultsWhenNamePartIsNullOrEmpty()
        {
            using (_simulator.SimulateRequest())
            {
                var totalCount = EmployeeRepository.SelectAllAsList().Count;

                Assert.AreEqual(totalCount,
                                EmployeeRepository.SelectByNamePart(null).Count);
                Assert.AreEqual(totalCount,
                                EmployeeRepository.SelectByNamePart(String.Empty).Count);
            }
        }
        
        [TestMethod]
        public void TestCannotCreateNewEmployee()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new Employee { UserName = "Test User" };

                MyAssert.Throws(() => EmployeeRepository.Insert(target),
                                typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotAlterEmployee()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.UserName = "foobar";

                MyAssert.Throws(() => EmployeeRepository.Update(target),
                                typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestCannotDeleteEmploye()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(() => EmployeeRepository.Delete(target),
                                typeof(DomainLogicException));
            }
        }

        [TestMethod]
        public void TestEmployeeHasOperatingCenters()
        {
            using (_simulator.SimulateRequest())
            {
                var mcuser = EmployeeRepository.GetEntity(REFERENCE_EMPLOYEE_ID);
                MyAssert.IsGreaterThan(mcuser.OperatingCentersUsers.Count, 1);
            }
        }
    }
}