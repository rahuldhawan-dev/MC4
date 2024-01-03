using System;

namespace MMSINC.Utilities.WorkDayEngine.DateFinders
{
    internal class MemorialDayFinder : DateFinder
    {
        #region Constructors

        public MemorialDayFinder(int year) : base(year) { }

        #endregion

        #region Private Methods

        private DateTime GetLastDayInMayOfYear()
        {
            return new DateTime(Year, 5, 31);
        }

        private DateTime GetLastMondayInMay()
        {
            var currentDate = GetLastDayInMayOfYear();

            while (currentDate.DayOfWeek != DayOfWeek.Monday)
                currentDate = currentDate.AddDays(-1);

            return currentDate;
        }

        #endregion

        #region Exposed Methods

        internal override DateTime FindDate()
        {
            return GetLastMondayInMay();
        }

        #endregion
    }
}
