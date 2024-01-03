using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceInstallationThirdActivityMap : EntityLookupMap<ServiceInstallationThirdActivity>
    {
        public const string TABLE_NAME = "ServiceInstallationThirdActivities";

        public ServiceInstallationThirdActivityMap()
        {
            Table(TABLE_NAME);
            Map(x => x.CodeGroup);
            Map(x => x.SAPCode);
        }
    }
}
