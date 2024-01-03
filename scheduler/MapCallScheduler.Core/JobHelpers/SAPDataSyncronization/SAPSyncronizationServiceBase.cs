using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Utility.Scheduling;

namespace MapCallScheduler.JobHelpers.SAPDataSyncronization
{
    public abstract class SAPSyncronizationServiceBase<TTask>
        where TTask : ITask
    {
        #region Private Members

        protected readonly ISAPSyncronizationTaskService _taskService;
        protected readonly ILog _log;

        #endregion

        #region Constructors

        public SAPSyncronizationServiceBase(ISAPSyncronizationTaskService taskService, ILog log)
        {
            _taskService = taskService;
            _log = log;
        }

        #endregion

        #region Abstract Methods

        protected abstract IEnumerable<TTask> GetTasks();

        #endregion

        #region Exposed Methods

        #region Public Methods

        public void Process()
        {
            var tasks = GetTasks().ToList();
            foreach (var task in tasks)
            {
                _log.Info($"Running sap syncronization task {task.GetType().Name}");
                task.Run();
                _log.Info($"Completed sap syncronization task {task.GetType().Name}");

            }
        }

        #endregion

        #endregion
    }
}
