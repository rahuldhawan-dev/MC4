using System.Linq;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Linq;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class TownRepositoryTest : InMemoryDatabaseTest<Town, TownRepository>
    {
        [TestMethod]
        public void TestGetByOperatingCenterIdReturnsByOperatingCenterId()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var invalid = GetFactory<TownFactory>().Create();
            var opct = GetEntityFactory<OperatingCenterTown>()
               .Create(new {OperatingCenter = opc, Town = town, Abbreviation = "QQ"});
            Session.Save(opc);
            Session.Flush();

            var result = Repository.GetByOperatingCenterId(opc.Id).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestSaveSavesTownContacts()
        {
            var contact = GetFactory<ContactFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var contactType = GetFactory<ContactTypeFactory>().Create();

            var tc = new TownContact {
                Town = town,
                Contact = contact,
                ContactType = contactType
            };

            town.TownContacts.Add(tc);

            Assert.AreEqual(0, tc.Id);
            Repository.Save(town);
            Assert.AreNotEqual(0, tc.Id);
        }

        [TestMethod]
        public void TestSaveDeletesAnyRemovedTownContacts()
        {
            var contact = GetFactory<ContactFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var toKeep = GetFactory<TownContactFactory>().Create(new {
                Contact = contact,
                Town = town
            });
            var toDelete = GetFactory<TownContactFactory>().Create(new {
                Contact = contact,
                Town = town
            });

            Session.Save(town);
            Session.Flush();
            town = Session.Load<Town>(town.Id);

            Assert.AreEqual(2, town.TownContacts.Count);

            town.TownContacts.Remove(toDelete);
            Repository.Save(town);
            Session.Evict(town);

            town = Session.Query<Town>().Single(x => x.Id == town.Id);
            Assert.AreEqual(1, town.TownContacts.Count);
            Assert.IsFalse(Session.Query<TownContact>().Any(x => x.Id == toDelete.Id));
            Assert.IsTrue(Session.Query<TownContact>().Any(x => x.Id == toKeep.Id));
        }
    }
}
