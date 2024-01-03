using log4net;
using MapCallScheduler.JobHelpers.GIS.DumpTasks;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.FileDumps;

namespace MapCallScheduler.JobHelpers.GIS
{
    public class DailyGISFileDumpService : FileDumpServiceBase<IGISFileDumpTaskService, IDailyGISFileDumpTask>, IDailyGISFileDumpService
    {
        #region Properties

        public override string Descriptor { get; }

        #endregion

        #region Constructors

        public DailyGISFileDumpService(ILog log, IGISFileDumpTaskService taskService) : base(log, taskService) { }

        #endregion
    }

    public interface IDailyGISFileDumpService : IProcessableService {}
}
