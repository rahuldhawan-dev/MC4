using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class PitcherFilterCustomerDeliveryMethodRepository
        : WorkOrdersRepository<PitcherFilterCustomerDeliveryMethod>
    {
        #region Exposed Static Methods

        public static IEnumerable<PitcherFilterCustomerDeliveryMethod> SelectAllSorted() =>
            from m in DataTable orderby m.Id select m;

        #endregion
    }
}
