using log4net;
using MapCallScheduler.JobHelpers.SapWaterQualityComplaint;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    // We want to start as close to midnight so that we do not miss the current day's notifications
    [StartAt(23, 58), Minutely(30)]
    public class SapWaterQualityComplaintJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<ISapWaterQualityComplaintService>
    {
        #region Constants

        public const string ERROR_MESSAGE = "Error updating MapCall Water Quality Complaints from SAP";

        #endregion

        #region Properties

        protected override string ExtraEmailSubject => ERROR_MESSAGE;

        #endregion

        #region Constructors

        public SapWaterQualityComplaintJob(ILog log, ISapWaterQualityComplaintService service, IDeveloperEmailer emailer) : base(log, service, emailer) {}

        #endregion
    }
}