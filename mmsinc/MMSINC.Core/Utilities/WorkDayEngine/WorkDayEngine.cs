using System;
using System.Collections.Generic;
using MMSINC.ClassExtensions.DateTimeExtensions;

namespace MMSINC.Utilities.WorkDayEngine
{
    /// <summary>
    /// Suitable for determining sequences of work days, with optional holiday
    /// configuration.
    /// </summary>
    public class WorkDayEngine<TWorkDayEngineConfig>
        where TWorkDayEngineConfig : WorkDayEngineConfiguration, new()
    {
        #region Private Static Members

        private static WorkDayEngineConfiguration _config;

        #endregion

        #region Static Properties

        public static WorkDayEngineConfiguration Configuration
        {
            get
            {
                if (_config == null)
                    _config = new TWorkDayEngineConfig();
                return _config;
            }
        }

        #endregion

        #region Private Static Methods

        private static bool SkipDay(DateTime date)
        {
            return date.IsWeekendDay() || date.IsHoliday(Configuration);
        }

        private static DateTime IncrementRecursive(DateTime date, int days, bool endOnSkipDay)
        {
            if (days == 0)
                return date;
            // bug #: 958
            if (days == 1 && !endOnSkipDay)
                return date.GetNextDay();
            var nextDay = date.GetNextDay();
            return IncrementRecursive(nextDay,
                SkipDay(nextDay) ? days : days - 1, endOnSkipDay);
        }

        private static DateTime DecrementRecursive(DateTime date, int days)
        {
            if (days == 0)
                return date;
            var previousDay = date.GetPreviousDay();
            return DecrementRecursive(previousDay,
                SkipDay(previousDay) ? days : days + 1);
        }

        #endregion

        #region Exposed Static Methods

        /// <summary>
        /// Increments and returns the given date by the given number of work
        /// days.
        /// </summary>
        /// <param name="date">Date to increment.</param>
        /// <param name="days">Number of days to increment.</param>
        /// <returns>The date, incremented by the given number of work days.
        /// </returns>
        public static DateTime IncrementByDays(DateTime date, int days, bool endOnSkipDay = false)
        {
            return days == 0
                ? date
                : IncrementRecursive(date, days + 1, endOnSkipDay);
        }

        public static DateTime DecrementByDays(DateTime date, int days)
        {
            return days == 0
                ? date
                : DecrementRecursive(date, days - 1);
        }

        public static DateTime IncrementByCalendarDays(DateTime date, int days)
        {
            return (days == 0) ? date : date.AddDays(days);
        }

        #endregion
    }

    public abstract class WorkDayEngineConfiguration
    {
        #region Private Members

        private int? _calendarYear;

        #endregion

        #region Properties

        public virtual bool UseChristmas => false;

        public virtual bool UseChristmasEve => false;

        public virtual bool UseColumbusDay => false;

        public virtual bool UseElectionDay => false;

        public virtual bool UseGoodFriday => false;

        public virtual bool UseIndependenceDay => false;

        public virtual bool UseJuneteenth => false;

        public virtual bool UseLaborDay => false;

        public virtual bool UseLincolnsBirthday => false;

        public virtual bool UseMartinLutherKingDay => false;

        public virtual bool UseMemorialDay => false;

        public virtual bool UseNewYearsDay => false;

        public virtual bool UseNewYearsEveDay => false;

        public virtual bool UsePresidentsDay => false;

        public virtual bool UseThanksgiving => false;

        public virtual bool UseThanksgivingFriday => false;

        public virtual bool UseVeteransDay => false;

        public virtual bool UseWashingtonsBirthday => false;

        public virtual List<DateTime> Holidays { get; protected set; }

        #endregion

        #region Private Methods

        protected virtual void BuildHolidayCalendar(DateTime year)
        {
            var list = new List<DateTime>();
            if (UseChristmasEve)
                list.Add(year.GetChristmasByYear().AddDays(-1));
            if (UseChristmas)
                list.Add(year.GetChristmasByYear());
            if (UseColumbusDay)
                list.Add(year.GetColumbusDayByYear());
            if (UseElectionDay)
                list.Add(year.GetElectionDayByYear());
            if (UseGoodFriday)
                list.Add(year.GetGoodFridayByYear());
            if (UseIndependenceDay)
                list.Add(year.GetIndependenceDayByYear());
            if (UseJuneteenth)
                list.Add(year.GetJuneteenthByYear());
            if (UseLaborDay)
                list.Add(year.GetLaborDayByYear());
            if (UseLincolnsBirthday)
                list.Add(year.GetLincolnsBirthdayByYear());
            if (UseMartinLutherKingDay)
                list.Add(year.GetMartinLutherKingDayByYear());
            if (UseMemorialDay)
                list.Add(year.GetMemorialDayByYear());
            if (UseNewYearsDay)
            {
                list.Add(year.GetNewYearsDayByYear());
                list.Add(year.AddYears(1).GetNewYearsDayByYear());
            }
            if (UseNewYearsEveDay)
            {
                list.Add(year.GetNewYearsDayByYear().AddDays(-1));
                list.Add(year.AddYears(1).GetNewYearsDayByYear().AddDays(-1));
            }
            if (UsePresidentsDay)
                list.Add(year.GetPresidentsDayByYear());

            if (UseThanksgiving)
                list.Add(year.GetThanksgivingByYear());
            if (UseThanksgivingFriday)
                list.Add(year.GetThanksgivingByYear().AddDays(1));
            if (UseVeteransDay)
                list.Add(year.GetVeteransDayByYear());
            if (UseWashingtonsBirthday)
                list.Add(year.GetWashingtonsBirthdayByYear());
            Holidays = list;
            _calendarYear = year.Year;
        }

        #endregion

        #region Exposed Methods

        public virtual bool IsHoldiay(DateTime date)
        {
            if (_calendarYear == null || _calendarYear.Value != date.Year)
                BuildHolidayCalendar(date);
            return Holidays.Contains(date.Date);
        }

        #endregion
    }
}
