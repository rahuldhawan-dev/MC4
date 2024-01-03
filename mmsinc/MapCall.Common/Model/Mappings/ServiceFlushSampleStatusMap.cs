using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceFlushSampleStatusMap : EntityLookupMap<ServiceFlushSampleStatus>
    {
        public ServiceFlushSampleStatusMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned().Not.Nullable();
        }
    }
}
