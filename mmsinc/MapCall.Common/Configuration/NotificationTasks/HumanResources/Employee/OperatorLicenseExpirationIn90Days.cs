using log4net;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MMSINC.Utilities;

namespace MapCall.Common.Configuration.NotificationTasks.HumanResources.Employee
{
    public class OperatorLicenseExpirationIn90Days : OperatorLicenseExpirationBase
    {
        #region Constructors

        public OperatorLicenseExpirationIn90Days(
            IOperatorLicenseRepository operatorLicenseRepository,
            INotificationConfigurationRepository notificationConfigurationRepository,
            INotifier notifier,
            INotificationService notificationService,
            ILog log,
            IDateTimeProvider dateTimeProvider)
            : base(
                operatorLicenseRepository,
                notificationConfigurationRepository,
                notifier,
                notificationService,
                log,
                dateTimeProvider) { }

        #endregion

        #region Private Properties

        protected override int TargetExpirationInDays => 90;

        #endregion
    }
}
