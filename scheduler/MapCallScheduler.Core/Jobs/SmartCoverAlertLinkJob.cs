using log4net;
using MapCallScheduler.JobHelpers.SmartCoverAlert;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    /* This job gets insert/update alerts and related date by communicating with smart cover alert api */
    [Daily, Immediate]
    public class SmartCoverAlertLinkJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<ISmartCoverAlertLinkService>
    {
        #region Constants

        public const string ERROR_MESSAGE = "Error populating Smart Cover Alerts";

        #endregion

        #region Properties

        protected override string ExtraEmailSubject => ERROR_MESSAGE;

        #endregion

        #region Constructors

        public SmartCoverAlertLinkJob(ILog log, ISmartCoverAlertLinkService service, IDeveloperEmailer emailer) : base(
            log, service, emailer) { }

        #endregion
    }
}
