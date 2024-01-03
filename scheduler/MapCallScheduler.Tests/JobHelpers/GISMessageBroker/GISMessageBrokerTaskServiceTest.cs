using System;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.GISMessageBroker;
using MapCallScheduler.JobHelpers.GISMessageBroker.Tasks;
using MapCallScheduler.Tests.Library.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Producer;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.GISMessageBroker
{
    [TestClass]
    public class GISMessageBrokerTaskServiceTest : TaskServiceTestBase<GISMessageBrokerTaskService, IGISMessageBrokerTask>
    {
        #region Properties

        protected override Type[] ExpectedTaskTypes => new[] {
            typeof(SampleSiteTask), 
            typeof(SewerMainCleaningTask), 
            typeof(W1VServiceTask)
        };

        #endregion

        #region Private Methods

        protected override void InitializeContainer(ConfigurationExpression e)
        {
            e.For<ILog>().Mock();
            e.For<IKafkaServiceFactory<IKafkaProducer>>().Mock();
            e.For<IGISMessageBrokerConfiguration>().Mock();
            e.For<IGISMessageBrokerSerializer>().Mock();
            e.For<IDateTimeProvider>().Mock();
            e.For<IMapCallSchedulerConfiguration>().Mock();

            e.For<IRepository<SampleSite>>().Mock();
            e.For<IRepository<SewerMainCleaning>>().Mock();
            e.For<IRepository<Service>>().Mock();
        }

        #endregion
    }
}
