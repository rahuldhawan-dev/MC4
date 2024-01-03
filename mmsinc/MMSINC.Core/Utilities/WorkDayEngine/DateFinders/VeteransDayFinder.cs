using System;

namespace MMSINC.Utilities.WorkDayEngine.DateFinders
{
    internal class VeteransDayFinder : DateFinder
    {
        #region Constructors

        internal VeteransDayFinder(int year) : base(year) { }

        #endregion

        #region Private Methods

        private DateTime GetNovemberEleventhOfYear()
        {
            return new DateTime(Year, 11, 11);
        }

        private DateTime GetVeteransDay()
        {
            var date = GetNovemberEleventhOfYear();

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return date.AddDays(-1);
                case DayOfWeek.Sunday:
                    return date.AddDays(1);
                default:
                    return date;
            }
        }

        #endregion

        #region Exposed Methods

        internal override DateTime FindDate()
        {
            return GetVeteransDay();
        }

        #endregion
    }
}
