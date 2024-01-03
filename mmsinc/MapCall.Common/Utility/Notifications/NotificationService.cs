using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data.NHibernate;
using Module = MapCall.Common.Model.Entities.Module;

namespace MapCall.Common.Utility.Notifications
{
    public class NotificationService : INotificationService
    {
        #region Private Members

        protected Assembly _assembly;
        protected readonly INotificationConfigurationRepository _notificationConfigurationRepository;
        protected readonly INotifier _notifier;
        protected readonly IRepository<NotificationPurpose> _notificationPurposeRepository;
        protected readonly IRepository<Module> _moduleRepository;

        #endregion

        #region Properties

        public virtual Assembly Assembly => _assembly ?? (_assembly = GetType().Assembly);

        public virtual INotificationConfigurationRepository NotificationConfigurationRepository => _notificationConfigurationRepository;

        protected virtual INotifier Notifier => _notifier;

        #endregion

        #region Constructors

        public NotificationService(INotifier notifier,
            INotificationConfigurationRepository notificationConfigurationRepository,
            IRepository<NotificationPurpose> notificationPurposeRepository, 
            IRepository<Module> moduleRepository)
        {
            _notifier = notifier;
            _notificationConfigurationRepository = notificationConfigurationRepository;
            _notificationPurposeRepository = notificationPurposeRepository;
            _moduleRepository = moduleRepository;
        }

        #endregion

        #region Private Methods

        private void SendNotification(RoleApplications application, 
            RoleModules module, 
            string purpose, 
            string email, 
            NotifierArgs args)
        {
            var emailTo = !string.IsNullOrWhiteSpace(args.Address) ? args.Address : email;
            var templateName = !string.IsNullOrEmpty(args.TemplateName) ? args.TemplateName : args.Purpose;

            Notifier.Notify(application,
                module,
                purpose, 
                args.Data,
                emailTo,
                args.Subject,
                args.Attachments,
                templateName);
        }

        protected void InnerNotify(NotifierArgs args)
        {
            var notificationConfigurations = args.OperatingCenterId > 0
                ? NotificationConfigurationRepository.FindByOperatingCenterModuleAndPurpose(args.OperatingCenterId, 
                    args.Module, 
                    args.Purpose)
                : NotificationConfigurationRepository.FindByModuleAndPurpose(args.Module, args.Purpose);

            var configurations = notificationConfigurations.Where(x => !string.IsNullOrWhiteSpace(x.Contact.Email))
                                                           .ToList();

            if (!string.IsNullOrWhiteSpace(args.Address))
            {
                NotifyForSingleAddress(args, configurations);
            }
            else
            {
                NotifyForAllConfigurations(args, configurations);
            }
        }

        private void NotifyForAllConfigurations(NotifierArgs args, List<NotificationConfiguration> configurations)
        {
            foreach (var notificationConfiguration in configurations)
            {
                var notificationPurpose =
                    notificationConfiguration.NotificationPurposes.FirstOrDefault(x => x.Purpose == args.Purpose);

                if (notificationPurpose != null)
                {
                    SendNotification((RoleApplications)notificationPurpose.Module.Application.Id,
                        (RoleModules)notificationPurpose.Module.Id,
                        notificationPurpose.Purpose,
                        notificationConfiguration.Contact.Email,
                        args);
                }
            }
        }

        private void NotifyForSingleAddress(NotifierArgs args, List<NotificationConfiguration> configurations)
        {
            NotificationConfiguration notificationConfiguration;
            if (configurations.Any())
            {
                notificationConfiguration = configurations.FirstOrDefault() ?? new NotificationConfiguration {
                    NotificationPurposes = _notificationPurposeRepository.GetAll()
                                                                         .Where(x => x.Purpose == args.Purpose)
                                                                         .ToList()
                };
            }
            else
            {
                notificationConfiguration = new NotificationConfiguration {
                    NotificationPurposes = new List<NotificationPurpose> {
                        new NotificationPurpose {
                            Purpose = args.Purpose,
                            Module = _moduleRepository.Find((int)args.Module)
                        }
                    }
                };
            }

            var notificationPurpose =
                notificationConfiguration.NotificationPurposes.FirstOrDefault(x => x.Purpose == args.Purpose);

            if (notificationPurpose != null)
            {
                SendNotification((RoleApplications)notificationPurpose.Module.Application.Id,
                    (RoleModules)notificationPurpose.Module.Id,
                    notificationPurpose.Purpose,
                    notificationConfiguration.Contact?.Email,
                    args);
            }
        }

        #endregion

        #region Exposed Methods

        public void Notify(
            int operatingCenterId, 
            RoleModules module, 
            string purpose, 
            object data,
            string subject = null, 
            string address = null,
            string templateName = null)
        {
            Notify(new NotifierArgs {
                OperatingCenterId = operatingCenterId,
                Module = module,
                Purpose = purpose,
                Subject = subject,
                Data = data,
                Address = address,
                TemplateName = templateName
            });
        }

        public void Notify(NotifierArgs args)
        {
            InnerNotify(args);
        }

        #endregion
    }

    public interface INotificationService
    {
        #region Abstract Methods

        void Notify(int operatingCenterId, 
            RoleModules module, 
            string purpose, 
            object data, 
            string subject = null,
            string address = null, 
            string templateName = null);

        void Notify(NotifierArgs args);

        #endregion
    }
}
