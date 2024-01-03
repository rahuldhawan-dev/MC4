using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCall.Common.Configuration.NotificationTasks.HumanResources.Environmental
{
    public abstract class EnvironmentalPermitRenewalExpirationBase : MapCallNotifierTask<EnvironmentalPermit>
    {
        #region Constants

        public const RoleApplications APPLICATION = RoleApplications.Environmental;
        public const RoleModules MODULE = RoleModules.EnvironmentalPermitTypesExpiration;
        public const string TEMPLATE_NAME = "Environmental Permit Expiration";

        public const string EMAIL_SUBJECT_WITHOUT_EXPIRATION_FORMAT = "Environmental Permit Renewal Due in {0}";
        public const string EMAIL_SUBJECT_WITH_EXPIRATION_FORMAT = "Environmental Permit Renewal Due in {0}, Expiration Date: {1}";

        #endregion

        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRepository<NotificationPurpose> _notificationPurposeRepository;

        #endregion

        #region Constructors

        protected EnvironmentalPermitRenewalExpirationBase(
            IRepository<EnvironmentalPermit> repository, 
            INotifier notifier,
            INotificationService notificationService, 
            IDateTimeProvider dateTimeProvider, 
            IRepository<NotificationPurpose> notificationPurposeRepository,
            ILog log) 
            : base(repository, notifier, notificationService, log)
        {
            _dateTimeProvider = dateTimeProvider;
            _notificationPurposeRepository = notificationPurposeRepository;
        }

        #endregion

        #region Private Properties

        protected abstract string RenewalPhraseForEmailSubject { get; }

        #endregion

        #region Private Methods

        protected abstract DateTime GetDateOfConcern(DateTime now);

        private string GetNotificationEmailSubject(EnvironmentalPermit permit)
        {
            return (permit.PermitExpirationDate.HasValue)
                ? string.Format(EMAIL_SUBJECT_WITH_EXPIRATION_FORMAT, RenewalPhraseForEmailSubject, permit.PermitExpirationDate.Value.ToShortDateString())
                : string.Format(EMAIL_SUBJECT_WITHOUT_EXPIRATION_FORMAT, RenewalPhraseForEmailSubject);
        }

        #endregion

        #region Exposed Methods

        public override void SendNotification(EnvironmentalPermit entity)
        {
            var purposes = _notificationPurposeRepository.GetAll()
                                                         .Where(x => x.Module.Id == (int)MODULE && 
                                                                     x.Purpose == entity.EnvironmentalPermitType.Description)
                                                         .Select(x => x.Purpose);

            var notificationEmailSubject = GetNotificationEmailSubject(entity);

            foreach (var operatingCenter in entity.OperatingCenters)
            {
                foreach (var purpose in purposes)
                {
                    NotificationService.Notify(operatingCenter.Id, 
                        MODULE, 
                        purpose, 
                        entity,
                        notificationEmailSubject,
                        templateName: TEMPLATE_NAME);
                }
            }
        }

        public override IEnumerable<EnvironmentalPermit> GetData()
        {
            var date = GetDateOfConcern(_dateTimeProvider.GetCurrentDate()).BeginningOfDay();
            var nextDate = date.GetNextDay();

            var environmentalPermitsUpForRenewal = Repository.Where(p => p.EnvironmentalPermitStatus.Description == "Active" && 
                                                                         p.PermitRenewalDate.HasValue && 
                                                                         p.PermitRenewalDate >= date &&
                                                                         p.PermitRenewalDate < nextDate && 
                                                                         p.EnvironmentalPermitType.Id != EnvironmentalPermitType.Indices.MASTER_PERMIT)
                                                             .Select(p => new EnvironmentalPermit {
                                                                  RecordUrl = $"{BaseUrl}Environmental/EnvironmentalPermit/Show/{p.Id}",
                                                                  Description = p.Description,
                                                                  EnvironmentalPermitType = p.EnvironmentalPermitType,
                                                                  Facilities = p.Facilities,
                                                                  Id = p.Id,
                                                                  OperatingCenters = p.OperatingCenters,
                                                                  PermitName = p.PermitName,
                                                                  PermitNumber = p.PermitNumber,
                                                                  PermitExpirationDate = p.PermitExpirationDate,
                                                                  PermitRenewalDate = p.PermitRenewalDate,
                                                                  PublicWaterSupply = p.PublicWaterSupply
                                                              })
                                                             .ToList();

            return environmentalPermitsUpForRenewal;
        }

        #endregion
    }
}
