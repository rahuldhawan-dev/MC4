using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class BusinessUnitRepository : WorkOrdersRepository<BusinessUnit>
    {
        #region Exposed Methods

        public static IEnumerable<BusinessUnit> SelectByOperatingCenter(
            int operatingCenterID)
        {
            return (from bu in DataTable
                    where bu.Is271Visible &&
                          bu.OperatingCenterID == operatingCenterID &&
                          bu.DepartmentID == 1 // O&M
                    orderby bu.Order
                    select bu);
        }

        #endregion
    }
}