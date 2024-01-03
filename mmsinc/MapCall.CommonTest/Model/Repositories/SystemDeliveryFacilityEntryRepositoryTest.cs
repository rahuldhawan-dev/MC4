using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class SystemDeliveryFacilityEntryRepositoryTest : MapCallMvcSecuredRepositoryTestBase<SystemDeliveryFacilityEntry, TestSystemDeliveryFacilityEntryRepository, User>
    {
        #region Fields

        private DateTime _now;
        
        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use(new TestDateTimeProvider(_now = DateTime.Now));
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetEntriesForFacility()
        {
            var startDate = _now.AddMonths(-6).Date;
            var endDate = _now.Date;
            var facility = GetEntityFactory<Facility>().Create();
            var systemDeliveryEntry = GetEntityFactory<SystemDeliveryEntry>().Create(new {WeekOf = startDate, IsValidated = true});
            var systemDeliveryEntryTwo = GetEntityFactory<SystemDeliveryEntry>().Create(new {WeekOf = endDate, IsValidated = true});
            var systemDeliveryEntryThree = GetEntityFactory<SystemDeliveryEntry>().Create(new {WeekOf = endDate, IsValidated = false});
            var systemDeliverFacilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new { Facility = facility, SystemDeliveryEntry = systemDeliveryEntry, EntryDate = startDate});
            var systemDeliverFacilityEntryTwo = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new { Facility = facility, SystemDeliveryEntry = systemDeliveryEntryTwo, EntryDate = endDate});
            var systemDeliverFacilityEntryThree = GetEntityFactory<SystemDeliveryFacilityEntry>().Create(new { Facility = facility, SystemDeliveryEntry = systemDeliveryEntryThree, EntryDate = endDate});

            var results = Repository.GetEntriesForFacility(facility.Id, startDate, endDate).ToArray();

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(systemDeliverFacilityEntry.SystemDeliveryEntryType.Description, results.First().EntryType);
            Assert.AreEqual(systemDeliverFacilityEntryTwo.SystemDeliveryEntryType.Description, results[1].EntryType);
            Assert.AreEqual(systemDeliveryEntry.WeekOf, results.First().Date);
            Assert.AreEqual(systemDeliveryEntryTwo.WeekOf, results[1].Date);
        }

        #endregion
    }

    public class TestSystemDeliveryFacilityEntryRepository : SystemDeliveryFacilityEntryRepository
    {
        public TestSystemDeliveryFacilityEntryRepository(ISession session, IContainer container, IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container, authenticationService, roleRepo) { }
    }
}
