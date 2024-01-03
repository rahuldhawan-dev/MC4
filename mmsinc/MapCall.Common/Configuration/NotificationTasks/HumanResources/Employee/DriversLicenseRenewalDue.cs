using System;
using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;

namespace MapCall.Common.Configuration.NotificationTasks.HumanResources.Employee
{
    public class DriversLicenseRenewalDue : MapCallNotifierTask<IEmployeeRepository, Model.Entities.Employee>
    {
        #region Constants

        public const RoleApplications APPLICATION = RoleApplications.HumanResources;
        public const RoleModules MODULE = RoleModules.HumanResourcesEmployee;
        public const string PURPOSE = "Drivers License Renewal Due";

        #endregion

        #region Constructors

        public DriversLicenseRenewalDue(IEmployeeRepository repository, INotifier notifier,
            INotificationService notificationService, ILog log) :
            base(repository, notifier, notificationService, log) { }

        #endregion

        #region Exposed Methods

        public override IEnumerable<Model.Entities.Employee> GetData()
        {
            return Repository.GetEmployeesWithCommercialDriversLicenseRenewalsDueInTwoMonths();
        }

        public override void SendNotification(Model.Entities.Employee entity)
        {
            var supervisorSubject = String.Format("{0} - {1} {2}, {3}", PURPOSE, entity.EmployeeId, entity.LastName,
                entity.FirstName);
            Notifier.Notify(APPLICATION, MODULE, PURPOSE, entity, entity.EmailAddress,
                "Your Drivers License Expires in 2 Months");
            Notifier.Notify(APPLICATION, MODULE, PURPOSE, entity, entity.ReportsTo.EmailAddress, supervisorSubject);
            NotificationService.Notify(entity.OperatingCenter.Id, MODULE, PURPOSE, entity, supervisorSubject);
        }

        #endregion
    }
}
