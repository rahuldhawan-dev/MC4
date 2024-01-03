using System;
using log4net;
using MapCallScheduler.JobHelpers;
using MapCallScheduler.Library.Email;
using Quartz;

namespace MapCallScheduler.Jobs
{
    public class MapCallNotifierJob : MapCallJobBase
    {
        #region Private Members

        protected readonly INotifierTaskService _taskService;

        #endregion

        #region Properties

        protected INotifierTaskService TaskService => _taskService;

        #endregion

        #region Constructors

        public MapCallNotifierJob(ILog log, IDeveloperEmailer emailer, INotifierTaskService taskService) : base(log, emailer)
        {
            _taskService = taskService;
        }

        #endregion

        #region Private Methods

        protected override void ExecuteJob(IJobExecutionContext context)
        {
            foreach (var task in TaskService.GetAllTasks())
            {
                var taskName = task.GetType().Name;

                try
                {
                    Log.InfoFormat("Running {0} task...", taskName);
                    task.Run();
                }
                catch (Exception e)
                {
                    Log.ErrorFormat("Exception encountered running {0} task: {1}", taskName, e);
                    Emailer.SendErrorMessage("MapCallScheduler: Error in MapCallNotifierJob", e);
                }
            }
        }

        #endregion
    }
}