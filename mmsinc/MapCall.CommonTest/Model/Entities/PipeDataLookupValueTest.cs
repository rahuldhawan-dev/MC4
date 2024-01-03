using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class PipeDataLookupValueTest
    {
        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var expected = "spanish inquisition";
            var target = new PipeDataLookupValue {Description = expected};

            Assert.AreEqual(expected, target.ToString());
        }
    }
}
