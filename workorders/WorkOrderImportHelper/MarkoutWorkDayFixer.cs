using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.Linq;
using MapCall.Common.Utility;
using WorkOrders.Library;
using WorkOrders.Model;

namespace WorkOrderImportHelper
{
    public class MarkoutWorkDayFixer : WorkOrderProcessor
    {
        #region Constants

        private struct StatusStrings
        {
            public const string WORK_STARTED =
                                    "Work has started for WorkOrder {0} (old #{1}).",
                                WORK_NOT_STARTED =
                                    "Work has not started for WorkOrder {0} (old #{1}).",
                                PROCESSING_MARKOUT =
                                    "Markout #{0} for WorkOrder {1} (old #{2}): Called {3}, Ready {4}, Expires {5}.",
                                SKIPPING_MARKOUT =
                                    "Skipping Markout #{0} for WorkOrder {1} (old #{2}): Called {3}, Ready {4}, Expires {5}.",
                                DATE_CALLED_IS_NULL =
                                    "Markout #{0} (ID {1}) for WorkOrder {2} (old #{3}) has null DateOfRequest.";
        }

        #endregion

        #region Private Members

        private IRepository<WorkOrder> _workOrderRepository;
        private IEnumerable<WorkOrder> _workOrders;

        #endregion

        #region Properties

        public IRepository<WorkOrder> Repository
        {
            get
            {
                if (_workOrderRepository == null)
                    _workOrderRepository = new WorkOrderRepository();
                return _workOrderRepository;
            }
        }
        public IEnumerable<WorkOrder> WorkOrders
        {
            get
            {
                if (_workOrders == null)
                    _workOrders = GetWorkOrdersWithMarkouts();
                return _workOrders;
            }
        }

        #endregion

        #region Constructors

        public MarkoutWorkDayFixer(Action<int> displayCurrentWorkOrderID, Action<string> outputFn)
            : base(displayCurrentWorkOrderID, outputFn)
        {
        }

        #endregion

        #region Private Methods

        private IEnumerable<WorkOrder> GetWorkOrdersWithMarkouts()
        {
            return Repository.GetFilteredSortedData(
                wo => wo.Markouts.Count > 0, "WorkOrderID");
        }

        #endregion

        #region Exposed Methods

        override public int LoadWorkOrders()
        {
            return WorkOrders.Count();
        }

        public void ProcessMarkouts()
        {
            int processed = 0, skipped = 0;
            foreach (var order in WorkOrders)
            {
                _displayCurrentWorkOrderID(order.WorkOrderID);
                _outputFn(
                    String.Format(
                        order.WorkStarted
                            ? StatusStrings.WORK_STARTED
                            : StatusStrings.WORK_NOT_STARTED,
                        order.WorkOrderID, order.OldWorkOrderNumber));

                if (order.MarkoutRequirement.RequirementEnum == MarkoutRequirementEnum.None)
                {
                    order.MarkoutRequirement =
                        MarkoutRequirementRepository.Routine;
                }

                var i = 0;
                foreach (var markout in order.Markouts)
                {
                    i++;
                    var newReadyDate =
                        WorkOrdersWorkDayEngine.GetReadyDate(
                            markout.DateOfRequest, order.MarkoutRequirement.RequirementEnum);
                    var originalExpirationDate =
                        WorkOrdersWorkDayEngine.GetExpirationDate(
                            markout.DateOfRequest,
                            order.MarkoutRequirement.RequirementEnum, false);
                    var newExpirationDate =
                        WorkOrdersWorkDayEngine.GetExpirationDate(
                            markout.DateOfRequest,
                            order.MarkoutRequirement.RequirementEnum, order.WorkStartedBetween(newReadyDate, originalExpirationDate));

                    if (newReadyDate.Date != markout.ReadyDate.Date ||
                        newExpirationDate.Date != markout.ExpirationDate.Date)
                    {
                        markout.ReadyDate = newReadyDate;
                        markout.ExpirationDate = newExpirationDate;

                        _outputFn(String.Format(
                            StatusStrings.PROCESSING_MARKOUT, i,
                            order.WorkOrderID, order.OldWorkOrderNumber,
                            markout.DateOfRequest, markout.ReadyDate,
                            markout.ExpirationDate));

                        FixerMarkoutRepository.UpdateOnlyMarkout(markout);

                        processed++;
                    }
                    else
                    {
                        _outputFn(String.Format(StatusStrings.SKIPPING_MARKOUT,
                            i, order.WorkOrderID, order.OldWorkOrderNumber,
                            markout.DateOfRequest, markout.ReadyDate,
                            markout.ExpirationDate));

                        skipped++;
                    }
                }
            }
            _outputFn(string.Format("Processed {0}, skipped {1}", processed,
                skipped));
        }

        #endregion
    }

    internal class FixerMarkoutRepository : MarkoutRepository
    {
        #region Constants

        private const string UPDATE_QUERY_FORMAT =
            "UPDATE [Markouts] SET [ReadyDate] = '{1}', [ExpirationDate] = '{2}' WHERE [MarkoutID] = {0};";

        #endregion

        #region Exposed Static Methods

        public static void UpdateOnlyMarkout(Markout markout)
        {
            DataContext.ExecuteCommand(String.Format(
                UPDATE_QUERY_FORMAT, markout.MarkoutID, markout.ReadyDate,
                markout.ExpirationDate));
        }

        #endregion
    }
}
