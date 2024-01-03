using System.Collections.Generic;
using log4net;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight
{
    public class SpaceTimeInsightDailyFileDumpService : SpaceTimeInsightFileDumpServiceBase, ISpaceTimeInsightDailyFileDumpService
    {
        #region Constructors

        public SpaceTimeInsightDailyFileDumpService(ILog log, ISpaceTimeInsightFileDumpTaskService taskService) : base(log, taskService) {}

        #endregion

        #region Private Methods

        protected override IEnumerable<ISpaceTimeInsightFileDumpTask> GetTasks()
        {
            return _taskService.GetAllDailyTasks();
        }

        #endregion
    }

    public interface ISpaceTimeInsightDailyFileDumpService : ISpaceTimeInsightFileDumpService {}
}
