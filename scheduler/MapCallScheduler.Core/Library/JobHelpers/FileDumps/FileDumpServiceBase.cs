using log4net;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.Common;

namespace MapCallScheduler.Library.JobHelpers.FileDumps
{
    public abstract class FileDumpServiceBase<TTaskService, TTask> : TaskProcessingServiceBase<TTaskService, TTask>, IFileDumpService
        where TTaskService : ITaskService<TTask>
        where TTask : IFileDumpTask
    {
        #region Properties

        public override string TaskDescriptor => "file dump";

        #endregion

        #region Constructors

        public FileDumpServiceBase(ILog log, TTaskService taskService) : base(log, taskService) { }

        #endregion
    }

    public interface IFileDumpService : IProcessableService {}
}
