using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SystemDeliveryEntryMap : ClassMap<SystemDeliveryEntry>
    {
        public const string TABLE_NAME = "SystemDeliveryEntries";

        public SystemDeliveryEntryMap()
        {
            Table(TABLE_NAME);
            
            Id(x => x.Id);
            
            Map(x => x.WeekOf).Not.Nullable();
            Map(x => x.IsValidated).Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.IsHyperionFileCreated).Not.Nullable();
            
            References(x => x.SystemDeliveryType).Not.Nullable();
            References(x => x.EnteredBy).Not.Nullable();
            References(x => x.UpdatedBy).Nullable();
            
            HasMany(x => x.FacilityEntries).KeyColumn("SystemDeliveryEntryId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Adjustments).KeyColumn("SystemDeliveryEntryId").Inverse().Cascade.AllDeleteOrphan();
            HasManyToMany(x => x.Facilities)
               .Table("SystemDeliveryEntriesFacilities")
               .ParentKeyColumn("SystemDeliveryEntryId")
               .ChildKeyColumn("FacilityId");
            HasManyToMany(x => x.PublicWaterSupplies)
               .Table("SystemDeliveryEntriesPublicWaterSupplies")
               .ParentKeyColumn("SystemDeliveryEntryId")
               .ChildKeyColumn("PublicWaterSupplyId");
            HasManyToMany(x => x.WasteWaterSystems)
               .Table("SystemDeliveryEntriesWasteWaterSystems")
               .ParentKeyColumn("SystemDeliveryEntryId")
               .ChildKeyColumn("WasteWaterSystemId");
            HasManyToMany(x => x.OperatingCenters)
               .Table("SystemDeliveryEntriesOperatingCenters")
               .ParentKeyColumn("SystemDeliveryEntryId")
               .ChildKeyColumn("OperatingCenterId");
        }
    }
}
