using log4net;
using MapCallScheduler.JobHelpers.W1V.ImportTasks;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.FileImports;

namespace MapCallScheduler.JobHelpers.W1V
{
    public class DailyW1VFileImportService
        : FileImportServiceBase<IW1VDailyFileImportTaskService, IDailyW1VFileImportTask>,
            IDailyW1VFileImportService
    {
        public DailyW1VFileImportService(ILog log, IW1VDailyFileImportTaskService taskService)
            : base(log, taskService) { }

        public override string Descriptor { get; }
    }
    
    public interface IDailyW1VFileImportService : IProcessableService {}
}
