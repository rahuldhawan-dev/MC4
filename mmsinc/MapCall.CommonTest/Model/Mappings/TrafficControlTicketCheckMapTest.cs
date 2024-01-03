using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using NHibernate.Linq;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class TrafficControlTicketCheckMapTest : InMemoryDatabaseTest<TrafficControlTicketCheck>
    {
        #region Tests

        [TestMethod]
        public void TestDeletingACheckDoesNotDeleteTheTicket()
        {
            var ticket = GetFactory<TrafficControlTicketFactory>().Create();
            var check = GetFactory<TrafficControlTicketCheckFactory>().Create(new {TrafficControlTicket = ticket});
            //Session.Flush();
            Session.Evict(ticket);
            Session.Evict(check);

            // Sanity here
            ticket = Session.Query<TrafficControlTicket>().Single(x => x.Id == ticket.Id);
            check = Session.Query<TrafficControlTicketCheck>().Single(x => x.Id == check.Id);
            Assert.IsTrue(ticket.TrafficcControlTicketChecks.Contains(check));

            Session.Evict(ticket);
            Session.Delete(check);
            Session.Flush();

            ticket = Session.Query<TrafficControlTicket>().SingleOrDefault(x => x.Id == ticket.Id);
            Assert.IsNotNull(ticket, "The ticket should not have been deleted when the check was deleted.");
        }

        #endregion
    }
}
