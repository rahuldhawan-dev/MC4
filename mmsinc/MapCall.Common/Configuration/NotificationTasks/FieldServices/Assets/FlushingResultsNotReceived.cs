using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;

namespace MapCall.Common.Configuration.NotificationTasks.FieldServices.Assets
{ 
    public class FlushingResultsNotReceived : MapCallNotifierTask<IServiceFlushRepository, ServiceFlush>
    {
        #region Constants

        public const RoleApplications APPLICATION = RoleApplications.FieldServices;
        public const RoleModules MODULE = RoleModules.FieldServicesAssets;
        public const string PURPOSE = "Flushing Results Not Received";

        #endregion
       
        #region Constructors

        public FlushingResultsNotReceived(IServiceFlushRepository repository,
            INotifier notifier, INotificationService notificationService, ILog log) : base(repository, notifier,
            notificationService, log) { }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ServiceFlush> GetData()
        {
            return Repository.GetServiceFlushNotReceivedAfterTwoWeeks();
        }

        public override void SendNotification(ServiceFlush entity)
        {
            NotificationService.Notify(new NotifierArgs {
                OperatingCenterId = entity.Service.OperatingCenter.Id,
                Module = MODULE,
                Purpose = PURPOSE,
                Data = entity,
                Subject = PURPOSE 
            });
            entity.HasSentNotification = true;
            if (entity.Service != null)
            {
                entity.Service.RecordUrl = $"{BaseUrl}FieldOperations/Service/Show/{entity.Id}";
            }
            Repository.Save(entity);
        }

        #endregion
    }
}
