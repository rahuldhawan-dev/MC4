using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceInstallationPurposeMap : EntityLookupMap<ServiceInstallationPurpose>
    {
        public ServiceInstallationPurposeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();
            Map(x => x.SAPCode).Nullable();
        }
    }
}
