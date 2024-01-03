using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.SapChemical;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SapChemical
{
    [TestClass]
    public class SapChemicalUpdaterServiceTest : SapEntityUpdaterServiceTestBase<SapChemicalFileRecord, ISapChemicalFileParser, Chemical, IRepository<Chemical>, SapChemicalUpdaterService>
    {
        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
        }

        [TestMethod]
        public void TestProcessActivatesChemicalIfDeletionFlagIsFalse()
        {
            var file = SetupFileAndRecords(
                new SapChemicalFileRecord {
                    PartNumber = "123",
                    Name = "blah",
                    UnitOfMeasure = "stone",
                    Cost = 6.66m
                });
            GetEntityFactory<Chemical>().Create(new {
                PartNumber = "123"
            });

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual("123", entity.PartNumber);
            Assert.AreEqual("blah", entity.Name);
        }

        [TestMethod]
        public void TestProcessCreatesChemicalIfItDoesNotExist()
        {
            var file = SetupFileAndRecords(
                new SapChemicalFileRecord {
                    PartNumber = "123",
                    Name = "blah"
                });

            MyAssert.CausesIncrease(() => _target.Process(file), () => Repository.GetAll().Count());

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual("123", entity.PartNumber);
            Assert.AreEqual("blah", entity.Name);
        }

        [TestMethod]
        public void TestProcessUpdatesChemicalIfItDoesExist()
        {
            var file = SetupFileAndRecords(
                new SapChemicalFileRecord {
                    PartNumber = "123",
                    Name = "blah"
                });
            GetEntityFactory<Chemical>().Create(new {
                PartNumber = "123"
            });

            _target.Process(file);

            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual("123", entity.PartNumber);
            Assert.AreEqual("blah", entity.Name);
        }
    }
}
