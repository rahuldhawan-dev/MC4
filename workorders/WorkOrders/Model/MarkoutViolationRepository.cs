using System.Collections.Generic;

namespace WorkOrders.Model
{
    public class MarkoutViolationRepository : WorkOrdersRepository<MarkoutViolation>
    {
        #region Exposed Static Methods

        public static IEnumerable<MarkoutViolation>
            GetMarkoutViolationsByWorkOrder(int workOrderID)
        {
            return (from mv in DataTable
                where mv.WorkOrderId == workOrderID
                select mv);
        }

        #endregion
    }
}