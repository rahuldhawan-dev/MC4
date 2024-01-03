using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class GradientRepositoryTest : InMemoryDatabaseTest<Gradient, GradientRepository>
    {
        #region Tests

        [TestMethod]
        public void TestGetByTownReturnsByTown()
        {
            var townA = GetEntityFactory<Town>().Create();
            var townB = GetEntityFactory<Town>().Create();
            var townC = GetEntityFactory<Town>().Create();

            var gradientAInTownA = GetEntityFactory<Gradient>().Create();
            var gradientBInTownA = GetEntityFactory<Gradient>().Create();
            var gradientCInTownB = GetEntityFactory<Gradient>().Create();
            var gradientDInTownC = GetEntityFactory<Gradient>().Create();
            
            gradientAInTownA.Towns.Add(townA);
            gradientBInTownA.Towns.Add(townA);
            gradientCInTownB.Towns.Add(townB);
            gradientDInTownC.Towns.Add(townC);

            Session.Flush();

            var gradientsInTownA = Repository.GetByTown(townA.Id).ToList();
            Assert.AreEqual(2, gradientsInTownA.Count);
            CollectionAssert.Contains(gradientsInTownA, gradientAInTownA);
            CollectionAssert.Contains(gradientsInTownA, gradientBInTownA);

            var gradientsInTownB = Repository.GetByTown(townB.Id).ToList();
            Assert.AreEqual(1, gradientsInTownB.Count);
            CollectionAssert.Contains(gradientsInTownB, gradientCInTownB);

            var gradientsInTownC = Repository.GetByTown(townC.Id).ToList();
            Assert.AreEqual(1, gradientsInTownC.Count);
            CollectionAssert.Contains(gradientsInTownC, gradientDInTownC);

            var gradientsInAllTownsExplicitly = Repository.GetByTown(townA.Id, townB.Id, townC.Id).ToList();
            Assert.AreEqual(4, gradientsInAllTownsExplicitly.Count);
            CollectionAssert.Contains(gradientsInAllTownsExplicitly, gradientAInTownA);
            CollectionAssert.Contains(gradientsInAllTownsExplicitly, gradientBInTownA);
            CollectionAssert.Contains(gradientsInAllTownsExplicitly, gradientCInTownB);
            CollectionAssert.Contains(gradientsInAllTownsExplicitly, gradientDInTownC);

            var gradientsInAllTownsImplicitly = Repository.GetByTown().ToList();
            Assert.AreEqual(4, gradientsInAllTownsImplicitly.Count);
            CollectionAssert.Contains(gradientsInAllTownsImplicitly, gradientAInTownA);
            CollectionAssert.Contains(gradientsInAllTownsImplicitly, gradientBInTownA);
            CollectionAssert.Contains(gradientsInAllTownsImplicitly, gradientCInTownB);
            CollectionAssert.Contains(gradientsInAllTownsImplicitly, gradientDInTownC);
        }

        #endregion
    }
}
