using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class StreetRepositoryTest : InMemoryDatabaseTest<Street, StreetRepository>
    {
        #region Tests

        [TestMethod]
        public void TestFindByTownIdAndStreetNameReturnsSingleStreetInstanceThatMatchesTownAndStreet()
        {
            var town = GetFactory<TownFactory>().Create();
            var n = GetEntityFactory<StreetPrefix>().Create(new {Description = "N"});
            var s = GetEntityFactory<StreetPrefix>().Create(new {Description = "S"});
            var st = GetEntityFactory<StreetSuffix>().Create(new {Description = "St"});
            var ave = GetEntityFactory<StreetSuffix>().Create(new {Description = "Ave"});
            var goodStreet = GetFactory<StreetFactory>().Create(new {
                Town = town,
                Prefix = n,
                Name = "Some",
                Suffix = st
            });
            var sameStreetDiffTown = GetFactory<StreetFactory>().Create(new {
                Prefix = n,
                Name = "Some",
                Suffix = st
            });
            var streetDiffPrefix = GetFactory<StreetFactory>().Create(new {
                Town = town,
                Prefix = s,
                Name = "Some",
                Suffix = st
            });
            var streetDiffSuffix = GetFactory<StreetFactory>().Create(new {
                Town = town,
                Prefix = n,
                Name = "Some",
                Suffix = ave
            });

            var result = Repository.FindByTownIdAndStreetName(town.Id, "N", "Some", "St");
            Assert.AreSame(goodStreet, result);
        }

        [TestMethod]
        public void TestGetByTownReturnsByTown()
        {
            var townA = GetFactory<TownFactory>().Create();
            var townB = GetFactory<TownFactory>().Create();
            var townC = GetFactory<TownFactory>().Create();

            var streetAInTownA = GetFactory<StreetFactory>().Create(new { Town = townA });
            var streetBInTownA = GetFactory<StreetFactory>().Create(new { Town = townA });
            var streetCInTownB = GetFactory<StreetFactory>().Create(new { Town = townB });
            var streetDInTownC = GetFactory<StreetFactory>().Create(new { Town = townC });

            Session.Flush();

            var streetsInTownA = Repository.GetByTown(townA.Id).ToList();
            Assert.AreEqual(2, streetsInTownA.Count);
            CollectionAssert.Contains(streetsInTownA, streetAInTownA);
            CollectionAssert.Contains(streetsInTownA, streetBInTownA);

            var streetsInTownB = Repository.GetByTown(townB.Id).ToList();
            Assert.AreEqual(1, streetsInTownB.Count);
            CollectionAssert.Contains(streetsInTownB, streetCInTownB);

            var streetsInTownC = Repository.GetByTown(townC.Id).ToList();
            Assert.AreEqual(1, streetsInTownC.Count);
            CollectionAssert.Contains(streetsInTownC, streetDInTownC);

            var streetsInAllTownsExplicitly = Repository.GetByTown(townA.Id, townB.Id, townC.Id).ToList();
            Assert.AreEqual(4, streetsInAllTownsExplicitly.Count);
            CollectionAssert.Contains(streetsInAllTownsExplicitly, streetAInTownA);
            CollectionAssert.Contains(streetsInAllTownsExplicitly, streetBInTownA);
            CollectionAssert.Contains(streetsInAllTownsExplicitly, streetCInTownB);
            CollectionAssert.Contains(streetsInAllTownsExplicitly, streetDInTownC);

            var streetsInAllTownsImplicitly = Repository.GetByTown().ToList();
            Assert.AreEqual(4, streetsInAllTownsImplicitly.Count);
            CollectionAssert.Contains(streetsInAllTownsImplicitly, streetAInTownA);
            CollectionAssert.Contains(streetsInAllTownsImplicitly, streetBInTownA);
            CollectionAssert.Contains(streetsInAllTownsImplicitly, streetCInTownB);
            CollectionAssert.Contains(streetsInAllTownsImplicitly, streetDInTownC);
        }

        #endregion
    }
}
