using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using MMSINC.Common;

namespace WorkOrders.Model
{
    public interface IWorkOrdersWorkOrderRepository
    {
        void UpdateSAPWorkOrder(WorkOrder entity, MapCall.Common.Model.Entities.WorkOrder workOrder = null);
        void UpdateCurrentEntity(WorkOrder entity);

        WorkOrder Get(object id);
    }

    /// <summary>
    /// Repository for storing/retrieving WorkOrder objects from persistence.
    /// </summary>
    public class WorkOrderRepository : SapWorkOrdersBaseRepository<WorkOrder>, IWorkOrdersWorkOrderRepository
    {
        #region Exposed Static Methods

        /// <summary>
        /// Gets the set of WorkOrders that are attached to the given asset.
        /// </summary>
        /// <param name="assetTypeID">ID value representing the AssetType of the
        /// asset.</param>
        /// <param name="assetKey">Key value of the asset within its storage
        /// table.</param>
        /// <returns>A typed EntitySet of WorkOrder objects, or null if none
        /// found.</returns>
        public static List<WorkOrder> GetWorkOrdersByOperatingCenterAndAsset(int assetTypeID, int operatingCenterID, object assetKey)
        {
            if (assetTypeID == 0 || assetKey == null)
                return null;

            var orders = GetWorkOrdersByAsset(
                AssetTypeRepository.GetEntity(assetTypeID), assetKey,
                operatingCenterID);
            return (orders == null) ? null : orders.ToList();
        }

        /// <summary>
        /// Gets the set of WorkOrders that are attached to the given asset.
        /// </summary>
        /// <param name="assetTypeID">ID value representing the AssetType of the
        /// asset.</param>
        /// <param name="assetKey">Key value of the asset within its storage
        /// table.</param>
        /// <param name="sortExpression">
        /// </param>
        /// <returns>A typed EntitySet of WorkOrder objects, or null if none
        /// found.</returns>
        public static List<WorkOrder> GetWorkOrdersByOperatingCenterAndAsset(int assetTypeID, int operatingCenterID, object assetKey, string sortExpression)
        {
            var list = GetWorkOrdersByOperatingCenterAndAsset(assetTypeID, operatingCenterID, assetKey);
            if (!String.IsNullOrEmpty(sortExpression))
                list.Sort(new EntityComparer<WorkOrder>(sortExpression));
            return list;
        }

        public static List<WorkOrder> GetWorkOrdersByCrewID(int? crewID)
        {
            if (crewID == null) return null;
            var crew = CrewRepository.GetEntity(crewID);
            var list = new List<WorkOrder>();
            foreach (var assignment in crew.CrewAssignments)
            {
                list.Add(assignment.WorkOrder);
            }
            return list;
        }

        private static IEnumerable<int> GetOperatingCentersByRegulation(
            bool isRegulated)
        {
            if (isRegulated)
            {
                return
                    OperatingCenterRepository
                        .SelectAll271OperatingCentersThatAreRegulated()
                        .Select(x => x.OperatingCenterID).ToArray();
            }
            else
            {
                return
                    OperatingCenterRepository
                        .SelectAll271OperatingCentersThatAreNotRegulated()
                        .Select(x => x.OperatingCenterID).ToArray();
            }
        }

        public static int GetCompleteOrderCountByMonthAndDescription(int? stateId, int? operatingCenterID, int workDescriptionID, DateTime month, bool regulatedOperatingCenters)
        {
            IEnumerable<int> opCenters = GetOperatingCentersByRegulation(regulatedOperatingCenters);
           
            return (from wo in DataTable
                    where
                        ((operatingCenterID == null && opCenters.Contains(wo.OperatingCenterID.Value)) || wo.OperatingCenterID == operatingCenterID) &&
                        (stateId == null || wo.OperatingCenter.StateID == stateId) &&
                        wo.WorkDescriptionID == workDescriptionID &&
                        wo.CancelledAt == null && // MC-83/WO0000000235728 Do not include cancelled workorders. 
                        wo.DateCompleted != null &&
                        wo.DateCompleted.Value.Month == month.Month &&
                        wo.DateCompleted.Value.Year == month.Year
                    select wo).Count();
        }

