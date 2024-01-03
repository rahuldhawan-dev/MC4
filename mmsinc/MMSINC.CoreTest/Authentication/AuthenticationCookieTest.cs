using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.CoreTest.Authentication
{
    [TestClass]
    public class AuthenticationCookieTest
    {
        #region Private Methods

        private static string FormatCookie(int? id, string UserName)
        {
            return string.Format(AuthenticationCookie.FORMAT, id, UserName);
        }

        private static AuthenticationCookie CreateCookie(int? id, string UserName)
        {
            return new AuthenticationCookie(null, FormatCookie(id, UserName));
        }

        #endregion

        #region Tests

        #region Parameterless Constructor Tests

        [TestMethod]
        public void TestConstructorWithoutParametersSetsIsValidToFalse()
        {
            Assert.IsFalse(new AuthenticationCookie().IsValidlyFormatted);
        }

        [TestMethod]
        public void TestConstructorWithoutParametersSetsIdToNull()
        {
            Assert.IsNull(new AuthenticationCookie().Id);
        }

        [TestMethod]
        public void TestConstructorWithoutParametersSetsEmailToNull()
        {
            Assert.IsNull(new AuthenticationCookie().UserName);
        }

        #endregion

        #region Cookie Parameter Constructor Tests

        #endregion

        #region Id and Email Parameters Constructor Tests

        [TestMethod]
        public void TestConstructorSetsId()
        {
            Assert.AreEqual(42, new AuthenticationCookie(42, "user").Id);
        }

        [TestMethod]
        public void TestConstructorSetsUserName()
        {
            Assert.AreEqual("user", new AuthenticationCookie(42, "user").UserName);
        }

        [TestMethod]
        public void TestConstructorThrowsExceptionIfUserNameParameterIsNullOrEmptyOrWhiteSpace()
        {
            new[] {null, string.Empty, "   "}.Each(x =>
                MyAssert.Throws<ArgumentNullException>(() => new AuthenticationCookie(1, x)));
        }

        #endregion

        #region General Tests

        #region ToCookieString

        [TestMethod]
        public void TestToCookieStringThrowsExceptionIfIsValidIsFalse()
        {
            MyAssert.Throws<InvalidOperationException>(() => new AuthenticationCookie(null, "").ToCookieString());
        }

        #endregion

        #region IsValidlyFormatted

        [TestMethod]
        public void TestIsNotValidlyFormattedWhenUserNameIsNullOrEmptyOrWhiteSpace()
        {
            new[] {null, string.Empty, "   "}.Each(x => Assert.IsFalse(CreateCookie(1, x).IsValidlyFormatted));
        }

        [TestMethod]
        public void TestIsNotValidlyFormattedIfIdIsNotSetButUserNameIsSet()
        {
            Assert.IsFalse(CreateCookie(null, "user").IsValidlyFormatted);
        }

        [TestMethod]
        public void TestIsValidlyFormattedIsFalseIfIdIsSetByUserNameIsNotSet()
        {
            Assert.IsFalse(CreateCookie(32, null).IsValidlyFormatted);
        }

        [TestMethod]
        public void TestIsNotValidlyFormattedIfIdAndUserNameBothHaveNonNullValuesButUserNameIsInValidEmail()
        {
            Assert.IsFalse(CreateCookie(1, "not an email").IsValidlyFormatted);
        }

        [TestMethod]
        public void TestIsValidlyFormattedIfIdIsGreaterThanZeroAndUserNameIsInValidEmail()
        {
            Assert.IsTrue(CreateCookie(1, "an@email.com").IsValidlyFormatted);
        }

        [TestMethod]
        public void TestIsNotValidlyFormattedIfIsIsLessThanOrEqualToZero()
        {
            Assert.IsFalse(CreateCookie(0, "an@email.com").IsValidlyFormatted);
            Assert.IsFalse(CreateCookie(-1, "an@email.com").IsValidlyFormatted);
        }

        #endregion

        #endregion

        #endregion
    }
}
