using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Common;

namespace WorkOrders.Model
{
    public class RestorationTypeCostRepository : WorkOrdersRepository<RestorationTypeCost>
    {
        #region Private Methods

        protected override IEnumerable<RestorationTypeCost> GetFilteredData(Expression<Func<RestorationTypeCost, bool>> filterExpression)
        {
            var builder = new ExpressionBuilder<RestorationTypeCost>(PredicateBuilder.True<RestorationTypeCost>());
            builder.And(filterExpression);
            builder.And(
                wo =>
                SecurityService.UserOperatingCenters.Contains(wo.OperatingCenter));

            return DataTable.Where(builder);
        }

        #endregion

        #region Private Static Methods

        private static IEnumerable<RestorationTypeCost> SelectAllSorted()
        {
            return (from rtc in DataTable
                    where SecurityService.UserOperatingCenters.Contains(rtc.OperatingCenter)
                    orderby rtc.OperatingCenter.OpCntr, rtc.RestorationType.Description
                    select rtc);            
        }

        private static IEnumerable<RestorationTypeCost> SelectAll()
        {
            return (from rtc in DataTable
                    where
                        SecurityService.UserOperatingCenters.Contains(
                            rtc.OperatingCenter)
                    select rtc);
        }

        #endregion

        #region Exposed Static Methods

        public static IEnumerable<RestorationTypeCost> SelectAllSorted(string sortExpression)
        {
            return String.IsNullOrEmpty(sortExpression) ? SelectAllSorted() :
                SelectAll().Sorting().Sort<RestorationTypeCost>(sortExpression);
        }

        public static IEnumerable<RestorationTypeCost> SelectByOperatingCenter(int operatingCenterID)
        {
            return (from rtc in DataTable
                    where rtc.OperatingCenterID == operatingCenterID
                    select rtc);
        }
        #endregion
    }
}