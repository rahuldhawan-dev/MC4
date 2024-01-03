using System.Linq;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class DestroyTownContactTest : MapCallMvcInMemoryDatabaseTestBase<TownContact>
    {
        #region Fields

        private DestroyTownContact _target;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new DestroyTownContact(_container);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntityRemovesExistingTownContactFromTown()
        {
            var existingTownContact = GetFactory<TownContactFactory>().Create();
            _target.Id = existingTownContact.Town.Id;
            _target.TownContactId = existingTownContact.Id;

            Assert.AreEqual(1, existingTownContact.Town.TownContacts.Count);
            _target.MapToEntity(existingTownContact.Town);
            Assert.IsFalse(existingTownContact.Town.TownContacts.Any());
        }

        [TestMethod]
        public void TestValidateReturnsErrorForTownContactIdIfItDoesNotMatchTown()
        {
            var existingTownContact = GetFactory<TownContactFactory>().Create();
            var someOtherTown = GetFactory<TownFactory>().Create();

            _target.Id = someOtherTown.Id;
            _target.TownContactId = existingTownContact.Id;

            ValidationAssert.ModelStateHasError(_target, x => x.TownContactId, "Contact does not exist for this town.");
        }

        [TestMethod]
        public void TestValidateReturnsSuccessfulForTownContactIdIfTownDoesNotExistBecauseItIsHandledByActionHelper()
        {
            var contact = GetFactory<TownContactFactory>().Create();
            _target.Id = 0; // Gives a nice null town
            _target.TownContactId = contact.Id;

            ValidationAssert.ModelStateIsValid(_target, x => x.TownContactId);
        }
        

        [TestMethod]
        public void TestValidateReturnsSuccessfulIfTownContactIdExistsInTown()
        {
            var existingTownContact = GetFactory<TownContactFactory>().Create();
            _target.Id = existingTownContact.Town.Id;
            _target.TownContactId = existingTownContact.Id;
            ValidationAssert.ModelStateIsValid(_target);
        }


        #endregion
    }
}
