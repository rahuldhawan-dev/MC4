using log4net;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.Common;

namespace MapCallScheduler.Library.JobHelpers.FileImports
{
    public abstract class FileImportServiceBase<TTaskService, TTask> : TaskProcessingServiceBase<TTaskService, TTask>, IFileImportService
        where TTaskService : ITaskService<TTask>
        where TTask : IFileImportTask
    {
        #region Properties

        public override string TaskDescriptor => "file import";

        #endregion

        #region Constructors

        public FileImportServiceBase(ILog log, TTaskService taskService) : base(log, taskService) { }

        #endregion
    }

    public interface IFileImportService : IProcessableService {}
}
