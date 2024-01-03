using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceTerminationPointMap : EntityLookupMap<ServiceTerminationPoint>
    {
        public ServiceTerminationPointMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned().Not.Nullable();
        }
    }
}
