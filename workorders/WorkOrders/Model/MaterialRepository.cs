using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class MaterialRepository : WorkOrdersRepository<Material>
    {
        #region Constants

        public const int MAX_RESULTS = 100;

        #endregion
        
        #region Private Methods

        /// <summary>
        /// We don't want them to get more than 100 results at a time
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        protected override IEnumerable<Material> GetFilteredData(System.Linq.Expressions.Expression<System.Func<Material, bool>> filterExpression)
        {
            return DataTable.Where(filterExpression).Take(MAX_RESULTS);
        }

        #endregion
        
        #region Exposed Static Methods

        public static IEnumerable<Material> GetStockedMaterialsByOperatingCenter(int operatingCenterID, bool activeMaterialsOnly = true)
        {
            var result = (from m in DataTable
                    where
                        m.OperatingCenterStockedMaterials.Any(
                        sm => sm.OperatingCenter.OperatingCenterID == operatingCenterID)
                    orderby m.PartNumber
                    select m);

            if (activeMaterialsOnly)
            {
                result = result.Where(x => x.IsActive).OrderBy(x => x.PartNumber);
            }
            return result;
        }
        
        #endregion
    }
}
