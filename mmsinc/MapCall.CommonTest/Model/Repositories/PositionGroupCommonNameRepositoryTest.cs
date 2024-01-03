using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.NHibernate;
using NHibernate.Linq;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        PositionGroupCommonNameRepositoryTest : InMemoryDatabaseTest<PositionGroupCommonName,
            PositionGroupCommonNameRepository>
    {
        [TestMethod]
        public void TestSearchByTrainingModuleSearchesCorrectly()
        {
            var cat1 = GetFactory<TrainingModuleCategoryFactory>().Create(new {Description = "Some category"});

            var expectedTrainingModule = GetEntityFactory<TrainingModule>().Create(new {
                TrainingModuleCategory = cat1,
                Title = "Neat"
            });
            var someOtherTrainingModule = GetEntityFactory<TrainingModule>().Create();

            var pgcn = GetFactory<PositionGroupCommonNameFactory>().Create(new {Description = "This one"});
            var req = GetFactory<TrainingRequirementFactory>().Create();
            req.TrainingModules.Add(expectedTrainingModule);
            pgcn.TrainingRequirements.Add(req);
            expectedTrainingModule.TrainingRequirement = req;

            Session.Flush();
            expectedTrainingModule = Session.Query<TrainingModule>().Single(x => x.Id == expectedTrainingModule.Id);

            var search = new TestSearchTrainingModulePositionGroupCommonName();
            search.EnablePaging = false;
            search.TrainingModule = expectedTrainingModule.Id;
            var result = Repository.SearchByTrainingModule(search).Single();

            Assert.AreEqual("Neat", result.ModuleTitle);
            Assert.AreEqual("Some category", result.ModuleCategory);
            Assert.AreEqual("This one", result.PositionGroupCommonName);
        }

        private class TestSearchTrainingModulePositionGroupCommonName :
            SearchSet<TrainingModulePositionGroupCommonNameReportItem>, ISearchTrainingModulePositionGroupCommonName
        {
            public int? TrainingModule { get; set; }
            public bool? IsOSHARequirement { get; set; }
            public int[] Ids { get; set; }
        }
    }
}
