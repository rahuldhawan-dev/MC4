using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Common;
using MMSINC.Interface;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MapCall.Common.Utility.Notifications
{
    public abstract class NotifierBase : INotifier
    {
        #region Constants

        public const string EMAIL_ADDRESS_OVERRIDE_KEY = "AllEmailsGoTo", FROM_ADDRESS_KEY = "noreply_address";

        #endregion

        #region Private Members

        private readonly IContainer _container;

        #endregion

        #region Public Members

        public virtual string EmailAddressOverride
        {
            get { return ConfigurationManager.AppSettings[EMAIL_ADDRESS_OVERRIDE_KEY]; }
        }

        #endregion

        #region Constructors

        public NotifierBase(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Remember to dispose of this.
        /// </summary>
        private ISmtpClient CreateSmtpClientInstance()
        {
            return _container.GetInstance<ISmtpClientFactory>().Build();
        }

        private void SendNotification(string purpose, string message, string address, string subject,
            IEnumerable<Attachment> attachments, ISmtpClient smtp)
        {
            subject = subject ?? String.Format(RazorNotifier.MAIL_SUBJECT_FORMAT, purpose);

            string actualSubject;
            string actualAddress;
            using (var mailMsg = CreateMailMessage(address, subject, message, attachments))
            {
                smtp.Send(mailMsg);
                actualSubject = mailMsg.Subject;
                actualAddress = mailMsg.To.ToString();
            }

            //#if !DEBUG
            LogNotification(actualSubject, actualAddress, message);
            //#endif
        }

        private void LogNotification(string subject, string address, string message)
        {
            // We're checking the Current UserID > 0 because of the DangerousAuthenticationService
            var currentUser = _container.TryGetInstance<IAuthenticationService<User>>()?.CurrentUser;
            // Obtain a new session here and dispose of it. This is so that MultipleRecordSet=true
            // can be used when these inserts need to happen.
            using (var session = _container.GetInstance<ISessionFactory>().OpenSession())
            {
                var auditLogRepository = _container.With(session).GetInstance<IAuditLogEntryRepository>();
                auditLogRepository.Save(new AuditLogEntry {
                    AuditEntryType = "Email Notification",
                    EntityName = subject,
                    FieldName = address,
                    NewValue = message,
                    Timestamp = _container.GetInstance<IDateTimeProvider>().GetCurrentDate(),
                    User = currentUser != null && currentUser.Id > 0 ? currentUser : null
                });
            }
        }

        private IMailMessage CreateMailMessage(string address, string subject, string message,
            IEnumerable<Attachment> attachments = null)
        {
            attachments = attachments ?? Enumerable.Empty<Attachment>();

            if (!String.IsNullOrWhiteSpace(EmailAddressOverride))
            {
                subject = String.Format("{0} (meant for {1})", subject, address);
                address = EmailAddressOverride;
            }

            var msg = new MailMessageWrapper(ConfigurationManager.AppSettings.EnsureValue(FROM_ADDRESS_KEY), address) {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            foreach (var attach in attachments)
            {
                msg.AddAttachment(attach.FileName, attach.BinaryData);
            }

            return msg;
        }

        #endregion

        #region Abstract Methods

        protected abstract string LoadNotification(RoleApplications application, RoleModules module, string templateName,
            object data);

        #endregion

        #region Exposed Methods

        public void Notify(RoleApplications application, 
            RoleModules module, 
            string purpose, 
            object data,
            string address, 
            string subject = null, 
            IList<Attachment> attachments = null,
            string templateName = null)
        {
            /* One hot summer's day, in MC-3241, it was decided that sometimes it would be nice
             * to specify a different template than the purpose of a notification... therefore,
             * if a templateName is provided, it should be preferred, else fall back to purpose.
             */
            if (string.IsNullOrEmpty(templateName))
            {
                templateName = purpose;
            }

            using (var smtp = CreateSmtpClientInstance())
            {
                var message = LoadNotification(application, module, templateName, data);
                SendNotification(purpose, message, address, subject, attachments, smtp);
            }
        }

        #endregion
    }

    public interface INotifier
    {
        #region Abstract Methods

        void Notify(RoleApplications application, 
            RoleModules module, 
            string purpose, 
            object data, 
            string address,
            string subject = null, 
            IList<Attachment> attachments = null, 
            string templateName = null);

        #endregion
    }
}
