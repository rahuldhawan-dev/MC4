using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;

namespace MapCall.Common.Configuration.NotificationTasks.HumanResources.Facilities.InterconnectContractEnds
{
    public abstract class InterconnectionContractEndsBase : MapCallNotifierTask<IInterconnectionRepository, Interconnection>
    {
        #region Consts

        public const RoleModules MODULE = RoleModules.ProductionInterconnections;
        public const string PURPOSE = "Interconnection Contract Ends";

        public const string EMAIL_SUBJECT = "Interconnection Contract Ends In {0}";

        #endregion

        #region Constructor

        public InterconnectionContractEndsBase(IInterconnectionRepository repository, INotifier notifier,
            INotificationService notificationService, ILog log) :
            base(repository, notifier, notificationService, log) { }

        #endregion

        #region Private Properties

        protected abstract int DaysTillExpiration { get; }

        protected abstract string TimeLeftPhraseForEmailSubject { get; }

        #endregion

        #region Private Methods
        
        private string GetNotificationEmailSubject()
        {
            return string.Format(EMAIL_SUBJECT, TimeLeftPhraseForEmailSubject);
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Interconnection> GetData()
        {
            return Repository.GetInterconnectionsThatHaveContractsExpiringInXDays(DaysTillExpiration);
        }

        public override void SendNotification(Interconnection entity)
        {
            NotificationService.Notify(new NotifierArgs {
                OperatingCenterId = entity.Facility.OperatingCenter.Id,
                Module = MODULE,
                Purpose = PURPOSE,
                Data = entity,
                Subject = GetNotificationEmailSubject()
            });
            entity.RecordUrl = $"{BaseUrl}Interconnection/Show/{entity.Id}";
        }

        #endregion
    }
}
