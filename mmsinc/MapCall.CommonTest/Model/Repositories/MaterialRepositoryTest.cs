using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class MaterialRepositoryTest : InMemoryDatabaseTest<Material, MaterialRepository>
    {
        [TestMethod]
        public void TestFindByPartialPartNumberOrDescriptionReturnsMaterialByPartNumber()
        {
            var material = GetFactory<MaterialFactory>()
               .Create(new {Description = "Hydrant Thingy", PartNumber = "1800"});
            var otherMaterial = GetFactory<MaterialFactory>()
               .Create(new {Description = "Valve Thingy", PartNumber = "1700"});

            var result = Repository.FindByPartialPartNumberOrDescription("hyd");

            Assert.AreEqual(material.Id, result.First().Id);

            result = Repository.FindByPartialPartNumberOrDescription("18");

            Assert.AreEqual(material.Id, result.First().Id);

            result = Repository.FindByPartialPartNumberOrDescription("You can't find me");

            Assert.AreEqual(0, result.Count());
        }
    }
}
