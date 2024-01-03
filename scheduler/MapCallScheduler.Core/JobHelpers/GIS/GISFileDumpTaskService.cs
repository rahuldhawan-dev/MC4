using System.Collections.Generic;
using System.Linq;
using MapCallScheduler.JobHelpers.GIS.DumpTasks;
using MapCallScheduler.Library.Common;
using StructureMap;

namespace MapCallScheduler.JobHelpers.GIS
{
    public class GISFileDumpTaskService : TaskServiceBase<IDailyGISFileDumpTask>, IGISFileDumpTaskService
    {
        #region Constructors

        public GISFileDumpTaskService(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<IDailyGISFileDumpTask> GetAllDailyTasks()
        {
            return GetAllTaskTypes()
               .Select(InstantiateTask<IDailyGISFileDumpTask>);
        }

        #endregion
    }

    public interface IGISFileDumpTaskService : ITaskService<IDailyGISFileDumpTask>
    {
        #region Abstract Methods

        IEnumerable<IDailyGISFileDumpTask> GetAllDailyTasks();

        #endregion
    }
}
