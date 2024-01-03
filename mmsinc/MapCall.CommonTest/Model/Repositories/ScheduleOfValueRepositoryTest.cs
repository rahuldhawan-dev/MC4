using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        ScheduleOfValueRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<ScheduleOfValue, ScheduleOfValueRepository>
    {
        [TestMethod]
        public void TestGetByScheduleOfValueCategoryIdReturnsByScheduleOfValueCategory()
        {
            var scheduleOfValueCategories = GetEntityFactory<ScheduleOfValueCategory>().CreateList(2);
            var scheduleOfValuesGood = GetEntityFactory<ScheduleOfValue>()
               .CreateList(3, new {ScheduleOfValueCategory = scheduleOfValueCategories[0]});
            var scheduleOfValuesBad = GetEntityFactory<ScheduleOfValue>()
               .CreateList(8, new {ScheduleOfValueCategory = scheduleOfValueCategories[1]});

            var result = Repository.GetByScheduleOfValueCategoryId(scheduleOfValueCategories[0].Id);

            Assert.AreEqual(3, result.Count());
        }
    }
}
