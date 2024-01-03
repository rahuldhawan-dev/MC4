using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceStatusMap : EntityLookupMap<ServiceStatus>
    {
        public const string TABLE_NAME = "ServiceStatuses";

        public ServiceStatusMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity().Column("ServiceStatusId").Not.Nullable();
        }
    }
}
