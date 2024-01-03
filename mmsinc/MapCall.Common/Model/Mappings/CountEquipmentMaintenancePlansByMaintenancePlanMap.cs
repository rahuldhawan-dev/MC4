using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations._2022;

namespace MapCall.Common.Model.Mappings
{
    public class CountEquipmentMaintenancePlansByMaintenancePlanMap : ClassMap<CountEquipmentMaintenancePlansByMaintenancePlan>
    {
        public CountEquipmentMaintenancePlansByMaintenancePlanMap()
        {
            ReadOnly();
            Table(MC5199CreateViewForCount.VIEW_NAME);
            Id(x => x.Id).Column("MaintenancePlanId");
            Map(x => x.AssetCount);

            // Need this so when SchemaExport doesn't create a table
            SchemaAction.None();
        }
    }
}
