using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentMaintenancePlanMap : ClassMap<EquipmentMaintenancePlan>
    {
        public const string TABLE_NAME = "EquipmentMaintenancePlans";

        public EquipmentMaintenancePlanMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.Equipment, "EquipmentId").Not.Nullable();
            References(x => x.MaintenancePlan, "MaintenancePlanId").Not.Nullable();
        }
    }
}
