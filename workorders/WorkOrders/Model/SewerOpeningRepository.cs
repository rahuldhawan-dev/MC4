using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class SewerOpeningRepository : WorkOrdersRepository<SewerOpening>
    {
        #region Exposed Methods

        public static new List<SewerOpening> SelectAllAsList()
        {
            return SelectAllAsList(0);
        }

        public static List<SewerOpening> SelectAllAsList(int count)
        {
            if (count <= 0)
                return
                    (from sm in DataTable select sm).ToList();
            return (from sm in DataTable select sm).Take(count).ToList();
        }

        #endregion
    }
}