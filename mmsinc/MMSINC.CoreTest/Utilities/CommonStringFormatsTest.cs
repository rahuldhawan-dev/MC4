using System;
using System.Collections.Generic;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.EnumExtensions;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class CommonStringFormatsTest
    {
        #region Private methods

        private static void TestFormat(string expected, object target, string format, string assertMessage = null)
        {
            Assert.AreEqual(expected, string.Format(format, target), assertMessage);
        }

        #endregion

        [TestMethod]
        public void TestDate()
        {
            TestFormat("4/4/1984", new DateTime(1984, 4, 4, 4, 4, 0), CommonStringFormats.DATE,
                "Neither day or month should have leading zeros");
            TestFormat("12/24/1984", new DateTime(1984, 12, 24, 4, 4, 0), CommonStringFormats.DATE);
        }

        [TestMethod]
        public void TestZeroPaddedDate()
        {
            TestFormat("04/04/1984", new DateTime(1984, 4, 4, 4, 4, 0), CommonStringFormats.DATE_ZERO_PADDED,
                "day or month should have leading zeros");
            TestFormat("12/24/1984", new DateTime(1984, 12, 24, 4, 4, 0), CommonStringFormats.DATE_ZERO_PADDED);
            TestFormat("12/04/1984", new DateTime(1984, 12, 4, 4, 4, 0), CommonStringFormats.DATE_ZERO_PADDED,
                "day or month should have leading zeros");
        }

        [TestMethod]
        public void TestDateWithoutParameterWhenUsedWithToStringOverload()
        {
            Action<string, DateTime, string> test = (expected, target, format) => {
                Assert.AreEqual(expected, target.ToString(format));
            };

            test("4/4/1984", new DateTime(1984, 4, 4, 4, 4, 0), CommonStringFormats.DATE_WITHOUT_PARAMETER);
            test("12/24/1984", new DateTime(1984, 12, 24, 4, 4, 0), CommonStringFormats.DATE_WITHOUT_PARAMETER);
        }

        [TestMethod]
        public void TestDateTimeWithoutSeconds()
        {
            TestFormat("4/4/1984 4:04 AM", new DateTime(1984, 4, 4, 4, 4, 0),
                CommonStringFormats.DATETIME_WITHOUT_SECONDS, "Date should not have leading zeros");
            TestFormat("12/24/1984 4:04 AM", new DateTime(1984, 12, 24, 4, 4, 0),
                CommonStringFormats.DATETIME_WITHOUT_SECONDS);
            TestFormat("12/24/1984 1:24 PM", new DateTime(1984, 12, 24, 13, 24, 0),
                CommonStringFormats.DATETIME_WITHOUT_SECONDS, "Date should not be in military time");
        }

        [TestMethod]
        public void TestDateTimeWithoutSecondsWithEstTimezone()
        {
            TestFormat("4/4/1984 4:04 AM (EST)", new DateTime(1984, 4, 4, 4, 4, 0),
                CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE, "Date should not have leading zeros");
            TestFormat("12/24/1984 4:04 AM (EST)", new DateTime(1984, 12, 24, 4, 4, 0),
                CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE);
            TestFormat("12/24/1984 1:24 PM (EST)", new DateTime(1984, 12, 24, 13, 24, 0),
                CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE, "Date should not be in military time");
        }

        [TestMethod]
        public void TestDateTimeWithSecondsWithEstTimezone()
        {
            TestFormat("4/4/1984 4:04:00 AM (EST)", new DateTime(1984, 4, 4, 4, 4, 0),
                CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE, "Date should not have leading zeros");
            TestFormat("12/24/1984 4:04:00 AM (EST)", new DateTime(1984, 12, 24, 4, 4, 0),
                CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE);
            TestFormat("12/24/1984 1:24:00 PM (EST)", new DateTime(1984, 12, 24, 13, 24, 0),
                CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE, "Date should not be in military time");
        }

        [TestMethod]
        public void TestDateTime24WithoutSeconds()
        {
            TestFormat("4/4/1984 04:04", new DateTime(1984, 4, 4, 4, 4, 0),
                CommonStringFormats.DATETIME24_WITHOUT_SECONDS, "Date should not have leading zeros, time should");
            TestFormat("12/24/1984 04:04", new DateTime(1984, 12, 24, 4, 4, 0),
                CommonStringFormats.DATETIME24_WITHOUT_SECONDS);
            TestFormat("12/24/1984 14:24", new DateTime(1984, 12, 24, 14, 24, 0),
                CommonStringFormats.DATETIME24_WITHOUT_SECONDS, "Date should be in military time");
        }

        [TestMethod]
        public void TestDecimalWithoutTrailingZeros()
        {
            TestFormat("1", 1.0000m, CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS,
                "Decimal should have no decimal point or zeros.");
            TestFormat("1.23", 1.2300m, CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS,
                "Decimal should have no trailing zeros.");
            TestFormat("1", 1m, CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS,
                "Decimal should have no decimal point or trailing zeros.");
        }

        [TestMethod]
        public void TestDecimalMaxTwoDecimalPlacesNoLeadingZerosDoesNotHaveLeadingZeros()
        {
            TestFormat("8.00", 8, CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS,
                "Decimal should not have leading zeros");
            TestFormat("8.00", 8.0000, CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS,
                "Decimal should not have leading zeros");
            TestFormat("8.00", 8.0010, CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS,
                "Decimal should not have leading zeros");
            TestFormat("8.01", 8.0050, CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS,
                "Decimal should not have leading zeros");
            TestFormat("8,000.00", 8000.0001, CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS,
                "Decimal two decimal places incorrect");
            TestFormat("8,000.000", 8000.0001, CommonStringFormats.DECIMAL_MAX_THREE_DECIMAL_PLACES,
                "Decimal three decimal places incorrect");
        }

        [TestMethod]
        public void TestPercentageModifiesTheValueBeingPresented()
        {
            TestFormat("100.00 %", 1m, CommonStringFormats.PERCENTAGE, "Percentage should have become 100.00 %.");
        }

        [TestMethod]
        public void TestPercentageUnmodifiedDoesNotModifyTheValueBeingPresented()
        {
            TestFormat("1.00 %", 1m, CommonStringFormats.PERCENTAGE_UNMODIFIED,
                "Percentage should not have become 100.00 %.");
        }

        [TestMethod]
        public void TestToFormatStringReturnsCorrectStringFormatsForEachFormatStyle()
        {
            var formatStyles = EnumExtensions.GetValues<FormatStyle>();
            var testedFormatStyles = new List<FormatStyle>(formatStyles.Length);

            Action<string, FormatStyle> areEqual = (expectedFormat, formatStyle) => {
                Assert.AreEqual(expectedFormat, CommonStringFormats.ToFormatString(formatStyle));
                testedFormatStyles.Add(formatStyle);
            };

            areEqual(CommonStringFormats.CURRENCY, FormatStyle.Currency);
            areEqual(CommonStringFormats.CURRENCY_NO_DECIMAL, FormatStyle.CurrencyNoDecimal);
            areEqual(CommonStringFormats.DATE, FormatStyle.Date);
            areEqual(CommonStringFormats.DATETIME_WITHOUT_SECONDS, FormatStyle.DateTimeWithoutSeconds);
            areEqual(CommonStringFormats.DATETIME24_WITHOUT_SECONDS, FormatStyle.DateTime24WithoutSeconds);
            areEqual(CommonStringFormats.DATE_WITHOUT_PARAMETER, FormatStyle.DateWithoutParameter);
            areEqual(CommonStringFormats.DATE_ZERO_PADDED, FormatStyle.DateZeroPadded);
            areEqual(CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES, FormatStyle.DecimalMaxTwoDecimalPlaces);
            areEqual(CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS,
                FormatStyle.DecimalMaxTwoDecimalPlacesWithoutLeadingZeroes);
            areEqual(CommonStringFormats.DECIMAL_WITHOUT_TRAILING_ZEROS, FormatStyle.DecimalWithoutTrailingZeroes);
            areEqual(CommonStringFormats.PERCENTAGE, FormatStyle.Percentage);
            areEqual(CommonStringFormats.PERCENTAGE_UNMODIFIED, FormatStyle.PercentageUnmodified);
            areEqual(CommonStringFormats.TIME_12, FormatStyle.Time12);
            areEqual(CommonStringFormats.TIME_24, FormatStyle.Time24);
            areEqual(CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES_WITH_TRAILING_ZEROS,
                FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes);
            areEqual(CommonStringFormats.DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE, FormatStyle.DateTimeWithoutSecondsWithEstTimezone);
            areEqual(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE, FormatStyle.DateTimeWithSecondsWithEstTimezone);
            areEqual(CommonStringFormats.TIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE,
                FormatStyle.TimeWithoutSecondsWithEstTimezone);
            areEqual(CommonStringFormats.DECIMAL_MAX_THREE_DECIMAL_PLACES,
                FormatStyle.DecimalMaxThreeDecimalPlaces);
            areEqual(CommonStringFormats.DECIMAL_MAX_FOUR_DECIMAL_PLACES,
                FormatStyle.DecimalMaxFourDecimalPlaces);
            areEqual(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS, FormatStyle.DateTimeSecondsWithEstTimeZoneForWebforms);

            Assert.AreEqual(formatStyles.Length, testedFormatStyles.Count,
                "You added a FormatStyle value but did not include it in the ToFormatString call.");
        }

        [TestMethod]
        public void TestToFormatStringThrowsAnExceptionIfAFormatStyleIsNotIncludedInTheDictionary()
        {
            var formatStyles = EnumExtensions.GetValues<FormatStyle>();
            foreach (var fs in formatStyles)
            {
                MyAssert.DoesNotThrow(() => CommonStringFormats.ToFormatString(fs));
            }
        }
    }
}
