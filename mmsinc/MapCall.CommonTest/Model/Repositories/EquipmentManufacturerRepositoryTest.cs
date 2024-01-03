using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.WebApi;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    public class
        EquipmentManufacturerRepositoryTest : InMemoryDatabaseTest<EquipmentManufacturer,
            EquipmentManufacturerRepository>
    {
        [TestMethod]
        public void TestFindOrCreateCreatesIfItDoesntExist()
        {
            var expected = "Foo";
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();

            MyAssert.CausesIncrease(() => { Repository.FindOrCreate(new[] {expected}, equipmentType); },
                () => Repository.GetAll().Count());
        }

        [TestMethod]
        public void TestFindOrCreateCreatesUnknownIfManufacturerIsUnknown()
        {
            var expected = "UNKNOWN";
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            Repository.FindOrCreate(new[] {expected}, equipmentType);

            var result = Repository.GetAll().Single();
            Assert.AreEqual("UNKNOWN", result.MapCallDescription);
            Assert.AreEqual("UNKNOWN", result.Description);
            Assert.AreSame(equipmentType, result.EquipmentType);
        }

        [TestMethod]
        public void TestFindOrCreateDoesNotCreatesIfTheyExist()
        {
            var expected = "Foo";
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var equipmentManufacturer = GetEntityFactory<EquipmentManufacturer>().Create(new {
                EquipmentType = equipmentType,
                Description = expected
            });

            MyAssert.DoesNotCauseIncrease(() => { Repository.FindOrCreate(new[] {expected}, equipmentType); },
                () => Repository.GetAll().Count());
        }
    }
}
