using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.Linq;

namespace WorkOrders.Model
{
    public class OperatingCenterRepository : WorkOrdersRepository<OperatingCenter>, IOperatingCenterRepository
    {
        #region Exposed Static Methods

        public static string GetOpCodeByOperatingCenterID(int operatingCenterID)
        {
            return
                (from oc in DataTable
                 where oc.OperatingCenterID == operatingCenterID
                 select oc).FirstOrDefault().OpCntr;
        }

        //TODO: THIS METHOD SUCKS, STRINGS FTL
        public static OperatingCenter GetOperatingCenterByOpCntr(string opCntr)
        {
            return
                (from oc in DataTable
                 where oc.OpCntr == opCntr
                 select oc).FirstOrDefault();
        }

        public static IEnumerable<OperatingCenter> SelectAll271OperatingCenters()
        {
            return DataTable.Where((op) => op.WorkOrdersEnabled).OrderBy(oc => oc.State.Abbreviation).ThenBy(oc => oc.OpCntr);
        }

        public static IEnumerable<OperatingCenter>
            SelectAll271OperatingCentersThatAreRegulated()
        {
            // Bug #2389: EW4 and LWC are unregulated op centers.
            return
                SelectAll271OperatingCenters()
                    .Where(x => x.IsContractedOperations == false);
        }

        public static IEnumerable<OperatingCenter>
            SelectAll271OperatingCentersThatAreNotRegulated()
        {
            // Bug #2389: EW4 and LWC are unregulated op centers.
            return
                SelectAll271OperatingCenters()
                    .Where(x => x.IsContractedOperations == true);
        }

        public static IEnumerable<OperatingCenter>
            SelectAll271OperatingCentersThatAreRegulatedByState(int stateId)
        {
            // Bug #2389: EW4 and LWC are unregulated op centers.
            return
                SelectAll271OperatingCenters()
                    .Where(x => x.IsContractedOperations == false && x.StateID == stateId);
        }

        public static IEnumerable<OperatingCenter>
            SelectAll271OperatingCentersThatAreNotRegulatedByState(int stateId)
        {
            // Bug #2389: EW4 and LWC are unregulated op centers.
            return
                SelectAll271OperatingCenters()
                    .Where(x => x.IsContractedOperations == true && x.StateID == stateId);
        }

        public static double? AverageTimeToCompleteWorkOrders(DateTime dateStart, DateTime dateEnd, int operatingCenterID)
        {
            return (from oc in DataTable
                    where oc.OperatingCenterID == operatingCenterID
                    select
                        oc.WorkOrders
                        .Where(wo =>
                               wo.DateCompleted != null &&
                               wo.DateCompleted >= dateStart &&
                               wo.DateCompleted <= dateEnd &&
                               wo.DateReceived != null)
                        .Average(wo => (double?)
                                       (wo.DateCompleted.Value -
                                        wo.DateReceived.Value).TotalHours)
                   ).FirstOrDefault();
        }

        public static double? AverageTimeToApproveWorkOrders(DateTime dateStart, DateTime dateEnd, int operatingCenterID)
        {
            return (from oc in DataTable
                    where oc.OperatingCenterID == operatingCenterID
                    select
                        oc.WorkOrders
                        .Where(wo =>
                               wo.ApprovedOn != null &&
                               wo.ApprovedOn >= dateStart &&
                               wo.ApprovedOn <= dateEnd)
                        .Average(wo => (double?)
                                       (wo.ApprovedOn.Value -
                                        wo.DateCompleted.Value).TotalHours)
                   ).FirstOrDefault();
        }

        public static double? AverageTimeToApproveMaterialsWorkOrders(DateTime dateStart, DateTime dateEnd, int operatingCenterID)
        {
            return (from oc in DataTable
                    where oc.OperatingCenterID == operatingCenterID
                    select
                        oc.WorkOrders
                        .Where(wo => wo.MaterialsApprovedOn != null &&
                                     wo.MaterialsApprovedOn >= dateStart &&
                                     wo.MaterialsApprovedOn <= dateEnd)
                        .Average(wo => (double?)
                                       (wo.MaterialsApprovedOn.Value -
                                        wo.ApprovedOn.Value)
                                           .TotalHours)
                   ).FirstOrDefault();
        }

        public static double? AverageTimeToCompleteWorkOrders(DateTime dateStart, DateTime dateEnd, int operatingCenterID, int workDescriptionID)
        {
            var avg = (from oc in DataTable
                       where oc.OperatingCenterID == operatingCenterID
                       select
                           oc.WorkOrders
                           .Where(wo =>
                                  wo.DateCompleted != null &&
                                  wo.DateCompleted >= dateStart &&
                                  wo.DateCompleted <= dateEnd &&
                                  wo.DateReceived != null &&
                                  wo.WorkDescriptionID == workDescriptionID)
                           .Average(wo => (double?)
                                          (wo.DateCompleted.Value -
                                           wo.DateReceived.Value).TotalHours)
                   ).FirstOrDefault();

            return (avg.HasValue && avg.Value < 0) ? 0 : avg;
        }

        public static double? AverageTimeToApproveMaterialsWorkOrders(DateTime dateStart, DateTime dateEnd, int operatingCenterID, int workDescriptionID)
        {
            return (from oc in DataTable
                    where oc.OperatingCenterID == operatingCenterID
                    select
                        oc.WorkOrders
                        .Where(wo => wo.MaterialsApprovedOn != null &&
                                     wo.MaterialsApprovedOn >= dateStart &&
                                     wo.MaterialsApprovedOn <= dateEnd &&
                                     wo.WorkDescriptionID == workDescriptionID)
                        .Average(wo => (double?)
                                       (wo.MaterialsApprovedOn.Value -
                                        wo.ApprovedOn.Value)
                                           .TotalHours)
                   ).FirstOrDefault();
        }

        public static double? AverageTimeToApproveWorkOrders(DateTime dateStart, DateTime dateEnd, int operatingCenterID, int workDescriptionID)
        {
            return (from oc in DataTable
                    where oc.OperatingCenterID == operatingCenterID
                    select
                        oc.WorkOrders
                        .Where(wo =>
                               wo.ApprovedOn != null &&
                               wo.ApprovedOn >= dateStart &&
                               wo.ApprovedOn <= dateEnd &&
                               wo.WorkDescriptionID == workDescriptionID)
                        .Average(wo => (double?)
                                       (wo.ApprovedOn.Value -
                                        wo.DateCompleted.Value).TotalHours)
                  ).FirstOrDefault();
        }

        #endregion

        #region Exposed Methods
        
        public OperatingCenter[] GetAll271OperatingCenters()
        {
            return SelectAll271OperatingCenters().ToArray();
        }

        #endregion
    }

    public interface IOperatingCenterRepository : IRepository<OperatingCenter>
    {
        OperatingCenter[] GetAll271OperatingCenters();
    }
}
