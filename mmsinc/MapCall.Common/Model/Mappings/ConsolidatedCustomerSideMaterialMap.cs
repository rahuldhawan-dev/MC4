using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ConsolidatedCustomerSideMaterialMap : ClassMap<ConsolidatedCustomerSideMaterial>
    {
        public ConsolidatedCustomerSideMaterialMap()
        {
            Id(x => x.Id, "Id").GeneratedBy.Identity();
            
            References(x => x.ConsolidatedEPACode).Not.Nullable();
            References(x => x.CustomerSideEPACode).Nullable();
            References(x => x.CustomerSideExternalEPACode).Nullable();
        }
    }
}
