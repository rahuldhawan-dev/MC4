using System;

namespace MMSINC.Utilities.WorkDayEngine.DateFinders
{
    internal class ElectionDayFinder : DateFinder
    {
        #region Constructors

        internal ElectionDayFinder(int year) : base(year) { }

        #endregion

        #region Private Methods

        private DateTime GetNovemberOfYear()
        {
            return new DateTime(Year, 11, 1);
        }

        private DateTime GetFirstMondayInNovember()
        {
            var currentDate = GetNovemberOfYear();

            while (currentDate.DayOfWeek != DayOfWeek.Monday)
                currentDate = currentDate.AddDays(1);

            return currentDate;
        }

        private DateTime GetTheTuesdayAfterTheFirstMondayInNovember()
        {
            return GetFirstMondayInNovember().AddDays(1);
        }

        #endregion

        #region Exposed Methods

        internal override DateTime FindDate()
        {
            return GetTheTuesdayAfterTheFirstMondayInNovember();
        }

        #endregion
    }
}
