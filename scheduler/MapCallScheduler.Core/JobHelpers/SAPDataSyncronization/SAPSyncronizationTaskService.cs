using MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks;
using MapCallScheduler.Library.Common;
using StructureMap;

namespace MapCallScheduler.JobHelpers.SAPDataSyncronization
{
    public class SAPSyncronizationTaskService : TaskServiceBase<SAPSyncronizationTaskBase>, ISAPSyncronizationTaskService
    {
        #region Constructors

        public SAPSyncronizationTaskService(IContainer container) : base(container) { }

        #endregion
    }
}