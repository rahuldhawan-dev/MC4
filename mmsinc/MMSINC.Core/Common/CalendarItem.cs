using System;

namespace MMSINC.Common
{
    public struct CalendarItem
    {
        public string title { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public string url { get; set; }
        public string color { get; set; }
        public string textColor { get; set; }
        public string description { get; set; }
    }
}
