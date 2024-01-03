using System.Collections.Generic;
using log4net;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight
{
    public class SpaceTimeInsightMonthlyFileDumpService : SpaceTimeInsightFileDumpServiceBase, ISpaceTimeInsightMonthlyFileDumpService
    {
        #region Constructors

        public SpaceTimeInsightMonthlyFileDumpService(ILog log, ISpaceTimeInsightFileDumpTaskService taskService) : base(log, taskService) {}

        #endregion

        #region Private Methods

        protected override IEnumerable<ISpaceTimeInsightFileDumpTask> GetTasks()
        {
            return _taskService.GetAllMonthlyTasks();
        }

        #endregion
    }

    public interface ISpaceTimeInsightMonthlyFileDumpService : ISpaceTimeInsightFileDumpService {}
}