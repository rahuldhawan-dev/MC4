using System.Collections.Generic;
using S22.Imap;

namespace MapCallScheduler.Library.Email
{
    /// <summary>
    /// Wrapped solely for the sake of testing, unfortunately.
    /// </summary>
    public class WrappedMailboxInfo : IMailboxInfo
    {
        #region Private Members

        private readonly MailboxInfo _mailboxInfo;

        #endregion

        #region Properties

        public IEnumerable<MailboxFlag> Flags
        {
            get { return _mailboxInfo.Flags; }
        }

        public ulong FreeStorage
        {
            get { return _mailboxInfo.FreeStorage; }
        }

        public int Messages
        {
            get { return _mailboxInfo.Messages; }
        }

        public string Name
        {
            get { return _mailboxInfo.Name; }
        }

        public uint NextUID
        {
            get { return _mailboxInfo.NextUID; }
        }

        public int Unread
        {
            get { return _mailboxInfo.Unread; }
        }

        public ulong UsedStorage
        {
            get { return _mailboxInfo.UsedStorage; }
        }

        #endregion

        #region Constructors

        public WrappedMailboxInfo(MailboxInfo mailboxInfo)
        {
            _mailboxInfo = mailboxInfo;
        }

        #endregion
    }

    public interface IMailboxInfo
    {
        #region Abstract Properties

        IEnumerable<MailboxFlag> Flags { get; }
        ulong FreeStorage { get; }
        int Messages { get; }
        string Name { get; }
        uint NextUID { get; }
        int Unread { get; }
        ulong UsedStorage { get; }

        #endregion
    }
}
