using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceInstallationFirstActivityMap : EntityLookupMap<ServiceInstallationFirstActivity>
    {
        public const string TABLE_NAME = "ServiceInstallationFirstActivities";

        public ServiceInstallationFirstActivityMap()
        {
            Table(TABLE_NAME);
            Map(x => x.CodeGroup);
            Map(x => x.SAPCode);
        }
    }
}
