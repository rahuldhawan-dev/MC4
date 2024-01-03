using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SystemDeliveryFacilityEntryMap : ClassMap<SystemDeliveryFacilityEntry>
    {
        public SystemDeliveryFacilityEntryMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            
            Map(x => x.EntryDate).Not.Nullable();
            Map(x => x.EntryValue).Not.Nullable();
            Map(x => x.WeeklyTotal).Nullable();
            Map(x => x.IsInjection).Not.Nullable();
            Map(x => x.HasBeenAdjusted).Not.Nullable();
            Map(x => x.AdjustmentComment).Nullable();
            Map(x => x.OriginalEntryValue).Nullable();
            
            References(x => x.SystemDeliveryType).Not.Nullable();
            References(x => x.SystemDeliveryEntry).Not.Nullable();
            References(x => x.SystemDeliveryEntryType).Not.Nullable();
            References(x => x.EnteredBy).Not.Nullable();
            References(x => x.Facility).Not.Nullable();
            References(x => x.SupplierFacility).Nullable();
            
            HasMany(x => x.Adjustments).KeyColumn("SystemDeliveryFacilityEntryId").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
