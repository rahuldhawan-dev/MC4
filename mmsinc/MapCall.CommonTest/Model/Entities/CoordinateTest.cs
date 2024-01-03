using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.ObjectExtensions;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class CoordinateTest
    {
        [TestMethod]
        public void TestToStringReturnsId()
        {
            var expected = 123;
            var coordinate = new Coordinate();
            coordinate.SetPropertyValueByName("Id", expected);

            Assert.AreEqual(expected, coordinate.Id);
        }
    }
}
