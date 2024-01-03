using System;

namespace MapCallScheduler.Metadata
{
    /// <summary>
    /// Allows a job to be started at a specific time of day.
    /// </summary>
    public class StartAtAttribute : Attribute
    {
        #region Properties

        public int Minute { get; }

        public int Hour { get; }

        #endregion

        #region Constructors

        public StartAtAttribute(int hour) : this(hour, 0) {}

        public StartAtAttribute(int hour, int minute)
        {
            Hour = hour;
            Minute = minute;
        }

        #endregion
    }
}
