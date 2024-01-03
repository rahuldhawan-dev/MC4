using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        RecurringFrequencyUnitRepositoryTest : InMemoryDatabaseTest<RecurringFrequencyUnit,
            RecurringFrequencyUnitRepository>
    {
        #region Tests

        [TestMethod]
        public void TestGetYearReturnsUnitWithDescriptionThatHasTheValueOfTheWordYearAsAString()
        {
            var expected = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create();
            Session.Flush();

            Assert.AreSame(expected, Repository.GetYear());
        }

        [TestMethod]
        public void TestGetYearThrowsExceptionIfThereIsNoYear()
        {
            MyAssert.Throws(() => Repository.GetYear());
        }

        #endregion
    }
}
