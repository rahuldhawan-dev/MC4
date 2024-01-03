using Humanizer;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;

namespace MapCall.Common.Configuration.NotificationTasks.HumanResources.Employee
{
    public abstract class
        MedicalCertificateExpirationBase : MapCallNotifierTask<IEmployeeRepository, Model.Entities.Employee>
    {
        #region Constants

        public const RoleApplications APPLICATION = RoleApplications.HumanResources;
        public const RoleModules MODULE = RoleModules.HumanResourcesEmployee;
        public const string PURPOSE = "Medical Certificate Expiration";

        #endregion

        #region Constructors

        protected MedicalCertificateExpirationBase(IEmployeeRepository repository, INotifier notifier,
            INotificationService notificationService, ILog log) :
            base(repository, notifier, notificationService, log) { }

        #endregion

        #region Exposed Methods

        public override void SendNotification(Model.Entities.Employee entity)
        {
            var purpose = GetType().Name.Humanize(LetterCasing.Title);
            Notifier.Notify(APPLICATION, MODULE, PURPOSE, entity, entity.EmailAddress, purpose);
            Notifier.Notify(APPLICATION, MODULE, PURPOSE, entity, entity.ReportsTo.EmailAddress,
                string.Format("{0} - {1} {2}, {3}", purpose, entity.EmployeeId, entity.LastName, entity.FirstName));
        }

        #endregion
    }
}
