using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SystemDeliveryIgnitionEntryMap : ClassMap<SystemDeliveryIgnitionEntry>
    {
        public SystemDeliveryIgnitionEntryMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FacilityId).Not.Nullable();
            Map(x => x.UnitOfMeasure).Not.Nullable();
            Map(x => x.SystemDeliveryType).Not.Nullable();
            Map(x => x.SystemDeliveryEntryType).Not.Nullable();
            Map(x => x.EntryDate).Not.Nullable();
            Map(x => x.FacilityName).Not.Nullable();
            Map(x => x.EntryValue).Not.Nullable();
        }
    }
}
