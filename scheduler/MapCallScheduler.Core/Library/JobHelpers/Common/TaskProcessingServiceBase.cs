using System.Collections.Generic;
using log4net;
using MapCall.Common.Utility.Scheduling;
using MapCallScheduler.Library.Common;

namespace MapCallScheduler.Library.JobHelpers.Common
{
    public abstract class TaskProcessingServiceBase<TTaskService, TTask> : IProcessableService
        where TTaskService : ITaskService<TTask>
        where TTask : ITask
    {
        #region Private Members

        protected readonly ILog _log;
        protected readonly TTaskService _taskService;

        #endregion

        #region Abstract Properties

        public abstract string TaskDescriptor { get; }
        public abstract string Descriptor { get; }

        #endregion

        #region Constructors

        public TaskProcessingServiceBase(ILog log, TTaskService taskService)
        {
            _log = log;
            _taskService = taskService;
        }

        #endregion

        #region Private Methods

        protected virtual IEnumerable<TTask> GetTasks()
        {
            return _taskService.GetAllTasks();
        }

        #endregion

        #region Exposed Methods

        public void Process()
        {
            foreach (var task in GetTasks())
            {
                _log.Info($"Running {Descriptor} {TaskDescriptor} task {task.GetType().Name}");
                task.Run();
            }
        }

        #endregion
    }
}
