using log4net;
using MapCallScheduler.JobHelpers.SapMaterial;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using Quartz;

namespace MapCallScheduler.Jobs
{
    [Hourly, Immediate]
    public class SapMaterialUpdaterJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<ISapMaterialService>
    {
        #region Constants

        public const string ERROR_MESSAGE = "Error updating MapCall material from SAP";

        #endregion

        #region Properties

        protected override string ExtraEmailSubject => ERROR_MESSAGE;

        #endregion

        #region Constructors

        public SapMaterialUpdaterJob(ILog log, ISapMaterialService service, IDeveloperEmailer emailer) : base(log, service, emailer) {}

        #endregion
    }
}