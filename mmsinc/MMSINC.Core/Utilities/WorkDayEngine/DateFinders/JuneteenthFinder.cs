using System;
using MMSINC.ClassExtensions.DateTimeExtensions;

namespace MMSINC.Utilities.WorkDayEngine.DateFinders
{
    internal class JuneteenthFinder : DateFinder
    {
        #region Constructors

        public JuneteenthFinder(int year) : base(year) { }

        #endregion

        #region Private Methods

        private DateTime GetFirstFridayInJune()
        {
            var currentDate = GetJuneOfYear();

            while (currentDate.DayOfWeek != DayOfWeek.Friday)
                currentDate = currentDate.AddDays(1);

            return currentDate;
        }

        private DateTime GetJuneOfYear()
        {
            return new DateTime(Year, 6, 1);
        }

        #endregion

        #region Exposed Methods

        internal override DateTime FindDate()
        {
            return GetFirstFridayInJune().AddWeeks(2);
        }

        #endregion
    }
}
