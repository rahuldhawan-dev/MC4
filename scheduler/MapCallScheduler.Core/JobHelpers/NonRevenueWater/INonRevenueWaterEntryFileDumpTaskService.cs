using System.Collections.Generic;
using MapCallScheduler.Library.Common;

namespace MapCallScheduler.JobHelpers.NonRevenueWater
{
    public interface INonRevenueWaterEntryFileDumpTaskService : ITaskService<INonRevenueWaterEntryFileDumpTask>
    {
        #region Abstract Methods

        IEnumerable<INonRevenueWaterEntryFileDumpTask> GetAllDailyTasks();

        #endregion
    }
}
