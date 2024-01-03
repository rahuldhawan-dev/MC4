using System;

namespace MMSINC.Utilities.WorkDayEngine.DateFinders
{
    internal abstract class DateFinder
    {
        #region Properties

        internal int Year { get; set; }

        #endregion

        #region Constructors

        internal DateFinder(int year)
        {
            Year = year;
        }

        #endregion

        #region Abstract Methods

        internal abstract DateTime FindDate();

        #endregion

        #region Exposed Methods

        internal virtual DateTime FindDate(int year)
        {
            Year = year;
            return FindDate();
        }

        #endregion

        #region Operators/Casts

        public static implicit operator DateTime(DateFinder finder)
        {
            return finder.FindDate();
        }

        #endregion
    }
}
