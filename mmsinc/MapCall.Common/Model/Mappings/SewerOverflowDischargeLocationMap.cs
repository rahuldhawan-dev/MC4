using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SewerOverflowDischargeLocationMap : EntityLookupMap<SewerOverflowDischargeLocation>
    {
        public const string TABLE_NAME = "SewerOverflowDischargeLocations";

        public SewerOverflowDischargeLocationMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}

