using log4net;
using MapCallScheduler.JobHelpers.MeterChangeOutStatusUpdate;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    [Daily]
    public class MeterChangeOutStatusUpdateJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<IMeterChangeOutStatusUpdateService>
    {
        #region Constants

        public const string ERROR_MESSAGE = "Error updating Meter Change Out status";

        #endregion

        #region Properties

        protected override string ExtraEmailSubject => ERROR_MESSAGE;

        #endregion

        #region Constructors

        public MeterChangeOutStatusUpdateJob(ILog log, IMeterChangeOutStatusUpdateService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }

        #endregion
    }
}
