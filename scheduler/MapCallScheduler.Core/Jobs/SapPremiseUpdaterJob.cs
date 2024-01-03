using log4net;
using MapCallScheduler.JobHelpers.SapPremise;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using Quartz;

namespace MapCallScheduler.Jobs
{
    [Minutely(5), Immediate]
    public class SapPremiseUpdaterJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<ISapPremiseService>
    {
        #region Constants

        public const string ERROR_MESSAGE = "Error updating MapCall premise from SAP";

        #endregion

        #region Properties

        protected override string ExtraEmailSubject => ERROR_MESSAGE;

        #endregion

        #region Constructors

        public SapPremiseUpdaterJob(ILog log, ISapPremiseService service, IDeveloperEmailer emailer) : base(log, service, emailer) {}

        #endregion
    }
}