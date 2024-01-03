using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Utilities;

namespace MapCall.Common.Configuration.NotificationTasks.HumanResources.Employee
{
    public abstract class OperatorLicenseExpirationBase : MapCallNotifierTask<IOperatorLicenseRepository, OperatorLicense>
    {
        #region Constants

        public const RoleApplications APPLICATION = RoleApplications.HumanResources;
        public const RoleModules MODULE = RoleModules.HumanResourcesEmployee;
        public const string PURPOSE = "Operator License Expiration";

        #endregion

        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly INotificationConfigurationRepository _notificationConfigurationRepository;

        #endregion

        #region Constructors

        protected OperatorLicenseExpirationBase(
            IOperatorLicenseRepository operatorLicenseRepository,
            INotificationConfigurationRepository notificationConfigurationRepository,
            INotifier notifier,
            INotificationService notificationService,
            ILog log,
            IDateTimeProvider dateTimeProvider) : base(operatorLicenseRepository, notifier, notificationService, log)
        {
            _dateTimeProvider = dateTimeProvider;
            _notificationConfigurationRepository = notificationConfigurationRepository;
        }

        #endregion

        #region Private Properties

        protected abstract int TargetExpirationInDays { get; }

        #endregion

        #region Private Methods

        private DateTime GetDateOfConcern() => _dateTimeProvider.GetCurrentDate().AddDays(TargetExpirationInDays);

        #endregion

        #region Exposed Methods

        public override void SendNotification(OperatorLicense entity)
        {
            var subjectLine = $"Licensed Operator Renewal Due in {TargetExpirationInDays} Days, Expiration Date: {GetDateOfConcern()}";

            Notifier.Notify(APPLICATION, MODULE, PURPOSE, entity, entity.Employee.EmailAddress, subjectLine);

            var notificationConfigurations = _notificationConfigurationRepository.FindByOperatingCenterModuleAndPurpose(entity.OperatingCenter.Id, MODULE, PURPOSE);

            foreach (var notificationConfiguration in notificationConfigurations)
            {
                Notifier.Notify(APPLICATION, MODULE, PURPOSE, entity, notificationConfiguration.Contact.Email, subjectLine);
            }
        }

        public override IEnumerable<OperatorLicense> GetData()
        {
            var targetExpirationDate = GetDateOfConcern().BeginningOfDay();

            return Repository.Where(x => x.Employee.Status.Id == EmployeeStatus.Indices.ACTIVE &&
                                         x.ExpirationDate == targetExpirationDate)
                             .ToList()
                             .Select(x => {
                                  x.RecordUrl = $"{BaseUrl}OperatorLicense/Show/{x.Id}";
                                  return x;
                              });
        }

        #endregion
    }
}
