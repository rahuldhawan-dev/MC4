using System;
using System.Security;
using MMSINC.DataPages.Permissions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.DataPages.Permissions
{
    [TestClass]
    public class PermissionTest
    {
        #region Fields

        private const string PERMISSION_NAME = "Scumbag Steve";

        #endregion

        [TestInitialize]
        public void InitializePermissionTest() { }

        private Permission InitializeBuilder()
        {
            return new Permission(PERMISSION_NAME);
        }

        [TestMethod]
        public void TestConstructorThrowsArgumentNullExceptionWhenPermissionNameIsNull()
        {
            MyAssert.Throws<ArgumentNullException>(() => new Permission(null));
        }

        [TestMethod]
        public void TestConstructorThrowsArgumentNullExceptionWhenPermissionNameIsEmptyString()
        {
            MyAssert.Throws<ArgumentNullException>(() => new Permission(string.Empty));
        }

        [TestMethod]
        public void TestConstructorThrowsArgumentNullExceptionWhenPermissionNameIsWhiteSpace()
        {
            MyAssert.Throws<ArgumentNullException>(() => new Permission("   "));
        }

        [TestMethod]
        public void TestAllowAndDenyAreFalseByDefault()
        {
            var p = InitializeBuilder();
            Assert.IsFalse(p.IsAllowed);
            Assert.IsFalse(p.IsDenied);
        }

        [TestMethod]
        public void TestIsAllowedReturnsTrueWhenAllowIsTrueAndDenyIsFalse()
        {
            var p = InitializeBuilder();
            p.Allow = true;
            p.Deny = false;

            Assert.IsTrue(p.IsAllowed);
        }

        [TestMethod]
        public void TestIsAllowedReturnsFalseWhenAllowIsFalseAndDenyIsFalse()
        {
            var p = InitializeBuilder();
            p.Allow = false;
            p.Deny = false;

            Assert.IsFalse(p.IsAllowed);
        }

        [TestMethod]
        public void TestIsAllowedReturnsFalseWhenAllIsFalseAndDenyIsTrue()
        {
            var p = InitializeBuilder();
            p.Allow = false;
            p.Deny = true;

            Assert.IsFalse(p.IsAllowed);
        }

        [TestMethod]
        public void TestIsDeniedReturnsFalseWhenDenyIsFalse()
        {
            var p = InitializeBuilder();
            p.Deny = false;

            Assert.IsFalse(p.IsDenied);
        }

        [TestMethod]
        public void TestIsDeniedReturnsTrueWhenDenyIsTrue()
        {
            var p = InitializeBuilder();
            p.Deny = true;

            Assert.IsTrue(p.IsDenied);
        }

        [TestMethod]
        public void TestIsAllowedThrowsExceptionIfAllowIsTrueAndDenyIsTrue()
        {
            var p = InitializeBuilder();
            p.Allow = true;
            p.Deny = true;

            bool result;
            MyAssert.Throws<InvalidOperationException>(() => result = p.IsAllowed);
        }

        [TestMethod]
        public void TestIsDeniedThrowsExceptionIfAllowIsTrueAndDenyIsTrue()
        {
            var p = InitializeBuilder();
            p.Allow = true;
            p.Deny = true;

            bool result;
            MyAssert.Throws<InvalidOperationException>(() => result = p.IsDenied);
        }

        [TestMethod]
        public void TestDemandThrowsExceptionIfAllowIsTrueAndDenyIsTrue()
        {
            var p = InitializeBuilder();
            p.Allow = true;
            p.Deny = true;

            MyAssert.Throws<InvalidOperationException>(() => p.Demand());
        }

        [TestMethod]
        public void TestDemandThrowsExceptionIfIsAllowedIsFalse()
        {
            var p = InitializeBuilder();
            p.Allow = false;
            p.Deny = false;

            MyAssert.Throws<SecurityException>((Action)p.Demand);
        }

        [TestMethod]
        public void TestDemandDoesNothingIfIsAllowedIsTrue()
        {
            var p = InitializeBuilder();
            p.Allow = true;
            p.Deny = false;

            MyAssert.DoesNotThrow(p.Demand, typeof(SecurityException));
        }
    }
}
