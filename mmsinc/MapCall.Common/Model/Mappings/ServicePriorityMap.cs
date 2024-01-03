using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServicePriorityMap : EntityLookupMap<ServicePriority>
    {
        public const string TABLE_NAME = "ServicePriorities";

        public ServicePriorityMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();
        }
    }
}
