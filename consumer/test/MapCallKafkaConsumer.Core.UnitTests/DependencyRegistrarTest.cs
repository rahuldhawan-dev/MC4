using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using NHibernate;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCallKafkaConsumer.Consumers.Lims;
using MapCallKafkaConsumer.Consumers.Lims.SequenceNumber;
using MapCall.Common.Model.Entities;
using MapCallKafkaConsumer.Consumers.Ignition;
using MapCallKafkaConsumer.Consumers.Ignition.SystemDelivery;
using MMSINC.Utilities.Kafka.Producer;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Consumer;

namespace MapCallKafkaConsumer.Core.UnitTests
{
    [TestClass]
    public class DependencyRegistrarTest
    {
        #region Private Members

        private IContainer _container;

        #endregion

        #region Init / Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            DependencyRegistry.IsInTestMode = true;
            _container = new Container(new DependencyRegistry());
            _container.Configure(e =>
                e.For<ISession>()
                 .Singleton()
                 .Use(ctx => ctx.GetInstance<ISessionFactory>()
                                .OpenSession()));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DependencyRegistry.IsInTestMode = false;
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestAllRegistrations()
        {
            _container.GetInstance<IUnitOfWorkFactory>().ShouldBeOfType<UnitOfWorkFactory>();
            _container.GetInstance<ILimsSequenceNumberProcessor>().ShouldBeOfType<LimsSequenceNumberProcessor>();
            _container.GetInstance<IRepository<SampleSite>>().ShouldBeOfType<RepositoryBase<SampleSite>>();
            _container.GetInstance<IKafkaServiceFactory<IKafkaProducer>>().ShouldBeOfType<KafkaServiceFactory<KafkaProducer>>();
            _container.GetInstance<IKafkaServiceFactory<IKafkaConsumer>>().ShouldBeOfType<KafkaServiceFactory<KafkaConsumer>>();
            _container.GetInstance<ILimsConfiguration>().ShouldBeOfType<LimsConfiguration>();
            _container.GetInstance<IIgnitionConfiguration>().ShouldBeOfType<IgnitionConfiguration>();
            _container.GetInstance<IIgnitionSystemDeliveryProcessor>().ShouldBeOfType<IgnitionSystemDeliveryProcessor>();
        }

        #endregion
    }
}
