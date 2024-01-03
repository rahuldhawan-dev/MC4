using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AssetReliabilityMap : ClassMap<AssetReliability>
    {
        public AssetReliabilityMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.ProductionWorkOrder).Nullable();
            References(x => x.Equipment).Nullable();
            References(x => x.Employee).Not.Nullable();
            References(x => x.AssetReliabilityTechnologyUsedType).Not.Nullable();

            Map(x => x.DateTimeEntered).Not.Nullable();
            Map(x => x.RepairCostNotAllowedToFail).Not.Nullable();
            Map(x => x.RepairCostAllowedToFail).Not.Nullable();
            Map(x => x.CostAvoidance).Not.Nullable();
            Map(x => x.TechnologyUsedNote).Length(AssetReliability.StringLengths.NOTE_LENGTH).Nullable();
            Map(x => x.CostAvoidanceNote).Length(AssetReliability.StringLengths.NOTE_LENGTH).Nullable();
        }
    }
}
