using System;
using System.Text;
using System.Collections.Generic;

namespace MapCall.Common.Model.Entities
{
    public class AspnetWebEventEvents
    {
        public virtual string EventId { get; set; }
        public virtual DateTime EventTimeUtc { get; set; }
        public virtual DateTime EventTime { get; set; }
        public virtual string EventType { get; set; }
        public virtual decimal EventSequence { get; set; }
        public virtual decimal EventOccurrence { get; set; }
        public virtual int EventCode { get; set; }
        public virtual int EventDetailCode { get; set; }
        public virtual string Message { get; set; }
        public virtual string ApplicationPath { get; set; }
        public virtual string ApplicationVirtualPath { get; set; }
        public virtual string MachineName { get; set; }
        public virtual string RequestUrl { get; set; }
        public virtual string ExceptionType { get; set; }
        public virtual string Details { get; set; }
    }
}
