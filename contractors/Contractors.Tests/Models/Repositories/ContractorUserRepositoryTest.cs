using System;
using System.Collections.Generic;
using System.Linq;

using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Utilities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class ContractorUserRepositoryTest : ContractorsControllerTestBase<ContractorUser>
    {
        #region Consts

        private const string WHITE_SPACE = "     ";

        /// <summary>
        /// Use this for any parameters that should throw for String.IsNullOrWhiteSpace stuff.
        /// </summary>
        public static readonly IEnumerable<string> BAD_STRING_VALUES = new[] { null, string.Empty, WHITE_SPACE };

        #endregion

        #region Fields

        private Contractor _contractor;
        private ContractorUserRepository _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void ContractorUserRepositoryTestInitialize()
        {
            _contractor = GetFactory<ContractorFactory>().Create();
            _target = _container.GetInstance<ContractorUserRepository>();
        }

        #endregion

        #region Test methods

        #region Linq/Criteria Filtering

        [TestMethod]
        public void TestLinqOnlyAllowsAccessToTheUsersBelongingToTheContractorThatTheCurrentUserBelongsTo()
        {
            var currentContractor = GetFactory<ContractorFactory>().Create();
            var currentUser = GetFactory<ContractorUserFactory>().Create(new { Contractor = currentContractor });
            var extraContractor = GetFactory<ContractorFactory>().Create();
            var expected = new[] {
                currentUser,
                GetFactory<ContractorUserFactory>().Create(new {Email = "foo@foo.foo", Contractor = currentContractor}),
                GetFactory<ContractorUserFactory>().Create(new {Email = "bar@bar.bar", Contractor = currentContractor}),
            };
            var extra = new[] {
                GetFactory<ContractorUserFactory>().Create(new {Email = "baz@baz.baz", Contractor = extraContractor}),
                GetFactory<ContractorUserFactory>().Create(new {Email = "foobar@foobar.foobar", Contractor = extraContractor}),
            };
            _container.Inject((_authenticationService = new MockAuthenticationService<ContractorUser>(currentUser)).Object);

            var repository = _container.GetInstance<ContractorUserRepository>();

            // .GetAll uses the Linq property (for now at least)
            var actual = repository.GetAll().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Email, actual[i].Email);
            }
        }

        [TestMethod]
        public void TestCriteriaOnlyAllowsAccessToTheAssetTypesBelongingToTheContractorThatTheCurrentUserBelongsTo()
        {
            var currentContractor = GetFactory<ContractorFactory>().Create();
            var currentUser = GetFactory<ContractorUserFactory>().Create(new { Contractor = currentContractor });
            var extraContractor = GetFactory<ContractorFactory>().Create();
            var expected = new[] {
                currentUser,
                GetFactory<ContractorUserFactory>().Create(new {Email = "foo@foo.foo", Contractor = currentContractor}),
                GetFactory<ContractorUserFactory>().Create(new {Email = "bar@bar.bar", Contractor = currentContractor}),
            };
            var extra = new[] {
                GetFactory<ContractorUserFactory>().Create(new {Email = "baz@baz.baz", Contractor = extraContractor}),
                GetFactory<ContractorUserFactory>().Create(new {Email = "foobar@foobar.foobar", Contractor = extraContractor}),
            };
            _container.Inject((_authenticationService = new MockAuthenticationService<ContractorUser>(currentUser)).Object);

            var repository = _container.GetInstance<ContractorUserRepository>();

            // .Search uses the Criteria property (for now at least)
            var search = repository.Search(Restrictions.Conjunction());
            var actual = search.List<ContractorUser>().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Email, actual[i].Email);
            }
        }

        #endregion

        #region IsAdmin

        [TestMethod]
        public void TestIsAdminThrowsContractorUserExceptionForInvalidUser()
        {
            BAD_STRING_VALUES.Each(s => MyAssert.Throws<AuthenticationException>(() => _target.IsAdmin(s)));
        }

        [TestMethod]
        public void TestIsAdminThrowsContractorUserExceptionIfUserDoesNotExist()
        {
            MyAssert.Throws<AuthenticationException>(() => _target.IsAdmin("ishould@not.exist.com"));
        }

        [TestMethod]
        public void TestIsAdminReturnsTrueIfUserIsAdmin()
        {
            var expectedEmail = "burt@reynolds.com";
            GetFactory<AdminUserFactory>().Create(new {Email = expectedEmail});

            Assert.IsTrue(_target.IsAdmin(expectedEmail));
        }

        [TestMethod]
        public void TestIsAdminReturnsFalseIfUserIsNotAdmin()
        {
            var expectedEmail = "burt@reynolds.com";
            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail});

            Assert.IsFalse(_target.IsAdmin(expectedEmail));
        }

        #endregion

        #region GetUser

        [TestMethod]
        public void TestGetUserReturnsMatchingRecord()
        {
            var expectedEmail = "burt@reynolds.com";

            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail});

            var result = _target.GetUser(expectedEmail);

            Assert.AreEqual(expectedEmail, result.Email);
        }

        [TestMethod]
        public void TestGetUserReturnsNullIfNoMatchingRecord()
        {
            var expectedEmail = "burt@reynolds.com";

            var result = _target.GetUser(expectedEmail);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetUserTrimsEmailAddress()
        {
            var expectedEmail = "burt@reynolds.com";

            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail});

            var result = _target.GetUser("    " + expectedEmail);

            Assert.AreEqual(expectedEmail, result.Email);
        }

        [TestMethod]
        public void TestGetUserLowercasesEmailAddress()
        {
            var expectedEmail = "burt@reynolds.com";

            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail});

            var result = _target.GetUser(expectedEmail.ToUpper());

            Assert.AreEqual(expectedEmail, result.Email);
        }

        #endregion

        #region ManglePassword

        [TestMethod]
        public void TestStuff()
        {
            var expectedEmptyHash =
                "0b6cbac838dfe7f47ea1bd0df00ec282fdf45510c92161072ccfb84035390c4da743d9c3b954eaa1b0f86fc9861b23cc6c8667ab232c11c686432ebb5c8c3f27";
            var result = string.Empty.Salt(Guid.Empty);
            Assert.AreEqual(expectedEmptyHash, result);

        }

        #endregion

        #endregion
    }
}
