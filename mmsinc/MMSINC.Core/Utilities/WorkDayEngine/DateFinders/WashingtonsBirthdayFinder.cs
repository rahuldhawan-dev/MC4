using System;
using MMSINC.ClassExtensions.DateTimeExtensions;

namespace MMSINC.Utilities.WorkDayEngine.DateFinders
{
    internal class WashingtonsBirthdayFinder : DateFinder
    {
        #region Constructors

        internal WashingtonsBirthdayFinder(int year) : base(year) { }

        #endregion

        #region Private Methods

        private DateTime GetFebruaryOfYear()
        {
            return new DateTime(Year, 2, 1);
        }

        private DateTime GetFirstMondayInFebruary()
        {
            var currentDate = GetFebruaryOfYear();

            while (currentDate.DayOfWeek != DayOfWeek.Monday)
                currentDate = currentDate.AddDays(1);

            return currentDate;
        }

        private DateTime GetThirdMondayInFebruary()
        {
            return GetFirstMondayInFebruary().AddWeeks(2);
        }

        #endregion

        #region Exposed Methods

        internal override DateTime FindDate()
        {
            return GetThirdMondayInFebruary();
        }

        #endregion
    }
}
