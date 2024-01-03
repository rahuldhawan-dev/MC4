using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using MapCallScheduler.Library;
using MapCallScheduler.Library.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.MarkoutTickets
{
    [TestClass]
    public class AuditFailureEmailerTest
    {
        #region Constants

        public const string EXPECTED_MESSAGE = @"<html>
  <head>
    <title>Markout Ticket Audit Failed</title>
    <style>
      table#auditTicketsTable {
        border: 1px solid black;
        border-collapse: collapse;
      }

      table#auditTicketsTable th, table#auditTicketsTable td {
        border-top: 1px solid black;
        border-bottom: 1px solid black;
        padding: 4px;
      }

      table#auditTicketsTable th {
        border-left: 1px solid black;
        border-right: 1px solid black;
      }

      table#auditTicketsTable td {
        border-left: 1px dashed black;
        border-right: 1px dashed black;
      }

      table#auditTicketsTable tbody tr:nth-child(even) td {
        background: #e1eef7;
      }

      table#auditTicketsTable tbody tr:hover td {
        background: #83b9de;
      }
    </style>
  </head>
  <body>
    <div><a href='http://mapcall.amwater.com/modules/mvc/FieldOperations/OneCallMarkoutAudit/Show/666'>Link</a><h3>Audit Information:</h3>
Transmitted At: 2/21/1984 8:30:00 PM
<h4>Expected Tickets:</h4>
<table id='auditTicketsTable'>
<thead><tr>
<th>Number</th><th>Type</th><th>In Database?</th><th>More Than One?</th><th>Type Matches?</th>
</tr></thead>
<tbody>
<tr>
<td>1234</td><td></td><td>False</td><td>False</td><td>False</td></tr>
<tr>
<td><a href='http://mapcall.amwater.com/modules/mvc/FieldOperations/OneCallMarkoutTicket/Show/1'>4321</a> </td><td></td><td>True</td><td>False</td><td>False</td></tr>
<tr>
<td><a href='http://mapcall.amwater.com/modules/mvc/FieldOperations/OneCallMarkoutTicket/Show/2'>666</a> </td><td></td><td>True</td><td>False</td><td>True</td></tr>
</tbody>
</table>
</div>
    <div>
      <h3>Original Audit Message:</h3>
      <pre><code>this is what the original message said</code></pre>
    </div>
  </body>
</html>";

        #endregion

        #region Private Members

        private AuditFailureEmailer _target;
        private Mock<IDeveloperEmailer> _innerEmailer;
        private Mock<IRepository<OneCallMarkoutTicket>> _ticketRepository;
        private OneCallMarkoutMessageType _routine;
        private OneCallMarkoutMessageType _emergency;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_innerEmailer = new Mock<IDeveloperEmailer>()).Object);
            _container.Inject((_ticketRepository = new Mock<IRepository<OneCallMarkoutTicket>>()).Object);
            _routine = new OneCallMarkoutMessageType();
            _emergency = new OneCallMarkoutMessageType();

            _target = _container.GetInstance<AuditFailureEmailer>();
        }

        #endregion

        #region Private Methods

        private OneCallMarkoutAudit BuildAudit()
        {
            return new OneCallMarkoutAudit {
                Id = 666,
                DateTransmitted = new DateTime(1984, 2, 21, 20, 30, 0),
                FullText = "this is what the original message said",
                TicketNumbers = new [] {
                    new OneCallMarkoutAuditTicketNumber {
                        RequestNumber = 1234,
                        MessageType = _routine
                    },
                    new OneCallMarkoutAuditTicketNumber {
                        RequestNumber = 4321,
                        MessageType = _routine
                    },
                    new OneCallMarkoutAuditTicketNumber {
                        RequestNumber = 666,
                        MessageType = _emergency
                    }
                }
            };
        }

        private IEnumerable<OneCallMarkoutTicket> BuildTickets()
        {
            return new[] {
                new OneCallMarkoutTicket {
                    Id = 1,
                    RequestNumber = 4321,
                    MessageType = _emergency
                },
                new OneCallMarkoutTicket {
                    Id = 2,
                    RequestNumber = 666,
                    MessageType = _emergency
                }
            };
        }

        #endregion

        [TestMethod]
        public void TestDisposeDisposesOfInnerEmailer()
        {
            _target.Dispose();

            _innerEmailer.Verify(x => x.Dispose());
        }

        [TestMethod]
        public void TestSendMessageGeneratesMessageAsExpected()
        {
            var audit = BuildAudit();
            var tickets = BuildTickets();
            string message = null;
            _innerEmailer
                .Setup(x => x.SendMessage(AuditFailureEmailer.SUBJECT, It.IsAny<string>(), true))
                .Callback<string, string, bool>((sub, msg, html) => message = msg);
            _ticketRepository
                .Setup(x => x.Where(It.IsAny<Expression<Func<OneCallMarkoutTicket, bool>>>()))
                .Returns<Expression<Func<OneCallMarkoutTicket, bool>>>(fn => tickets.Where(fn.Compile()).AsQueryable());

            _target.SendMessage(audit);

            Assert.AreEqual(EXPECTED_MESSAGE, message);
        }
    }
}
