using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class StateRepository : WorkOrdersRepository<State>
    {
        #region Exposed Static Methods

        public static IEnumerable<State> GetAllStates()
        {
            return DataTable.OrderBy(x => x.Abbreviation).Select(x => x).ToList();
        }

        #endregion
    }
}
