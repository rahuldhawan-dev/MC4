using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Permits.Data.Client;
using Permits.Data.Client.Repositories;

namespace Permits.Data.ClientTest.Repositories
{
    [TestClass]
    public class PermitsDataClientRepositoryTest
    {
        #region BaseAddress

        [TestMethod]
        public void TestBaseAddressReadsTheBaseAddressFromUtilitiesBaseAddress()
        {
            Assert.AreEqual(
                Utilities.BaseAddress,
                new PermitsDataClientRepository<Object>("foo").BaseAddress);
        }

        #endregion

        #region RsaTokenConfigKey

        [TestMethod]
        public void TestRsaTokenConfigKeyReturnsRsaTokenConfigKey()
        {
            Assert.AreEqual(PermitsDataClientRepository<Object>.RSA_TOKEN_CONFIG_KEY,
                new PermitsDataClientRepository<Object>("foo").RsaTokenConfigKey);
        }

        #endregion

        #region UserName

        [TestMethod]
        public void TestUserNameReturnsValueSuppliedToTheConstructor()
        {
            var userName = "foo";
            var target = new PermitsDataClientRepository<Object>(userName);

            Assert.AreEqual(userName, target.UserName);
        }

        #endregion
    }
}
