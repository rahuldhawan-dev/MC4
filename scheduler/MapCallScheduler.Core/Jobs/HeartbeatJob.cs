using log4net;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Common;
using MMSINC.Interface;
using MMSINC.Utilities;
using MMSINC.Utilities.ErrorHandling;
using Quartz;
using System;
using System.Configuration;

namespace MapCallScheduler.Jobs
{
    public class HeartbeatJob : MapCallJobBase
    {
        #region Constants

        public const string 
            TO_ADDRESS = ErrorMessageGenerator.RECIPIENT,
            SUBJECT_FORMAT = "{0} Heartbeat Notification",
            BODY_FORMAT = "I am alive and able to send email at {0}.",
            NOREPLY_KEY = "noreply_address";

        #endregion

        #region Private Members

        private readonly ISmtpClient _smtp;
        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public HeartbeatJob(ILog log, ISmtpClientFactory smtp, IDateTimeProvider dateTimeProvider) : base(log, null) // no need for an emailer, we are an emailer
        {
            _smtp = smtp.Build();
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        protected override void ExecuteJob(IJobExecutionContext context)
        {
            using (_smtp)
            {
                using (var msg = new MailMessageWrapper(ConfigurationManager.AppSettings.EnsureValue(NOREPLY_KEY), TO_ADDRESS) {
                    Subject = String.Format(SUBJECT_FORMAT, AppDomain.CurrentDomain.FriendlyName),
                    Body = String.Format(BODY_FORMAT, _dateTimeProvider.GetCurrentDate())
                })
                {
                    _smtp.Send(msg);
                }
            }
        }

        #endregion
    }
}