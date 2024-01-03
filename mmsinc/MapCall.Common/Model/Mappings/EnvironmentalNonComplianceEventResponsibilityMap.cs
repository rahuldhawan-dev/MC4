using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class
        EnvironmentalNonComplianceEventResponsibilityMap : EntityLookupMap<EnvironmentalNonComplianceEventResponsibility>
    {
        public const string TABLE_NAME = "EnvironmentalNonComplianceEventResponsibilities";

        public EnvironmentalNonComplianceEventResponsibilityMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
        }
    }
}
