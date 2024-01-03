using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class TrafficControlTicketStatusMap : EntityLookupMap<TrafficControlTicketStatus>
    {
        public const string TABLE_NAME = "TrafficControlTicketStatuses";

        public TrafficControlTicketStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
