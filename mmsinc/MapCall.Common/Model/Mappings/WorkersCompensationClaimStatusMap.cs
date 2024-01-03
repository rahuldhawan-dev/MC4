using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WorkersCompensationClaimStatusMap : EntityLookupMap<WorkersCompensationClaimStatus>
    {
        public WorkersCompensationClaimStatusMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
