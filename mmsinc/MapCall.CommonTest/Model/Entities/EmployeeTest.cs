using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class EmployeeTest
    {
        [TestMethod]
        public void TestFullNameReturnsFormattedFullName()
        {
            string first = "Bill", last = "Preston, Esq", middle = "S.";

            var target = new Employee {FirstName = first, MiddleName = middle, LastName = last};

            Assert.AreEqual(String.Format(Employee.FULL_NAME_FORMAT, first, middle, last), target.ToString());

            target.MiddleName = "";

            Assert.AreEqual(first + " " + last, target.ToString());
        }

        [TestMethod]
        public void TestDisplayFormatReturnsFormattedDisplayFormat()
        {
            var expected = "Preston, Bill S. : 11223344";
            string first = "Bill", last = "Preston", middle = "S.", employeeId = "11223344";

            var target = new Employee {
                FirstName = first,
                MiddleName = middle,
                LastName = last,
                EmployeeId = employeeId
            };

            Assert.AreEqual(expected, target.Display);
        }

        [TestMethod]
        public void TestIsActiveReturnsFalseIfStatusIsNull()
        {
            var target = new Employee {
                Status = null
            };
            Assert.IsFalse(target.IsActive);
        }

        [TestMethod]
        public void TestIsActiveReturnsFalseIfStatusDescriptionIsNotActive()
        {
            var target = new Employee {
                Status = new EmployeeStatus {
                    Description = "Not Active at all"
                }
            };

            Assert.IsFalse(target.IsActive);
        }

        [TestMethod]
        public void TestIsActiveReturnsTrueIfStatusDescriptionIsActive()
        {
            var target = new Employee {
                Status = new EmployeeStatus {
                    Description = "Active"
                }
            };

            Assert.IsTrue(target.IsActive);
        }

        [TestMethod]
        public void TestIsLicensedReturnsFalseWhenNoLicenses()
        {
            var target = new Employee();

            Assert.IsFalse(target.IsLicensed);
        }

        [TestMethod]
        public void TestIsLicensedReturnsTrueWhenLicensed()
        {
            var target = new Employee {TLicense = "Foo"};

            Assert.IsTrue(target.IsLicensed);

            target = new Employee {WLicense = "Foo"};

            Assert.IsTrue(target.IsLicensed);

            target = new Employee {CLicense = "Foo"};

            Assert.IsTrue(target.IsLicensed);

            target = new Employee {SLicense = "Foo"};

            Assert.IsTrue(target.IsLicensed);

            target = new Employee {NLicense = "Foo"};

            Assert.IsTrue(target.IsLicensed);
        }
    }
}
