using System;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Utilities;

namespace MapCallScheduler.STIGenerator
{
    public class StaticDateTimeProvider : IDateTimeProvider
    {
        private readonly DateTime _now;

        public StaticDateTimeProvider(DateTime now)
        {
            _now = now;
        }

        public DateTime GetCurrentDate()
        {
            return _now;
        }

        public DateTime GetNext(int hour, int minute = 0, int second = 0)
        {
            return _now.GetNext(hour, minute, second);
        }
    }
}