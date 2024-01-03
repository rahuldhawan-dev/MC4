using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

// ReSharper disable CheckNamespace
namespace MMSINC.ClassExtensions.DateTimeExtensions
    // ReSharper restore CheckNamespace
{
    public static class DateTimeExtensions
    {
        #region Contants

        public const int DAYS_IN_WEEK = 7;

        #endregion

        #region Extension Methods

        public static DateTime AddWeeks(this DateTime date, int weeks)
        {
            return date.AddDays(weeks * DAYS_IN_WEEK);
        }

        public static DateTime GetNextWeek(this DateTime date)
        {
            return date.AddWeeks(1);
        }

        public static DateTime GetNextDay(this DateTime date)
        {
            return date.AddDays(1);
        }

        public static DateTime Next(this DateTime from, DayOfWeek dayOfTheWeek)
        {
            var date = from.Date.AddDays(1); 
            var days = (DAYS_IN_WEEK + (int)dayOfTheWeek - (int)date.DayOfWeek) % DAYS_IN_WEEK; 
            
            return date.AddDays(days);
        }

        public static DateTime GetDayFromWeek(this DateTime date, DayOfWeek day)
        {
            return date.AddDays(((int)day) - ((int)date.DayOfWeek));
        }

        public static DateTime SubtractYears(this DateTime date, int value)
        {
            return new DateTime(date.Year - value, date.Month, date.Month,
                date.Hour, date.Minute, date.Second, date.Millisecond,
                date.Kind);
        }

        public static DateTime GetBeginningOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Get the next instance of an hour (+ minute and second).
        /// 
        /// For instance, if it's currently 7 am, calling this for 8 will return 
        /// the current date at 8 am.  If it's currently 8:30 am, calling this
        /// for 8 will return the next day at 8 am.
        /// </summary>
        /// <param name="hour">Hour in 24 hour time.</param>
        public static DateTime GetNext(this DateTime date, int hour, int minute = 0, int second = 0)
        {
            if ((date.Hour > hour || (date.Hour == hour && date.Minute >= minute)))
            {
                date = date.AddDays(1);
            }

            return new DateTime(date.Year, date.Month, date.Day, hour, minute, second);
        }

        public static DateTime GetEndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        public static DateTime GetPreviousDay(this DateTime date)
        {
            return date.AddDays(-1);
        }

        public static DateTime BeginningOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static DateTime BeginningOfMinute(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0, 0);
        }

        public static bool IsWeekendDay(this DateTime date)
        {
            return (date.DayOfWeek == DayOfWeek.Saturday ||
                    date.DayOfWeek == DayOfWeek.Sunday);
        }

        public static bool IsBetween(this DateTime date, DateTime start, DateTime end)
        {
            return date.IsBetween(start, end, true);
        }

        public static bool IsBetween(this DateTime date, DateTime start, DateTime end, bool inclusive)
        {
            if (start > end)
                throw new ArgumentException("Start must lie chronologically before End.");

            return inclusive ? (date >= start && date <= end) : (date > start && date < end);
        }

        public static int GetYearSixth(this DateTime date)
        {
            return (date.Month - 1) / 2 + 1;
        }

        public static int GetQuarter(this DateTime date)
        {
            return (date.Month - 1) / 3 + 1;
        }

        public static int GetYearThird(this DateTime date)
        {
            return (date.Month - 1) / 4 + 1;
        }

        /// <summary>
        /// Returns a date to the beginning of the hour, rounding down to the earliest hour. 
        /// ie: 11:59 will return 11:00, not 12:00.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToStartOfHour(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
        }

        public static long SecondsSinceEpoch(this DateTime that)
        {
            return new DateTimeOffset(that).ToUnixTimeSeconds();
        }

        public static long MillisecondsSinceEpoch(this DateTime that)
        {
            return new DateTimeOffset(that).ToUnixTimeMilliseconds();
        }

        public static int Decade(this DateTime date)
        {
            var x = (decimal)date.Year / 10;
            var y = Math.Floor(x);
            return (int)y * 10;
        }

        public static int GetWeekOfMonth(this DateTime date)
        {
            var first = new DateTime(date.Year, date.Month, 1);
            return date.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        public static int GetWeekOfYear(this DateTime time)
        {
            var gc = new GregorianCalendar();
            return gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        public static int MonthsDifference(DateTime start, DateTime end)
        {
            if (start.Year == end.Year && start.Month == end.Month)
            {
                return 0;
            }

            if (start > end)
            {
                var swap = start;
                start = end;
                end = swap;
            }

            return 1 + MonthsDifference(start.AddMonths(1), end);
        }

        public static int WeeksDifference(DateTime start, DateTime end)
        {
            return (int)(start.Date.GetDayFromWeek(DayOfWeek.Sunday) - end.Date.GetDayFromWeek(DayOfWeek.Sunday))
               .TotalDays / DAYS_IN_WEEK;
        }
        
        /// <summary>
        /// Returns true if the month follows a quarter end month, otherwise false.
        /// Quarter end months are defined as March, June, October, and December.
        /// Months following quarter end months are defined as April, July, November, and January.
        /// </summary>
        /// <param name="date">Date containing the month to evaluate.</param>
        /// <returns>A boolean indicating whether the month follows a quarter end month.</returns>
        public static bool DoesMonthFollowQuarterEndMonth(this DateTime date) =>
            date.AddMonths(-1).GetQuarter() != date.GetQuarter();

        /// <summary>
        /// Returns a collection of days of the month.
        /// </summary>
        /// <param name="date">Date containing the month to return days for</param>
        /// <returns>A collection of DateTime values representing each day of the given dates month</returns>
        public static IEnumerable<DateTime> GetDaysInMonth(this DateTime date) =>
            Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month))
                      .Select(d => new DateTime(date.Year, date.Month, d));

        #endregion
    }
}
