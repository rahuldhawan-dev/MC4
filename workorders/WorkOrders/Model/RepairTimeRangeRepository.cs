using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class RepairTimeRangeRepository : WorkOrdersRepository<RepairTimeRange>
    {
        #region Exposed Static Methods

        public static IEnumerable<RepairTimeRange> SelectAllSorted()
        {
            return (from r in DataTable orderby r.RepairTimeRangeID select r);
        }

        #endregion
    }
}