using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionWorkDescriptionMap : ClassMap<ProductionWorkDescription>
    {
        public ProductionWorkDescriptionMap()
        {
            Id(x => x.Id);

            Map(x => x.Description).Not.Nullable().Length(ProductionWorkDescription.StringLengths.DESCRIPTION);
            Map(x => x.BreakdownIndicator).Not.Nullable();

            References(x => x.EquipmentType).Nullable();
            References(x => x.OrderType).Not.Nullable();
            References(x => x.PlantMaintenanceActivityType).Nullable();
            References(x => x.MaintenancePlanTaskType).Nullable();
            References(x => x.TaskGroup).Nullable();
            References(x => x.ProductionSkillSet);
        }
    }
}
