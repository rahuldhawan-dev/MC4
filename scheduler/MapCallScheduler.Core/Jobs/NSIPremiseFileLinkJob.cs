using log4net;
using MapCallScheduler.JobHelpers.NSIPremiseFileLink;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    [Minutely(15), Immediate]
    public class NSIPremiseFileLinkJob : MapCallJobWithProcessableServiceBase<INSIPremiseFileLinkService>
    {
        public NSIPremiseFileLinkJob(ILog log, INSIPremiseFileLinkService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }
    }
}
