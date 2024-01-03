using log4net;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using MapCallScheduler.JobHelpers.MarkoutTickets.MessageHandlers;
using MapCallScheduler.Library;
using MapCallScheduler.Library.Configuration;
using MapCallScheduler.Library.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.MarkoutTickets
{
    [TestClass]
    public class OneCallMessageServiceTest
    {
        #region Private Members

        private OneCallMessageService _target;
        private Mock<IMarkoutTicketServiceConfiguration> _config;
        private Mock<IIncomingEmailConfigSection> _emailConfig;
        private Mock<IWrappedImapClient> _imapClient;
        private Mock<ILog> _log;
        private Mock<IOneCallMessageProcessorService> _processor;
        private Mock<IMailboxInfo> _inbox;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(c => {
                c.For<IImapClientFactory>().Use<ImapClientFactory>();
                c.For<IOneCallMessageHandlerFactory>().Use<OneCallMessageHandlerFactory>();
            });

            _log = new Mock<ILog>();
            _container.Inject(_log.Object);
            _config = new Mock<IMarkoutTicketServiceConfiguration>();
            _container.Inject(_config.Object);
            _config.SetupGet(x => x.IncomingEmailConfig)
                .Returns((_emailConfig = new Mock<IIncomingEmailConfigSection>()).Object);
            _imapClient = new Mock<IWrappedImapClient>();
            _container.Inject(_imapClient.Object);
            _processor = new Mock<IOneCallMessageProcessorService>();
            _container.Inject(_processor.Object);
            _inbox = new Mock<IMailboxInfo>();
            _imapClient.Setup(x => x.GetWrappedMailboxInfo(OneCallMessageService.INBOX)).Returns(_inbox.Object);
            _target = _container.GetInstance<OneCallMessageService>();
        }

        #endregion

        [TestMethod]
        public void TestProcessCreatesImapClientFromConfigValues()
        {
            _target.Process();

            _emailConfig.VerifyGet(x => x.Server);
            _emailConfig.VerifyGet(x => x.Port);
            _emailConfig.VerifyGet(x => x.Username);
            _emailConfig.VerifyGet(x => x.Password);
        }

        [TestMethod]
        public void TestProcessProcessesEachMessageInEachMailbox()
        {
            var mail = new[] {1U, 2U};

            _imapClient.Setup(x => x.Search(OneCallMessageService.UNDELETED, OneCallMessageService.INBOX)).Returns(mail);

            _target.Process();

            foreach (var uid in mail)
            {
                _processor.Verify(p => p.Process(OneCallMessageService.INBOX, uid));
            }
        }

        [TestMethod]
        public void TestOnlyGrabsUndeletedMessages()
        {
            Assert.AreEqual("UNDELETED", OneCallMessageService.UNDELETED.ToString());
        }
    }
}
