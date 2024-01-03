using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.FileDumps;

namespace MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks
{
    public class MonthlySystemDeliveryEntryFileDumpService : FileDumpServiceBase<ISystemDeliveryEntryFileDumpTaskService, ISystemDeliveryEntryFileDumpTask>, IMonthlySystemDeliveryEntryFileDumpService
    {
        #region Exposed Methods

        public override string Descriptor { get; }

        protected override IEnumerable<ISystemDeliveryEntryFileDumpTask> GetTasks()
        {
            return _taskService.GetAllDailyTasks();
        }

        #endregion

        public MonthlySystemDeliveryEntryFileDumpService(ILog log, ISystemDeliveryEntryFileDumpTaskService taskService) : base(log, taskService) { }
    }

    public interface IMonthlySystemDeliveryEntryFileDumpService : IProcessableService {}
}
