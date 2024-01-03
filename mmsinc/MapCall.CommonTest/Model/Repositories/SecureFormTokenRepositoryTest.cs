using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class SecureFormTokenRepositoryTest : InMemoryDatabaseTest<Contact, ContactRepository>
    {
        #region Fields

        private Mock<IDateTimeProvider> _dateTimeProvider;
        private SecureFormTokenRepository _target;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use(new Mock<IAuthenticationService<User>>().Object);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
        }

        [TestInitialize]
        public void InitializeTest()
        {

            _target = _container.GetInstance<SecureFormTokenRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestFindByTokenFindsByToken()
        {
            var toFind = GetFactory<SecureFormTokenFactory>().Create();
            var toNotToFind = GetFactory<SecureFormTokenFactory>().Create();

            var result = _target.FindByToken(toFind.Token);
            Assert.AreSame(toFind, result);
            Assert.AreNotSame(toNotToFind, result);
        }

        [TestMethod]
        public void TestSaveSetsCreatedAtIfDateIsNotSet()
        {
            var token = GetFactory<SecureFormTokenFactory>().Build();
            token.CreatedAt = DateTime.MinValue;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Today);

            _target.Save(token);
            Assert.AreEqual(DateTime.Today, token.CreatedAt);
        }

        [TestMethod]
        public void TestSaveDoesNotSetCreatedAtIfTheValueIsAlreadySet()
        {
            var token = GetFactory<SecureFormTokenFactory>().Build();
            token.CreatedAt = DateTime.Today;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);

            _target.Save(token);
            Assert.AreEqual(DateTime.Today, token.CreatedAt);
        }

        [TestMethod]
        public void TestSaveSetsTokenToNewGuidIfTokenIsNotSet()
        {
            var token = GetFactory<SecureFormTokenFactory>().Build();
            token.Token = Guid.Empty;
            _target.Save(token);
            Assert.AreNotEqual(Guid.Empty, token.Token);
        }

        [TestMethod]
        public void TestSaveDoesNotSetTokenIfTokenIsAlreadySet()
        {
            var expected = Guid.NewGuid();
            var token = GetFactory<SecureFormTokenFactory>().Build();
            token.Token = expected;
            _target.Save(token);
            Assert.AreEqual(expected, token.Token);
        }

        #endregion
    }
}
