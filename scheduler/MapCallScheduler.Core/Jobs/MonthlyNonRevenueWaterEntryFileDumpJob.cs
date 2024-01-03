using log4net;
using MapCallScheduler.JobHelpers.NonRevenueWater;
using MapCallScheduler.JobHelpers.SystemDeliveryEntry;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Utilities;

namespace MapCallScheduler.Jobs
{
    [Daily, StartAt	(23)]
    public class MonthlyNonRevenueWaterEntryFileDumpJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<
        IMonthlyNonRevenueWaterEntryFileDumpService>
    {
        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        protected override string ExtraEmailSubject { get; }

        #endregion

        #region Constructors

        public MonthlyNonRevenueWaterEntryFileDumpJob(ILog log, IMonthlyNonRevenueWaterEntryFileDumpService service,
            IDeveloperEmailer emailer, IDateTimeProvider dateTimeProvider) : base(log, service, emailer)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        protected override void DoProcess()
        {
            var today = _dateTimeProvider.GetCurrentDate().Date;
            var doesMonthFollowQuarterEndMonth = today.DoesMonthFollowQuarterEndMonth();
            
            if ((doesMonthFollowQuarterEndMonth && SystemDeliveryWorkDayEngine.IsSecondBusinessDay(today)) ||
                (!doesMonthFollowQuarterEndMonth && SystemDeliveryWorkDayEngine.IsFourthBusinessDay(today)))
            {
                base.DoProcess();
            }
        }
    }
}
