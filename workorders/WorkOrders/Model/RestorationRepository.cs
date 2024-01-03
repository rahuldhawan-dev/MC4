using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace WorkOrders.Model
{
    public class RestorationRepository : WorkOrdersRepository<Restoration>
    {
        #region Exposed Static Methods

        public static IEnumerable<Restoration> GetRestorationsByWorkOrder(int workOrderID)
        {
            return
                (from r in DataTable
                 where r.WorkOrderID == workOrderID
                 select r);
        }

        public static IEnumerable<Restoration> GetApprovedRestorationsByWorkOrderDateCompletedAndAccountingType(DateTime dateStart, DateTime dateEnd, int accountingTypeID, int operatingCenterID, Func<Restoration, object> sortBy, string sortDirection)
        {
            // NOTE THIS MAY BE IN THE PAGES INSTRUCTIONS, UPDATE THERE IF YOU CHANGE
            var ret =  (from res in DataTable
                    where
                        res.WorkOrder != null && // Bug 2784: Ignore restorations without a work order as they no longer require a work order.
                        res.WorkOrder.DateCompleted >= dateStart &&
                        res.WorkOrder.DateCompleted <= dateEnd &&
                        res.WorkOrder.ApprovedOn != null && 
                        res.WorkOrder.WorkDescription.AccountingTypeID ==
                        accountingTypeID &&
                        res.WorkOrder.OperatingCenterID == operatingCenterID
                        && res.FinalRestorationDate == null
                    select res);

            return sortDirection == SortDirection.Ascending.ToString()
                       ? ret.OrderBy(sortBy)
                       : ret.OrderByDescending(sortBy);
        }

        public static IEnumerable<Restoration> GetRestorationsByWorkOrderDateCompletedAndAccountingType(DateTime dateStart, DateTime dateEnd, int accountingTypeID, int operatingCenterID)
        {
            // default to WorkOrderID Ascending
            // NOTE THIS MAY BE IN THE PAGES INSTRUCTIONS, UPDATE THERE IF YOU CHANGE
            return
                GetApprovedRestorationsByWorkOrderDateCompletedAndAccountingType(
                    dateStart, dateEnd, accountingTypeID, operatingCenterID,
                    r => r.WorkOrderID, SortDirection.Ascending.ToString());
        }

        public static IEnumerable<Restoration> GetIncompleteRestorationsRestorationsByWorkOrderOperatingCenterID(int operatingCenterID)
        {
            return (from res in DataTable
                    where
                        res.WorkOrder.OperatingCenterID == operatingCenterID &&
                        res.FinalRestorationDate == null
                    orderby res.WorkOrderID
                    select res);
        }

        public static void DeleteById(int restorationID)
        {
            var restoration = GetEntity(restorationID);
            Delete(restoration);
        }

        public static void UpdateResponsePriority(int restorationID, int responsePriorityID)
        {
            var restoration = GetEntity(restorationID);
            restoration.ResponsePriorityID = responsePriorityID;
            Update(restoration);
        }

        #endregion
    }
}