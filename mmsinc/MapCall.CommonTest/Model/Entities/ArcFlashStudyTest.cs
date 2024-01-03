using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ArcFlashStudyTest : InMemoryDatabaseTest<ArcFlashStudy>
    {
        [TestMethod]
        public void TestExpiringWithinYearReturnsTrueWhenExpiringWithin365Days()
        {
            var target = GetFactory<ArcFlashStudyFactory>().Create();

            Assert.IsNull(target.ExpiringWithinAYear);

            target = GetFactory<ArcFlashStudyFactory>().Create(new {
                DateLabelsApplied = DateTime.Now.SubtractYears(5).AddDays(15)
            });
            Session.Refresh(target);

            Assert.IsTrue(target.ExpiringWithinAYear.Value,
                $"{target.DateLabelsApplied.Value} + 5 years is not within the next 365 days");

            target = GetFactory<ArcFlashStudyFactory>().Create(new {
                DateLabelsApplied = DateTime.Now
            });
            Session.Refresh(target);

            Assert.IsFalse(target.ExpiringWithinAYear.Value,
                $"{target.DateLabelsApplied.Value} + 5 years is not within the next 365 days");
        }
    }
}