        public static int GetCompleteOrderCountByYearAndDescription(int? stateId, int? operatingCenterID, int workDescriptionID, int year, bool regulatedOperatingCenters)
        {
            IEnumerable<int> opCenters = GetOperatingCentersByRegulation(regulatedOperatingCenters);

            return (from wo in DataTable
                    where
                        ((operatingCenterID == null && opCenters.Contains(wo.OperatingCenterID.Value)) ||
                         wo.OperatingCenterID == operatingCenterID) &&
                        (stateId == null || wo.OperatingCenter.StateID == stateId) &&
                        wo.WorkDescriptionID == workDescriptionID &&
                        wo.CancelledAt == null && // MC-83/WO0000000235728 Do not include cancelled workorders. 
                        wo.DateCompleted != null &&
                        wo.DateCompleted.Value.Year == year
                    select wo).Count();
        }

        public static int GetORCOMOrderCountByMonthAndWorkDescription(int? operatingCenterID, int workDescriptionID, DateTime month)
        {
            return (from wo in DataTable
                    where
                        (operatingCenterID == null || wo.OperatingCenterID == operatingCenterID) &&
                        wo.WorkDescriptionID == workDescriptionID &&
                        wo.DateCompleted != null &&
                        wo.DateCompleted.Value.Month == month.Month &&
                        wo.DateCompleted.Value.Year == month.Year &&
                        wo.ORCOMServiceOrderNumber != null &&
                        wo.ORCOMServiceOrderNumber != String.Empty
                    select wo).Count();
        }

        public static int GetORCOMOrderCountByYearAndWorkDescription(int? operatingCenterID, int workDescriptionID, int year)
        {
            return (from wo in DataTable
                    where
                        (operatingCenterID == null || wo.OperatingCenterID == operatingCenterID) &&
                        wo.WorkDescriptionID == workDescriptionID &&
                        wo.DateCompleted != null &&
                        wo.DateCompleted.Value.Year == year &&
                        wo.ORCOMServiceOrderNumber != null &&
                        wo.ORCOMServiceOrderNumber != String.Empty
                    select wo).Count();
        }

        public static int GetIncompleteWorkOrderCountByDescription(int? stateId, int? operatingCenterID, int workDescriptionID, bool regulatedOperatingCenters)
        {
            IEnumerable<int> opCenters = GetOperatingCentersByRegulation(regulatedOperatingCenters);

            return (from wo in DataTable
                    where
                        ((operatingCenterID == null && opCenters.Contains(wo.OperatingCenterID.Value)) ||
                         wo.OperatingCenterID == operatingCenterID) &&
                        (stateId == null || wo.OperatingCenter.StateID == stateId) &&
                        wo.DateCompleted == null &&
                        wo.CancelledAt == null &&
                        wo.WorkDescriptionID == workDescriptionID
                    select wo).Count();
        }

        public static int GetCompleteOrderCountByMonthAndCategory(int operatingCenterID, int workCategoryID, DateTime month)
        {
            return (from wo in DataTable
                    where
                        wo.OperatingCenterID == operatingCenterID &&
                        wo.WorkDescription.WorkCategoryID == workCategoryID &&
                        wo.DateCompleted != null &&
                        wo.DateCompleted.Value.Month == month.Month &&
                        wo.DateCompleted.Value.Year == month.Year
                    select wo).Count();
        }

        public static int GetCompleteOrderCountByYearAndCategory(int operatingCenterID, int workCategoryID, int year)
        {
            return (from wo in DataTable
                    where
                        wo.OperatingCenterID == operatingCenterID &&
                        wo.WorkDescription.WorkCategoryID == workCategoryID &&
                        wo.DateCompleted != null &&
                        wo.DateCompleted.Value.Year == year
                    select wo).Count();
        }

