using System;
using System.Collections.Generic;
using Humanizer;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCall.Common.Configuration.NotificationTasks.HealthandSafety
{
    public class EnvironmentalNearMiss : MapCallNotifierTask<NearMiss>
    {
        #region Constants

        public const RoleApplications APPLICATION = RoleApplications.Operations;
        public const RoleModules MODULE = RoleModules.OperationsHealthAndSafety;
        public const string PURPOSE = "Environmental Near Miss";

        #endregion

        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public EnvironmentalNearMiss(IRepository<NearMiss> repository, INotifier notifier,
            INotificationService notificationService, ILog log, IDateTimeProvider dateTimeProvider) : base(repository,
            notifier, notificationService, log)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<NearMiss> GetData()
        {
            return Repository.GetNearMissesInPriorOneDay(_dateTimeProvider, NearMissType.Indices.ENVIRONMENTAL);
        }

        public override void SendNotification(NearMiss entity)
        {
            entity.RecordUrl = $"{BaseUrl}HealthAndSafety/NearMiss/Show/{entity.Id}";
            NotificationService.Notify(entity.OperatingCenter.Id, MODULE, PURPOSE, entity, PURPOSE);
        }

        #endregion
    }
}
