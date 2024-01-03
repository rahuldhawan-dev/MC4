using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MMSINC.Utilities
{
    /// <summary>
    /// Consts of common string formats so we don't have to rewrite them all the time.
    /// </summary>
    /// <remarks>
    /// 
    /// If you add a string format here, PLEASE add the functionality to the FormatStyle enum.
    /// 
    /// </remarks>
    public static class CommonStringFormats
    {
        #region constants

        /// <summary>
        /// Formats a date to look like: 4/24/1984 or 10/9/2013. No padded zeros for single digit months/days.
        /// Use this one when formatting via string.Format(). 
        /// Use this one for DisplayFormatAttribute in MVC.
        /// </summary>
        public const string DATE = "{0:d}";

        /// <summary>
        /// Formats a date to look like: 1984-02-21 or 2013-05-19.  Adds zeroes for single digit months/days.
        /// Use this one for when formatting via ToString overloads.
        /// </summary>
        public const string SQL_DATE_WITHOUT_PARAMETER = "yyyy-MM-dd";

        /// <summary>
        /// Formats a date to look like: 04/24/1984 or 10/09/2013. Adds zeros for single digit months/days.
        /// Use this one when formatting via string.Format(). 
        /// Use this one for DisplayFormatAttribute in MVC.
        /// </summary>
        public const string DATE_ZERO_PADDED = "{0:MM/dd/yyyy}";

        /// <summary>
        /// Formats a date to look like: 4/24/1984 or 10/9/2013. No padded zeros for single digit months/days.
        /// Use this one when formatting via ToString overloads.
        /// </summary>
        public const string DATE_WITHOUT_PARAMETER = "d";

        /// <summary>
        /// Formats a date to look like: 4/24/1984 4:04 AM.
        /// Use this one for DisplayFormatAttribute in MVC.
        /// </summary>
        public const string DATETIME_WITHOUT_SECONDS = "{0:M/d/yyyy h:mm tt}";

        /// <summary>
        /// Formats a date to look like: 4/24/1984 4:04 AM (EST).
        /// Use this one for DisplayFormatAttribute in MVC.
        /// </summary>
        public const string DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE = "{0:M/d/yyyy h:mm tt (EST)}";

        /// <summary>
        /// Formats a date to look like: 4/24/1984 4:04:00 AM (EST).
        /// Use this one for DisplayFormatAttribute in MVC.
        /// </summary>
        public const string DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE = "{0:M/d/yyyy h:mm:ss tt (EST)}";

        /// <summary>
        /// Formats a date to look like: 4:04 AM (EST).
        /// Use this one for DisplayFormatAttribute in MVC.
        /// </summary>
        public const string TIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE = "{0:h:mm tt (EST)}";

        /// <summary>
        /// Formats a date to look like: 4/24/1984 14:04.
        /// Use this one for DisplayFormatAttribute in MVC.
        /// </summary>
        public const string DATETIME24_WITHOUT_SECONDS = "{0:M/d/yyyy HH:mm}";

        public const string DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS = "M/d/yyyy h:mm:ss tt (EST)";
        
        /// <summary>
        /// Military Time with seconds 13:21:59
        /// </summary>
        public const string TIME_24 = "{0:H:mm:ss}";

        public const string TIME_12 = "{0:hh:mm:ss tt}";

        /// <summary>
        /// Formats a date to look like: January 18, 2023.
        /// Use this one for DisplayFormatAttribute in MVC.
        /// </summary>
        public const string DATE_WITH_MONTH_TEXT_FORMAT = "{0:MMMM d, yyyy}";

        /// <summary>
        /// Removes trailing zeroes from ecimals. ex: "4.210000" will become "4.21"
        /// and "5.00000" will become "5".
        /// </summary>
        public const string DECIMAL_WITHOUT_TRAILING_ZEROS = "{0:G29}";

        /// <summary>
        /// It's money.
        /// </summary>
        [Obsolete("Use CURRENCY")] public const string MONEY = "{0:c}";

        /// <summary>
        /// String.Format("{0:0.##}", 123.4567);      // "123.46"
        /// String.Format("{0:0.##}", 123.4);         // "123.4"
        /// String.Format("{0:0.##}", 123.0);         // "123"
        /// String.Format("{0:#,###.000}", 1,234.000);    // "1,234.000"
        /// String.Format("{0:#,###.0000}", 1,234.0000);    // "1,234.0000"
        /// </summary>
        public const string DECIMAL_MAX_TWO_DECIMAL_PLACES = "{0:0,0.##}";

        public const string DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS = "{0:N}";
        public const string DECIMAL_MAX_THREE_DECIMAL_PLACES = "{0:#,###.000}";
        public const string DECIMAL_MAX_FOUR_DECIMAL_PLACES = "{0:#,##0.0000}";

        /// <summary>
        /// ie: 10 becomes 10.00. The other format doesn't pad the zeros at the end.
        /// </summary>
        public const string DECIMAL_MAX_TWO_DECIMAL_PLACES_WITH_TRAILING_ZEROS = "{0:0.00}";

        public const string CURRENCY = "{0:c}";
        public const string CURRENCY_NO_DECIMAL = "{0:c0}";

        /// <remarks>
        /// Don't use "{0:p}" as it's dependent on the current system CultureInfo. A Windows
        /// update can cause this formatting to change, which leads to test failures and changes
        /// in production.
        ///
        /// The "p" formatting comes from CultureInfo.CurrentCulture.NumberFormat
        /// In particular, the actual formatting of where the "%" is placed is based on NumberFormat's
        /// PercentPositivePattern and PercentNegativePattern properties. When it's set to 0, you
        /// get "100.00 %"(same as invariant culture). When it's set 1 you get "100.00%".
        /// </remarks>
        public const string PERCENTAGE = "{0:0.00 %}";

        public const string PERCENTAGE_UNMODIFIED = "{0:0.00 \\%}";

        #endregion

        #region Fields

        private static readonly ReadOnlyDictionary<FormatStyle, string> _formatStringsByStyle;

        #endregion

        #region Constructor

        static CommonStringFormats()
        {
            // ReadOnlyDictionary should work concurrently here since the internal dictionary can't be 
            // accessed by anything other than the ReadOnlyDictionary.
            _formatStringsByStyle = new ReadOnlyDictionary<FormatStyle, string>(new Dictionary<FormatStyle, string> {
                {FormatStyle.Currency, CURRENCY},
                {FormatStyle.CurrencyNoDecimal, CURRENCY_NO_DECIMAL},
                {FormatStyle.Date, DATE},
                {FormatStyle.DateTimeWithoutSeconds, DATETIME_WITHOUT_SECONDS},
                {FormatStyle.DateTimeWithoutSecondsWithEstTimezone, DATETIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE},
                {FormatStyle.DateTimeWithSecondsWithEstTimezone, DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE},
                {FormatStyle.TimeWithoutSecondsWithEstTimezone, TIME_WITHOUT_SECONDS_WITH_EST_TIMEZONE},
                {FormatStyle.DateTime24WithoutSeconds, DATETIME24_WITHOUT_SECONDS},
                {FormatStyle.DateTimeSecondsWithEstTimeZoneForWebforms, DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS},
                {FormatStyle.DateWithoutParameter, DATE_WITHOUT_PARAMETER},
                {FormatStyle.DateZeroPadded, DATE_ZERO_PADDED},
                {FormatStyle.DecimalMaxTwoDecimalPlaces, DECIMAL_MAX_TWO_DECIMAL_PLACES}, {
                    FormatStyle.DecimalMaxTwoDecimalPlacesWithoutLeadingZeroes,
                    DECIMAL_MAX_TWO_DECIMAL_PLACES_NO_LEADING_ZEROS
                },
                {
                    FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes,
                    DECIMAL_MAX_TWO_DECIMAL_PLACES_WITH_TRAILING_ZEROS
                },
                {FormatStyle.DecimalMaxThreeDecimalPlaces, DECIMAL_MAX_THREE_DECIMAL_PLACES},
                {FormatStyle.DecimalMaxFourDecimalPlaces, DECIMAL_MAX_FOUR_DECIMAL_PLACES},
                {FormatStyle.DecimalWithoutTrailingZeroes, DECIMAL_WITHOUT_TRAILING_ZEROS},
                {FormatStyle.Percentage, PERCENTAGE},
                {FormatStyle.PercentageUnmodified, PERCENTAGE_UNMODIFIED},
                {FormatStyle.Time12, TIME_12},
                {FormatStyle.Time24, TIME_24},
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts a FormatStyle value to the appropriate string format.
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static string ToFormatString(FormatStyle style)
        {
            return _formatStringsByStyle[style];
        }

        #endregion
    }

    /// <summary>
    /// Enum representation of CommonStringFormats.
    /// </summary>
    public enum FormatStyle
    {
        Currency,
        CurrencyNoDecimal,
        Date,
        DateWithoutParameter,
        DateTimeWithoutSeconds,
        DateTime24WithoutSeconds,
        DateZeroPadded,
        DecimalMaxTwoDecimalPlaces,
        DecimalMaxTwoDecimalPlacesWithoutLeadingZeroes,
        DecimalMaxTwoDecimalPlacesWithTrailingZeroes,
        DecimalWithoutTrailingZeroes,
        DecimalMaxThreeDecimalPlaces,
        DecimalMaxFourDecimalPlaces,
        Percentage,
        PercentageUnmodified,
        Time24,
        Time12,
        DateTimeWithoutSecondsWithEstTimezone,
        DateTimeWithSecondsWithEstTimezone,
        TimeWithoutSecondsWithEstTimezone,
        DateTimeSecondsWithEstTimeZoneForWebforms,
    }
}
