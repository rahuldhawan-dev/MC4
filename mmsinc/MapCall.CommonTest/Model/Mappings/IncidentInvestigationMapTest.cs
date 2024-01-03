using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class IncidentInvestigationMapTest : InMemoryDatabaseTest<IncidentInvestigation>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<DateTimeProvider>();
        }

        private IncidentInvestigation EvictAndRequery(IncidentInvestigation model)
        {
            // Evict and requery to ensure the database is being queried.
            Session.Evict(model);
            return Session.Query<IncidentInvestigation>().Single(x => x.Id == model.Id);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestDeletingAnInvestigationDeletesTheManyToManyRecordsThatLinksUsers()
        {
            var user = GetFactory<UserFactory>().Create();
            var incident = GetFactory<IncidentFactory>().Create();
            var invest = GetFactory<IncidentInvestigationFactory>().Create(new {Incident = incident});
            invest.RootCauseFindingPerformedByUsers.Add(user);
            Session.Save(invest);
            Session.Flush();

            Session.Evict(incident);
            invest = EvictAndRequery(invest);
            Session.Delete(invest);
            Session.Flush();

            Assert.IsNull(Session.QueryOver<IncidentInvestigation>().Where(x => x.Id == invest.Id).SingleOrDefault(),
                "Investigation must be deleted");
            // Quite honestly, I have no clue how to test this as we don't get direct access to the many-to-many table
            // in nhibernate. Can probably, at the very least, use the interceptors to ensure the sql commands are ran.
            // Because this ticket is being rushed out, I don't have time to do this. -Ross 4/23/2020
            Assert.Inconclusive("Figure out how to test this.");
        }

        [TestMethod]
        public void TestDeletingAnInvestigationDoesNotDeleteTheIncident()
        {
            var incident = GetFactory<IncidentFactory>().Create();
            var invest = GetFactory<IncidentInvestigationFactory>().Create(new {Incident = incident});

            Session.Evict(incident);
            invest = EvictAndRequery(invest);
            Session.Delete(invest);
            Session.Flush();

            Assert.IsNotNull(Session.QueryOver<Incident>().Where(x => x.Id == incident.Id).SingleOrDefault(),
                "Incident must still exist");
            Assert.IsNull(Session.QueryOver<IncidentInvestigation>().Where(x => x.Id == invest.Id).SingleOrDefault(),
                "Investigation must be deleted");
        }

        #endregion
    }
}
