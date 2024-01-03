using log4net;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Utilities;
using Quartz;

namespace MapCallScheduler.Jobs
{
    [Daily]
    public class MonthlySpaceTimeInsightFileDumpJob : MapCallJobWithProcessableServiceBase<ISpaceTimeInsightMonthlyFileDumpService>
    {
        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public MonthlySpaceTimeInsightFileDumpJob(ILog log, ISpaceTimeInsightMonthlyFileDumpService service,
            IDeveloperEmailer emailer, IDateTimeProvider dateTimeProvider) : base(log, service, emailer)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Private Methods

        protected override void DoProcess()
        {
            var today = _dateTimeProvider.GetCurrentDate().Date;
            if (today == today.GetBeginningOfMonth())
            {
                base.DoProcess();
            }
        }

        #endregion
    }
}