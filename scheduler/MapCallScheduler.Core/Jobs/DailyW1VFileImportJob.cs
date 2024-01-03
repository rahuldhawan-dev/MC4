using log4net;
using MapCallScheduler.JobHelpers.W1V;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    [Daily]
    public class DailyW1VFileImportJob
        : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<IDailyW1VFileImportService>
    {
        public DailyW1VFileImportJob(
            ILog log,
            IDailyW1VFileImportService service,
            IDeveloperEmailer emailer)
            : base(
                log,
                service,
                emailer) { }

        protected override string ExtraEmailSubject { get; }
    }
}
