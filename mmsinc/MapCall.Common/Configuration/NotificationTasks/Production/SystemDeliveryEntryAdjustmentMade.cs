using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using MMSINC.Utilities;
using log4net;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;
using MMSINC.ClassExtensions.DateTimeExtensions;

namespace MapCall.Common.Configuration.NotificationTasks.Production
{
    public class SystemDeliveryEntryAdjustmentMade : MapCallNotifierTask<SystemDeliveryFacilityEntryAdjustment>
    {
        #region Constants

        public const RoleApplications APPLICATION = RoleApplications.Production;
        public const RoleModules MODULE = RoleModules.ProductionSystemDeliveryEntry;
        public const string PURPOSE = "System Delivery Entry Adjustment";

        #endregion

        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructor

        public SystemDeliveryEntryAdjustmentMade(IRepository<SystemDeliveryFacilityEntryAdjustment> repository, INotifier notifier, INotificationService notificationService, ILog log, IDateTimeProvider dateTimeProvider) : base(repository, notifier, notificationService, log)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<SystemDeliveryFacilityEntryAdjustment> GetData()
        {
            // Only send on Mondays
            if (_dateTimeProvider.GetCurrentDate().DayOfWeek == DayOfWeek.Monday)
            {
                var now = _dateTimeProvider.GetCurrentDate();
                var lastMonday = _dateTimeProvider.GetCurrentDate().AddWeeks(-1).GetDayFromWeek(DayOfWeek.Monday);

                var results = Repository.Where(x => x.DateTimeEntered.Date < now && x.DateTimeEntered.Date >= lastMonday);

                return results;
            }

            return Enumerable.Empty<SystemDeliveryFacilityEntryAdjustment>();
        }

        public override void SendNotification(SystemDeliveryFacilityEntryAdjustment entity)
        {
            var purpose = GetType().Name.Humanize(LetterCasing.Title);
            var operatingCenter = entity.SystemDeliveryFacilityEntry.Facility.OperatingCenter;

            NotificationService.Notify(operatingCenter.Id, MODULE, PURPOSE, entity);
        }

        #endregion
    }
}
