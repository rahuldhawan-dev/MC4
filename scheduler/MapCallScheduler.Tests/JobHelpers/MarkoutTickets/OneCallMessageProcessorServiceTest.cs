using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using MapCallScheduler.JobHelpers.MarkoutTickets.MessageHandlers;
using MapCallScheduler.Library;
using MapCallScheduler.Library.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.MarkoutTickets
{
    [TestClass]
    public class OneCallMessageProcessorServiceTest
    {
        #region Constants

        private const string MAILBOX = "foo";
        private const uint UID = 123U;

        #endregion

        #region Private Members

        private OneCallMessageProcessorService _target;
        private Mock<IWrappedImapClient> _client;
        private Mock<IMarkoutTicketServiceConfiguration> _config;
        private Mock<IMailMessage> _message;
        private Mock<ILog> _log;
        private Mock<IMarkoutTicketAuditor> _auditor;
        private Mock<IRepository<OneCallMarkoutTicket>> _ticketRepository;
        private Mock<IMarkoutAuditParser> _parser;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_log = new Mock<ILog>()).Object);
            _container.Inject((_client = new Mock<IWrappedImapClient>()).Object);
            _message = new Mock<IMailMessage>();
            _client.Setup(x => x.GetWrappedMessage(UID, true, MAILBOX)).Returns(_message.Object);
            _container.Inject(_client.Object);

            // THIS IS ALL FOR CHILD DEPENDENCIES:
            _container.Inject((_ticketRepository = new Mock<IRepository<OneCallMarkoutTicket>>()).Object);
            _container.Inject(new Mock<IMarkoutTicketParser>().Object);
            _container.Inject(new Mock<IRepository<OneCallMarkoutAudit>>().Object);
            _container.Inject((_auditor = new Mock<IMarkoutTicketAuditor>()).Object);
            _container.Inject((_parser = new Mock<IMarkoutAuditParser>()).Object);
            _container.Inject(new Mock<IAuditFailureEmailer>().Object);

            _config = new Mock<IMarkoutTicketServiceConfiguration>();
            // this is the default anyway
            _config.SetupGet(x => x.IncomingEmailConfig.MakeChanges).Returns(false);
            _container.Inject(_config.Object);
            _container.Inject<IOneCallMessageHandlerFactory>(new OneCallMessageHandlerFactory(_container));

            _target = _container.GetInstance<OneCallMessageProcessorService>();
        }

        #endregion

        private void SetupDuplicate()
        {
            _ticketRepository
                .Setup(x => x.Any(It.IsAny<Expression<Func<OneCallMarkoutTicket, bool>>>()))
                .Returns(true);
        }

        [TestMethod]
        public void TestProcessDeletesOriginalMessageIfItWasValidAndConfigurationSaysToDoSo()
        {
            var subjects = new[] {"ROUTINE 1", "Center Messages"};

            foreach (var subject in subjects)
            {
                var audit = new OneCallMarkoutAudit {Success = false};
                _message.SetupGet(x => x.From).Returns(new MailAddress(OneCallMessageHandlerFactory.EXPECTED_SENDER));
                _message.SetupGet(x => x.Subject).Returns(subject);
                _config.SetupGet(x => x.IncomingEmailConfig.MakeChanges).Returns(true);
                _parser.Setup(x => x.Parse(_message.Object)).Returns(audit);
                _auditor.Setup(x => x.Audit(audit));

                _target.Process(MAILBOX, UID);

                _client.Verify(x => x.DeleteMessage(UID, MAILBOX));
            }
        }

        [TestMethod]
        public void TestProcessDoesNotDeleteOriginalMessageIfFoundToBeADuplicate()
        {
            var subjects = new[] {"ROUTINE 1", "Center Messages"};

            _message.SetupGet(x => x.From).Returns(new MailAddress(OneCallMessageHandlerFactory.EXPECTED_SENDER));
            _message.SetupGet(x => x.Subject).Returns(subjects.First());
            SetupDuplicate();

            _target.Process(MAILBOX, UID);

            _client.Verify(x => x.DeleteMessage(UID, MAILBOX), Times.Never());
        }

        [TestMethod]
        public void TestProcessDoesNotDeleteOriginalMessageIfNotConfiguredTo()
        {
            var subjects = new[] {"ROUTINE 1", "Center Messages"};

            foreach (var subject in subjects)
            {
                var audit = new OneCallMarkoutAudit {Success = false};
                _message.SetupGet(x => x.From).Returns(new MailAddress(OneCallMessageHandlerFactory.EXPECTED_SENDER));
                _message.SetupGet(x => x.Subject).Returns(subject);
                _target.Process(MAILBOX, UID);
                _parser.Setup(x => x.Parse(_message.Object)).Returns(audit);
                _auditor.Setup(x => x.Audit(audit));

                _client.Verify(x => x.DeleteMessage(UID, MAILBOX), Times.Never());
            }
        }

        [TestMethod]
        public void TestProcessDoesNotMoveMessageIfNotConfiguredToButFoundToBeADuplicate()
        {
            var subjects = new[] {"ROUTINE 1", "Center Messages"};

            _message.SetupGet(x => x.From).Returns(new MailAddress(OneCallMessageHandlerFactory.EXPECTED_SENDER));
            _message.SetupGet(x => x.Subject).Returns(subjects.First());
            _config.SetupGet(x => x.IncomingEmailConfig.MakeChanges).Returns(false);
            SetupDuplicate();

            _target.Process(MAILBOX, UID);

            _client.Verify(x => x.MoveMessage(UID, MarkoutTicketMessageHandler.DUPLICATE_BOX, MAILBOX), Times.Never());
        }

        [TestMethod]
        public void TestProcessDoesNotMoveMessageToJunkIfItWasNotValidAndNotConfiguredTo()
        {
            _message.SetupGet(x => x.From).Returns(new MailAddress("not" + OneCallMessageHandlerFactory.EXPECTED_SENDER));

            _target.Process(MAILBOX, UID);

            _client.Verify(x => x.MoveMessage(UID, JunkMessageHandler.DEFAULT_JUNKBOX, MAILBOX), Times.Never());
        }

        [TestMethod]
        public void TestProcessMovesMessageIfFoundToBeADuplicate()
        {
            var subjects = new[] {"ROUTINE 1", "Center Messages"};

            _message.SetupGet(x => x.From).Returns(new MailAddress(OneCallMessageHandlerFactory.EXPECTED_SENDER));
            _message.SetupGet(x => x.Subject).Returns(subjects.First());
            _config.SetupGet(x => x.IncomingEmailConfig.MakeChanges).Returns(true);
            SetupDuplicate();

            _target.Process(MAILBOX, UID);

            _client.Verify(x => x.MoveMessage(UID, MarkoutTicketMessageHandler.DUPLICATE_BOX, MAILBOX));
        }

        [TestMethod]
        public void TestProcessMovesMessageToJunkIfItWasNotValidAndConfigurationSaysToDoSo()
        {
            _message.SetupGet(x => x.From).Returns(new MailAddress("not" + OneCallMessageHandlerFactory.EXPECTED_SENDER));
            _config.SetupGet(x => x.IncomingEmailConfig.MakeChanges).Returns(true);

            _target.Process(MAILBOX, UID);

            _client.Verify(x => x.MoveMessage(UID, JunkMessageHandler.DEFAULT_JUNKBOX, MAILBOX));
        }

        [TestMethod]
        public void TestProcessHandlesCancellationsRatherThanMovingThemToJunk()
        {
            _message.SetupGet(x => x.From).Returns(new MailAddress(OneCallMessageHandlerFactory.EXPECTED_SENDER));
            _message.SetupGet(x => x.Subject).Returns("CANCELLED 1");
            _config.SetupGet(x => x.IncomingEmailConfig.MakeChanges).Returns(true);

            _target.Process(MAILBOX, UID);

            _client.Verify(x => x.MoveMessage(UID, JunkMessageHandler.DEFAULT_JUNKBOX, MAILBOX), Times.Never);
        }
    }
}