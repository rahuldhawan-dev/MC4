using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    /// <summary>
    /// Repository for retrieving Town objects from persistence.
    /// </summary>
    public class TownRepository : WorkOrdersRepository<Town>
    {
        #region Exposed Methods

        public static IEnumerable<Town> SelectByOperatingCenter(OperatingCenter center)
        {
            return center.Towns;
        }

        public static IEnumerable<Town> SelectByOperatingCenterID(int operatingCenterID)
        {
            return
                SelectByOperatingCenter(
                    OperatingCenterRepository.GetEntity(operatingCenterID));
        }

        // TODO: This should be an override (new) of SelectAllAsList()
        // however, that doesn't seem to work for ObjectDataSources.
        // somehow they still seem to call the base method.
        public static List<Town> SelectAllSorted()
        {
            return (from t in DataTable where t.OperatingCentersTowns.Count(x => x.OperatingCenter.WorkOrdersEnabled) > 0  orderby t.Name select t).ToList();
        }

        // TODO: this method does not appear to be used as of 20090819
        // if that holds true after a week, remove this.
        //public static Town GetTownByName(string townName)
        //{
        //    return (from t in DataTable
        //            where t.Name == townName
        //            select t).FirstOrDefault();
        //}

        #endregion
    }
}
