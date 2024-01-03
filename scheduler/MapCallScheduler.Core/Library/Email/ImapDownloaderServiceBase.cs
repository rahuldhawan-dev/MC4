using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.Configuration;
using S22.Imap;
using StructureMap;
using StructureMap.Pipeline;

namespace MapCallScheduler.Library.Email
{
    /// <summary>
    /// Implementations should usually only need to provide TConfig and TProcessor, unless anything weird needs to happen
    /// in processing individual messages.
    /// </summary>
    public abstract class ImapMessageProcessorServiceBase<TConfig, TProcessor> : IImapMessageProcessorService
        where TConfig : IIncomingEmailServiceConfiguration
        where TProcessor : IIncomingEmailMessageProcessorService
    {
        #region Constants

        // FOR SOME REASON THEY NAMED THIS CONDITION UNDELETED
        // SOURCE: Finds messages that do not have the \Deleted flag set.
        public static readonly SearchCondition UNDELETED = SearchCondition.Undeleted();

        public const string INBOX = "INBOX";

        #endregion

        #region Private Members

        protected readonly ILog _log;
        protected readonly IImapClientFactory _factory;
        protected readonly TConfig _config;
        protected readonly IContainer _container;

        #endregion

        #region Properties

        public virtual string Inbox
        {
            get { return INBOX; }
        }

        #endregion

        #region Constructors

        protected ImapMessageProcessorServiceBase(ILog log, IImapClientFactory factory, TConfig config, IContainer container)
        {
            _log = log;
            _factory = factory;
            _config = config;
            _container = container;
        }

        #endregion

        #region Private Methods

        protected virtual void ProcessMessage(IWrappedImapClient imapClient, string mailbox, uint uid)
        {
            _container.GetInstance<TProcessor>(
                new ExplicitArguments(new Dictionary<string, object> {{"imapClient", imapClient}}))
                .Process(mailbox, uid);
        }

        #endregion

        #region Exposed Methods

        public void Process()
        {
            using (var imapClient = _factory.Build(_config.IncomingEmailConfig))
            {
                var mailboxInfo = imapClient.GetWrappedMailboxInfo(Inbox);

                var undeleted = imapClient.Search(UNDELETED, Inbox);

                _log.InfoFormat("Mailbox '{0}' has {1} messages, {2} unread, {3} undeleted, {4} space used, {5} space remaining.",
                    mailboxInfo.Name, mailboxInfo.Messages, mailboxInfo.Unread, undeleted.Count(), mailboxInfo.UsedStorage,
                    mailboxInfo.FreeStorage);

                foreach (var uid in undeleted)
                {
                    ProcessMessage(imapClient, Inbox, uid);
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Downloads messages from an Imap server, and hands them off to a processor instance.
    /// </summary>
    public interface IImapMessageProcessorService : IProcessableService {}
}