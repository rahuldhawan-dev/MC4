using System.Collections.Generic;
using System.Linq;
using MapCallScheduler.Library.Common;
using StructureMap;

namespace MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks
{
    public class SystemDeliveryEntryFileDumpTaskService : TaskServiceBase<ISystemDeliveryEntryFileDumpTask>, ISystemDeliveryEntryFileDumpTaskService
    {
        #region Constructor

        public SystemDeliveryEntryFileDumpTaskService(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<ISystemDeliveryEntryFileDumpTask> GetAllDailyTasks()
        {
            return GetAllTaskTypes().Select(InstantiateTask<ISystemDeliveryEntryFileDumpTask>);
        }

        #endregion
    }

    public interface ISystemDeliveryEntryFileDumpTaskService : ITaskService<ISystemDeliveryEntryFileDumpTask>
    {
        #region Abstract Methods

        IEnumerable<ISystemDeliveryEntryFileDumpTask> GetAllDailyTasks();

        #endregion
    }
}