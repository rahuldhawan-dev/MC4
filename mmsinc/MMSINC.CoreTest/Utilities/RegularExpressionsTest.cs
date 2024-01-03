using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class RegularExpressionsTest
    {
        #region HEX_COLOR_VALUE

        [TestMethod]
        public void TestHexColorValueMatchesSomeStringButNotOthers()
        {
            var target = new Regex(RegularExpressions.HEX_COLOR_VALUE);

            Assert.IsTrue(target.IsMatch("#000000"));
            Assert.IsTrue(target.IsMatch("#0099AA"));
            Assert.IsTrue(target.IsMatch("#ABCDEF"));

            Assert.IsFalse(target.IsMatch("#01234G"));
        }

        #endregion

        #region HEX_COLOR_VALUE

        [TestMethod]
        public void TestRGBColorValueMatchesSomeStringButNotOthers()
        {
            var target = new Regex(RegularExpressions.RGB_COLOR_VALUE);

            Assert.IsTrue(target.IsMatch("rgba(255, 189, 189, 1)"));
            Assert.IsTrue(target.IsMatch("rgb(255, 189, 189, 1)"));

            Assert.IsFalse(target.IsMatch("THIS ISN'T A MATCH"));
        }

        [TestMethod]
        public void TestRGBColorValueGroupsMatchUpCorrectlyWithRGB()
        {
            var expected = "rgb(255, 189, 191, 1)";

            var target = new Regex(RegularExpressions.RGB_COLOR_VALUE);

            var matches = target.Match(expected);
            Assert.AreEqual(5, matches.Groups.Count);
            Assert.AreEqual(expected, matches.Groups[0].ToString());
            Assert.AreEqual("255", matches.Groups[1].Value);
            Assert.AreEqual("189", matches.Groups[2].Value);
            Assert.AreEqual("191", matches.Groups[3].Value);
            Assert.AreEqual("1", matches.Groups[4].Value);
        }

        [TestMethod]
        public void TestRGBColorValueGroupsMatchUpCorrectlyWithRGBA()
        {
            var expected = "rgba(255, 189, 191, 1)";

            var target = new Regex(RegularExpressions.RGB_COLOR_VALUE);

            var matches = target.Match(expected);
            Assert.AreEqual(5, matches.Groups.Count);
            Assert.AreEqual(expected, matches.Groups[0].ToString());
            Assert.AreEqual("255", matches.Groups[1].Value);
            Assert.AreEqual("189", matches.Groups[2].Value);
            Assert.AreEqual("191", matches.Groups[3].Value);
            Assert.AreEqual("1", matches.Groups[4].Value);
        }

        [TestMethod]
        public void TestRGBColorValueGroupsMatchUpCorrectlyWithRGBAWithOptionalValueExcluded()
        {
            var expected = "rgba(255, 189, 191)";

            var target = new Regex(RegularExpressions.RGB_COLOR_VALUE);

            var matches = target.Match(expected);
            Assert.AreEqual(5, matches.Groups.Count);
            Assert.AreEqual(expected, matches.Groups[0].ToString());
            Assert.AreEqual("255", matches.Groups[1].Value);
            Assert.AreEqual("189", matches.Groups[2].Value);
            Assert.AreEqual("191", matches.Groups[3].Value);
            Assert.AreEqual("", matches.Groups[5].Value);
        }

        #endregion

        #region NUMERICAL

        [TestMethod]
        public void TestNumericMatchesOnlyNumbers()
        {
            var target = new Regex(RegularExpressions.NUMERICAL);

            Assert.IsTrue(target.IsMatch("0000123456789"));
            Assert.IsFalse(target.IsMatch("1.0"));
            Assert.IsFalse(target.IsMatch("A1"));
        }

        #endregion

        #region PHONE

        [TestMethod]
        public void TestPhoneMatchesVariousPhonePatterns()
        {
            var target = new Regex(RegularExpressions.PHONE);

            Assert.IsTrue(target.IsMatch("(123)123-1234"));
            Assert.IsTrue(target.IsMatch("(123)123-1234"));
            Assert.IsTrue(target.IsMatch("(123) 123-1234"));
            Assert.IsTrue(target.IsMatch("123-123-1234"));
            Assert.IsTrue(target.IsMatch("1231231234"));
            Assert.IsTrue(target.IsMatch("123 123 1234"));

            Assert.IsTrue(target.IsMatch("(123)123-1234 x1"));
            Assert.IsTrue(target.IsMatch("(123)123-1234 x12"));
            Assert.IsTrue(target.IsMatch("(123)123-1234 x123"));
            Assert.IsTrue(target.IsMatch("(123)123-1234 x1234"));
            Assert.IsTrue(target.IsMatch("(123)123-1234 x12345"));
            Assert.IsTrue(target.IsMatch("(123)123-1234x1"));
            Assert.IsTrue(target.IsMatch("(123)123-1234x12"));
            Assert.IsTrue(target.IsMatch("(123)123-1234x123"));
            Assert.IsTrue(target.IsMatch("(123)123-1234x1234"));
            Assert.IsTrue(target.IsMatch("(123)123-1234x12345"));

            Assert.IsFalse(target.IsMatch("THIS IS NOT NUMBER"));
        }

        #endregion
    }
}
