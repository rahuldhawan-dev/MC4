using MapCallScheduler.Library.JobHelpers.Sap;

namespace MapCallScheduler.JobHelpers.LeakAlert
{
    public class LeakAlertService : SapFileProcessingServiceBase<ILeakAlertFileService, ILeakAlertUpdaterService>, ILeakAlertService
    {
        public LeakAlertService(ILeakAlertFileService fileService, ILeakAlertUpdaterService updaterService) : base(fileService, updaterService) { }
    }
}
