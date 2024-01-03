using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class PipeDiameterTest
    {
        [TestMethod]
        public void TestToStringReturnsDiameter()
        {
            var expected = 1;
            var target = new PipeDiameter {Diameter = expected};

            Assert.AreEqual(expected.ToString(), target.ToString());
        }
    }
}
