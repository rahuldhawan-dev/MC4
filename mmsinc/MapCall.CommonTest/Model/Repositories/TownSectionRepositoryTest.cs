using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class TownSectionRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<TownSection, TownSectionRepository>
    {
        #region Tests

        [TestMethod]
        public void TestGetByTownReturnsByTown()
        {
            var townA = GetEntityFactory<Town>().Create();
            var townB = GetEntityFactory<Town>().Create();
            var townC = GetEntityFactory<Town>().Create();

            var townSectionAInTownA = GetEntityFactory<TownSection>().Create(new { Town = townA });
            var townSectionBInTownA = GetEntityFactory<TownSection>().Create(new { Town = townA });
            var townSectionCInTownB = GetEntityFactory<TownSection>().Create(new { Town = townB });
            var townSectionDInTownC = GetEntityFactory<TownSection>().Create(new { Town = townC });

            Session.Flush();

            var townSectionsInTownA = Repository.GetByTown(townA.Id).ToList();
            Assert.AreEqual(2, townSectionsInTownA.Count);
            CollectionAssert.Contains(townSectionsInTownA, townSectionAInTownA);
            CollectionAssert.Contains(townSectionsInTownA, townSectionBInTownA);

            var townSectionsInTownB = Repository.GetByTown(townB.Id).ToList();
            Assert.AreEqual(1, townSectionsInTownB.Count);
            CollectionAssert.Contains(townSectionsInTownB, townSectionCInTownB);

            var townSectionsInTownC = Repository.GetByTown(townC.Id).ToList();
            Assert.AreEqual(1, townSectionsInTownC.Count);
            CollectionAssert.Contains(townSectionsInTownC, townSectionDInTownC);

            var townSectionsInAllTownsExplicitly = Repository.GetByTown(townA.Id, townB.Id, townC.Id).ToList();
            Assert.AreEqual(4, townSectionsInAllTownsExplicitly.Count);
            CollectionAssert.Contains(townSectionsInAllTownsExplicitly, townSectionAInTownA);
            CollectionAssert.Contains(townSectionsInAllTownsExplicitly, townSectionBInTownA);
            CollectionAssert.Contains(townSectionsInAllTownsExplicitly, townSectionCInTownB);
            CollectionAssert.Contains(townSectionsInAllTownsExplicitly, townSectionDInTownC);

            var townSectionsInAllTownsImplicitly = Repository.GetByTown().ToList();
            Assert.AreEqual(4, townSectionsInAllTownsExplicitly.Count);
            CollectionAssert.Contains(townSectionsInAllTownsImplicitly, townSectionAInTownA);
            CollectionAssert.Contains(townSectionsInAllTownsImplicitly, townSectionBInTownA);
            CollectionAssert.Contains(townSectionsInAllTownsImplicitly, townSectionCInTownB);
            CollectionAssert.Contains(townSectionsInAllTownsImplicitly, townSectionDInTownC);
        }

        [TestMethod]
        public void TestGetActiveByTownReturnsActiveByTown()
        {
            var townA = GetEntityFactory<Town>().Create();
            var townB = GetEntityFactory<Town>().Create();
            var townC = GetEntityFactory<Town>().Create();

            var activeTownSectionAInTownA = GetEntityFactory<TownSection>().Create(new { Town = townA, Active = true });
            var activeTownSectionDInTownC = GetEntityFactory<TownSection>().Create(new { Town = townC, Active = true });
            GetEntityFactory<TownSection>().Create(new { Town = townA, Active = false });
            GetEntityFactory<TownSection>().Create(new { Town = townB, Active = false });
            
            Session.Flush();

            var townSectionsInTownA = Repository.GetActiveByTown(townA.Id).ToList();
            Assert.AreEqual(1, townSectionsInTownA.Count);
            CollectionAssert.Contains(townSectionsInTownA, activeTownSectionAInTownA);

            var townSectionsInTownB = Repository.GetActiveByTown(townB.Id).ToList();
            Assert.AreEqual(0, townSectionsInTownB.Count);

            var townSectionsInTownC = Repository.GetActiveByTown(townC.Id).ToList();
            Assert.AreEqual(1, townSectionsInTownC.Count);
            CollectionAssert.Contains(townSectionsInTownC, activeTownSectionDInTownC);

            var townSectionsInAllTownsExplicitly = Repository.GetActiveByTown(townA.Id, townB.Id, townC.Id).ToList();
            Assert.AreEqual(2, townSectionsInAllTownsExplicitly.Count);
            CollectionAssert.Contains(townSectionsInAllTownsExplicitly, activeTownSectionAInTownA);
            CollectionAssert.Contains(townSectionsInAllTownsExplicitly, activeTownSectionDInTownC);

            var townSectionsInAllTownsImplicitly = Repository.GetActiveByTown().ToList();
            Assert.AreEqual(2, townSectionsInAllTownsExplicitly.Count);
            CollectionAssert.Contains(townSectionsInAllTownsImplicitly, activeTownSectionAInTownA);
            CollectionAssert.Contains(townSectionsInAllTownsImplicitly, activeTownSectionDInTownC);
        }

        #endregion
    }
}
