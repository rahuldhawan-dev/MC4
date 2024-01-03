using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class CustomerImpactRangeRepository : WorkOrdersRepository<CustomerImpactRange>
    {
        #region Exposed Static Methods

        public static IEnumerable<CustomerImpactRange> SelectAllSorted()
        {
            return
                (from c in DataTable orderby c.CustomerImpactRangeID select c);
        }

        #endregion
    }

    public class FlushingNoticeTypeRepository : WorkOrdersRepository<FlushingNoticeType>
    {
        public static IEnumerable<FlushingNoticeType> SelectAllSorted()
        {
            return from t in DataTable orderby t.Id select t;
        }
    }
}