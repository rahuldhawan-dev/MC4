using log4net;
using MapCallScheduler.JobHelpers.ServicePremiseLink;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.Jobs
{
    // This job links services and short cycle work orders with premises 
    [Daily, StartAt(3)]
    public class ServicePremiseLinkJob : MapCallJobWithProcessableServiceAndAwwErrorEmailBase<IServicePremiseLinkService>
    {
        #region Constants

        public const string ERROR_MESSAGE = "Error linking premise to service record";

        #endregion

        #region Properties

        protected override string ExtraEmailSubject => ERROR_MESSAGE;

        #endregion

        #region Constructors

        public ServicePremiseLinkJob(ILog log, IServicePremiseLinkService service, IDeveloperEmailer emailer) : base(log, service, emailer) { }

        #endregion
    }
}
