using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Repository for retrieving Valve objects from persistence.
    /// </summary>
    public class ValveRepository : WorkOrdersRepository<Valve>
    {
        #region Exposed Methods

        public static new List<Valve> SelectAllAsList()
        {
            return SelectAllAsList(0);
        }

        public static List<Valve> SelectAllAsList(int count)
        {
            if (count <= 0)
                return
                    (from v in DataTable select v).ToList();
            return (from v in DataTable select v).Take(count).ToList();
        }

        #endregion
    }
}