using System;
using System.Configuration;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using MapCallScheduler.Library;
using MapCallScheduler.Library.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.MarkoutTickets
{
    [TestClass]
    public class OneCallMessageHeartbeatServiceTest
    {
        #region Private Members

        private OneCallMessageHeartbeatService _target;
        private Mock<IRepository<OneCallMarkoutTicket>> _repo;
        private Mock<IDeveloperEmailer> _emailer;
        private Mock<IMarkoutTicketServiceConfiguration> _config;
        private DateTime _now;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _now = DateTime.Now;
            _container.Inject((_emailer = new Mock<IDeveloperEmailer>()).Object);
            _container.Inject((_config = new Mock<IMarkoutTicketServiceConfiguration>()).Object);
            _container.Inject((_repo = new Mock<IRepository<OneCallMarkoutTicket>>()).Object);
            _container.Inject((IDateTimeProvider)new TestDateTimeProvider(_now));
            _target = _container.GetInstance<OneCallMessageHeartbeatService>();

            _config.SetupGet(x => x.IncomingEmailConfig.GapInterval).Returns((int?)null);
        }

        #endregion

        [TestMethod]
        public void TestProcessDoesNotSendNotificationIfRecentTicketFound()
        {
            _repo.Setup(x => x.Where(t => t.DateReceived > _now.AddMinutes(OneCallMessageHeartbeatService.DEFAULT_RECENT_INTERVAL)))
                .Returns(new [] {new OneCallMarkoutTicket()}.AsQueryable());

            _target.Process();

            _emailer.VerifyAll();
        }

        [TestMethod]
        public void TestProcessSendsNotificationIfNoRecentTicketsFound()
        {
            _repo.Setup(x => x.Where(t => t.DateReceived > _now.AddMinutes(OneCallMessageHeartbeatService.DEFAULT_RECENT_INTERVAL)))
                .Returns(Enumerable.Empty<OneCallMarkoutTicket>().AsQueryable());

            _target.Process();

            _emailer.Verify(
                x =>
                    x.SendMessage(OneCallMessageHeartbeatService.NO_RECENT_MESSAGE_SUBJECT,
                        String.Format(OneCallMessageHeartbeatService.NO_RECENT_MESSAGE_MESSAGE_FORMAT, _now), false));
        }

        [TestMethod]
        public void TestProcessUsesIntervalFromConfigurationIfSet()
        {
            _config.SetupGet(x => x.IncomingEmailConfig.GapInterval).Returns(666);
            _repo.Setup(x => x.Where(t => t.DateReceived > _now.AddMinutes(666)))
                .Returns(Enumerable.Empty<OneCallMarkoutTicket>().AsQueryable());

            _target.Process();

            _emailer.Verify(
                x =>
                    x.SendMessage(OneCallMessageHeartbeatService.NO_RECENT_MESSAGE_SUBJECT,
                        String.Format(OneCallMessageHeartbeatService.NO_RECENT_MESSAGE_MESSAGE_FORMAT, _now), false));
        }
    }
}