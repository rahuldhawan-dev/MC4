using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Conventions;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.Common.Configuration.NotificationTasks.Production
{
    public class SystemDeliveryEntryDue : MapCallNotifierTask<Facility>
    {
        #region Constants

        public const RoleApplications APPLICATION = RoleApplications.Production;
        public const RoleModules MODULE = RoleModules.ProductionSystemDeliveryEntry;
        public const string PURPOSE = "System Delivery Entry Due";

        #endregion

        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IContainer _container;

        #endregion

        #region Contructor

        public SystemDeliveryEntryDue(IRepository<Facility> repository, INotifier notifier, INotificationService notificationService, ILog log, IDateTimeProvider dateTimeProvider, IContainer container) : base(repository, notifier, notificationService, log)
        {
            _dateTimeProvider = dateTimeProvider;
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<Facility> GetData()
        {
            // only send on monday
            if (_dateTimeProvider.GetCurrentDate().DayOfWeek == DayOfWeek.Monday)
            {
                var result = Repository.Where(x => x.FacilitySystemDeliveryEntryTypes.Any(y => y.IsEnabled));
                return result;
            }

            return Enumerable.Empty<Facility>();
        }

        public override void SendNotification(Facility entity)
        {
            var lastMonday = _dateTimeProvider.GetCurrentDate().AddWeeks(-1);
            var systemDeliveryEntryRepo = _container.GetInstance<IRepository<SystemDeliveryEntry>>();
            var entryEntered = systemDeliveryEntryRepo.Any(x => x.WeekOf == lastMonday && x.Facilities.Contains(entity));

            if (!entryEntered)
            {
                var model = new SystemDeliveryEntryDueNotification {
                    OperatingCenter = entity.OperatingCenter,
                    Facility = entity
                };

                NotificationService.Notify(entity.OperatingCenter.Id, MODULE, PURPOSE, model);
            }
        }

        #endregion
    }
}
