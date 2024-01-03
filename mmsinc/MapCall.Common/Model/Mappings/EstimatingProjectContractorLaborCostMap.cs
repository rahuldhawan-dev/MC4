using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EstimatingProjectContractorLaborCostMap : ClassMap<EstimatingProjectContractorLaborCost>
    {
        public EstimatingProjectContractorLaborCostMap()
        {
            Table("EstimatingProjectContractorLaborCosts");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.EstimatingProject).Not.Nullable();
            References(x => x.ContractorLaborCost).Not.Nullable();
            References(x => x.AssetType).Nullable();

            Map(x => x.Quantity).Not.Nullable();
        }
    }
}
