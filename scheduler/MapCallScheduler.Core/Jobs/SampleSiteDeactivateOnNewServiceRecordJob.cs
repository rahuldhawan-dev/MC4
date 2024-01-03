using log4net;
using MapCallScheduler.JobHelpers.SampleSiteDeactivation;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    [Daily]
    public class SampleSiteDeactivateOnNewServiceRecordJob : MapCallJobWithProcessableServiceBase<ISampleSiteDeactivationProcessorService>
    {
        public SampleSiteDeactivateOnNewServiceRecordJob(ILog log, ISampleSiteDeactivationProcessorService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }
    }
}
