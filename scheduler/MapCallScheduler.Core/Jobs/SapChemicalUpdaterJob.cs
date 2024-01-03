using log4net;
using MapCallScheduler.JobHelpers.SapChemical;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using Quartz;

namespace MapCallScheduler.Jobs
{
    [Hourly, Immediate]
    public class SapChemicalUpdaterJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<ISapChemicalService>
    {
        #region Constants

        public const string ERROR_MESSAGE = "Error updating MapCall Chemical from SAP";

        #endregion

        #region Properties

        protected override string ExtraEmailSubject => ERROR_MESSAGE;

        #endregion

        #region Constructors

        public SapChemicalUpdaterJob(ILog log, ISapChemicalService service, IDeveloperEmailer emailer) : base(log, service, emailer) {}

        #endregion
    }
}