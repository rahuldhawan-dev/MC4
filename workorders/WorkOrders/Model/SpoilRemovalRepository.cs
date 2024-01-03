using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace WorkOrders.Model
{
    public class SpoilRemovalRepository : WorkOrdersRepository<SpoilRemoval>
    {
        #region Exposed Static Methods

        public static IEnumerable<SpoilRemoval> SelectByOperatingCenter(int operatingCenterID)
        {
            return
                (from sr in DataTable
                 where sr.RemovedFrom.OperatingCenterID == operatingCenterID
                 select sr);
        }

        public static IEnumerable<SpoilRemoval> SelectLastTenByOperatingCenter(int operatingCenterID, string sortExpression)
        {
            var results = (from sr in DataTable
                           where
                               sr.RemovedFrom.OperatingCenterID ==
                               operatingCenterID
                           orderby sr.DateRemoved descending
                           select sr);

            return string.IsNullOrEmpty(sortExpression)
                       ? results.OrderByDescending(x => x.DateRemoved)
                       : results.Sorting().Sort<SpoilRemoval>(sortExpression);
        }

        public static void InsertSpoilRemoval(int removedFrom, int finalDestination, DateTime dateRemoved, decimal quantity)
        {
            Insert(new SpoilRemoval
            {
                RemovedFromID = removedFrom, 
                FinalDestinationID = finalDestination, 
                DateRemoved = dateRemoved, 
                Quantity = quantity
            });
        }

        public static void Update(int removedFromID, int finalDestinationID, DateTime dateRemoved, decimal quantity, int spoilRemovalID)
        {
            Update(new SpoilRemoval
            {
                SpoilRemovalID = spoilRemovalID,
                RemovedFromID = removedFromID,
                FinalDestinationID = finalDestinationID,
                DateRemoved = dateRemoved,
                Quantity = quantity
            });
        }

        public static decimal GetTotalByStorageLocation(SpoilStorageLocation location)
        {
            return
                GetTotalByStorageLocation(
                    location.SpoilStorageLocationID);
        }

        public static decimal GetTotalByStorageLocation(int locationID)
        {
            var ret = (from sr in DataTable
                    where
                        sr.RemovedFromID == locationID
                    select (decimal?)sr.Quantity).Sum();
            return ret ?? 0;
        }

        #endregion
    }
}