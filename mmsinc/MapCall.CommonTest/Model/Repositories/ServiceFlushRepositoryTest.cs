using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class ServiceFlushRepositoryTest : InMemoryDatabaseTest<ServiceFlush, ServiceFlushRepository>
    {
        #region Fields

        private Mock<IDateTimeProvider> _dateTimeProvider;
 
        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(_dateTimeProvider.Object);

            // These are needed due to property injection on Service, but they
            // aren't actually used by the test.
            _container.Inject(new Mock<IAuthenticationService<User>>().Object);
            _container.Inject(new Mock<ITapImageRepository>().Object);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IServiceRepository>().Use<ServiceRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetServiceFlushNotReceivedAfterTwoWeeksReturnsExpectedResults()
        {
            // I'm not gonna try to list all of the rules in the test name, it'll scroll for days.
            // This method should return results that meet all of the following criteria:
            //  - The sample status is not "Results Received"
            //  - The sample date is > 14 days old
            //  - The HasSentNotification property is false

            // The repo should convert this date to "today"(midnight), that's why there is random hour/min/sec here.
            var today = new DateTime(2018, 5, 15, 12, 4, 21); 
            var twoWeeksAgo = today.Date.AddDays(-14);

            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(today);

            var receivedStatus = GetFactory<ResultsReceivedServiceFlushSampleStatusFactory>().Create();
            var otherStatus = GetFactory<TakenServiceFlushSampleStatusFactory>().Create();

            var flush = GetFactory<ServiceFlushFactory>().Create();
            flush.HasSentNotification = false;
            flush.SampleStatus = otherStatus;
            flush.SampleDate = twoWeeksAgo;

            Assert.AreSame(flush, Repository.GetServiceFlushNotReceivedAfterTwoWeeks().Single());

            // All variations of this should return no results.
            flush.HasSentNotification = true;
            Repository.Save(flush);
            Assert.IsFalse(Repository.GetServiceFlushNotReceivedAfterTwoWeeks().Any());

            flush.HasSentNotification = false;
            flush.SampleDate = twoWeeksAgo.AddDays(1);
            Repository.Save(flush);
            Assert.IsFalse(Repository.GetServiceFlushNotReceivedAfterTwoWeeks().Any());

            flush.SampleDate = twoWeeksAgo;
            flush.SampleStatus = receivedStatus;
            Repository.Save(flush);
            Assert.IsFalse(Repository.GetServiceFlushNotReceivedAfterTwoWeeks().Any());
        }

        #endregion
    }
}
