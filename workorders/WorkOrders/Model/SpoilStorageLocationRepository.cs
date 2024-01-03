using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace WorkOrders.Model
{
    public class SpoilStorageLocationRepository : WorkOrdersRepository<SpoilStorageLocation>
    {
        #region Exposed Static Methods

        public static IEnumerable<SpoilStorageLocation> SelectByOperatingCenterID(int operatingCenterID)
        {
            return (from ssl in DataTable
                    where ssl.Active && ssl.OperatingCenter.OperatingCenterID == operatingCenterID
                    orderby ssl.Name
                    select ssl);
        }

        public static IEnumerable<SpoilStorageLocation> SelectByOperatingCenterID(int operatingCenterID, string sortExpression)
        {
            var results = (from ssl in DataTable
                    where ssl.Active && ssl.OperatingCenter.OperatingCenterID == operatingCenterID
                    select ssl);

            return string.IsNullOrEmpty(sortExpression)
                       ? results.OrderBy(x => x.Name)
                       : results.Sorting().Sort<SpoilStorageLocation>(sortExpression);
        }

        public static IEnumerable<SpoilStorageLocation> SelectByOperatingCenters(IEnumerable<OperatingCenter> operatingCenters)
        {
            return (from ssl in DataTable
                    where operatingCenters.Contains(ssl.OperatingCenter)
                    orderby ssl.Name
                    select ssl);
        }

        public static void InsertSpoilStorageLocation(int operatingCenterID, string name, int? townID, int? streetID)
        {
            Insert(new SpoilStorageLocation {
                OperatingCenterID = operatingCenterID,
                Name = name,
                TownID = townID,
                StreetID = streetID
            });
        }

        public static void Update(int spoilStorageLocationID, string name, int? townID, int? streetID)
        {
            var location = GetEntity(spoilStorageLocationID);

            location.Name = name;
            location.TownID = townID;
            location.StreetID = streetID;

            UpdateLiterally(location);
        }

        #endregion
    }
}