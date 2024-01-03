using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EnvironmentalNonComplianceEventStatusMap : EntityLookupMap<EnvironmentalNonComplianceEventStatus>
    {
        public const string TABLE_NAME = "EnvironmentalNonComplianceEventStatuses";

        public EnvironmentalNonComplianceEventStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
