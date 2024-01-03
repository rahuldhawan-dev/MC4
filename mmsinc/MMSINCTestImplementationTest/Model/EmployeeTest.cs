using System;
using System.Data.Linq.Mapping;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Exceptions;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINCTestImplementation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINCTestImplementationTest.Model
{
    /// <summary>
    /// Summary description for EmployeeTest
    /// </summary>
    [TestClass]
    public class EmployeeTest : LinqUnitTestClass<Employee>
    {
        #region Exposed Static Methods

        public static Employee GetValidEmployee()
        {
            return new Employee {
                FirstName = "Alex",
                LastName = "Rystrom",
                BirthDate = DateTime.Parse("08/30/1970")
            };
        }

        protected override Employee GetValidObjectFromDatabase()
        {
            return EmployeeRepository.GetEntity(1);
        }

        public static void DeleteEmployee(Employee entity)
        {
            EmployeeRepository.Delete(entity);
        }

        #endregion

        #region Exposed Methods

        protected override Employee GetValidObject()
        {
            return GetValidEmployee();
        }

        protected override void DeleteObject(Employee entity)
        {
            DeleteEmployee(entity);
        }

        #endregion

        [TestMethod]
        public void TestChangeBirthDateAfterSaveThrowsException()
        {
            Employee target = new Employee {BirthDate = DateTime.Now};

            MyAssert.Throws(() => target.BirthDate = DateTime.Now.SubtractYears(20),
                typeof(DomainLogicException));
        }

        [TestMethod]
        public void TestFullNamePropertyReflectsFirstAndLastName()
        {
            Employee target = new Employee {
                FirstName = "John",
                LastName = "Smith"
            };

            Assert.AreEqual("Smith, John", target.FullName);
        }

        [TestMethod]
        public void TestToStringReflectsFullNameProperty()
        {
            Employee target = new Employee {
                FirstName = "John",
                LastName = "Smith"
            };

            Assert.AreEqual(target.FullName, target.ToString());
        }

        [TestMethod]
        public void TestEmployeesOtherKeyIsSetToReportsTo()
        {
            var target = new Employee();
            var expected = new AssociationAttribute();
            var propertyInfo = target.GetType().GetProperties();
            foreach (var propInfo in propertyInfo)
            {
                var custAttr = propInfo.GetCustomAttributes(true);
                if (custAttr == null || custAttr.Length <= 0) continue;
                foreach (var attr in custAttr)
                {
                    if (custAttr[0] is AssociationAttribute)
                        if (((AssociationAttribute)custAttr[0]).Name == "Employee_Employees")
                            expected = (AssociationAttribute)custAttr[0];
                }
            }

            Assert.AreEqual("ReportsToID", expected.OtherKey,
                "Linq Generated the wrong OtherKey for you. Has to be fixed manually.");
        }

        [TestMethod]
        public void TestEmployeeWasNotBornYesterday()
        {
            var target = new Employee {BirthDate = DateTime.Today.Date.AddDays(-1)};
            var repository = new MockRepository<Employee>();
            MyAssert.Throws(() => repository.InsertNewEntity(target), typeof(DomainLogicException));
        }
    }

    public class MockEmployeeRepository : MockRepository<Employee> { }
}
