using System;
using System.Text.RegularExpressions;
using MapCall.Common.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Authentication
{
    [TestClass]
    public class ContractorUserCredentialPolicyTest
    {
        #region Fields

        private ContractorUserCredentialPolicy _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new ContractorUserCredentialPolicy();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPasswordMeetsRequirementReturnsFalseForNullPassword()
        {
            Assert.IsFalse(_target.PasswordMeetsRequirement(null));
        }

        [TestMethod]
        public void TestPasswordMeetsRequirementDoesNotTrimStrings()
        {
            // This will fail the string length requirement if the string gets trimmed.
            Assert.IsTrue(_target.PasswordMeetsRequirement("  AbC2  "));
        }

        [TestMethod]
        public void TestPasswordMeetsRequirementForATonOfThings()
        {
            Action<string> passes = (p) =>
                Assert.IsTrue(_target.PasswordMeetsRequirement(p), $"'{p}' should have passed, but it failed.");
            Action<string> fails = (p) =>
                Assert.IsFalse(_target.PasswordMeetsRequirement(p), $"'{p}' should have failed, but it passed.");

            fails(null); // nulls are bad

            // These should fail due to string length < 8
            fails(string.Empty);
            fails("ab3");
            fails("aB3#");
            fails("aB3# ");
            fails("aB3#  ");
            fails("aB3#   ");
            passes("aB3#    "); // 8 length should pass

            fails("abcdeghisgohsg"); // fail all lowercase
            fails("ABCDEFGHJIJGE"); // fail all uppercase
            fails("1234567890"); // fail all numerical
            fails("#@#$*(*%#@"); // fail all special symbols even though that's sorta silly.
            fails("abcdABCD"); // only meets two requirements

            passes("ABCdefg1"); // passes length, lowercase, uppercase, one number
            passes("WHAT15#@"); // passes length, special, uppercase, one number

            // variations on the special characters
            passes("ABCdefg!");
            passes("ABCdefg@");
            passes("ABCdefg#");
            passes("ABCdefg$");
            passes("ABCdefg%");
            passes("ABCdefg^");
            passes("ABCdefg&");
            passes("ABCdefg*");
            passes("ABCdefg(");
            passes("ABCdefg)");
            passes("ABCdefg,");
            passes("ABCdefg.");
            passes("ABCdefg;");
            passes("ABCdefg:");
            passes("ABCdefg'");
            passes("ABCdefg\"");
            passes("ABCdeff{");
            passes("ABCdeff}");
            passes("ABCdeff[");
            passes("ABCdeff]");
            passes("ABCdeff|");
            passes("ABCdeff?");
            passes("ABCdeff=");
            passes("ABCdeff+");
            passes("ABCdeff_");
            passes("ABCdeff-");
            passes("ABCdeff`");
            passes("ABCdeff~");
            passes("ABCdeff/");
            passes("ABCdeff<");
            passes("ABCdeff>");
        }

        #endregion
    }
}
