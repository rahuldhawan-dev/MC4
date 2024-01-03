using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FacilitySystemDeliveryEntryTypeMap : ClassMap<FacilitySystemDeliveryEntryType>
    {
        public const string TABLE_NAME = "FacilitiesSystemDeliveryEntryTypes";

        public FacilitySystemDeliveryEntryTypeMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.Facility).Not.Nullable();
            References(x => x.SupplierFacility).Nullable();
            References(x => x.SystemDeliveryEntryType).Not.Nullable();
            Map(x => x.IsEnabled).Not.Nullable();
            Map(x => x.MinimumValue).Nullable();
            Map(x => x.MaximumValue).Nullable();
            Map(x => x.PurchaseSupplier).Length(100).Nullable();
            Map(x => x.BusinessUnit).Nullable();
            Map(x => x.IsInjectionSite).Not.Nullable();
            Map(x => x.IsAutomationEnabled).Not.Nullable();
        }
    }
}
