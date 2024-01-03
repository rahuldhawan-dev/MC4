using log4net;
using MapCallScheduler.JobHelpers.LeakAlert;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    [Hourly, Immediate]
    public class LeakAlertUpdaterJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<ILeakAlertService>
    {
        #region Constants

        public const string ERROR_MESSAGE = "Error updating Leak Alert";

        #endregion

        #region Properties

        protected override string ExtraEmailSubject => ERROR_MESSAGE;

        #endregion

        #region Constructors

        public LeakAlertUpdaterJob(ILog log, ILeakAlertService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }

        #endregion
    }
}
