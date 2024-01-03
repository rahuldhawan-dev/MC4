using System;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using MapCallScheduler.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.MarkoutTickets
{
    [TestClass]
    public class MarkoutTicketAuditorTest
    {
        #region Private Members

        private Mock<IMarkoutAuditParser> _parser;
        private Mock<IRepository<OneCallMarkoutTicket>> _repository;
        private Mock<IMailMessage> _message;
        private string _messageText;
        private OneCallMarkoutAudit _audit;
        private MarkoutTicketAuditor _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_parser = new Mock<IMarkoutAuditParser>()).Object);
            _container.Inject((_repository = new Mock<IRepository<OneCallMarkoutTicket>>()).Object);
            _messageText = "this is the full text of the audit message";
            _message = new Mock<IMailMessage>();
            _audit = new OneCallMarkoutAudit {
                FullText = _messageText
            };
            _parser.Setup(x => x.Parse(_message.Object)).Returns(_audit);

            _target = _container.GetInstance<MarkoutTicketAuditor>();
        }

        #endregion

        [TestMethod]
        public void TestAuditSetsSuccessToFalse()
        {
            var type = new OneCallMarkoutMessageType {Description = "foo"};
            var tickets = new[] {
                new OneCallMarkoutTicket {RequestNumber = 1234, MessageType = type},
                new OneCallMarkoutTicket {RequestNumber = 4321, MessageType = type},
            };
            foreach (var ticket in tickets)
            {
                _audit.TicketNumbers.Add(new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = ticket.RequestNumber,
                    MessageType = ticket.MessageType
                });
            }
            _repository
                .Setup(r => r.Where(It.IsAny<Expression<Func<OneCallMarkoutTicket, bool>>>()))
                .Returns(Enumerable.Empty<OneCallMarkoutTicket>().AsQueryable());

            _target.Audit(_audit);

            Assert.IsFalse(_audit.Success.Value);

            _repository.VerifyAll();
        }

        [TestMethod]
        public void TestAuditSetsSuccessToTrueIfAllExpectedTicketsExist()
        {
            var type = new OneCallMarkoutMessageType {Description = "foo"};
            var tickets = new[] {
                new OneCallMarkoutTicket {RequestNumber = 1234, MessageType = type},
                new OneCallMarkoutTicket {RequestNumber = 4321, MessageType = type},
            };
            foreach (var ticket in tickets)
            {
                _audit.TicketNumbers.Add(new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = ticket.RequestNumber,
                    MessageType = ticket.MessageType
                });
            }
            _repository
                .Setup(r => r.Where(It.IsAny<Expression<Func<OneCallMarkoutTicket, bool>>>()))
                .Returns<Expression<Func<OneCallMarkoutTicket, bool>>>(fn => tickets.Where(fn.Compile()).AsQueryable());

            _target.Audit(_audit);

            Assert.IsTrue(_audit.Success.Value);

            _repository.VerifyAll();
        }
    }
}