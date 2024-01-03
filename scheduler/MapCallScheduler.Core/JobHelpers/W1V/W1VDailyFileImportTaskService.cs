using MapCallScheduler.JobHelpers.W1V.ImportTasks;
using MapCallScheduler.Library.Common;
using StructureMap;

namespace MapCallScheduler.JobHelpers.W1V
{
    public class W1VDailyFileImportTaskService
        : TaskServiceBase<IDailyW1VFileImportTask>, IW1VDailyFileImportTaskService
    {
        #region Constructors

        public W1VDailyFileImportTaskService(IContainer container) : base(container) { }

        #endregion
    }

    public interface IW1VDailyFileImportTaskService : ITaskService<IDailyW1VFileImportTask> {}
}
