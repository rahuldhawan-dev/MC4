using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class EstimatingProjectOtherCostMap : ClassMap<EstimatingProjectOtherCost>
    {
        public EstimatingProjectOtherCostMap()
        {
            Table("EstimatingProjectOtherCosts");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.EstimatingProject).Not.Nullable();
            References(x => x.AssetType).Nullable();

            Map(x => x.Quantity).Not.Nullable().Precision(10);
            Map(x => x.Description).Not.Nullable().Length(EstimatingProjectOtherCost.StringLengths.DESCRIPTION);
            Map(x => x.Cost).Not.Nullable().Precision(19).Scale(4);
        }
    }
}
