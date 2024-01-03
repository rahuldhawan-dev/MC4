using System;
using System.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Utilities
{
    /// <summary>
    /// Summary description for WordifyTest
    /// </summary>
    [TestClass]
    public class WordifyTest
    {
        #region Testing GetWordsFromCamelCase

        #region Tests for passing null/empty values to GetWordsFromCamelCase

        [TestMethod]
        public void TestGetWordsFromCamelCaseReturnsEmptyIEnumerableWhenGivenNullValue()
        {
            TestTheEmpties(null);
        }

        [TestMethod]
        public void TestGetWordsFromCamelCaseReturnsEmptyIEnumerableWhenGivenEmptyValue()
        {
            TestTheEmpties(string.Empty);
        }

        [TestMethod]
        public void TestGetWordsFromCamelCaseReturnsEmptyIEnumerableWhenGivenWhitespaceValue()
        {
            TestTheEmpties("          ");
        }

        private static void TestTheEmpties(string value)
        {
            var result = Wordify.GetWordsFromCamelCase(value);
            Assert.IsFalse(result.Any());
        }

        #endregion

        #region Testing for expected results from GetWordsFromCamelCase

        private static void TestCamelCase(string input, string expectedOutput, string errorMessage = "")
        {
            var words = Wordify.GetWordsFromCamelCase(input);
            var result = string.Join(" ", words);

            Assert.AreEqual(expectedOutput, result, errorMessage);
        }

        [TestMethod]
        public void TestGetWordsFromCamelCase()
        {
            TestCamelCase("NoThanks", "No Thanks");
            TestCamelCase("abcdefghijklmnop", "abcdefghijklmnop",
                "All lowercase should be returned exactly the way it was entered");
            TestCamelCase("ABCTelevision", "ABC Television", "Acronyms must be supported");
            TestCamelCase("C15Bingo", "C15 Bingo");
            TestCamelCase("OMGWTFBBQ", "OMGWTFBBQ", "All uppercase should be returned exactly the way it was entered");
            TestCamelCase("IHateDataFromTheNOAA", "I Hate Data From The NOAA");
            TestCamelCase("    IStartWithSpace", "I Start With Space",
                "Words should be trimmed");
        }

        #endregion

        #region Other

        [TestMethod]
        public void TestGetWordsFromCamelCaseThrowsAnExceptionIfItEncountersNonLetterOrDigitCharacters()
        {
            MyAssert.Throws<NotSupportedException>(
                () =>
                    Wordify.GetWordsFromCamelCase("WAAAAAAAAASS UUUUUUUUUUUUUUUP"));

            MyAssert.Throws<NotSupportedException>(
                () =>
                    Wordify.GetWordsFromCamelCase("WAAAAAAAAASS,UUUUUUUUUUUUUUUP"));
            MyAssert.Throws<NotSupportedException>(
                () =>
                    Wordify.GetWordsFromCamelCase("WAAAAAA@$^&UUUUUUUP"));
        }

        #endregion

        #endregion
    }
}
