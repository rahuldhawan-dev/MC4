using System.Collections.Generic;
using System.Linq;
using MapCallScheduler.Library.Common;
using StructureMap;

namespace MapCallScheduler.JobHelpers.NonRevenueWater
{
    public class NonRevenueWaterEntryFileDumpTaskService : TaskServiceBase<INonRevenueWaterEntryFileDumpTask>,
        INonRevenueWaterEntryFileDumpTaskService
    {
        #region Constructors

        public NonRevenueWaterEntryFileDumpTaskService(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<INonRevenueWaterEntryFileDumpTask> GetAllDailyTasks() =>
            GetAllTaskTypes().Select(InstantiateTask<INonRevenueWaterEntryFileDumpTask>);

        #endregion
    }
}
