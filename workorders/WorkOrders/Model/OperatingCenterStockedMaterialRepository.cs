using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MMSINC.Common;

namespace WorkOrders.Model
{
    public class OperatingCenterStockedMaterialRepository : WorkOrdersRepository<OperatingCenterStockedMaterial>
    {
        #region Private Static Properties

        private static Expression<Func<OperatingCenterStockedMaterial, bool>> baseExpression
        {
            get { return PredicateBuilder.True<OperatingCenterStockedMaterial>(); }
        }

        #endregion

        #region Private Static Methods

        private static ExpressionBuilder<OperatingCenterStockedMaterial> GetExpressionBuilder(int operatingCenterID)
        {
            var builder =
                new ExpressionBuilder<OperatingCenterStockedMaterial>(
                    baseExpression);
            builder.And(m => m.OperatingCenter.OperatingCenterID == operatingCenterID);
            return builder;
        }

        private static Expression<Func<OperatingCenterStockedMaterial, bool>> ApplyFuzzyFilter(string filter, Func<string, Expression<Func<OperatingCenterStockedMaterial, bool>>> fn)
        {
            // this makes things a bit convoluted, but saves from having to do
            // this by hand in the exposed methods
            var builder =
                new ExpressionBuilder<OperatingCenterStockedMaterial>(
                    baseExpression);
            var parts = filter.Split(' ');
            foreach (var str in parts)
            {
                var tmpStr = str;
                builder.And(fn(tmpStr));
            }
            return builder.Build();
        }

        #endregion

        #region Exposed Static Methods

        public static IEnumerable<Material> LookupMaterialByStockNumber(int operatingCenterID, string stockNumber, bool activeMaterialsOnly = true)
        {
            stockNumber = stockNumber.ToLower();
            var builder = GetExpressionBuilder(operatingCenterID);
            if (activeMaterialsOnly)
            {
                builder.And(m => m.Material.IsActive); 
            }
          
            builder.And(ApplyFuzzyFilter(stockNumber, str => m => m.Material.PartNumber.Contains(str)));
            return DataTable.Where(builder.Build()).Select(m => m.Material);
        }

        public static IEnumerable<Material> LookupMaterialByDescription(int operatingCenterID, string description, bool activeMaterialsOnly = true)
        {
            description = description.ToLower();
            var builder = GetExpressionBuilder(operatingCenterID);
            if (activeMaterialsOnly)
            {
                builder.And(m => m.Material.IsActive);
            }
            builder.And(ApplyFuzzyFilter(description, str => m => m.Material.Description.Contains(str)));
            return DataTable.Where(builder.Build()).Select(m => m.Material);
        }

        #endregion
    }
}
