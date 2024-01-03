using log4net;
using MapCallScheduler.JobHelpers.SAPDataSyncronization;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using Quartz;

namespace MapCallScheduler.Jobs
{
    [Minutely(4), Immediate]
    public class SAPEquipmentSyncronizationJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<ISAPSyncronizationService>
    {
        #region Constants

        public const string ERROR_MESSAGE = "Error processing retry errors for SAP";

        #endregion

        #region Properties

        protected override string ExtraEmailSubject => ERROR_MESSAGE;
        
        #endregion

        #region Constructors

        public SAPEquipmentSyncronizationJob(ILog log, ISAPSyncronizationService service, IDeveloperEmailer emailer) : base(log, service, emailer) {}

        #endregion
    }
}