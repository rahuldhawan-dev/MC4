using log4net;
using MapCallScheduler.JobHelpers.SapEmployee;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using Quartz;

namespace MapCallScheduler.Jobs
{
    [Hourly, Immediate]
    public class SapEmployeeUpdaterJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<ISapEmployeeService>
    {
        #region Constants

        public const string ERROR_MESSAGE = "Error updating MapCall employee from SAP";

        #endregion

        #region Properties

        protected override string ExtraEmailSubject => ERROR_MESSAGE;

        #endregion

        #region Constructors

        public SapEmployeeUpdaterJob(ILog log, ISapEmployeeService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }

        #endregion
    }
}