using System;
using MMSINC.ClassExtensions.DateTimeExtensions;

namespace MMSINC.Utilities.WorkDayEngine.DateFinders
{
    internal class ThanksgivingFinder : DateFinder
    {
        #region Constructors

        internal ThanksgivingFinder(int year) : base(year) { }

        #endregion

        #region Private Methods

        private DateTime GetNovemberOfYear()
        {
            return new DateTime(Year, 11, 1);
        }

        private DateTime GetFirstThursdayInNovember()
        {
            var currentDate = GetNovemberOfYear();

            while (currentDate.DayOfWeek != DayOfWeek.Thursday)
                currentDate = currentDate.AddDays(1);

            return currentDate;
        }

        private DateTime GetFourthThursdayInNovember()
        {
            return GetFirstThursdayInNovember().AddWeeks(3);
        }

        #endregion

        #region Exposed Methods

        internal override DateTime FindDate()
        {
            return GetFourthThursdayInNovember();
        }

        #endregion
    }
}
