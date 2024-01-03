﻿using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;

namespace MapCall.Common.Configuration.NotificationTasks.HumanResources.Employee
{
    public class MedicalCertificateDueInTwoMonths : MedicalCertificateExpirationBase
    {
        #region Constructors

        public MedicalCertificateDueInTwoMonths(IEmployeeRepository repository, INotifier notifier,
            INotificationService notificationService, ILog log) :
            base(repository, notifier, notificationService, log) { }

        #endregion

        #region Exposed Methods

        public override IEnumerable<Model.Entities.Employee> GetData()
        {
            return Repository.GetEmployeesWithMedicalCertificatesDueInTwoMonths();
        }

        #endregion
    }
}
