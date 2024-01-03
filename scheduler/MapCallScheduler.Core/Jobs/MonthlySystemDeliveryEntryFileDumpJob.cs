using log4net;
using MapCallScheduler.JobHelpers.SystemDeliveryEntry;
using MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Utilities;

namespace MapCallScheduler.Jobs
{
    /// <summary>
    /// In order to run this job locally, you'll have to comment out the condition in
    /// the DoProcess method for the date check. To the best of my knowledge this is the
    /// only place the day is involved. If you're running it for previous months, then
    /// you will have to do further investigating. Once you have commented that you can
    /// change the app.config for the console app to point to the mapcalldev db in nonprod.
    /// If that generates the file as desired, you can set the config to the production db.
    /// </summary>
    [Daily, StartAt(23)]
    public class MonthlySystemDeliveryEntryFileDumpJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<IMonthlySystemDeliveryEntryFileDumpService>
    {
        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        protected override string ExtraEmailSubject { get; }

        #endregion

        #region Constructors

        public MonthlySystemDeliveryEntryFileDumpJob(ILog log, IMonthlySystemDeliveryEntryFileDumpService service, IDeveloperEmailer emailer, IDateTimeProvider dateTimeProvider) : base(log, service, emailer)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Private Methods

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

        #endregion
    }
}
