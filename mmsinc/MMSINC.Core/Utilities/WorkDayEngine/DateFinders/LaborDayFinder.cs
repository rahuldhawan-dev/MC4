using System;

namespace MMSINC.Utilities.WorkDayEngine.DateFinders
{
    internal class LaborDayFinder : DateFinder
    {
        #region Constructors

        internal LaborDayFinder(int year) : base(year) { }

        #endregion

        #region Private Methods

        private DateTime GetSeptemberOfYear()
        {
            return new DateTime(Year, 9, 1);
        }

        private DateTime GetFirstMondayInSeptember()
        {
            var currentDate = GetSeptemberOfYear();

            while (currentDate.DayOfWeek != DayOfWeek.Monday)
                currentDate = currentDate.AddDays(1);

            return currentDate;
        }

        #endregion

        #region Exposed Methods

        internal override DateTime FindDate()
        {
            return GetFirstMondayInSeptember();
        }

        #endregion
    }
}
