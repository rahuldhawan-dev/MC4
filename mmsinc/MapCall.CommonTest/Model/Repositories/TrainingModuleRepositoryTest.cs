using System.Linq;
using System.Web.UI.WebControls;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class TrainingModuleRepositoryTest : InMemoryDatabaseTest<TrainingModule, TrainingModuleRepository>
    {
        [TestMethod]
        public void TestGetActiveModulesOnlyReturnsActiveModules()
        {
            var validModule = GetEntityFactory<TrainingModule>().Create(new {IsActive = true});
            var invalidModule = GetEntityFactory<TrainingModule>().Create(new {IsActive = false});

            var target = Repository.GetActiveTrainingModules();

            Assert.IsTrue(target.Contains(validModule));
            Assert.IsFalse(target.Contains(invalidModule));
        }
    }
}
