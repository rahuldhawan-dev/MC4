using log4net;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;

namespace MapCall.Common.Configuration.NotificationTasks.HumanResources.Facilities.InterconnectContractEnds
{
    public class InterconnectionContractEndsIn6Months : InterconnectionContractEndsBase
    {
        #region Constants

        public const string DESCRIPTIVE_TIME_LEFT = "6 Months";

        public const int DAYS_LEFT = 180;

        #endregion

        #region Constructor

        public InterconnectionContractEndsIn6Months(IInterconnectionRepository repository, INotifier notifier,
            INotificationService notificationService, ILog log) :
            base(repository, notifier, notificationService, log) { }

        #endregion

        #region Private Properties

        protected override int DaysTillExpiration => DAYS_LEFT;

        protected override string TimeLeftPhraseForEmailSubject => DESCRIPTIVE_TIME_LEFT;

        #endregion
    }
}
