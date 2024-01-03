using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WorkOrders.Model
{
    public class CrewAssignmentRepository : SapWorkOrdersBaseRepository<CrewAssignment>
    {
        #region Exposed Static Methods

        public static IEnumerable<CrewAssignment> GetPendingAssignmentsByCrewAndDate(int? crewID, DateTime? date)
        {
            if (crewID == null || date == null) return null;

            return (from a in DataTable
                    where a.CrewID == crewID.Value && a.AssignedFor.Date == date.Value.Date
                    && a.DateStarted == null
                    && SecurityService.UserOperatingCenters.Contains(a.Crew.OperatingCenter)
                    orderby a.Priority
                    select a);
        }

        public static IEnumerable<CrewAssignment> GetAssignmentsByWorkOrder(int workOrderID)
        {
            return (from ca in DataTable
                    where ca.WorkOrderID == workOrderID
                    orderby ca.AssignedFor
                    select ca);
        }
        
        public static IEnumerable GetWorkOrderTimeAverages(DateTime dateStart, DateTime dateEnd, int operatingCenterID)
        {
            return (from ca in DataTable
                    where ca.WorkOrder.DateCompleted >= dateStart &&
                          ca.WorkOrder.DateCompleted <= dateEnd 
                    group ca by ca.WorkOrder.OperatingCenter
                        into oc
                        where (operatingCenterID > 0) ? oc.Key.OperatingCenterID == operatingCenterID : true
                        orderby oc.Key.OpCntr
                        select new
                        {
                            OpCntr = oc.Key.FullDescription,
                            Completion = OperatingCenterRepository.
                                AverageTimeToCompleteWorkOrders(
                                    dateStart, dateEnd, oc.Key.OperatingCenterID),
                            ManHours =
                                oc.Average(ca => (ca.DateEnded - ca.DateStarted).Value.TotalHours *
                                        ca.EmployeesOnJob),
                            Approval = 
                                OperatingCenterRepository.AverageTimeToApproveWorkOrders(
                                    dateStart, dateEnd, oc.Key.OperatingCenterID),
                            StockApproval = OperatingCenterRepository.
                                AverageTimeToApproveMaterialsWorkOrders(
                                    dateStart, dateEnd, oc.Key.OperatingCenterID
                                )
                        });
        }

        public static void Update(int crewAssignmentID, DateTime dateStarted, DateTime dateEnded, float employeesOnJob)
        {
            var assignment = GetEntity(crewAssignmentID);
            assignment.DateStarted = dateStarted;
            assignment.DateEnded = dateEnded;
            assignment.EmployeesOnJob = employeesOnJob;
            Update(assignment);
            UpdateSAPWorkOrderStatic(assignment.WorkOrder);
        }

        public static IEnumerable GetWorkOrderTimeAveragesByWorkDescription(
            DateTime dateStart, DateTime dateEnd, int operatingCenterID)
        {
            return (from ca in DataTable
                    where ca.WorkOrder.DateCompleted >= dateStart &&
                          ca.WorkOrder.DateCompleted <= dateEnd
                    group ca by new {
                        ca.WorkOrder.OperatingCenter,
                        ca.WorkOrder.WorkDescription
                    }
                    into grp
                    where
                        !(operatingCenterID > 0) ||
                        grp.Key.OperatingCenter.OperatingCenterID ==
                        operatingCenterID
                    orderby grp.Key.OperatingCenter.OpCntr ascending,
                        grp.Key.WorkDescription.Description ascending
                    select new {
                        grp.Key.OperatingCenter.OpCntr,
                        Completion = OperatingCenterRepository.
                        AverageTimeToCompleteWorkOrders(
                            dateStart, dateEnd,
                            grp.Key.OperatingCenter.OperatingCenterID,
                            grp.Key.WorkDescription.WorkDescriptionID),
                        ManHours = grp
                        .Average(ca =>
                                 (ca.DateEnded - ca.DateStarted).Value.
                                                                 TotalHours *
                                 ca.EmployeesOnJob),
                        Approval = OperatingCenterRepository
                        .AverageTimeToApproveWorkOrders(dateStart, dateEnd,
                            grp.Key.OperatingCenter.OperatingCenterID,
                            grp.Key.WorkDescription.WorkDescriptionID),
                        StockApproval = OperatingCenterRepository
                        .AverageTimeToApproveMaterialsWorkOrders(
                            dateStart, dateEnd,
                            grp.Key.OperatingCenter.OperatingCenterID,
                            grp.Key.WorkDescription.WorkDescriptionID
                        ),
                        WorkDescription = grp.Key.WorkDescription.Description,
                        Count = grp.Count(),
                        CrewAverage = grp.Average(ca => ca.EmployeesOnJob)
                    });
        }

        #endregion
    }
}
