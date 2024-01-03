using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Humanizer;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCall.Common.Configuration.NotificationTasks.HumanResources.Facilities
{
    public class ArcFlashStudyExpiresIn1Year : MapCallNotifierTask<ArcFlashStudy>
    {
        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constants

        public const RoleApplications APPLICATION = RoleApplications.HumanResources;
        public const RoleModules MODULE = RoleModules.ProductionFacilities;
        public const string PURPOSE = "Arc Flash Study Expires In 1 Year";

        #endregion

        #region Constructor

        public ArcFlashStudyExpiresIn1Year(IRepository<ArcFlashStudy> repository, INotifier notifier,
            INotificationService notificationService, IDateTimeProvider dateTimeProvider, ILog log) : base(repository,
            notifier, notificationService, log)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        public override void SendNotification(ArcFlashStudy entity)
        {
            var purpose = GetType().Name.Humanize(LetterCasing.Title);

            NotificationService.Notify(entity.Facility.OperatingCenter.Id, MODULE, PURPOSE, entity, purpose);
        }

        // This will grab all Arc Flash Studies that are 1 year from expiring. Studies expire every 5 years, we send the notification on the 4th year.
        public override IEnumerable<ArcFlashStudy> GetData()
        {
            var date = _dateTimeProvider.GetCurrentDate().BeginningOfDay().AddYears(-4);
            var endDate = date.AddDays(1);

            return Repository.Where(x => x.DateLabelsApplied.HasValue &&
                                         x.DateLabelsApplied >= date &&
                                         x.DateLabelsApplied < endDate)
                             .Select(x => new ArcFlashStudy {
                                  RecordUrl = $"{BaseUrl}Engineering/ArcFlashStudy/Show/{x.Id}",
                                  Id = x.Id
                              });
        }

        #endregion
    }
}
