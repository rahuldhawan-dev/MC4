using System.Collections.Generic;

namespace WorkOrders.Model
{
    public class MarkoutDamageRepository : WorkOrdersRepository<MarkoutDamage>
    {
        #region Exposed Static Methods

        public static IEnumerable<MarkoutDamage>
            GetMarkoutDamagesByWorkOrder(int workOrderID)
        {
            return (from mv in DataTable
                where mv.WorkOrderId == workOrderID
                select mv);
        }

        #endregion
    }
}