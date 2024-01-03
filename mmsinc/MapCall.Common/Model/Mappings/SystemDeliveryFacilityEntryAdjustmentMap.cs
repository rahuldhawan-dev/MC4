using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SystemDeliveryFacilityEntryAdjustmentMap : ClassMap<SystemDeliveryFacilityEntryAdjustment>
    {
        public SystemDeliveryFacilityEntryAdjustmentMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.AdjustedDate).Not.Nullable();
            Map(x => x.AdjustedEntryValue).Not.Nullable();
            Map(x => x.OriginalEntryValue).Not.Nullable();
            Map(x => x.DateTimeEntered).Not.Nullable();
            Map(x => x.Comment).Length(SystemDeliveryFacilityEntryAdjustment.StringLengths.MAX_COMMENT).Nullable();

            References(x => x.SystemDeliveryFacilityEntry).Not.Nullable();
            References(x => x.SystemDeliveryEntry).Not.Nullable();
            References(x => x.Facility).Not.Nullable();
            References(x => x.EnteredBy).Not.Nullable();
        }
    }
}
