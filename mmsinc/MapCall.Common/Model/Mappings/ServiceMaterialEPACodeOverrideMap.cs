using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceMaterialEPACodeOverrideMap : ClassMap<ServiceMaterialEPACodeOverride>
    {
        public ServiceMaterialEPACodeOverrideMap()
        {
            Id(x => x.Id, "Id").GeneratedBy.Identity();
            
            References(x => x.ServiceMaterial).Not.Nullable();
            References(x => x.State).Not.Nullable();
            References(x => x.CustomerEPACode).Not.Nullable();
            References(x => x.CompanyEPACode).Not.Nullable();
        }
    }
}
