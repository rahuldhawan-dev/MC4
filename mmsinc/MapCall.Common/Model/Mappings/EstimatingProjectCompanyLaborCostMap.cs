using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EstimatingProjectCompanyLaborCostMap : ClassMap<EstimatingProjectCompanyLaborCost>
    {
        public EstimatingProjectCompanyLaborCostMap()
        {
            Table("EstimatingProjectsCompanyLaborCosts");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.EstimatingProject).Not.Nullable();
            References(x => x.CompanyLaborCost).Not.Nullable();
            References(x => x.AssetType).Nullable();

            Map(x => x.Quantity).Not.Nullable();
        }
    }
}
