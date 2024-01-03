using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace WorkOrders.Model
{
    public class SpoilFinalProcessingLocationRepository : WorkOrdersRepository<SpoilFinalProcessingLocation>
    {
        #region Exposed Static Methods

        public static IEnumerable<SpoilFinalProcessingLocation> SelectByOperatingCenterID(int operatingCenterID)
        {
            return (from sfpl in DataTable
                    where sfpl.OperatingCenterID == operatingCenterID
                    orderby sfpl.Name
                    select sfpl);
        }

        public static IEnumerable<SpoilFinalProcessingLocation> SelectByOperatingCenterID(int operatingCenterID, string sortExpression)
        {
            var results = (from sfpl in DataTable
                    where sfpl.OperatingCenterID == operatingCenterID
                    select sfpl);

            return string.IsNullOrEmpty(sortExpression)
                       ? results.OrderBy(x => x.Name)
                       : results.Sorting().Sort<SpoilFinalProcessingLocation>(
                           sortExpression);
        }

        public static void InsertSpoilFinalProcessingLocation(int operatingCenterID, string name, int? townID, int? streetID)
        {
            Insert(new SpoilFinalProcessingLocation
            {
                OperatingCenterID = operatingCenterID,
                Name = name,
                TownID = townID,
                StreetID = streetID
            });
        }

        public static void Update(int spoilFinalProcessingLocationID, string name, int? townID, int? streetID)
        {
            var location = GetEntity(spoilFinalProcessingLocationID);

            location.Name = name;
            location.TownID = townID;
            location.StreetID = streetID;

            UpdateLiterally(location);
        }

        #endregion
    }
}