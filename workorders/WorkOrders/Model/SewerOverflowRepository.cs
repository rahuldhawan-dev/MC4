using System.Collections.Generic;

namespace WorkOrders.Model
{
    public class SewerOverflowRepository : WorkOrdersRepository<SewerOverflow>
    {
        public static IEnumerable<SewerOverflow> GetSewerOverflowsByWorkOrder(int workOrderID)
        {
            return from so in DataTable
                where so.WorkOrderId == workOrderID
                select so;
        }
    }
}