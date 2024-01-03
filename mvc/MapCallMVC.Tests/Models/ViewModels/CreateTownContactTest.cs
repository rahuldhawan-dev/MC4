using System.Linq;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateTownContactTest : MapCallMvcInMemoryDatabaseTestBase<TownContact>
    {
        #region Fields

        private CreateTownContact _target;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IContactRepository>().Use<ContactRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IAuthenticationService<User>>().Use(new Mock<IAuthenticationService<User>>().Object);
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _target = _container.GetInstance<CreateTownContact>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntityAddsTownContactToTown()
        {
            var town = GetFactory<TownFactory>().Create();
            var contact = GetFactory<ContactFactory>().Create();
            var contactType = GetFactory<ContactTypeFactory>().Create();

            _target.Contact = contact.Id;
            _target.ContactType = contactType.Id;

            Assert.IsFalse(town.TownContacts.Any());

            _target.MapToEntity(town);

            var tc = town.TownContacts.Single();
            Assert.AreSame(town, tc.Town);
            Assert.AreSame(contact, tc.Contact);
            Assert.AreSame(contactType, tc.ContactType);
        }

        [TestMethod]
        public void TestValidateReturnsErrorOnContactIdPropertyIfTownAlreadyHasMatchingTownContact()
        {
            var town = GetFactory<TownFactory>().Create(new { ShortName = "Short People" });
            var contact = GetFactory<ContactFactory>().Create();
            var contactType = GetFactory<ContactTypeFactory>().Create();
            var existingTownContact = GetFactory<TownContactFactory>().Create(new
            {
                Town = town,
                Contact = contact,
                ContactType = contactType
            });

            Assert.IsTrue(town.TownContacts.Contains(existingTownContact), "Test isn't setup correctly.");

            _target.Id = town.Id;
            _target.Contact = contact.Id;
            _target.ContactType = contactType.Id;

            ValidationAssert.ModelStateHasError(_target, "ContactId", "A contact already exists for this contact and contact type for Short People.");
        }

        [TestMethod]
        public void TestValidateDoesNotReturnErrorIfTownDoesNotHaveMatchingTownContact()
        {
            var town = GetFactory<TownFactory>().Create(new { ShortName = "Short People" });
            var contact = GetFactory<ContactFactory>().Create();
            var contactType = GetFactory<ContactTypeFactory>().Create();

            _target.Id = town.Id;
            _target.Contact = contact.Id;
            _target.ContactType = contactType.Id;

            ValidationAssert.ModelStateIsValid(_target);
        }

        [TestMethod]
        public void TestValidateDoesNotReturnErrorForContactIdPropertyIfTownDoesNotExistBecauseItIsHandledBeforeValidateIsTrulyValidated()
        {
            var contact = GetFactory<ContactFactory>().Create();
            _target.Id = 0; // Gives a nice null town
            _target.Contact = contact.Id;

            ValidationAssert.ModelStateIsValid(_target, x => x.Contact);
        }

        #endregion
    }
}
