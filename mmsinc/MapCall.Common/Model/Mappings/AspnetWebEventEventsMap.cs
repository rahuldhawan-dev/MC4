using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AspnetWebEventEventsMap : ClassMap<AspnetWebEventEvents>
    {
        public AspnetWebEventEventsMap()
        {
            Table("aspnet_WebEvent_Events");

            LazyLoad();
            ReadOnly();

            Id(x => x.EventId).GeneratedBy.Assigned().Column("EventId");

            Map(x => x.EventTimeUtc).Column("EventTimeUtc").Not.Nullable();
            Map(x => x.EventTime).Column("EventTime").Not.Nullable();
            Map(x => x.EventType).Column("EventType").Not.Nullable();
            Map(x => x.EventSequence).Column("EventSequence").Not.Nullable();
            Map(x => x.EventOccurrence).Column("EventOccurrence").Not.Nullable();
            Map(x => x.EventCode).Column("EventCode").Not.Nullable();
            Map(x => x.EventDetailCode).Column("EventDetailCode").Not.Nullable();
            Map(x => x.Message).Column("Message").Nullable();
            Map(x => x.ApplicationPath).Column("ApplicationPath").Nullable();
            Map(x => x.ApplicationVirtualPath).Column("ApplicationVirtualPath").Nullable();
            Map(x => x.MachineName).Column("MachineName").Not.Nullable();
            Map(x => x.RequestUrl).Column("RequestUrl").Nullable();
            Map(x => x.ExceptionType).Column("ExceptionType").Nullable();
            Map(x => x.Details).Column("Details").Nullable();
        }
    }
}
