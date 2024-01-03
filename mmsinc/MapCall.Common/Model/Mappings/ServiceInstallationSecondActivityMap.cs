using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceInstallationSecondActivityMap : EntityLookupMap<ServiceInstallationSecondActivity>
    {
        public const string TABLE_NAME = "ServiceInstallationSecondActivities";

        public ServiceInstallationSecondActivityMap()
        {
            Table(TABLE_NAME);
            Map(x => x.CodeGroup);
            Map(x => x.SAPCode);
        }
    }
}
