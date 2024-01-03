using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class ContractorUserRepositoryTest : InMemoryDatabaseTest<ContractorUser, ContractorUserRepository>
    {
        #region Consts

        private const string WHITE_SPACE = "     ";

        /// <summary>
        /// Use this for any parameters that should throw for String.IsNullOrWhiteSpace stuff.
        /// </summary>
        public static readonly IEnumerable<string> BAD_STRING_VALUES = new[] {null, string.Empty, WHITE_SPACE};

        #endregion

        #region Fields

        #endregion

        #region Test methods

        #region TryGetUserByEmail

        [TestMethod]
        public void TestTryGetUserByEmailReturnsMatchingRecord()
        {
            var expectedEmail = "burt@reynolds.com";

            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail});

            var result = Repository.TryGetUserByEmail(expectedEmail);

            Assert.AreEqual(expectedEmail, result.Email);
        }

        [TestMethod]
        public void TestTryGetUserByEmailReturnsNullIfNoMatchingRecord()
        {
            var expectedEmail = "burt@reynolds.com";

            var result = Repository.TryGetUserByEmail(expectedEmail);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestTryGetUserByEmailTrimsEmailAddress()
        {
            var expectedEmail = "burt@reynolds.com";

            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail});

            var result = Repository.TryGetUserByEmail("    " + expectedEmail);

            Assert.AreEqual(expectedEmail, result.Email);
        }

        [TestMethod]
        public void TestTryGetUserByEmailLowercasesEmailAddress()
        {
            var expectedEmail = "burt@reynolds.com";

            GetFactory<ContractorUserFactory>().Create(new {Email = expectedEmail});

            var result = Repository.TryGetUserByEmail(expectedEmail.ToUpper());

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