        public static int GetIncompleteWorkOrderCountByCategory(int operatingCenterID, int workCategoryID)
        {
            return (from wo in DataTable
                    where
                        wo.OperatingCenterID == operatingCenterID &&
                        wo.DateCompleted == null &&
                        wo.CancelledAt == null &&
                        wo.WorkDescription.WorkCategoryID == workCategoryID
                    select wo).Count();
        }

        public static List<int> GetValidWorkOrderCompletionYears(int operatingCenterID)
        {
            // MC-2144: This method's only ever called on a few report pages and they want
            // the years to be newest to oldest.
            var currentYear = DateTime.Now.Year;
            var minYear = DataTable.Min(wo => wo.DateCompleted).Value.Year;
            var ret = new List<int>(currentYear - minYear);
            for (var i = currentYear; i >= minYear; --i)
            {
                ret.Add(i);
            }

            return ret;
        }

        public static List<int> GetValidORCOMWorkOrderCreationYears(int operatingCenterID)
        {
            var currentYear = DateTime.Now.Year;
            var minYear = DataTable
                .Where(wo => wo.ORCOMServiceOrderNumber != null && wo.ORCOMServiceOrderNumber != String.Empty)
                .Min(wo => wo.CreatedOn).Year;

            var ret = new List<int>(currentYear - minYear);
            for (var i = minYear; i <= currentYear; ++i)
                ret.Add(i);

            return ret;
        }

        public static TimeSpan GetCompleteOrderManHoursByMonthAndCategory(int operatingCenterID, int workCategoryID, DateTime month)
        {
            var assignments = (from wo in DataTable
                               where
                                   wo.OperatingCenterID == operatingCenterID &&
                                   wo.WorkDescription.WorkCategoryID ==
                                   workCategoryID &&
                                   wo.DateCompleted != null &&
                                   wo.DateCompleted.Value.Month == month.Month &&
                                   wo.DateCompleted.Value.Year == month.Year
                               select wo.CrewAssignments);

            var timeSpan = new TimeSpan();

            if (assignments.Count() == 0)
                return timeSpan;

            foreach (var assignmentSet in assignments)
            {
                foreach (var assignment in assignmentSet)
                {
                    if (assignment.DateStarted == null || assignment.DateEnded == null)
                        continue;

                    timeSpan +=
                        (assignment.DateEnded - assignment.DateStarted).Value;
                }
            }

            return timeSpan;
        }

        public static TimeSpan GetCompleteOrderManHoursByYearAndCategory(int operatingCenterID, int workCategoryID, int year)
        {
            var assignments = (from wo in DataTable
                               where
                                   wo.OperatingCenterID == operatingCenterID &&
                                   wo.WorkDescription.WorkCategoryID ==
                                   workCategoryID &&
                                   wo.DateCompleted != null &&
                                   wo.DateCompleted.Value.Year == year
                               select wo.CrewAssignments);

            var timeSpan = new TimeSpan();

            if (assignments.Count() == 0)
                return timeSpan;

            foreach (var assignmentSet in assignments)
            {
                foreach (var assignment in assignmentSet)
                {
                    if (assignment.DateStarted == null || assignment.DateEnded == null)
                        continue;

                    timeSpan +=
                        (assignment.DateEnded - assignment.DateStarted).Value;
                }
            }

            return timeSpan;
        }

        public static TimeSpan GetIncompleteOrderManHoursByCategory(int operatingCenterID, int workCategoryID)
        {
            var hours = (from wo in DataTable
                         where
                             wo.OperatingCenterID == operatingCenterID &&
                             wo.DateCompleted == null &&
                             wo.CancelledAt == null &&
                             wo.WorkDescription.WorkCategoryID == workCategoryID
                         select wo.WorkDescription.TimeToComplete);
            return new TimeSpan(0,
                hours.Count() > 0 ? (int)(hours.Sum() * 60) : 0, 0);
        }

