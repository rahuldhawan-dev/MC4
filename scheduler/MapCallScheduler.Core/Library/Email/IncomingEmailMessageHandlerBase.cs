using System;
using MapCallScheduler.Library.Configuration;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;

namespace MapCallScheduler.Library.Email
{
    public abstract class IncomingEmailMessageHandlerBase<TConfig> : IIncomingEmailMessageHandler
        where TConfig : IIncomingEmailServiceConfiguration
    {
        #region Private Members

        protected readonly IWrappedImapClient _imapClient;
        protected readonly TConfig _config;

        #endregion

        #region Properties

        public virtual bool MakeChanges
        {
            get { return _config.IncomingEmailConfig.MakeChanges; }
        }

        #endregion

        #region Constructors

        protected IncomingEmailMessageHandlerBase(IncomingEmailMessageHandlerArgs<TConfig> args)
        {
            _imapClient = args.ImapClient;
            _config = args.Config;
        }

        #endregion

        #region Abstract Methods

        public abstract void Handle(uint uid, string mailbox, IMailMessage message);
        public abstract void Dispose();

        #endregion

        #region Nested Type: IncomingEmailMessageHandlerArgs

        public class IncomingEmailMessageHandlerArgs<TConfig>
            where TConfig : IIncomingEmailServiceConfiguration
        {
            #region Private Members

            protected readonly IWrappedImapClient _imapClient;
            protected readonly TConfig _config;

            #endregion

            #region Properties

            public IWrappedImapClient ImapClient => _imapClient;

            public TConfig Config => _config;

            #endregion

            #region Constructors

            public IncomingEmailMessageHandlerArgs(IWrappedImapClient imapClient, TConfig config)
            {
                _imapClient = imapClient;
                _config = config;
            }

            #endregion
        }

        #endregion
    }

    public abstract class IncomingEmailMessageHandlerBase<TConfig, TEntity, TParser> : IncomingEmailMessageHandlerBase<TConfig>
        where TConfig : IIncomingEmailServiceConfiguration
        where TParser : IParser<TEntity>
    {
        #region Private Members

        protected readonly TParser _parser;
        protected readonly IRepository<TEntity> _repository;

        #endregion

        #region Constructors

        public IncomingEmailMessageHandlerBase(IncomingEmailMessageHandlerArgs<TConfig, TEntity, TParser> args) : base(args)
        {
            _parser = args.Parser;
            _repository = args.Repository;
        }

        #endregion

        #region Private Methods

        protected virtual TEntity Parse(uint uid, string mailbox, IMailMessage message)
        {
            return _parser.Parse(message);
        }

        /// <summary>
        /// Save the parsed entity to the database and delete it from imap.
        /// Override here for any checks prior to saving for handling duplicates
        /// and such.
        /// </summary>
        protected virtual void SaveAndCleanup(uint uid, string mailbox, TEntity entity)
        {
            _repository.Save(entity);

            if (MakeChanges)
            {
                _imapClient.DeleteMessage(uid, mailbox);
            }
        }

        protected virtual void BeforeSaveAndCleanup(IMailMessage message, TEntity entity)
        {
            // noop
        }

        protected virtual void AfterSaveAndCleanup(IMailMessage message, TEntity entity)
        {
            // noop
        }

        #endregion

        #region Exposed Methods

        public override void Handle(uint uid, string mailbox, IMailMessage message)
        {
            var entity = Parse(uid, mailbox, message);

            BeforeSaveAndCleanup(message, entity);

            SaveAndCleanup(uid, mailbox, entity);

            AfterSaveAndCleanup(message, entity);
        }

        public override void Dispose()
        {
            // noop
        }

        #endregion

        #region Nested Type: IncomingEmailMessageHandlerArgs

        public class IncomingEmailMessageHandlerArgs<TConfig, TEntity, TParser> : IncomingEmailMessageHandlerArgs<TConfig>
            where TConfig : IIncomingEmailServiceConfiguration
        {
            #region Properties

            public IRepository<TEntity> Repository { get; }
            public TParser Parser { get; }

            #endregion

            #region Constructors

            public IncomingEmailMessageHandlerArgs(IWrappedImapClient imapClient, TConfig config,
                IRepository<TEntity> repository, TParser parser) : base(imapClient, config)
            {
                Repository = repository;
                Parser = parser;
            }

            #endregion
        }

        #endregion
    }
}
