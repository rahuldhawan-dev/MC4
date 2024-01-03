using log4net;
using MapCallScheduler.JobHelpers.LIMSSynchronization;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs.LIMSSynchronization
{
    [Daily(1), Immediate]
    public class SampleSiteProfileSyncJob : MapCallJobWithProcessableServiceBase<ISampleSiteProfileSyncService>
    {
        public SampleSiteProfileSyncJob(ILog log, ISampleSiteProfileSyncService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }
    }
}
