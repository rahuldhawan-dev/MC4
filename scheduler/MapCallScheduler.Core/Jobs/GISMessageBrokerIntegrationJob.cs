using log4net;
using MapCallScheduler.JobHelpers.GISMessageBroker;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    [Minutely(5)]
    public class GISMessageBrokerIntegrationJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<IGISMessageBrokerService>
    {
        #region Properties

        protected override string ExtraEmailSubject { get; }

        #endregion

        #region Constructors

        public GISMessageBrokerIntegrationJob(ILog log, IGISMessageBrokerService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }

        #endregion
    }
}
