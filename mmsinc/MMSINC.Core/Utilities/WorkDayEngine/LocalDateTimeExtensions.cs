using System;
using MMSINC.Utilities.WorkDayEngine.DateFinders;

namespace MMSINC.Utilities.WorkDayEngine
{
    public static class LocalDateTimeExtensions
    {
        #region Exposed Methods

        /// <summary>
        /// Determines whether the given date is a holiday based on the given
        /// configuration.
        /// </summary>
        /// <param name="date">The date to test against the config.</param>
        /// <param name="config">The configuration of various holidays and
        /// specifics to define what is or is not a work day.</param>
        /// <returns>A boolean value indicating whether or not the given date
        /// is a holiday based on the given configuration.</returns>
        public static bool IsHoliday(this DateTime date, WorkDayEngineConfiguration config)
        {
            return config.IsHoldiay(date);
        }

        /// <summary>
        /// Finds and returns the observed date of New Year's day within the given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for New
        /// Year's Day within.</param>
        /// <returns>The observed date of New Year's Day in the given year.</returns>
        public static DateTime GetNewYearsDayByYear(this DateTime date)
        {
            // very first day in a given year is the official day
            return GetNewYearsDayByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the date of Martin Luther King day within the
        /// given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for Martin
        /// Luther King Day within.</param>
        /// <returns>The date of Martin Luther King Day in the given year.</returns>
        public static DateTime GetMartinLutherKingDayByYear(this DateTime date)
        {
            // third monday in January
            return GetMartinLutherKingDayByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the observed date of Abraham Lincoln's birthday within the
        /// given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for Abraham
        /// Lincoln's birthday.
        /// </param>
        /// <returns>The observed date of Abraham Lincoln's birthday in the given year.
        /// </returns>
        public static DateTime GetLincolnsBirthdayByYear(this DateTime date)
        {
            // Feburary 12 is his official birthday.
            return GetLincolnsBirthdayByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the date of George Washington's birthday within the
        /// given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for George
        /// Washington's birthday.
        /// </param>
        /// <returns>The date of George Washington's birthday in the given year.
        /// </returns>
        public static DateTime GetWashingtonsBirthdayByYear(this DateTime date)
        {
            // third Monday in February
            return GetWashingtonsBirthdayByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the date of Good Friday within the given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for Good
        /// Friday within.</param>
        /// <returns>The date of Good Friday in the given year.</returns>
        public static DateTime GetGoodFridayByYear(this DateTime date)
        {
            return GetGoodFridayByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the date of Easter Sunday within the given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for Easter
        /// within.</param>
        /// <returns>The date of Easter Sunday in the given year.</returns>
        public static DateTime GetEasterByYear(this DateTime date)
        {
            return GetEasterByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the date of Memorial Day within the given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for Memorial
        /// Day within.</param>
        /// <returns>The date of Memorial Day in the given year.</returns>
        public static DateTime GetMemorialDayByYear(this DateTime date)
        {
            // last Monday in May
            return GetMemorialDayByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the observed date of Independence Day within the given
        /// year.
        /// </summary>
        /// <param name="date">Date containing the year to search for
        /// Independence Day within.</param>
        /// <returns>The observed date of Independence Day in the given year.</returns>
        public static DateTime GetIndependenceDayByYear(this DateTime date)
        {
            // July 4
            return GetIndependenceDayByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the observed date of Juneteenth within the given year.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetJuneteenthByYear(this DateTime date)
        {
            return GetJuneteenthByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the date of Labor Day within the given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for 
        /// Labor Day within.</param>
        /// <returns>The date of Labor Day in the given year.</returns>
        public static DateTime GetLaborDayByYear(this DateTime date)
        {
            // first Monday in September
            return GetLaborDayByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the observed date of Columbus Day within the given year.
        /// (Second Monday in October)
        /// </summary>
        /// <param name="date">Date containing the year to search for
        /// Columbus Day within.</param>
        /// <returns>The date of Columbus Day in the given year.</returns>
        public static DateTime GetColumbusDayByYear(this DateTime date)
        {
            // second Monday in October
            return GetColumbusDayByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the date of Election Day within the given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for
        /// Election Day within.</param>
        /// <returns>The date of Election Day in the given year.</returns>
        public static DateTime GetElectionDayByYear(this DateTime date)
        {
            // Tuesday after the first Monday in November
            return GetElectionDayByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the date of Veterans Day within the given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for
        /// Veterans Day within.</param>
        /// <returns>The date of Veterans Day in the given year.</returns>
        public static DateTime GetVeteransDayByYear(this DateTime date)
        {
            // either November the 11th or:
            //    if on a Sunday, the following Monday is reserved
            //    if on a Saturday, the preceding Friday is reserved
            return GetVeteransDayByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the date of Thanksgiving within the given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for
        /// Thanksgiving within.</param>
        /// <returns>The date of Thanksgiving in the given year.</returns>
        public static DateTime GetThanksgivingByYear(this DateTime date)
        {
            // fourth Thursday in November
            return GetThanksgivingByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the observed date of Christmas within the given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for
        /// Christmas within.</param>
        /// <returns>The observed date of Christmas in the given year.</returns>
        public static DateTime GetChristmasByYear(this DateTime date)
        {
            return GetChristmasByYear(date.Year);
        }

        /// <summary>
        /// Finds and returns the observed date of Presidents Day within the given year.
        /// </summary>
        /// <param name="date">Date containing the year to search for
        /// Presidents Day within.</param>
        /// <returns>The observed date of Presidents Day in the given year.</returns>
        public static DateTime GetPresidentsDayByYear(this DateTime date)
        {
            return GetPresidentsDayByYear(date.Year);
        }

        #endregion

        #region Private Methods

        private static DateTime GetNewYearsDayByYear(int year)
        {
            return GetObservedDate(new DateTime(year, 1, 1));
        }

        private static DateTime GetMartinLutherKingDayByYear(int year)
        {
            return new MartinLutherKingDayFinder(year);
        }

        private static DateTime GetPresidentsDayByYear(int year)
        {
            return new PresidentsDayFinder(year);
        }

        private static DateTime GetLincolnsBirthdayByYear(int year)
        {
            return GetObservedDate(new DateTime(year, 2, 12));
        }

        private static DateTime GetWashingtonsBirthdayByYear(int year)
        {
            return new WashingtonsBirthdayFinder(year);
        }

        private static DateTime GetMemorialDayByYear(int year)
        {
            return new MemorialDayFinder(year);
        }

        private static DateTime GetIndependenceDayByYear(int year)
        {
            return GetObservedDate(new DateTime(year, 7, 4));
        }

        public static DateTime GetJuneteenthByYear(int year)
        {
            return new JuneteenthFinder(year);
        }

        private static DateTime GetLaborDayByYear(int year)
        {
            return new LaborDayFinder(year);
        }

        private static DateTime GetColumbusDayByYear(int year)
        {
            return new ColumbusDayFinder(year);
        }

        private static DateTime GetGoodFridayByYear(int year)
        {
            return GetEasterByYear(year).AddDays(-2);
        }

        private static DateTime GetEasterByYear(int year)
        {
            return new EasterFinder(year);
        }

        private static DateTime GetElectionDayByYear(int year)
        {
            return new ElectionDayFinder(year);
        }

        private static DateTime GetVeteransDayByYear(int year)
        {
            return new VeteransDayFinder(year);
        }

        private static DateTime GetThanksgivingByYear(int year)
        {
            return new ThanksgivingFinder(year);
        }

        private static DateTime GetChristmasByYear(int year)
        {
            return GetObservedDate(new DateTime(year, 12, 25));
        }

        /// <summary>
        /// Returns the observed date for a holiday if it falls on a weekend,
        /// otherwise returns the date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static DateTime GetObservedDate(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return date.AddDays(-1);
                case DayOfWeek.Sunday:
                    return date.AddDays(1);
                default:
                    return date;
            }
        }

        #endregion
    }
}
