using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityKwhCostMap : ClassMap<FacilityKwhCost>
    {
        public FacilityKwhCostMap()
        {
            Table("FacilityKWHCosts");
            LazyLoad();
            Id(x => x.Id);
            References(x => x.Facility).Not.Nullable();
            Map(x => x.CostPerKwh).Column("CostPerKWH").Not.Nullable().Precision(19).Scale(4);
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.EndDate).Not.Nullable();
        }
    }
}
