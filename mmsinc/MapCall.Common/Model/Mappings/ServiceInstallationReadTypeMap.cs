using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceInstallationReadTypeMap : EntityLookupMap<ServiceInstallationReadType>
    {
        public ServiceInstallationReadTypeMap()
        {
            Map(x => x.SAPCode);
        }
    }
}
