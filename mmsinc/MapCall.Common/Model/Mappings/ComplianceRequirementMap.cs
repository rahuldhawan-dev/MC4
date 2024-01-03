using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ComplianceRequirementMap : EntityLookupMap<ComplianceRequirement>
    {
        public ComplianceRequirementMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
