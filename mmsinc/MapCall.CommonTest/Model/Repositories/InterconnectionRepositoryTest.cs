using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class InterconnectionRepositoryTest : InMemoryDatabaseTest<Interconnection, InterconnectionRepository>
    {
        #region Fields

        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
        }

        #endregion

        [TestMethod]
        public void TestGetInterconnectionsThatHaveContractsExpiringInXDaysDoesExactlyWhatItsNameSays()
        {
            var today = new DateTime(2018, 6, 1, 23, 1,
                1); // Random time, the repository should be querying for "Today" and stripping the time off.
            var is29DaysLater = today.Date.AddDays(29);
            var is184DaysLater = today.Date.AddDays(184);
            var is30DaysLater = today.Date.AddDays(30);
            var is90DaysLater = today.Date.AddDays(90);
            var is180DaysLater = today.Date.AddDays(180);
            var is365DaysLater = today.Date.AddDays(365);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(today);

            var interconnectionFactory = GetFactory<InterconnectionFactory>();

            //shouldn't be found
            var aBad29DayInterconnection = interconnectionFactory.Create(new { ContractEndDate = is29DaysLater });
            var aBad184DayInterconnection = interconnectionFactory.Create(new { ContractEndDate = is184DaysLater });
            var badInterconnectionNoEndDate = interconnectionFactory.Create(new { ContractEndDate = (DateTime?)null });

            //Should be found 
            var a30DayInterconnection = interconnectionFactory.Create(new { ContractEndDate = is30DaysLater });
            var a90DayInterconnection = interconnectionFactory.Create(new { ContractEndDate = is90DaysLater });
            var a180DayInterconnection = interconnectionFactory.Create(new { ContractEndDate = is180DaysLater });
            var a365DayInterconnection = interconnectionFactory.Create(new { ContractEndDate = is365DaysLater });
            
            var results30 = Repository.GetInterconnectionsThatHaveContractsExpiringInXDays(30);
            var results90 = Repository.GetInterconnectionsThatHaveContractsExpiringInXDays(90);
            var results180 = Repository.GetInterconnectionsThatHaveContractsExpiringInXDays(180);
            var results365 = Repository.GetInterconnectionsThatHaveContractsExpiringInXDays(365);

            Assert.AreSame(a30DayInterconnection, results30.Single());
            Assert.AreSame(a90DayInterconnection, results90.Single());
            Assert.AreSame(a180DayInterconnection, results180.Single());
            Assert.AreSame(a365DayInterconnection, results365.Single());
        }
    }
}
