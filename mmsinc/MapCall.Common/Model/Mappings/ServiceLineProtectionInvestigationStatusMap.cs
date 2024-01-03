using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceLineProtectionInvestigationStatusMap : EntityLookupMap<ServiceLineProtectionInvestigationStatus>
    {
        public const string TABLE_NAME = "ServiceLineProtectionInvestigationStatuses";

        public ServiceLineProtectionInvestigationStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
