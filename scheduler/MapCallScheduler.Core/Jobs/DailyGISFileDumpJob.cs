using log4net;
using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    [Daily, StartAt(17, 30)]
    public class DailyGISFileDumpJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<IDailyGISFileDumpService>
    {
        #region Properties

        protected override string ExtraEmailSubject { get; }

        #endregion

        #region Constructors

        public DailyGISFileDumpJob(ILog log, IDailyGISFileDumpService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }

        #endregion
    }
}
