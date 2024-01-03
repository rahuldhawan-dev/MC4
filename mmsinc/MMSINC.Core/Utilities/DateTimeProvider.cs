using System;
using MMSINC.ClassExtensions.DateTimeExtensions;

namespace MMSINC.Utilities
{
    public class DateTimeProvider : IDateTimeProvider
    {
        #region Exposed Methods

        public virtual DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }

        public virtual DateTime GetNext(int hour, int minute = 0, int second = 0)
        {
            return GetCurrentDate().GetNext(hour, minute, second);
        }

        #endregion
    }

    public interface IDateTimeProvider
    {
        #region Methods

        /// <summary>
        /// Gets the current date.
        /// </summary>
        /// <returns>The current date.</returns>
        DateTime GetCurrentDate();

        /// <summary>
        /// Get the next instance of an hour (+ minute and second).
        /// 
        /// For instance, if it's currently 7 am, calling this for 8 will return 
        /// the current date at 8 am.  If it's currently 8:30 am, calling this
        /// for 8 will return the next day at 8 am.
        /// </summary>
        /// <param name="hour">Hour in 24 hour time.</param>
        DateTime GetNext(int hour, int minute = 0, int second = 0);

        #endregion
    }
}
