using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WorkOrderCancellationReasonMap : EntityLookupMap<WorkOrderCancellationReason>
    {
        public WorkOrderCancellationReasonMap()
        {
            Map(x => x.Status).Not.Nullable();
        }
    }
}
