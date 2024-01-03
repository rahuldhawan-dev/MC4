using log4net;
using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    [Daily, StartAt(5, 30)]
    public class DailyGISFileImportJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<IDailyGISFileImportService>
    {
        protected override string ExtraEmailSubject { get; }

        public DailyGISFileImportJob(ILog log, IDailyGISFileImportService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }
    }
}
