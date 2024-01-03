using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.MarkoutTickets
{
    [TestClass]
    public class MarkoutAuditParserTest
    {
        #region Constants

        public const string SAMPLE_MESSAGE = @"0 
Date/Time :  9-28-15 at  0:21 

New Jersey One Call
SUMM
DAILY AUDIT OF TICKETS SENT ON 09/27/15

Date/Time: 09/28/2015 12:15:03 AM
Receiving Terminal: SCNJ7

Seq #   Ticket #          Seq #   Ticket #                                
----- ---------------     ----- ---------------                           
    1 152700041-ROUT|         4 152700153-ROUT|     
    2 152700054-EMER|         5 152700182-ROUT|     
    3 152700150-ROUT|         6 152700161-ROUT|     

* indicates ticket # is repeated

Total Tickets: 6

ROUT - ROUTINE                        |    5

EMER - EMERGENCY                      |    1


Please call (877) 256-2697 if this data does
not match the tickets you received on 09/27/15
",
            SAMPLE_RETRANSMIT = @"0 
Date/Time :  9-28-15 at  0:21 

New Jersey One Call
SUMM
DAILY AUDIT OF TICKETS SENT ON 09/27/15

Date/Time: 09/28/2015 12:15:03 AM
Receiving Terminal: SCNJ7

Seq #   Ticket #          Seq #   Ticket #                                
----- ---------------     ----- ---------------                           
    1 152700041-ROUT|         4 152700153-ROUT|     
    2 152700054-RETR|         5 152700182-ROUT|     
    3 152700150-ROUT|         6 152700161-ROUT|     

* indicates ticket # is repeated

Total Tickets: 6

ROUT - ROUTINE                        |    5

RETR - RETRANSMIT                     |    1


Please call (877) 256-2697 if this data does
not match the tickets you received on 09/27/15
",
            SAMPLE_EMERGENCY_RETRANSMIT = @"0 
Date/Time :  9-28-15 at  0:21 

New Jersey One Call
SUMM
DAILY AUDIT OF TICKETS SENT ON 09/27/15

Date/Time: 09/28/2015 12:15:03 AM
Receiving Terminal: SCNJ7

Seq #   Ticket #          Seq #   Ticket #                                
----- ---------------     ----- ---------------                           
    1 152700041-ROUT|         4 152700153-ROUT|     
    2 152700054-ERTR|         5 152700182-ROUT|     
    3 152700150-ROUT|         6 152700161-ROUT|     

* indicates ticket # is repeated

Total Tickets: 6

ROUT - ROUTINE                        |    5

ERTR - EMER RETRANSMIT                |    1


Please call (877) 256-2697 if this data does
not match the tickets you received on 09/27/15
",
            SAMPLE_UNKNOWN_MESSAGE_TYPE = @"0 
Date/Time :  9-28-15 at  0:21 

New Jersey One Call
SUMM
DAILY AUDIT OF TICKETS SENT ON 09/27/15

Date/Time: 09/28/2015 12:15:03 AM
Receiving Terminal: SCNJ7

Seq #   Ticket #          Seq #   Ticket #                                
----- ---------------     ----- ---------------                           
    1 152700041-ROUT|         4 152700153-ROUT|     
    2 152700054-BLAH|         5 152700182-ROUT|     
    3 152700150-ROUT|         6 152700161-ROUT|     

* indicates ticket # is repeated

Total Tickets: 6

ROUT - ROUTINE                        |    5

BLAH - vOv                            |    1


Please call (877) 256-2697 if this data does
not match the tickets you received on 09/27/15
";



        #endregion

        #region Private Members

        private MarkoutAuditParser _target;
        private Mock<IRepository<OneCallMarkoutMessageType>> _typeRepo;
        private IDateTimeProvider _dateTimeProvider;
        private DateTime _now;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_typeRepo = new Mock<IRepository<OneCallMarkoutMessageType>>()).Object);
            _container.Inject(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));

            _target = _container.GetInstance<MarkoutAuditParser>();
        }

        #endregion

        [TestMethod]
        public void TestParseReturnsAuditObjectWithTicketNumbersAndSuch()
        {
            var types = new[] {
                new OneCallMarkoutMessageType {Description = "rout"},
                new OneCallMarkoutMessageType {Description = "emer"},
            };
            _typeRepo
                .Setup(x => x.Where(It.IsAny<Expression<Func<OneCallMarkoutMessageType, bool>>>()))
                .Returns<Expression<Func<OneCallMarkoutMessageType, bool>>>(
                    fn => types.Where(fn.Compile()).AsQueryable());
            var message = new Mock<IMailMessage>();
            message.SetupGet(x => x.Body).Returns(SAMPLE_MESSAGE);
            var expectedTicketNumbers = new List<OneCallMarkoutAuditTicketNumber> {
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700041,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700054,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("emer")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700150,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700153,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700182,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700161,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                }
            };

            var audit = _target.Parse(message.Object);

            Assert.AreEqual(new DateTime(2015, 9, 28, 0, 21, 0), audit.DateTransmitted);
            Assert.AreEqual(_now, audit.DateReceived);
            Assert.AreEqual(SAMPLE_MESSAGE, audit.FullText);
            Assert.AreEqual(expectedTicketNumbers.Count, audit.TicketNumbers.Count);

            foreach (var num in expectedTicketNumbers)
            {
                Assert.IsTrue(
                    audit.TicketNumbers.Any(
                        n =>
                            n.RequestNumber == num.RequestNumber && n.MessageType == num.MessageType &&
                            n.CDCCode == num.CDCCode));
            }
        }

        [TestMethod]
        public void TestParseWorksWithRetransmitMessageType()
        {
            var types = new[] {
                new OneCallMarkoutMessageType {Description = "rout"},
                new OneCallMarkoutMessageType {Description = "emer"},
            };
            _typeRepo
                .Setup(x => x.Where(It.IsAny<Expression<Func<OneCallMarkoutMessageType, bool>>>()))
                .Returns<Expression<Func<OneCallMarkoutMessageType, bool>>>(
                    fn => types.Where(fn.Compile()).AsQueryable());
            var message = new Mock<IMailMessage>();
            message.SetupGet(x => x.Body).Returns(SAMPLE_RETRANSMIT);
            var expectedTicketNumbers = new List<OneCallMarkoutAuditTicketNumber> {
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700041,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700054,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700150,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700153,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700182,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700161,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                }
            };

            var audit = _target.Parse(message.Object);

            Assert.AreEqual(new DateTime(2015, 9, 28, 0, 21, 0), audit.DateTransmitted);
            Assert.AreEqual(_now, audit.DateReceived);
            Assert.AreEqual(SAMPLE_RETRANSMIT, audit.FullText);
            Assert.AreEqual(expectedTicketNumbers.Count, audit.TicketNumbers.Count);

            foreach (var num in expectedTicketNumbers)
            {
                Assert.IsTrue(
                    audit.TicketNumbers.Any(
                        n =>
                            n.RequestNumber == num.RequestNumber && n.MessageType == num.MessageType &&
                            n.CDCCode == num.CDCCode));
            }
        }

        [TestMethod]
        public void TestParseWorksWithEmergencyRetransmitMessageType()
        {
            var types = new[] {
                new OneCallMarkoutMessageType {Description = "rout"},
                new OneCallMarkoutMessageType {Description = "emer"},
            };
            _typeRepo
                .Setup(x => x.Where(It.IsAny<Expression<Func<OneCallMarkoutMessageType, bool>>>()))
                .Returns<Expression<Func<OneCallMarkoutMessageType, bool>>>(
                    fn => types.Where(fn.Compile()).AsQueryable());
            var message = new Mock<IMailMessage>();
            message.SetupGet(x => x.Body).Returns(SAMPLE_EMERGENCY_RETRANSMIT);
            var expectedTicketNumbers = new List<OneCallMarkoutAuditTicketNumber> {
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700041,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700054,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("emer")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700150,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700153,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700182,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                },
                new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = 152700161,
                    MessageType = types.Single(t => t.Description.ToLower().StartsWith("rout")),
                    CDCCode = "SCNJ7"
                }
            };

            var audit = _target.Parse(message.Object);

            Assert.AreEqual(new DateTime(2015, 9, 28, 0, 21, 0), audit.DateTransmitted);
            Assert.AreEqual(_now, audit.DateReceived);
            Assert.AreEqual(SAMPLE_EMERGENCY_RETRANSMIT, audit.FullText);
            Assert.AreEqual(expectedTicketNumbers.Count, audit.TicketNumbers.Count);

            foreach (var num in expectedTicketNumbers)
            {
                Assert.IsTrue(
                    audit.TicketNumbers.Any(
                        n =>
                            n.RequestNumber == num.RequestNumber && n.MessageType == num.MessageType &&
                            n.CDCCode == num.CDCCode));
            }
        }

        [TestMethod]
        public void TestParseThrowsUsefulExceptionForUnknownMessageType()
        {
            var types = new[] {
                new OneCallMarkoutMessageType {Description = "rout"},
                new OneCallMarkoutMessageType {Description = "emer"},
            };
            _typeRepo
                .Setup(x => x.Where(It.IsAny<Expression<Func<OneCallMarkoutMessageType, bool>>>()))
                .Returns<Expression<Func<OneCallMarkoutMessageType, bool>>>(
                    fn => types.Where(fn.Compile()).AsQueryable());
            var message = new Mock<IMailMessage>();
            message.SetupGet(x => x.Body).Returns(SAMPLE_UNKNOWN_MESSAGE_TYPE);

            var thrown = false;

            try
            {
                _target.Parse(message.Object);
            }
            catch (Exception e)
            {
                thrown = true;
                Assert.AreEqual(String.Format(MarkoutAuditParser.UNKNOWN_MESSAGE_TYPE_FORMAT, "blah"), e.Message);
            }
            finally
            {
                Assert.IsTrue(thrown, "Exception not thrown when expected.");
            }
        }
    }
}