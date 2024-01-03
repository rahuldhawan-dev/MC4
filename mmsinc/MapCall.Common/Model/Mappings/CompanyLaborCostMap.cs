using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CompanyLaborCostMap : ClassMap<CompanyLaborCost>
    {
        public CompanyLaborCostMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable();
            Map(x => x.Unit).Not.Nullable();
            Map(x => x.Cost).Not.Nullable();

            Map(x => x.LaborItem);
        }
    }
}
