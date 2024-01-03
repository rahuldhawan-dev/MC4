using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Utilities.WorkDayEngine;

namespace MapCallScheduler.JobHelpers.SystemDeliveryEntry
{
    public class SystemDeliveryWorkDayEngine : WorkDayEngine<SystemDeliveryWorkDayConfiguration>
    {
        #region Private Methods

        /// <summary>
        ///     Returns a collection of business days for the given month.
        /// </summary>
        /// <param name="date">The date to use to gather the business days</param>
        /// <returns>A collection of DateTime values representing each business day of the given month</returns>
        private static IEnumerable<DateTime> GetBusinessDaysInMonth(DateTime date) =>
            date.GetDaysInMonth().Where(d => !d.IsWeekendDay() && !d.IsHoliday(Configuration))
                .OrderBy(d => d.Day);

        #endregion

        #region Exposed Methods

        /// <summary>
        ///     Returns true when the date is the second business day of the month, otherwise false.
        ///     A business day is defined as a day that is not a weekend (Saturday or Sunday), nor a holiday.
        /// </summary>
        /// <param name="date">The date to inspect</param>
        /// <returns>A boolean value based on whether the date is the second business day of the month</returns>
        public static bool IsSecondBusinessDay(DateTime date) => GetBusinessDaysInMonth(date).ElementAt(1) == date;

        /// <summary>
        ///     Returns true when the date is the fourth business day of the month, otherwise false.
        ///     A business day is defined as a day that is not a weekend (Saturday or Sunday), nor a holiday.
        /// </summary>
        /// <param name="date">The date to inspect</param>
        /// <returns>A boolean value based on whether the date is the fourth business day of the month</returns>
        public static bool IsFourthBusinessDay(DateTime date) => GetBusinessDaysInMonth(date).ElementAt(3) == date;

        #endregion
    }

    public class SystemDeliveryWorkDayConfiguration : WorkDayEngineConfiguration
    {
        #region Properties

        public override bool UseChristmas => true;
        public override bool UseChristmasEve => true;
        public override bool UseIndependenceDay => true;
        public override bool UseLaborDay => true;
        public override bool UseMartinLutherKingDay => true;
        public override bool UseMemorialDay => true;
        public override bool UseNewYearsDay => true;
        public override bool UseNewYearsEveDay => true;
        public override bool UsePresidentsDay => true;
        public override bool UseThanksgiving => true;
        public override bool UseThanksgivingFriday => true;

        #endregion
    }
}
