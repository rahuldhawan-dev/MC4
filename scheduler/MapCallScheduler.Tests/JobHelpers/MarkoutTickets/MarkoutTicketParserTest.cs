using System;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.MarkoutTickets
{
    [TestClass]
    public class MarkoutTicketParserTest
    {
        #region Constants

        public const string SAMPLE_MESSAGE = @"New Jersey One Call System        SEQUENCE NUMBER 0074    CDC = SCNJ7 

Transmit:  Date: 09/25/15   At: 15:57 

*** R O U T I N E         *** Request No.: 152682238 

Operators Notified: 
BAN     = VERIZON                       CC4     = COMCAST CABLEVISION OF NE      
ESA     = EATONTOWN SEWERAGE AUTHOR     GP9     = JERSEY CENTRAL POWER & LI      
NJN     = NEW JERSEY NATURAL GAS CO     SCNJ7   = NJ AMERICAN WATER              

Start Date/Time:    10/01/15   At 08:00  Expiration Date: 12/03/15 

Location Information: 
County: MONMOUTH               Municipality: EATONTOWN 
Subdivision/Community:  
Street:               56 CEDAR ST 
Nearest Intersection: PARKER RD 
Other Intersection:    
Lat/Lon: 
Type of Work: TERMITE TREATMENT 
Block:                Lot:                Depth: 4FT 
Extent of Work: 10FT PERIMETER OF FOUNDATION. 
Remarks:  
  Working For Contact:  LIZ GAVIN 

Working For: HOMEOWNER 
Address:     56 CEDAR ST 
City:        EATONTOWN, NJ  07724 
Phone:       732-460-0362   Ext:  

Excavator Information: 
Caller:      SHIRLEY GIMBUT 
Phone:       609-683-1444   Ext:  

Excavator:   WESTERN PEST SERVICES 
Address:     423 SHREWSBURY AVE 
City:        SHREWSBURY, NJ  07702 
Phone:       609-683-1444   Ext:          Fax:  732-741-7329 
Cellular:    732-741-3311 
Email:       sgimbut@westernpest.com 
End Request",
            SAMPLE_UPDATE = @"New Jersey One Call System        SEQUENCE NUMBER 0003    CDC = SCNJ7 

Transmit:  Date: 09/28/15   At: 06:59 

*** U P D A T E           *** Request No.: 152710027  Of Request No.: 152642070 

Operators Notified: 
BAN     = VERIZON                       CC4     = COMCAST CABLEVISION OF NE      
GP9     = JERSEY CENTRAL POWER & LI     KEA     = KEANSBURG MUA                  
MCB     = MONMOUTH COUNTY BAYSHORE      NJN     = NEW JERSEY NATURAL GAS CO      
SCNJ7   = NJ AMERICAN WATER             SHW     = SHORELANDS WATER COMPANY,      
TK8     = CABLEVISION OF RARITAN VA      

Start Date/Time:    09/25/15   At 00:15  Expiration Date: 11/27/15 

Location Information: 
County: MONMOUTH               Municipality: KEANSBURG 
Subdivision/Community:  
Street:               242 - 417 MAIN ST 
Nearest Intersection: LANCASTER AVE 
Other Intersection:    
Lat/Lon: 
Type of Work: INSTALL GAS SERVICE 
Block:                Lot:                Depth: 6FT 
Extent of Work: CURB TO CURB.  CURB TO ENTIRE PROPERTY.  CURB TO 20FT BEHIND
  OPPOSITE CURB. 
Remarks:  
  CONSECUTIVE ALL 
  CALLER STATES WATER AND GAS MIS- MARKED 
  Working For Contact:  CHARLES MCVEY 

Working For: NJNG 
Address:     1415 WYCKOFF ROAD 
City:        WALL, NJ  07719 
Phone:       732-919-8156   Ext:  

Excavator Information: 
Caller:      RICK HOFFMAN 
Phone:       732-222-4400   Ext:  

Excavator:   J. F. KIELY CONSTRUCTION 
Address:     700 MCCLELLAN ST 
City:        LONG BRANCH, NJ  07740 
Phone:       732-222-4400   Ext:          Fax:  732-229-2353 
Cellular:     
Email:       khoey@jfkiely.com 
End Request",
            SAMPLE_CANCELLATION = @"New Jersey One Call System        SEQUENCE NUMBER 0030    CDC = SCNJ7 

Transmit:  Date: 09/29/15   At: 11:58 

*** U P D A T E           *** Request No.: 152721385  Of Request No.: 152712881 
                          *** Cancellation Of Request No.: 152712881 

Operators Notified: 
BAN     = VERIZON                       CC4     = COMCAST CABLEVISION OF NE      
GP9     = JERSEY CENTRAL POWER & LI     LBS     = LONG BRANCH SEWERAGE AUTH      
NJN     = NEW JERSEY NATURAL GAS CO     SCNJ7   = NJ AMERICAN WATER              

Start Date/Time:    10/02/15   At 00:01  Expiration Date: 12/04/15 

Location Information: 
County: MONMOUTH               Municipality: LONG BRANCH 
Subdivision/Community:  
Street:               165 KINGSLEY ST 
Nearest Intersection: PATTEN AVE 
Other Intersection:    
Lat/Lon: 
Type of Work: INSTALL PAVEMENT 
Block: 454            Lot:  14.01         Depth: 3FT 
Extent of Work: 13FT PERIMETER OF FOUNDATION.
  CANCEL:  09/29/15 11:57 am: EOW 
Remarks:  
  Working For Contact:  LEONARD GRAY 

Working For: HOMEOWNER 
Address:     165 KINGSLEY ST 
City:        LONG BRANCH, NJ  07740 
Phone:       732-229-2918   Ext:  

Excavator Information: 
Caller:      LEONARD GRAY 
Phone:       732-229-2918   Ext:  

Excavator:   LEONARD GRAY 
Address:     165 KINGSLEY ST 
City:        LONG BRANCH, NJ  07740 
Phone:       732-229-2918   Ext:          Fax:  732-222-7656 
Cellular:    732-299-1759 
Email:       leonardgray@gmail.com 
End Request",
            SAMPLE_COMPANY_EXCAVATOR = @"New Jersey One Call System        SEQUENCE NUMBER 0056    CDC = SCNJ7 

Transmit:  Date: 09/21/15   At: 12:52 

*** R O U T I N E         *** Request No.: 152641449 

Operators Notified: 
BAN     = VERIZON                       CC4     = COMCAST CABLEVISION OF NE      
GP9     = JERSEY CENTRAL POWER & LI     LBS     = LONG BRANCH SEWERAGE AUTH      
NJN     = NEW JERSEY NATURAL GAS CO     SCNJ7   = NJ AMERICAN WATER              

Start Date/Time:    09/25/15   At 08:00  Expiration Date: 11/27/15 

Location Information: 
County: MONMOUTH               Municipality: LONG BRANCH 
Subdivision/Community:  
Street:               0 OCEAN TER 
Nearest Intersection: OCEAN BLVD 
Other Intersection:   MARINE TER 
Lat/Lon: 
Type of Work: INSTALL WATER SERVICE 
Block:                Lot:                Depth: 10FT 
Extent of Work: M/O ENTIRE LENGTH OF OCEAN TER FROM C/L OF OCEAN BLVD TO C/L
  OF MARINE TER INCLUDING ALL INTERSECTIONS.  CURB TO 20FT BEHIND ALL
  CURBS.  CURB TO CURB.  WORKING WESTERNMOST INTERSECTION OF MARINE TER. 
Remarks:  
  Working For Contact:  JEREMIAH HULSART 

Working For: NEW JERSEY AMERICAN WATER 
Address:     661 SHREWSBURY AVE 
City:        SHREWSBURY, NJ  07702 
Phone:       732-933-5941   Ext:  

Excavator Information: 
Caller:      JEREMIAH HULSART 
Phone:       732-933-5941   Ext:  

Excavator:   NEW JERSEY AMERICAN WATER 
Address:     661 SHREWSBURY AVE 
City:        SHREWSBURY, NJ  07702 
Phone:       732-933-5941   Ext:          Fax:  732-842-6234 
Cellular:    732-933-5941 
Email:       MARKOUTS-NJ7@MMSINC.COM 
End Request",
            SAMPLE_SHORELANDS = @"New Jersey One Call System        SEQUENCE NUMBER 0003    CDC = SHW 

Transmit:  Date: 04/06/17   At: 10:11 

*** R O U T I N E         *** Request No.: 170960699 

Operators Notified: 
BAN     = VERIZON                       CC4     = COMCAST CABLEVISION OF NE      
GP9     = JERSEY CENTRAL POWER & LI     IPMF3   = PENTA COMMUNICATIONS           
NJN     = NEW JERSEY NATURAL GAS CO     SHW     = SHORELANDS WATER COMPANY,      
USS     = SPRINT                         

Start Date/Time:    04/12/17   At 08:00  Expiration Date: 06/12/17 

Location Information: 
County: MONMOUTH               Municipality: HAZLET 
Subdivision/Community:  
Street:               4 DOGWOOD LN 
Nearest Intersection: BEERS ST 
Other Intersection:   MOAK DR 
Lat/Lon: 
Type of Work: LANDSCAPING 
Block: 213.1          Lot:  2             Depth: 2FT 
Extent of Work: CURB TO ENTIRE PROPERTY. 
Remarks:  
  Working For Contact:  ROBERT WILDS 

Working For: HOMEOWNER 
Address:     4 DOGWOOD LN 
City:        HAZLET, NJ  07730 
Phone:       732-522-8785   Ext:  

Excavator Information: 
Caller:      ROBERT WILDS 
Phone:       732-522-8785   Ext:  

Excavator:   ROBERT WILDS 
Address:     4 DOGWOOD LN 
City:        HAZLET, NJ  07730 
Phone:       732-522-8785   Ext:          Fax:  
Cellular:    732-522-8785 
Email:       ROBERT_A_WILDS@YAHOO.COM 
End Request";

        #endregion

        #region Private Members

        private MarkoutTicketParser _target;
        private Mock<IRepository<OneCallMarkoutMessageType>> _typeRepo;
        private TestRepository<OperatingCenter> _opCenterRepo;
        private Mock<IUserRepository> _userRepo;
        private IDateTimeProvider _dateTimeProvider;
        private IContainer _container;
        private DateTime _now;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_typeRepo = new Mock<IRepository<OneCallMarkoutMessageType>>()).Object);
            _container.Inject<IRepository<OperatingCenter>>(_opCenterRepo = new TestRepository<OperatingCenter>());
            _container.Inject((_userRepo = new Mock<IUserRepository>()).Object);
            _container.Inject(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));

            _target = _container.GetInstance<MarkoutTicketParser>();
        }

        #endregion

        private OneCallMarkoutTicket TestParse(string messageBody, string messageType, string opCode)
        {
            var message = new Mock<IMailMessage>();
            message.SetupGet(x => x.Body).Returns(messageBody);
            var type = new OneCallMarkoutMessageType {
                Description = messageType
            };
            var opCenter = new OperatingCenter {
                OperatingCenterCode = opCode
            };
            _typeRepo
                .Setup(x => x.Where(It.IsAny<Expression<Func<OneCallMarkoutMessageType, bool>>>()))
                .Returns<Expression<Func<OneCallMarkoutMessageType, bool>>>(fn => new[] {type}.Where(fn.Compile()).AsQueryable());
            _opCenterRepo.SetData(new [] {opCenter});

            var ticket = _target.Parse(message.Object);
            Assert.AreEqual(type, ticket.MessageType);
            Assert.AreEqual(opCenter, ticket.OperatingCenter);
            return ticket;
        }

        [TestMethod]
        public void TestParseAddsResponseAutomaticallyIfExcavatorIsCompany()
        {
            var mcAdmin = new User {UserName = "mcadmin"};
            _userRepo.Setup(r => r.Where(It.Is<Expression<Func<User, bool>>>(fn => fn.Compile().Invoke(mcAdmin)))).Returns(new [] {mcAdmin}.AsQueryable());

            var ticket = TestParse(SAMPLE_COMPANY_EXCAVATOR, "ROUTINE", "NJ7");

            Assert.AreEqual(1, ticket.Responses.Count());
            Assert.AreEqual(_now, ticket.Responses.First().CompletedAt);
            Assert.AreSame(mcAdmin, ticket.Responses.First().CompletedBy);
        }

        [TestMethod]
        public void TestParseReturnsTicketWithCorrectValues()
        {
            var ticket = TestParse(SAMPLE_MESSAGE, "ROUTINE", "NJ7");

            Assert.AreEqual(152682238, ticket.RequestNumber);
            Assert.AreEqual("MONMOUTH", ticket.CountyText);
            Assert.AreEqual("EATONTOWN", ticket.TownText);
            Assert.AreEqual("56 CEDAR ST", ticket.StreetText);
            Assert.AreEqual("PARKER RD", ticket.NearestCrossStreetText);
            Assert.AreEqual(new DateTime(2015, 9, 25, 15, 57, 0), ticket.DateTransmitted);
            Assert.AreEqual("TERMITE TREATMENT", ticket.TypeOfWork);
            Assert.AreEqual("HOMEOWNER", ticket.WorkingFor);
            Assert.AreEqual("WESTERN PEST SERVICES", ticket.Excavator);
            Assert.AreEqual("SCNJ7", ticket.CDCCode);
            Assert.AreEqual(_now, ticket.DateReceived);
            Assert.AreEqual(SAMPLE_MESSAGE, ticket.FullText);
        }

        [TestMethod]
        public void TestParseReturnsTicketWithCorrectValuesForCancellation()
        {
            var ticket = TestParse(SAMPLE_CANCELLATION, "CANCELLED", "NJ7");

            Assert.AreEqual(152721385, ticket.RequestNumber);
            Assert.AreEqual(152712881, ticket.RelatedRequestNumber);
            Assert.AreEqual("MONMOUTH", ticket.CountyText);
            Assert.AreEqual("LONG BRANCH", ticket.TownText);
            Assert.AreEqual("165 KINGSLEY ST", ticket.StreetText);
            Assert.AreEqual("PATTEN AVE", ticket.NearestCrossStreetText);
            Assert.AreEqual(new DateTime(2015, 9, 29, 11, 58, 0), ticket.DateTransmitted);
            Assert.AreEqual("INSTALL PAVEMENT", ticket.TypeOfWork);
            Assert.AreEqual("HOMEOWNER", ticket.WorkingFor);
            Assert.AreEqual("LEONARD GRAY", ticket.Excavator);
            Assert.AreEqual("SCNJ7", ticket.CDCCode);
            Assert.AreEqual(_now, ticket.DateReceived);
            Assert.AreEqual(SAMPLE_CANCELLATION, ticket.FullText);
        }

        [TestMethod]
        public void TestParseReturnsTicketWithCorrectValuesForUpdate()
        {
            var ticket = TestParse(SAMPLE_UPDATE, "UPDATE", "NJ7");

            Assert.AreEqual(152710027, ticket.RequestNumber);
            Assert.AreEqual(152642070, ticket.RelatedRequestNumber);
            Assert.AreEqual("MONMOUTH", ticket.CountyText);
            Assert.AreEqual("KEANSBURG", ticket.TownText);
            Assert.AreEqual("242 - 417 MAIN ST", ticket.StreetText);
            Assert.AreEqual("LANCASTER AVE", ticket.NearestCrossStreetText);
            Assert.AreEqual(new DateTime(2015, 9, 28, 6, 59, 0), ticket.DateTransmitted);
            Assert.AreEqual("INSTALL GAS SERVICE", ticket.TypeOfWork);
            Assert.AreEqual("NJNG", ticket.WorkingFor);
            Assert.AreEqual("J. F. KIELY CONSTRUCTION", ticket.Excavator);
            Assert.AreEqual("SCNJ7", ticket.CDCCode);
            Assert.AreEqual(_now, ticket.DateReceived);
            Assert.AreEqual(SAMPLE_UPDATE, ticket.FullText);
        }

        [TestMethod]
        public void TestCompanyExcavatorRegex()
        {
            var matchingValues = new[] {
                "N J AMERICAN WATER CO",
                "NEW JERSEY AMERCAN WATER COMP",
                "NEW JERSEY AMERICAN WATER",
                "NEW JERSEY AMERICAN WATER CO",
                "NEW JERSEY AMERICAN WATER CO.",
                "NEW JERSEY AMERICAN WATER COM",
                "NEW JERSEY AMERICAN WATER COMP",
                "NEW JERSEY AMERICAN WATER COMPANY",
                "NEW JERSEY AMERICAN WATRE",
                "NJ AMERICAN WATER",
                "NJ AMERICAN WATER CO",
                "NJ AMERICAN WATER CO.",
                "NJ AMERICAN WATER COMP",
                "NJ AMERICAN WATER ",
                "NJ AMERICAN WTR",
                "NJ AMERICAN WTR CO",
                "NJ AMERICAN WTR CO.",
                "NJAM WATER"
            };

            var nonMatchingValues = new[] {
                "DIPASQUALE FENCE",
                "DIRECT ELECTRIC LLC",
                "DISCOVER CONSTRUCTION LLC",
                "DISH NETWORK",
                "DISPOSAL SYSTEMS",
                "DISPOSAL SYSTEMS INC.",
                "DISTINCTIVE INC",
                "DOG GUARD OF CENTRAL NJ",
                "DOLPHIN HOMES"
            };

            foreach (var value in matchingValues)
            {
                Assert.IsTrue(MarkoutTicketParser.Regexes.COMPANY_IS_EXCAVATOR.IsMatch(value),
                    String.Format("Regex failed to match the string \"{0}\"", value));
            }

            foreach (var value in nonMatchingValues)
            {
                Assert.IsFalse(MarkoutTicketParser.Regexes.COMPANY_IS_EXCAVATOR.IsMatch(value));
            }
        }

        [TestMethod]
        public void TestOperatingCenterSHWIsTreatedAsNJ7()
        {
            var ticket = TestParse(SAMPLE_SHORELANDS, "ROUTINE", "NJ7");

            Assert.AreEqual("NJ7", ticket.OperatingCenter.OperatingCenterCode);
        }
    }
}
