using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Interface;
using MMSINC.Utilities.ErrorHandling;
using System;
using System.Configuration;

namespace MapCallScheduler.Library.Email
{
    public class DeveloperEmailer : IDeveloperEmailer
    {
        #region Constants

        public const string  EXTERMINATOR = "mapcall_exterminator@amwater.com", NO_REPLY_KEY = "noreply_address";

        #endregion

        #region Private Members

        private readonly IEmailGenerator _generator;
        private readonly ISmtpClient _smtpClient;
        private readonly IErrorMessageGenerator _errorMessageGenerator;

        #endregion

        #region Constructors

        public DeveloperEmailer(IEmailGenerator generator, IErrorMessageGenerator errorMessageGenerator, ISmtpClient smtpClient)
        {
            _generator = generator;
            _errorMessageGenerator = errorMessageGenerator;
            _smtpClient = smtpClient;
        }

        #endregion

        #region Exposed Methods

        public void SendMessage(string address, string subject, string message, bool useHtml)
        {
            using (var email = _generator.Generate(ConfigurationManager.AppSettings.EnsureValue(NO_REPLY_KEY), address, subject, message, useHtml))
            {
                _smtpClient.Send(email);
            }
        }

        public void SendMessage(string subject, string message, bool useHtml)
        {
            SendMessage(EXTERMINATOR, subject, message, useHtml);
        }

        public void SendErrorMessage(string subject, Exception error)
        {
            SendMessage(subject, _errorMessageGenerator.GenerateMessageBody(error), true);
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }

        #endregion
    }

    public interface IDeveloperEmailer : IDisposable
    {
        #region Abstract Methods

        void SendMessage(string address, string subject, string message, bool useHtml);
        void SendMessage(string subject, string message, bool useHtml);

        void SendErrorMessage(string subject, Exception error);

        #endregion
    }
}
