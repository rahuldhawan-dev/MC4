using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ServiceInstallationWorkTypeMap : EntityLookupMap<ServiceInstallationWorkType>
    {
        public const string TABLE_NAME = "ServiceInstallationAdditionalWorkTypes";

        public ServiceInstallationWorkTypeMap()
        {
            Table(TABLE_NAME);
            Map(x => x.CodeGroup);
            Map(x => x.SAPCode);
        }
    }
}
