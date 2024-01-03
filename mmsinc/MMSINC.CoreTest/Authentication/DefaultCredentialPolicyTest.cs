using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;

namespace MMSINC.CoreTest.Authentication
{
    [TestClass]
    public class DefaultCredentialPolicyTest
    {
        #region Tests

        [TestMethod]
        public void TestMeetsRequirementsAlwaysReturnsTrue()
        {
            var target = new DefaultCredentialPolicy();
            Assert.IsTrue(target.PasswordMeetsRequirement(null));
            Assert.IsTrue(target.PasswordMeetsRequirement(string.Empty));
            Assert.IsTrue(target.PasswordMeetsRequirement("     "));
            Assert.IsTrue(target.PasswordMeetsRequirement("a password!"));
        }

        [TestMethod]
        public void TestMaximumFailedLoginAttemptCountReturnsIntMaxValue()
        {
            var target = new DefaultCredentialPolicy();
            Assert.AreEqual(int.MaxValue, target.MaximumFailedLoginAttemptCount);
        }

        #endregion
    }
}
