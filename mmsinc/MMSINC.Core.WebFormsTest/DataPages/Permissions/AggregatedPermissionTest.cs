using System;
using MMSINC.DataPages.Permissions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.DataPages.Permissions
{
    [TestClass]
    public class AggregatedPermissionTest
    {
        [TestMethod]
        public void TestIsAllowedThrowsIfNoPermissionsAreAdded()
        {
            var target = new AggregatedPermission();
            try
            {
                var oh = target.IsAllowed;
                Assert.Fail("Must throw an exception");
            }
            catch (Exception)
            {
                // Do nothing, you passed!
            }
        }

        [TestMethod]
        public void TestIsDeniedThrowsIfNoPermissionsAreAdded()
        {
            var target = new AggregatedPermission();
            try
            {
                var oh = target.IsDenied;
                Assert.Fail("Must throw an exception");
            }
            catch (Exception)
            {
                // Do nothing, you passed!
            }
        }

        [TestMethod]
        public void TestDemandThrowsIfNoPermissionsAreAdded()
        {
            var target = new AggregatedPermission();
            MyAssert.Throws(target.Demand);
        }

        [TestMethod]
        public void TestIsAllowedReturnsTrueWhenContainsOnePermissionWithAllowTrue()
        {
            var target = new AggregatedPermission();
            target.AddPermission(new Permission("Sup") {Allow = true});
            Assert.IsTrue(target.IsAllowed);
        }

        [TestMethod]
        public void TestIsAllowedReturnsTrueWhenContainsAtleastOnePermissionWithAllowTrue()
        {
            var target = new AggregatedPermission();
            target.AddPermission(new Permission("Sup") {Allow = true});
            target.AddPermission(new Permission("Nerp") {Allow = false});
            Assert.IsTrue(target.IsAllowed);
        }

        [TestMethod]
        public void TestIsAllowedReturnsFalseWhenPermissionsDoNotAllow()
        {
            var target = new AggregatedPermission();
            target.AddPermission(new Permission("Sup") {Allow = false});
            target.AddPermission(new Permission("Nerp") {Allow = false});
            Assert.IsFalse(target.IsAllowed);
        }

        [TestMethod]
        public void TestIsAllowedReturnsFalseWhenOnePermissionIsAllowTrueAndOnePermissionIsDenyTrue()
        {
            var target = new AggregatedPermission();
            target.AddPermission(new Permission("Sup") {Allow = true});
            target.AddPermission(new Permission("Nerp") {Deny = true});
            Assert.IsFalse(target.IsAllowed);
        }

        [TestMethod]
        public void TestIsDenyReturnsTrueWhenAtleastOnePermissionIsDenyTrue()
        {
            var target = new AggregatedPermission();
            target.AddPermission(new Permission("Sup") {Allow = true});
            target.AddPermission(new Permission("Nerp") {Deny = true});
            Assert.IsTrue(target.IsDenied);
        }

        [TestMethod]
        public void TestIsDenyReturnsFalseWhenAllPermissionsHaveDenyFalse()
        {
            var target = new AggregatedPermission();
            target.AddPermission(new Permission("Sup") {Allow = true});
            Assert.IsTrue(target.IsAllowed);
        }

        [TestMethod]
        public void TestIsAllowedIsFalseWhenIsDenyIsTrue()
        {
            var target = new AggregatedPermission();
            target.AddPermission(new Permission("Sup") {Allow = true});
            target.AddPermission(new Permission("Nerp") {Deny = true});
            Assert.IsFalse(target.IsAllowed);
        }

        [TestMethod]
        public void TestDemandDoesNothingIfIsAllowedIstrue()
        {
            var target = new AggregatedPermission();
            target.AddPermission(new Permission("Sup") {Allow = true});
            MyAssert.DoesNotThrow((Action)target.Demand);
        }

        [TestMethod]
        public void TestDemandThrowsIfIsAllowedIsFalse()
        {
            var target = new AggregatedPermission();
            target.AddPermission(new Permission("Sup") {Allow = false});
            MyAssert.Throws(target.Demand);
        }

        [TestMethod]
        public void TestDemandThrowsIfIsDeniedIsTrue()
        {
            var target = new AggregatedPermission();
            target.AddPermission(new Permission("Nerp") {Deny = true});
            MyAssert.Throws(target.Demand);
        }
    }
}
