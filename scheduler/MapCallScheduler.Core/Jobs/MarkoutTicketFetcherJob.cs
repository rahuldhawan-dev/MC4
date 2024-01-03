using log4net;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using Quartz;

namespace MapCallScheduler.Jobs
{
    [Minutely(30), Immediate]
    public class MarkoutTicketFetcherJob : MapCallJobWithProcessableServiceBase<IOneCallMessageService>
    {
        #region Private Members

        private readonly IOneCallMessageHeartbeatService _heartbeatService;

        #endregion

        #region Constructors

        public MarkoutTicketFetcherJob(ILog log, IOneCallMessageService service, IOneCallMessageHeartbeatService heartbeatService, IDeveloperEmailer emailer) : base(log, service, emailer)
        {
            _heartbeatService = heartbeatService;
        }

        #endregion

        #region Private Methods

        protected override void ExecuteJob(IJobExecutionContext context)
        {
            base.ExecuteJob(context);

            _heartbeatService.Process();
        }

        #endregion

        #region Exposed Methods

        public override string GetServiceName()
        {
            return "markout ticket message service";
        }

        #endregion
    }
}
