using System.Collections.Generic;
using log4net;
using MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks;

namespace MapCallScheduler.JobHelpers.SAPDataSyncronization
{
    /// <summary>
    /// This will run any tasks that are SAPSyncronizationTaskBase 
    /// </summary>
    public class SAPSyncronizationService : SAPSyncronizationServiceBase<ISAPSyncronizationTask>, ISAPSyncronizationService
    {
        #region Constructors

        public SAPSyncronizationService(ISAPSyncronizationTaskService taskService, ILog log) : base(taskService, log) { }

        #endregion

        #region Private Methods

        protected override IEnumerable<ISAPSyncronizationTask> GetTasks()
        {
            return _taskService.GetAllTasks();
        }

        #endregion
    }
}