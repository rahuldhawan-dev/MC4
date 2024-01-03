using log4net;
using MapCallScheduler.JobHelpers.GIS.ImportTasks;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.FileImports;

namespace MapCallScheduler.JobHelpers.GIS
{
    public class DailyGISFileImportService : FileImportServiceBase<IGISFileImportTaskService, IDailyGISFileImportTask>,
        IDailyGISFileImportService
    {
        #region Properties

        public override string Descriptor { get; }

        #endregion

        #region Constructors

        public DailyGISFileImportService(ILog log, IGISFileImportTaskService taskService) : base(log, taskService) { }

        #endregion
    }

    public interface IDailyGISFileImportService : IProcessableService {}
}
