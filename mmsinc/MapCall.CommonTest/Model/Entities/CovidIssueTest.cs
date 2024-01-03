using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class CovidIssueTest
    {
        [TestMethod]
        public void TestTotalDaysReturnsCorrectValueWhenBothDatesPopulated()
        {
            var target = new CovidIssue
                {StartDate = new DateTime(2020, 3, 13), ReleaseDate = new DateTime(2020, 3, 27)};

            Assert.AreEqual(14, target.TotalDays);
        }
        
        [TestMethod]
        public void TestTotalDaysReturnsNullWhenReleaseDateNull()
        {
            var target = new CovidIssue {StartDate = new DateTime(2020, 3, 13),};

            Assert.IsNull(target.TotalDays);
        }

        [TestMethod]
        public void TestTotalDaysReturnsNullWhenStartDateNull()
        {
            var target = new CovidIssue {ReleaseDate = new DateTime(2020, 3, 13),};

            Assert.IsNull(target.TotalDays);
        }    
        
        [TestMethod]
        public void TestTotalDaysReturnsNullWhenBothDatesBeNull()
        {
            var target = new CovidIssue();

            Assert.IsNull(target.TotalDays);
        }
    }
}
