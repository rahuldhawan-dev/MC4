using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Common;

namespace WorkOrders.Model
{
    public class StockLocationRepository : WorkOrdersRepository<StockLocation>
    {
        #region Private Methods

        protected override IEnumerable<StockLocation> GetFilteredData(Expression<Func<StockLocation, bool>> filterExpression)
        {
            var builder = new ExpressionBuilder<StockLocation>(PredicateBuilder.True<StockLocation>());
            builder.And(filterExpression);
            builder.And(
                sl =>
                SecurityService.UserOperatingCenters.Contains(sl.OperatingCenter));

            return DataTable.Where(builder);
        }

        #endregion

        #region Exposed Static Methods

        public static IEnumerable<StockLocation> SelectActiveByOperatingCenter(int operatingCenterID)
        {
            return
                (from sl in DataTable
                 where sl.OperatingCenterID == operatingCenterID && sl.IsActive
                 select sl);
        }

        public static IEnumerable<StockLocation> SelectByOperatingCenter(int operatingCenterID)
        {
            return
                (from sl in DataTable
                 where sl.OperatingCenterID == operatingCenterID
                 select sl);
        }

        public static IEnumerable<StockLocation> SelectByOperatingCenter(int operatingCenterID, string sortExpression)
        {
            return
                SelectByOperatingCenter(operatingCenterID)
                .Sorting().Sort<StockLocation>(sortExpression);
        }

        public static void InsertStockLocation(int operatingCenterID, string description, string sapStockLocation, bool isActive)
        {
            Insert(new StockLocation {
                Description = description,
                SAPStockLocation = sapStockLocation,
                OperatingCenterID = operatingCenterID, 
                IsActive = isActive
            });
        }

        public static void Update(string description, string sapStockLocation, int stockLocationID, bool isActive)
        {
            Update(new StockLocation {
                StockLocationID = stockLocationID,
                SAPStockLocation = sapStockLocation,
                Description = description, 
                IsActive = isActive
            });
        }

        #endregion
    }
}