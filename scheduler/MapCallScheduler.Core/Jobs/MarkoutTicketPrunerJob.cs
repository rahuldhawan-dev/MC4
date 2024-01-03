using System;
using log4net;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using MapCallScheduler.JobHelpers.MarkoutTickets.MessageHandlers;
using MapCallScheduler.Library;
using MapCallScheduler.Metadata;
using MMSINC.Interface;
using Quartz;
using StructureMap;

namespace MapCallScheduler.Jobs
{
    #if DEBUG

    /*

    /// <summary>
    /// Deletes all messages on the smtp server before CUTOFF.  This should never be released.
    /// </summary>
    [Immediate]
    public class MarkoutTicketPrunerJob : MarkoutTicketFetcherJob
    {
        #region Constants

        public static readonly DateTime CUTOFF = new DateTime(2016, 3, 10, 17, 30, 0);

        #endregion

        #region Constructors

        public MarkoutTicketPrunerJob(ILog log, IOneCallMessagePrunerService service, IOneCallMessageHeartbeatService heartbeatService) : base(log, service, heartbeatService) {}

        #endregion
    }

    public class OneCallMessagePrunerService : ImapMessageProcessorServiceBase<IMarkoutTicketServiceConfiguration, IOneCallMessagePrunerProcessorService>, IOneCallMessagePrunerService
    {
        #region Constructors

        public OneCallMessagePrunerService(ILog log, IImapClientFactory factory, IMarkoutTicketServiceConfiguration config, IContainer container) : base(log, factory, config, container) {}

        #endregion
    }

    public interface IOneCallMessagePrunerService : IOneCallMessageService {}

    public class OneCallMessagePrunerProcessorService : IncomingEmailMessageProcessorServiceBase<IOneCallMessagePrunerHandlerFactory>, IOneCallMessagePrunerProcessorService
    {
        #region Constructors

        public OneCallMessagePrunerProcessorService(ILog log, IWrappedImapClient imapClient, IOneCallMessagePrunerHandlerFactory handlerFactory) : base(log, imapClient, handlerFactory) {}

        #endregion
    }

    public interface IOneCallMessagePrunerProcessorService : IIncomingEmailMessageProcessorService {}

    public class OneCallMessagePrunerHandlerFactory : IncomingEmailMessageHandlerFactoryBase<IOneCallMessagePrunerHandler>, IOneCallMessagePrunerHandlerFactory
    {
        #region Properties

        public override Type JunkMessageHandlerType
        {
            get { return typeof(JunkMessageHandler); }
        }

        public override string ExpectedSender
        {
            get { return OneCallMessageHandlerFactory.EXPECTED_SENDER; }
        }

        #endregion

        #region Constructors

        public OneCallMessagePrunerHandlerFactory(IContainer container) : base(container) {}

        #endregion
    }

    public interface IOneCallMessagePrunerHandlerFactory : IIncomingEmailMessageHandlerFactory {}

    public class HandlesAnyMessageAttribute : HandlesMessageAttribute
    {
        #region Constructors

        public HandlesAnyMessageAttribute() : base(_ => true) {}

        #endregion
    }
     
    [HandlesAnyMessage]
    public class OneCallMessagePrunerHandler : OneCallMessageHandlerBase, IOneCallMessagePrunerHandler
    {
        #region Private Members

        private readonly ILog _log;

        #endregion

        #region Constructors

        public OneCallMessagePrunerHandler(OneCallMessageHandlerBaseArgs args, ILog log) : base(args)
        {
            _log = log;
        }

        #endregion

        #region Exposed Methods

        public override void Handle(uint uid, string mailbox, IMailMessage message)
        {
            DateTime dateReceived;

            try
            {
                dateReceived = MarkoutTicketParser.ParseDateReceived(message.Body);
            }
            catch
            {
                dateReceived = MarkoutTicketPrunerJob.CUTOFF.AddDays(-1);
            }


            if (dateReceived <= MarkoutTicketPrunerJob.CUTOFF)
            {
                _log.Info(String.Format("Deleting message {0} ({1})", uid, dateReceived));

                if (MakeChanges)
                {
                    _imapClient.DeleteMessage(uid, mailbox);
                }
            }
            else
            {
                _log.Info(String.Format("NOT deleting message {0} ({1})", uid, dateReceived));
            }
        }

        #endregion
    }

    public interface IOneCallMessagePrunerHandler : IIncomingEmailMessageHandler {}

    */

    #endif
}
