using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class DepartmentTest
    {
        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var expected = "Production";
            var department = new Department {Description = expected};

            Assert.AreEqual(expected, department.ToString());
        }
    }
}
