using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace WorkOrders.Model
{
    public class OperatingCenterSpoilRemovalCostRepository : WorkOrdersRepository<OperatingCenterSpoilRemovalCost>
    {
        #region Private Static Methods

        private static IEnumerable<OperatingCenterSpoilRemovalCost> SelectAll()
        {
            return (from ocst in DataTable
                    where
                        SecurityService.UserOperatingCenters.Contains(
                        ocst.OperatingCenter)
                    select ocst);
        }

        #endregion

        #region Exposed Static Methods

        public static IEnumerable<OperatingCenterSpoilRemovalCost> SelectAllSorted(string sortExpression)
        {
            var results = SelectAll();
            return
                string.IsNullOrEmpty(sortExpression) ? results :
                    results.Sorting()
                        .Sort<OperatingCenterSpoilRemovalCost>(sortExpression);
        }

        #endregion
    }
}