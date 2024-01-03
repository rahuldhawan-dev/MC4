using System;
using System.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.ClassExtensions
{
    /// <summary>
    /// 
    /// This is a really basic test. Don't see a reason to really flesh this out to the full testing scheme.
    /// 
    /// If another method gets added to StringExtensions... well then it'll probably need it. 
    /// 
    /// </summary>
    [TestClass]
    public class StringExtensionsTest
    {
        #region IsValidEmail

        //[TestMethod]
        //public void TestIsValidEmailReturnsFalseForNullValue()
        //{
        //    Assert.IsFalse(AuthenticationService.IsValidEmail(null));
        //}

        [TestMethod]
        public void TestIsValidEmailReturnsFalseForEmptyValue()
        {
            Assert.IsFalse(string.Empty.IsValidEmail());
        }

        [TestMethod]
        public void TestIsValidEmailReturnsFalseForWhitespaceValue()
        {
            Assert.IsFalse("    ".IsValidEmail());
        }

        [TestMethod]
        public void TestIsValidEmailReturnsFalseForBadEmails()
        {
            Assert.IsFalse("herp".IsValidEmail());
            Assert.IsFalse("herp@@ae.com".IsValidEmail());
            Assert.IsFalse("herp@derp..com".IsValidEmail());
        }

        [TestMethod]
        public void TestIsValidEmailReturnsTrueForGoodEmails()
        {
            Assert.IsTrue("herp@derp.com".IsValidEmail());
        }

        #endregion

        #region Salt

        [TestMethod]
        public void TestSaltsMatchWithTheSameKey()
        {
            var str = "Foo";
            var guid = new Guid();
            var expected = str.Salt(guid);
            var actual = str.Salt(guid);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region SplitEvery

        [TestMethod]
        public void TestSplitEvery()
        {
            var length = 8;
            var str = "12345678abcdefgh";

            var result = str.SplitEvery(length).ToArray();

            Assert.AreEqual("12345678", result[0]);
            Assert.AreEqual("abcdefgh", result[1]);
        }

        [TestMethod]
        public void TestSplitEveryReturnsTrimmedStringIfLastPieceIsShorterThanChunkLength()
        {
            var str = "123456";
            var length = 5;

            var result = str.SplitEvery(length);
            Assert.AreEqual("12345", result.First());
            Assert.AreEqual("6", result.Last());
        }

        #endregion

        #region SplitOnWhiteSpace

        [TestMethod]
        public void TestSplitOnWhiteSpaceSplitsOnWhiteSpaceCharacters()
        {
            var result = "one two".SplitOnWhiteSpace().ToArray();
            Assert.AreEqual("one", result[0]);
            Assert.AreEqual("two", result[1]);

            result = "one \n two".SplitOnWhiteSpace().ToArray();
            Assert.AreEqual("one", result[0]);
            Assert.AreEqual("two", result[1]);

            result = ("one" + Environment.NewLine + "two").SplitOnWhiteSpace().ToArray();
            Assert.AreEqual("one", result[0]);
            Assert.AreEqual("two", result[1]);
        }

        #endregion

        #region ToDateTime

        [TestMethod]
        public void TestToDateTimeThrowsForNullsAndEmptiesAndWhiteSpacies()
        {
            MyAssert.Throws<ArgumentNullException>(() => StringExtensions.ToDateTime(null));
            MyAssert.Throws<ArgumentNullException>(() => StringExtensions.ToDateTime(string.Empty));
            MyAssert.Throws<ArgumentNullException>(() => StringExtensions.ToDateTime("     "));
        }

        [TestMethod]
        public void TestToDateTimeAFortnight()
        {
            var expected = DateTime.Now.AddDays(14);
            MyAssert.AreClose(expected, "a fortnight".ToDateTime());
            expected = DateTime.Now.AddDays(-14);
            MyAssert.AreClose(expected, "a fortnight ago".ToDateTime());
        }

        [TestMethod]
        public void TestToDateTimeTomorrow()
        {
            var expected = DateTime.Now.AddDays(1);
            MyAssert.AreClose(expected, "tomorrow".ToDateTime());
        }

        [TestMethod]
        public void TestToDateTimeYesterday()
        {
            var expected = DateTime.Now.AddDays(-1);
            MyAssert.AreClose(expected, "yesterday".ToDateTime());
        }

        [TestMethod]
        public void TestToDateTimeToday()
        {
            MyAssert.AreClose(DateTime.Now, "today".ToDateTime());
        }

        [TestMethod]
        public void TestToDateTimeReturnsDateTimeFromString()
        {
            var dateString = "9/19/2011";
            var expected = DateTime.Parse(dateString);
            MyAssert.AreClose(expected, dateString.ToDateTime());
        }

        [TestMethod]
        public void TestToDateTimeXDaysAgoReturnsDateTimeXDaysAgo()
        {
            var expected = DateTime.Now.AddDays(-10);
            MyAssert.AreClose(expected, "10 days ago".ToDateTime());
            expected = DateTime.Now.AddDays(-2);
            MyAssert.AreClose(expected, "2 days ago".ToDateTime());
            expected = DateTime.Now.AddDays(-100);
            MyAssert.AreClose(expected, "100 days ago".ToDateTime());
        }

        [TestMethod]
        public void TestToDateTimeXDaysFromNowReturnsDateTimeXDaysFromNow()
        {
            var expected = DateTime.Now.AddDays(10);
            MyAssert.AreClose(expected, "10 days from now".ToDateTime());
            expected = DateTime.Now.AddDays(2);
            MyAssert.AreClose(expected, "2 days from now".ToDateTime());
            expected = DateTime.Now.AddDays(100);
            MyAssert.AreClose(expected, "100 days from now".ToDateTime());
        }

        #endregion

        #region Pluralize

        [TestMethod]
        public void TestPluralizePluralizesSomeCommonStringsCorrectly()
        {
            Assert.AreEqual("People", "Person".Pluralize());
            Assert.AreEqual("Cats", "Cat".Pluralize());
        }

        #endregion

        #region Singularize

        [TestMethod]
        public void TestSingularizeSingularizesCommonStringsCorrectly()
        {
            Assert.AreEqual("Person", "People".Singularize());
            Assert.AreEqual("Cat", "Cats".Singularize());
        }

        #endregion

        #region PascalCase

        [TestMethod]
        public void TestToPascalCase()
        {
            Assert.AreEqual("ThePascalCase", "thePascalCase".ToPascalCase());
            Assert.AreEqual("ThePascalCase", "the Pascal Case".ToPascalCase());
            Assert.AreEqual("ThePascalCase", "the pascal Case".ToPascalCase());
        }

        #endregion

        #region camelCase

        [TestMethod]
        public void TestToCamelCase()
        {
            Assert.AreEqual("theCamelCase", "TheCamelCase".ToCamelCase());
            Assert.AreEqual("theCamelCase", "The Camel Case".ToCamelCase());
            Assert.AreEqual("theCamelCase", "The camel Case".ToCamelCase());
        }

        #endregion

        #region lower space case

        [TestMethod]
        public void TestToLowerSpaceCase()
        {
            Assert.AreEqual("lower space case", "LowerSpaceCase".ToLowerSpaceCase());
            Assert.AreEqual("lower space case", "lowerSpaceCase".ToLowerSpaceCase());
            Assert.AreEqual("lower space case", "Lower Space Case".ToLowerSpaceCase());
            Assert.AreEqual("s o p sub system", "SOPSubSystem".ToLowerSpaceCase());
        }

        #endregion

        #region TitleCase

        [TestMethod]
        public void TestToTitleCase()
        {
            Assert.AreEqual("The Title", "TheTitle".ToTitleCase());
            Assert.AreEqual("The Catcher in the Rye", "TheCatcherInTheRye".ToTitleCase());
            Assert.AreEqual("Never the The", "NeverTheThe".ToTitleCase());
            Assert.AreEqual("The Applicant Is the Owner", "TheApplicantIsTheOwner".ToTitleCase());
            Assert.AreEqual("How to Get AAA Coverage", "HowToGetAAACoverage".ToTitleCase());
        }

        [TestMethod]
        public void TestToCultureTitleCase()
        {
            Assert.AreEqual("The Title", "The Title".ToCultureTitleCase());
            Assert.AreEqual("The Catcher in the Rye", "The Catcher In The Rye".ToCultureTitleCase());
            Assert.AreEqual("Never the The", "Never The The".ToCultureTitleCase());
            Assert.AreEqual("The Applicant Is the Owner", "The Applicant Is The Owner".ToCultureTitleCase());
            Assert.AreEqual("New Jersey", "NEW JERSEY".ToCultureTitleCase());
        }

        #endregion

        #region ToRouteValues

        [TestMethod]
        public void TestRouteValuesTurnsQueryStringIntoRouteValues()
        {
            var queryString = "foo=2&bar=1";

            var target = queryString.ToRouteValues();

            Assert.AreEqual(2, target.Count);
            Assert.AreEqual("2", target["foo"]);
            Assert.AreEqual("1", target["bar"]);
        }

        #endregion

        #region ConvertToSqlite

        [TestMethod]
        public void TestConvertToSqliteConvertsIsNullToIfNull()
        {
            var sql = "if IsNull(x, '') = ''";

            var target = sql.ToSqlite();

            Assert.AreEqual("if ifnull(x, '') = ''", target);
        }

        [TestMethod]
        public void TestConvertToSqliteConvertsYearSomeTableNameDotColumnNameCorrectly()
        {
            var sql = "YEAR(vi.DateInspected)";

            var target = sql.ToSqlite();

            Assert.AreEqual("strftime('%Y', vi.DateInspected)", target);
        }

        [TestMethod]
        public void TestConvertToSqliteConvertsYearGetDateCorrectly()
        {
            var sql = "AND ValveZoneId <> ((ABS(2011-YEAR(GETDATE()))%2)+5))";

            var target = sql.ToSqlite();

            Assert.AreEqual("AND ValveZoneId <> ((ABS(2011-strftime('%Y', 'now'))%2)+5))", target);
        }

        #endregion
    }
}
