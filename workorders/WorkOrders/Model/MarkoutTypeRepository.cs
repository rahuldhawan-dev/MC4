using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class MarkoutTypeRepository : WorkOrdersRepository<MarkoutType>
    {
        #region Exposed Static Methods

        public static IEnumerable<MarkoutType> SelectAllSorted()
        {
            return (from mt in DataTable
                    orderby mt.Order
                    select mt);
        }

        #endregion
    }
}
