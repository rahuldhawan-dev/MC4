using System;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCall.Common.Configuration.NotificationTasks.HumanResources.Environmental
{
    public class EnvironmentalPermitRenewalExpiresIn6Months : EnvironmentalPermitRenewalExpirationBase
    {
        #region Constants

        public const string RENEWAL_TEXT = "6 Months";

        #endregion

        #region Constructors

        public EnvironmentalPermitRenewalExpiresIn6Months(
            IRepository<EnvironmentalPermit> repository,
            IRepository<NotificationPurpose> notificationPurposesRepository,
            INotifier notifier, 
            INotificationService notificationService, 
            IDateTimeProvider dateTimeProvider, 
            ILog log)
            : base(repository, notifier, notificationService, dateTimeProvider, notificationPurposesRepository, log) { }

        #endregion

        #region Private Properties

        protected override string RenewalPhraseForEmailSubject => RENEWAL_TEXT;

        #endregion

        #region Private Methods

        protected override DateTime GetDateOfConcern(DateTime now)
        {
            return now.AddWeeks(24);
        }

        #endregion
    }
}
