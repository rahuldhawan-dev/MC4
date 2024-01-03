using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCall.Common.Configuration.NotificationTasks.Environmental
{
    public class EnvironmentalNonComplianceActionItemAssigned : MapCallNotifierTask<EnvironmentalNonComplianceEventActionItem>
    {
        #region Constants

        public const RoleApplications APPLICATION = RoleApplications.Environmental;
        public const RoleModules MODULE = RoleModules.EnvironmentalGeneral;
        public const string PURPOSE = "Environmental NonCompliance Action Item Assigned";

        #endregion

        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public EnvironmentalNonComplianceActionItemAssigned(IRepository<EnvironmentalNonComplianceEventActionItem> repository, INotifier notifier,
            INotificationService notificationService, ILog log, IDateTimeProvider dateTimeProvider) : base(repository,
            notifier, notificationService, log)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<EnvironmentalNonComplianceEventActionItem> GetData()
        {
            return Repository.GetAllActionItemsEvery30DaysFromEstimatedCompletion(_dateTimeProvider);
        }

        public override void SendNotification(EnvironmentalNonComplianceEventActionItem entity)
        {
            var model = new EnvironmentalNonComplianceActionItemAssignedNotification {
                AssignedToFullName = entity.ResponsibleOwner.FullName,
                EnvironmentalNonComplianceEvent = entity.EnvironmentalNonComplianceEvent,
                EnvironmentalNonComplianceEventActionItem = entity,
                RecordUrl = $"{BaseUrl}Environmental/EnvironmentalNonComplianceEventActionItem/Show/{entity.Id}",
                HelpUrl = $"{BaseUrl}HelpTopic/Show/271"
            };

            Notifier.Notify(APPLICATION, MODULE, PURPOSE, model, entity.ResponsibleOwner.Email);
        }

        #endregion
    }
}