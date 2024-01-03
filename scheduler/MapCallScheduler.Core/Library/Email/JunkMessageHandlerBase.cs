using MapCallScheduler.Library.Configuration;
using MMSINC.Interface;

namespace MapCallScheduler.Library.Email
{
    public abstract class JunkMessageHandlerBase<TConfig> : IncomingEmailMessageHandlerBase<TConfig>
        where TConfig : IIncomingEmailServiceConfiguration
    {
        #region Constants

        public const string DEFAULT_JUNKBOX = "INBOX.Junk";

        #endregion

        #region Properties

        public virtual string Junkbox
        {
            get { return DEFAULT_JUNKBOX; }
        }

        #endregion

        #region Constructors

        protected JunkMessageHandlerBase(IncomingEmailMessageHandlerArgs<TConfig> args) : base(args) {}

        #endregion

        #region Exposed Methods

        public override void Handle(uint uid, string mailbox, IMailMessage message)
        {
            if (MakeChanges)
            {
                _imapClient.MoveMessage(uid, Junkbox, mailbox);
            }
        }

        public override void Dispose()
        {
            // noop
        }

        #endregion
    }
}