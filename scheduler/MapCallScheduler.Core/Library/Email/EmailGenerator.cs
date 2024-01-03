using MMSINC.Common;
using MMSINC.Interface;

namespace MapCallScheduler.Library.Email
{
    public class EmailGenerator : IEmailGenerator
    {
        #region Private Members

        private readonly IMapCallSchedulerConfiguration _config;

        #endregion

        #region Properties

        protected string OverrideToAddress => string.IsNullOrWhiteSpace(_config.AllEmailsGoTo) ? null : _config.AllEmailsGoTo;

        #endregion

        #region Constructors

        public EmailGenerator(IMapCallSchedulerConfiguration config)
        {
            _config = config;
        }

        #endregion

        #region Exposed Methods

        public IMailMessage Generate(string from, string to, string subject, string message, bool useHtml)
        {
            return new MailMessageWrapper(from, OverrideToAddress ?? to) {
                Subject = subject,
                Body = message,
                IsBodyHtml = useHtml
            };
        }

        #endregion
    }

    public interface IEmailGenerator
    {
        #region Abstract Methods

        IMailMessage Generate(string from, string to, string subject, string message, bool useHtml);

        #endregion
    }
}