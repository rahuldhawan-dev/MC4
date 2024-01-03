using System;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public static class NullableDateTimeExtensions
    {
        #region Exposed Methods

        public static DateTime? FromDbRecord(this DateTime? when)
        {
            return when == null ? (DateTime?)null : when.Value.ToUniversalTime();
        }

        #endregion
    }
}
