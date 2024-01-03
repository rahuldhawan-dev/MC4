using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class FacilityStatusTest
    {
        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var expected = "Active";
            var department = new FacilityStatus {Description = expected};

            Assert.AreEqual(expected, department.ToString());
        }
    }
}
