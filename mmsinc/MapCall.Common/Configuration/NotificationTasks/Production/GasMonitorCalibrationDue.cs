using System.Collections.Generic;
using Humanizer;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCall.Common.Configuration.NotificationTasks.Production
{
    public class GasMonitorCalibrationDue : MapCallNotifierTask<GasMonitor>
    {
        #region Constants

        public const RoleApplications APPLICATION = RoleApplications.Production;
        public const RoleModules MODULE = RoleModules.ProductionEquipment;
        public const string PURPOSE = "Gas Monitor Calibration Due";

        #endregion

        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public GasMonitorCalibrationDue(IRepository<GasMonitor> repository, INotifier notifier,
            INotificationService notificationService, ILog log, IDateTimeProvider dateTimeProvider) : base(repository,
            notifier, notificationService, log)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<GasMonitor> GetData()
        {
            return Repository.GetWithCalibrationDueSevenDaysFrom(_dateTimeProvider.GetCurrentDate());
        }

        public override void SendNotification(GasMonitor entity)
        {
            var purpose = GetType().Name.Humanize(LetterCasing.Title);

            if (entity.AssignedEmployee != null && !string.IsNullOrWhiteSpace(entity.AssignedEmployee.EmailAddress))
            {
                // SET THE RECORD URL
                entity.RecordUrl = $"{BaseUrl}HealthAndSafety/GasMonitor/Show/{entity.Id}";

                Notifier.Notify(APPLICATION, MODULE, PURPOSE, entity, entity.AssignedEmployee.EmailAddress, purpose);
            }
        }

        #endregion
    }
}
