using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Repository for retrieving TownSection objects from persistence.
    /// </summary>
    public class TownSectionRepository : WorkOrdersRepository<TownSection>
    {
        #region Exposed Methods

        public static TownSection GetTownSectionByNameAndTownID(string townSectionName, int townID)
        {
            return (from ts in DataTable
                    where (ts.Name == townSectionName &&
                           ts.TownID == townID)
                    select ts).FirstOrDefault();
        }

        public static List<TownSection> SelectByTownID(int townID)
        {
            return (from ts in DataTable
                    where ts.TownID == townID
                    orderby ts.Name ascending 
                    select ts).ToList();
        }

        #endregion
    }
}