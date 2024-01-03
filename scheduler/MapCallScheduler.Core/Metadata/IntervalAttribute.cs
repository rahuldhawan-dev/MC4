using System;
using Quartz;

namespace MapCallScheduler.Metadata
{
    /// <summary>
    /// Used to determine the interval at which a job should be repeated.  Jobs without
    /// this attribute will default to every 24 hours.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class IntervalAttribute : Attribute
    {
        #region Properties

        /// <summary>
        /// Number of hour
        /// </summary>
        public int Interval { get; private set; }
        public IntervalType IntervalType { get; private set; }

        #endregion

        #region Constructors

        protected IntervalAttribute(int interval, IntervalType intervalType)
        {
            Interval = interval;
            IntervalType = intervalType;
        }

        #endregion

        #region Abstract Methods

        public abstract SimpleScheduleBuilder SetInterval(SimpleScheduleBuilder builder);

        #endregion
    }

    /// <summary>
    /// Used to specify that a job should run Daily.
    /// </summary>
    public class DailyAttribute : IntervalAttribute
    {
        #region Constructors

        public DailyAttribute() : base(24, IntervalType.Hourly) {}
        public DailyAttribute(int days) : base(24 * days, IntervalType.Hourly) {}

        #endregion

        #region Exposed Methods

        public override SimpleScheduleBuilder SetInterval(SimpleScheduleBuilder builder)
        {
            return builder.WithIntervalInHours(Interval);
        }

        #endregion
    }

    /// <summary>
    /// Used to specify that a job should run Hourly.
    /// </summary>
    public class HourlyAttribute : IntervalAttribute
    {
        #region Constructors

        public HourlyAttribute() : base(1, IntervalType.Hourly) {}
        public HourlyAttribute(int hours) : base(hours, IntervalType.Hourly) {}

        #endregion

        #region Exposed Methods

        public override SimpleScheduleBuilder SetInterval(SimpleScheduleBuilder builder)
        {
            return builder.WithIntervalInHours(Interval);
        }

        #endregion
    }

    /// <summary>
    /// Used to specify that a job should run Minutely.
    /// </summary>
    public class MinutelyAttribute : IntervalAttribute
    {
        #region Constructors

        public MinutelyAttribute() : base(1, IntervalType.Minutely) {}
        public MinutelyAttribute(int minutes) : base(minutes, IntervalType.Minutely) {}

        #endregion

        #region Exposed Methods

        public override SimpleScheduleBuilder SetInterval(SimpleScheduleBuilder builder)
        {
            return builder.WithIntervalInMinutes(Interval);
        }

        #endregion
    }

    /// <summary>
    /// Used to specify that a job should run Secondly.
    /// </summary>
    public class SecondlyAttribute : IntervalAttribute
    {
        #region Constructors

        public SecondlyAttribute() : base(1, IntervalType.Secondly) {}
        public SecondlyAttribute(int seconds) : base(seconds, IntervalType.Secondly) {}

        #endregion

        #region Exposed Methods

        public override SimpleScheduleBuilder SetInterval(SimpleScheduleBuilder builder)
        {
            return builder.WithIntervalInSeconds(Interval);
        }

        #endregion
    }

    public enum IntervalType
    {
        Hourly,
        Minutely,
        Secondly
    }
}
