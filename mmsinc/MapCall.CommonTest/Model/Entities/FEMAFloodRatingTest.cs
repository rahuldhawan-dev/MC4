using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class FEMAFloodRatingTest
    {
        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var expected = "NJAW";
            var fr = new FEMAFloodRating {Description = expected};

            Assert.AreEqual(expected, fr.ToString());
        }
    }
}
