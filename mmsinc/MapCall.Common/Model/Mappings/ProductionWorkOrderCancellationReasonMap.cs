using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionWorkOrderCancellationReasonMap : EntityLookupMap<ProductionWorkOrderCancellationReason>
    {
        public ProductionWorkOrderCancellationReasonMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.SAPCode);
        }
    }
}
