using System;
using MMSINC.ClassExtensions.DateTimeExtensions;

namespace MMSINC.Utilities.WorkDayEngine.DateFinders
{
    internal class ColumbusDayFinder : DateFinder
    {
        #region Constructors

        internal ColumbusDayFinder(int year) : base(year) { }

        #endregion

        #region Private Methods

        private DateTime GetOctoberOfYear()
        {
            return new DateTime(Year, 10, 1);
        }

        private DateTime GetFirstMondayInOctober()
        {
            var currentDate = GetOctoberOfYear();

            while (currentDate.DayOfWeek != DayOfWeek.Monday)
                currentDate = currentDate.AddDays(1);

            return currentDate;
        }

        private DateTime GetSecondMondayInOctober()
        {
            return GetFirstMondayInOctober().AddWeeks(1);
        }

        #endregion

        #region Exposed Methods

        internal override DateTime FindDate()
        {
            return GetSecondMondayInOctober();
        }

        #endregion
    }
}
