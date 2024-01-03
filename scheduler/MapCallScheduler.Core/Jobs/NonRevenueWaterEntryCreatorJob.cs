using log4net;
using MapCallScheduler.JobHelpers.NonRevenueWaterEntryCreator;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    [Daily]
    [StartAt(6, 30)]
    public class
        NonRevenueWaterEntryCreatorJob : MapCallJobWithProcessableServiceBase<INonRevenueWaterEntryCreatorService>
    {
        public NonRevenueWaterEntryCreatorJob(ILog log, INonRevenueWaterEntryCreatorService service,
            IDeveloperEmailer emailer) : base(log, service, emailer) { }
    }
}