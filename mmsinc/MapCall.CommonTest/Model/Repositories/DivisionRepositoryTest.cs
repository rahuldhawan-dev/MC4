using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class DivisionRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<Division, DivisionRepository>
    {
        [TestMethod]
        public void TestGetByStateIdReturnsByStateId()
        {
            var nj = GetEntityFactory<State>().Create(new {Name = "New Jersey", Abbreviation = "NJ"});
            var ny = GetEntityFactory<State>().Create(new {Name = "New York", Abbreviation = "NY"});
            var division1 = GetEntityFactory<Division>().Create(new {State = nj});
            var division2 = GetEntityFactory<Division>().Create(new {State = ny});

            var result = Repository.GetByStateId(nj.Id);

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(division1));
            Assert.IsFalse(result.Contains(division2));
        }
    }
}
