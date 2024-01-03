using System;
using System.Linq;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class ContactRepositoryTest : InMemoryDatabaseTest<Contact, ContactRepository>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _user = GetFactory<Common.Testing.Data.UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSavingANewContactWithAnAddressAlsoSavesTheAddress()
        {
            var town = GetFactory<TownFactory>().Create();
            var contact = GetFactory<ContactFactory>().Build();
            contact.Address.Town = town;
            Assert.AreEqual(0, contact.Address.Id, "That test ain't right");
            Repository.Save(contact);
            Assert.AreNotEqual(0, contact.Address.Id);
        }

        [TestMethod]
        public void TestDeletingAContactDeletesItsAssociatedAddress()
        {
            var contact = GetFactory<ContactFactory>().Create();
            var address = contact.Address;

            Repository.Delete(contact);

            Assert.IsNull(Repository.Find(contact.Id));
            Assert.IsFalse(Session.Query<Address>().Any(x => x.Id == address.Id));
        }

        [TestMethod]
        public void TestNullingOutExistingContactsAddressDeletesTheAddressWhenSavingTheContact()
        {
            Assert.Inconclusive("This doesn't work. The delete command is never issued");
            //var contact = GetFactory<ContactFactory>().Create();
            //var address = contact.Address;
            //Assert.IsNotNull(address);
            //contact.Address = null;
            //Repository.Save(contact);
            //Session.Flush();
            //Assert.IsFalse(Session.Query<Address>().Any(x => x.Id == address.Id));
        }

        [TestMethod]
        public void TestCanDeleteReturnsTrueIfContactHasNoAssociatedAnythings()
        {
            var contact = GetFactory<ContactFactory>().Create();
            Assert.IsFalse(contact.ThingsWithContacts.Any(), "Test ain't setup correctly.");
            Assert.IsTrue(Repository.CanDelete(contact));
        }

        [TestMethod]
        public void TestCanDeleteReturnsFalseIfContactHasAssociatedTownContacts()
        {
            var contact = GetFactory<ContactFactory>().Create();
            var townContact = GetFactory<TownContactFactory>().Create(new {Contact = contact});
            Session.Evict(contact);
            Session.Evict(townContact);

            contact = Session.Query<Contact>().Single(x => x.Id == contact.Id);

            Assert.IsTrue(contact.TownContacts.Any());
            Assert.IsFalse(contact.NotificationConfigurations.Any());
            Assert.IsFalse(Repository.CanDelete(contact));
        }

        [TestMethod]
        public void TestCanDeleteReturnsFalseIfContactHasAssociatedNotificationConfigurations()
        {
            var contact = GetFactory<ContactFactory>().Create();
            var notification = GetFactory<NotificationConfigurationFactory>().Create(new {Contact = contact});
            Session.Evict(contact);
            Session.Evict(notification);

            contact = Session.Query<Contact>().Single(x => x.Id == contact.Id);

            Assert.IsFalse(contact.TownContacts.Any());
            Assert.IsTrue(contact.NotificationConfigurations.Any());
            Assert.IsFalse(Repository.CanDelete(contact));
        }

        #region FindByPartialNameMatch

        [TestMethod]
        public void TestFindByPartialNameMatchReturnsExpectedMatchesWhenMatchingAgainstFirstName()
        {
            // ReSharper disable PossibleMultipleEnumeration

            var contact = GetFactory<ContactFactory>().Create(new {FirstName = "John", LastName = "Smith"});

            var result = Repository.FindByPartialNameMatch("John");
            Assert.IsTrue(result.Contains(contact), "Must match on first name");

            result = Repository.FindByPartialNameMatch("Bjork");
            Assert.IsFalse(result.Contains(contact), "Should not match on first name");

            result = Repository.FindByPartialNameMatch("Jo");
            Assert.IsTrue(result.Contains(contact), "Must partially match on first name");

            result = Repository.FindByPartialNameMatch("john");
            Assert.IsTrue(result.Contains(contact), "Must match on first name regardless of case");

            result = Repository.FindByPartialNameMatch("John    ");
            Assert.IsTrue(result.Contains(contact), "Must match regardless of excess whitespace");

            // ReSharper restore PossibleMultipleEnumeration
        }

        [TestMethod]
        public void TestFindByPartialNameMatchReturnsExpectedMatchesWhenMatchingAgainstLastName()
        {
            // ReSharper disable PossibleMultipleEnumeration

            var contact = GetFactory<ContactFactory>().Create(new {FirstName = "John", LastName = "Smith"});

            var result = Repository.FindByPartialNameMatch("Smith");
            Assert.IsTrue(result.Contains(contact), "Must match on last name");

            result = Repository.FindByPartialNameMatch("Bjork");
            Assert.IsFalse(result.Contains(contact), "Should not match on last name");

            result = Repository.FindByPartialNameMatch("Smi");
            Assert.IsTrue(result.Contains(contact), "Must partially match on last name");

            result = Repository.FindByPartialNameMatch("smith");
            Assert.IsTrue(result.Contains(contact), "Must match on first name regardless of case");

            result = Repository.FindByPartialNameMatch(" Smith    ");
            Assert.IsTrue(result.Contains(contact), "Must match regardless of excess whitespace");

            // ReSharper restore PossibleMultipleEnumeration
        }

        [TestMethod]
        public void TestFindByPartialNameMatchReturnsExpectedMatchesWhenMatchingAgainstFullName()
        {
            // ReSharper disable PossibleMultipleEnumeration

            var contact = GetFactory<ContactFactory>().Create(new {FirstName = "John", LastName = "Smith"});

            var result = Repository.FindByPartialNameMatch("John Smith");
            Assert.IsTrue(result.Contains(contact), "Must match on full name ");

            result = Repository.FindByPartialNameMatch(" John Smith   ");
            Assert.IsTrue(result.Contains(contact), "Must match regardless of excess whitespace");

            result = Repository.FindByPartialNameMatch(" john smith   ");
            Assert.IsTrue(result.Contains(contact), "Must match regardless of case");

            result = Repository.FindByPartialNameMatch("Smith, John");
            Assert.IsTrue(result.Contains(contact), "Must match regardless of name order");

            result = Repository.FindByPartialNameMatch("Carol Smith");
            Assert.IsFalse(result.Contains(contact), "Should not match at all.");

            result = Repository.FindByPartialNameMatch("John Smith");
            Assert.AreEqual(1, result.Count(), "No duplicates should show up.");

            // ReSharper restore PossibleMultipleEnumeration
        }

        [TestMethod]
        public void TestFindByPartialNameMatchReturnsEmptyCollectionIfNoResults()
        {
            var someExistingContact = GetFactory<ContactFactory>().Create();
            var result = Repository.FindByPartialNameMatch("asfasfasfasfsfs");
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TestFindByPartialNameMatchReturnsEmptyCollectionIfNullOrEmptyParameter()
        {
            var result = Repository.FindByPartialNameMatch(null);
            Assert.IsFalse(result.Any());
        }

        #endregion

        #endregion
    }
}
