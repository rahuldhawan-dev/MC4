using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IQueryableExtensions;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class ScadaTagNameRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<ScadaTagName, ScadaTagNameRepository>
    {
        [TestMethod]
        public void TestFindByPartialNameMatchReturnsResults()
        {
            var scadaTagName1 = GetEntityFactory<ScadaTagName>().Create(new {
                TagName = "AMW_DW_CN_EO_SD_PFWM", Description = "SchanckRoadIC Schanck Rd IC Flow Total previous day"
            });
            var scadaTagName2 = GetEntityFactory<ScadaTagName>().Create(new {
                TagName = "AMW_DW_CN_EO_SD_SWP", Description = "NewmanSpringsMainService Newman Springs Main Service"
            });

            var results = Repository.FindByPartialNameMatch("spring");

            Assert.AreEqual(1, results.Count());
        }
    }
}
