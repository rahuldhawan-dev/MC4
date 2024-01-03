using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WorkOrderInvoiceStatusMap : EntityLookupMap<WorkOrderInvoiceStatus>
    {
        public const string TABLE_NAME = "WorkOrderInvoiceStatuses";

        public WorkOrderInvoiceStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
