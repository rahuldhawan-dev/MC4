using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceTypeMap : ClassMap<ServiceType>
    {
        public ServiceTypeMap()
        {
            CompositeId()
               .KeyReference(x => x.OperatingCenter, "OperatingCenterId")
               .KeyReference(x => x.ServiceCategory, "ServiceCategoryId");

            References(x => x.CategoryOfServiceGroup).Nullable();

            Map(x => x.Description).Not.Nullable();
        }
    }
}
