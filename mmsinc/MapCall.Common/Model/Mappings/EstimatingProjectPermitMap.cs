using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EstimatingProjectsPermitsMap : ClassMap<EstimatingProjectPermit>
    {
        public EstimatingProjectsPermitsMap()
        {
            Table("EstimatingProjectsPermits");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.EstimatingProject).Not.Nullable();
            References(x => x.PermitType).Not.Nullable();
            References(x => x.AssetType).Nullable();

            Map(x => x.Quantity).Not.Nullable();
            Map(x => x.Cost).Not.Nullable();
        }
    }
}