        public static IEnumerable<WorkOrder> GetWorkOrdersByMaterialsApprovedDateRange(DateTime dateStart, DateTime dateEnd, int approvedByID, int operatingCenterID)
        {
            // remember, "approving" materials is actually the regular supervisor approval,
            // regular stock charging is not supervisory approval
            return (from wo in DataTable
                    where
                        wo.ApprovedByID == approvedByID &&
                        wo.OperatingCenterID == operatingCenterID &&
                        wo.MaterialsUseds.Count > 0 &&
                        wo.ApprovedOn >= dateStart &&
                        wo.ApprovedOn <= dateEnd
                    select wo);
        }

        public static IEnumerable<WorkOrder> GetCapitalWorkOrdersByOpCenterAndDateRange(DateTime dateStart, DateTime dateEnd, int operatingCenterID)
        {
            return (from wo in DataTable
                    where
                        wo.DateCompleted >= dateStart
                        && wo.DateCompleted <= dateEnd
                        && wo.OperatingCenterID == operatingCenterID
                        && wo.WorkDescription.AccountingTypeID == (int)AccountingTypeEnum.Capital
                    select wo);
        }

        public static IEnumerable<WorkOrder> GetCapitalWorkOrdersByOpCenterAndDateRange(DateTime dateStart, DateTime dateEnd, int operatingCenterID, Func<WorkOrder, object> sortBy, string sortDirection)
        {
            var ret = GetCapitalWorkOrdersByOpCenterAndDateRange(dateStart,
                dateEnd, operatingCenterID);

            return sortDirection == SortDirection.Ascending.ToString()
                ? ret.OrderBy(sortBy)
                : ret.OrderByDescending(sortBy);
        }

        public static IEnumerable<WorkOrder> GetIncompleteWorkOrdersByWorkDescriptionID(int workDescriptionID)
        {
            return (from wo in DataTable
                    where
                        wo.WorkDescriptionID == workDescriptionID &&
                        wo.DateCompleted == null &&
                        wo.CancelledAt == null
                        orderby wo.OperatingCenter.OpCntr
                    select wo);
        }

        public static IQueryable<WorkOrder> GetCompletedOrdersByOperatingCenterAndTrafficControlRequirement(int? operatingCenterID, int? townID, bool? trafficControlRequired)
        {
            return (from wo in DataTable
                    where
                        wo.DateCompleted != null &&
                        (operatingCenterID == null || wo.OperatingCenterID == operatingCenterID) &&
                        (townID == null || wo.TownID == townID) &&
                        (trafficControlRequired == null ||
                         wo.TrafficControlRequired == trafficControlRequired)
                    select wo);
        }

        #endregion

        #region Private Static Methods

        private static IEnumerable<WorkOrder> GetWorkOrdersByAsset(AssetType assetType, object assetKey, int operatingCenterID)
        {
            if (assetType.TypeEnum != AssetTypeEnum.Service && assetType.TypeEnum != AssetTypeEnum.SewerLateral)
            {
                var asset = Asset.GetAssetByTypeAndKey(assetType, assetKey);
                return (asset == null || asset.WorkOrders == null)
                           ? null : asset.WorkOrders;
            }

            return
                (from wo in DataTable
                 where
                     wo.PremiseNumber == assetKey.ToString() &&
                     wo.OperatingCenterID == operatingCenterID
                 select wo);
        }

        #endregion

        #region Private Methods

        protected override IEnumerable<WorkOrder> GetFilteredData(Expression<Func<WorkOrder, bool>> filterExpression)
        {
            var builder = new ExpressionBuilder<WorkOrder>(PredicateBuilder.True<WorkOrder>());
            builder.And(filterExpression);
            builder.And(
                wo =>
                SecurityService.UserOperatingCenters.Contains(wo.OperatingCenter));

            return DataTable.Where(builder);
        }

        #endregion
    }

    public static class WorkOrderEntitySetExtensions
    {
        public static List<WorkOrder> ToList(this EntitySet<WorkOrder> es)
        {
            if (es == null)
                return null;
            return new List<WorkOrder>(es);
        }
    }
}
