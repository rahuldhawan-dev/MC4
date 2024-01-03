using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class ReportViewingRepository : WorkOrdersRepository<ReportViewing>
    {
        #region Exposed Static Methods

        public static IEnumerable<ReportViewing> GetReportViewingsByOpCenterAndDateRange(DateTime? dateStart, DateTime? dateEnd, string opCode, int? employeeID)
        {
            var opcntr =
                OperatingCenterRepository.GetOperatingCenterByOpCntr(opCode);
            return (from r in DataTable
                    where (dateStart == null || dateStart <= r.DateViewed)
                          && (dateEnd == null || dateEnd >= r.DateViewed)
                          &&
                          (opCode == null || opCode == String.Empty ||
                           (from oc in r.Employee.OperatingCentersUsers
                            where
                                oc.OperatingCenterID == opcntr.OperatingCenterID
                            select oc).Any())
                          &&
                          (employeeID == null || r.EmployeeID == employeeID)
                    select r);
        }

        #endregion

        public IEnumerable GetUserNames()
        {
            return (from r in DataTable
                    select new {
                        r.EmployeeID,
                        r.Employee.UserName
                    }).Distinct().OrderBy(o => o.UserName);
        }
    }
}