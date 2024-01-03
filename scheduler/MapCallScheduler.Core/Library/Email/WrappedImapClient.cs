using MMSINC.Common;
using MMSINC.Interface;
using S22.Imap;

namespace MapCallScheduler.Library.Email
{
    /// <summary>
    /// Wrapped solely for the sake of testing, unfortunately.
    /// </summary>
    public class WrappedImapClient : ImapClient, IWrappedImapClient
    {
        #region Constants

        public const int IMAP_SSL_PORT = 993;

        #endregion

        #region Constructors

        public WrappedImapClient(string hostname, int port, string username, string password) : base(hostname, port, username, password, AuthMethod.Auto, port == IMAP_SSL_PORT, null) {}

        #endregion

        #region Exposed Methods

        public IMailboxInfo GetWrappedMailboxInfo(string mailbox = null)
        {
            return new WrappedMailboxInfo(GetMailboxInfo(mailbox));
        }

        public IMailMessage GetWrappedMessage(uint uid, bool seen = true, string mailbox = null)
        {
            return new MailMessageWrapper(GetMessage(uid, seen, mailbox));
        }

        #endregion
    }

    public interface IWrappedImapClient : IImapClient
    {
        #region Abstract Methods

        IMailboxInfo GetWrappedMailboxInfo(string mailbox = null);
        IMailMessage GetWrappedMessage(uint uid, bool seen = true, string mailbox = null);

        #endregion
    }
}