using System;
using MMSINC.ClassExtensions.DateTimeExtensions;

namespace MMSINC.Utilities.WorkDayEngine.DateFinders
{
    internal class MartinLutherKingDayFinder : DateFinder
    {
        #region Constructors

        internal MartinLutherKingDayFinder(int year)
            : base(year) { }

        #endregion

        #region Private Methods

        private DateTime GetJanuaryOfYear()
        {
            return new DateTime(Year, 1, 1);
        }

        private DateTime GetFirstMondayInJanuary()
        {
            var currentDate = GetJanuaryOfYear();

            while (currentDate.DayOfWeek != DayOfWeek.Monday)
                currentDate = currentDate.AddDays(1);

            return currentDate;
        }

        private DateTime GetThirdMondayInJanuary()
        {
            return GetFirstMondayInJanuary().AddWeeks(2);
        }

        #endregion

        #region Exposed Methods

        internal override DateTime FindDate()
        {
            return GetThirdMondayInJanuary();
        }

        #endregion
    }
}
