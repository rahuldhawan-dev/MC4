using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCall.Common.Configuration.NotificationTasks.Production
{
    public class SystemDeliveryEntryValidated : MapCallNotifierTask<SystemDeliveryEntry>
    {
        #region Constants

        public const RoleApplications APPLICATION = RoleApplications.Production;
        public const RoleModules MODULE = RoleModules.ProductionSystemDeliveryApprover;
        public const string PURPOSE = "System Delivery Entry Validation";

        #endregion

        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Contructor

        public SystemDeliveryEntryValidated(IRepository<SystemDeliveryEntry> repository, INotifier notifier, INotificationService notificationService, ILog log, IDateTimeProvider dateTimeProvider) : base(repository, notifier, notificationService, log)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<SystemDeliveryEntry> GetData()
        {
            // Only send on Mondays
            if (_dateTimeProvider.GetCurrentDate().DayOfWeek == DayOfWeek.Monday)
            {
                var lastMonday = _dateTimeProvider.GetCurrentDate().AddWeeks(-1).Date;

                var results = Repository.Where(x => x.IsValidated == true && x.WeekOf == lastMonday);

                return results;
            }

            return Enumerable.Empty<SystemDeliveryEntry>();
        }

        public override void SendNotification(SystemDeliveryEntry entity)
        {
            var operatingCenters = entity.FacilityEntries.Select(x => x.Facility.OperatingCenter); // Grabbing operating centers that have entries

            foreach (var operatingCenter in operatingCenters)
            {
                var model = new SystemDeliveryEntryNotification {
                    Entity = entity,
                    OperatingCenter = operatingCenter,
                    RecordUrl = $"{BaseUrl}Production/SystemDeliveryEntry/Show/{entity.Id}"
                };

                NotificationService.Notify(operatingCenter.Id, MODULE, PURPOSE, model);
            }
        }

        #endregion
    }
}
