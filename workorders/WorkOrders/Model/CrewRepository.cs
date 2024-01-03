using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MMSINC.Common;

namespace WorkOrders.Model
{
    public class CrewRepository : WorkOrdersRepository<Crew>
    {
        #region Private Methods
        
        protected override IEnumerable<Crew> GetFilteredData(Expression<Func<Crew, bool>> filterExpression)
        {
            var builder = new ExpressionBuilder<Crew>(PredicateBuilder.True<Crew>());
            builder.And(filterExpression);
            builder.And(
                krew =>
                SecurityService.UserOperatingCenters.Contains(
                    krew.OperatingCenter)
                && krew.ContractorID == null);
                
            return DataTable.Where(builder);
        }

        #endregion

        #region Exposed Methods

        public static IEnumerable<Crew> SelectActiveByOperatingCenterID(int operatingCenterID)
        {
            return (from c in DataTable
                    where c.OperatingCenterID == operatingCenterID && c.ContractorID == null && c.Active
                    orderby c.Description
                    select c);
        }

        public static IEnumerable<Crew> SelectByOperatingCenterID(int operatingCenterID)
        {
            return (from c in DataTable
                    where c.OperatingCenterID == operatingCenterID && c.ContractorID == null
                    orderby c.Description
                    select c);
        }

        public static IEnumerable<Crew> SelectByOperatingCenter(OperatingCenter center)
        {
            return (from c in DataTable
                    where c.OperatingCenterID == center.OperatingCenterID && c.ContractorID == null
                    orderby c.Description
                    select c);
        }

        public static IEnumerable<Crew> SelectAllSorted()
        {
            return (from c in DataTable
                    where SecurityService.UserOperatingCenters.Contains(c.OperatingCenter)
                     && c.ContractorID == null
                    orderby c.OperatingCenter.OpCntr ascending, c.Description
                    select c);
        }

        public static IEnumerable<Crew> SelectActive()
        {
            return (from c in DataTable
                    where SecurityService.UserOperatingCenters.Contains(c.OperatingCenter)
                     && c.ContractorID == null && c.Active
                    orderby c.OperatingCenter.OpCntr ascending, c.Description
                    select c);
        }

        public static IEnumerable<Crew> SelectByContractor(Contractor contractor)
        {
            return (from c in DataTable
                    where c.ContractorID == contractor.ContractorID
                    orderby c.Description
                    select c);
        }

        #endregion
    }
}