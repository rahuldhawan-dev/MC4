using System.Collections.Generic;
using System.Linq;
using MapCallScheduler.JobHelpers.GIS.ImportTasks;
using MapCallScheduler.Library.Common;
using StructureMap;

namespace MapCallScheduler.JobHelpers.GIS
{
    public class GISFileImportTaskService : TaskServiceBase<IDailyGISFileImportTask>, IGISFileImportTaskService
    {
        #region Constructors

        public GISFileImportTaskService(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<IDailyGISFileImportTask> GetAllDailyTasks()
        {
            return GetAllTaskTypes().Select(InstantiateTask<IDailyGISFileImportTask>);
        }

        #endregion
    }

    public interface IGISFileImportTaskService : ITaskService<IDailyGISFileImportTask>
    {
        #region Abstract Methods

        IEnumerable<IDailyGISFileImportTask> GetAllDailyTasks();

        #endregion
    }
}
