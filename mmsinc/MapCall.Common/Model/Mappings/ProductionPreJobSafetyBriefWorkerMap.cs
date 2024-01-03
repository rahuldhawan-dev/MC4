using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionPreJobSafetyBriefWorkerMap : ClassMap<ProductionPreJobSafetyBriefWorker>
    {
        public ProductionPreJobSafetyBriefWorkerMap()
        {
            Id(x => x.Id);
            Map(x => x.SignedAt).Not.Nullable();
            Map(x => x.Contractor).Length(ProductionPreJobSafetyBriefWorker.StringLengths.CONTRACTOR).Nullable();
            References(x => x.Employee).Nullable();
            References(x => x.ProductionPreJobSafetyBrief).Not.Nullable();
        }
    }
}
