using System.Collections.Generic;
using log4net;
using MapCallScheduler.Library.JobHelpers.FileDumps;

namespace MapCallScheduler.JobHelpers.NonRevenueWater
{
    public class MonthlyNonRevenueWaterEntryFileDumpService :
        FileDumpServiceBase<INonRevenueWaterEntryFileDumpTaskService, INonRevenueWaterEntryFileDumpTask>,
        IMonthlyNonRevenueWaterEntryFileDumpService
    {
        #region Properties

        public override string Descriptor { get; }

        #endregion

        #region Constructors

        public MonthlyNonRevenueWaterEntryFileDumpService(ILog log,
            INonRevenueWaterEntryFileDumpTaskService taskService) : base(log, taskService) { }

        #endregion

        #region Private Methods

        protected override IEnumerable<INonRevenueWaterEntryFileDumpTask> GetTasks()
        {
            return _taskService.GetAllDailyTasks();
        }

        #endregion
    }
}
