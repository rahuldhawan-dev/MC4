using System;
using System.Text;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class PhoneNumberFormatterTest
    {
        [TestMethod]
        public void TestFormatRemovesAllNonNumericCharacters()
        {
            var sb = new StringBuilder();
            for (var i = Char.MinValue; i < Char.MaxValue; i++)
            {
                sb.Append(Convert.ToChar(i));
            }

            var result = PhoneNumberFormatter.Format(sb.ToString());
            Assert.AreEqual("(012) 345-6789", result);
        }

        [TestMethod]
        public void TestFormatFormats10DigitPhoneNumberCorrectly()
        {
            var result = PhoneNumberFormatter.Format("7327796891");
            Assert.AreEqual("(732) 779-6891", result);
        }

        [TestMethod]
        public void TestFormatFormatsRemovesInternationalCallingNumberWhichIsTheNumber1()
        {
            var result = PhoneNumberFormatter.Format("17327796891");
            Assert.AreEqual("(732) 779-6891", result);
        }

        [TestMethod]
        public void TestFormatIncludesExtension()
        {
            var result = PhoneNumberFormatter.Format("732 779 6891 x1234");
            Assert.AreEqual("(732) 779-6891 x1234", result);
        }

        [TestMethod]
        public void TestFormatReturnsNumberWithoutFormattingIfFormatParameterIsNull()
        {
            var result = PhoneNumberFormatter.Format("7327796891", format: null);
            Assert.AreEqual("7327796891", result);
        }

        [TestMethod]
        public void TestFormatWithoutFormattingIncludesExtension()
        {
            var result = PhoneNumberFormatter.Format("732 779 6891 x1234", format: null);
            Assert.AreEqual("7327796891x1234", result);
        }

        [TestMethod]
        public void TestFormatUsesFormatParameter()
        {
            var result = PhoneNumberFormatter.Format("7327796891", format: "{0}!{1}!{2}");
            Assert.AreEqual("732!779!6891", result);
        }

        [TestMethod]
        public void TestTryFormatReturnsNullIfItCantFormat()
        {
            Assert.IsNull(PhoneNumberFormatter.TryFormat("AFEF"), "Should throw for no numbers");
            Assert.IsNull(PhoneNumberFormatter.TryFormat(null), "should throw for null");
            Assert.IsNull(PhoneNumberFormatter.TryFormat("123456789"), "Should throw for not enough numbers");
        }
    }
}
