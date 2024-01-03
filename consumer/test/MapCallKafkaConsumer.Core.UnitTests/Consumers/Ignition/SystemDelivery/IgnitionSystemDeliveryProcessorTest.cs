using log4net;
using MapCallKafkaConsumer.Consumers.Ignition.SystemDelivery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using Moq;
using MapCall.Common.Model.Entities;
using MapCallKafkaConsumer.Core.UnitTests.Testing;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using StructureMap;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using MapCall.Common.Model.Repositories;
using MMSINC.Utilities;

namespace MapCallKafkaConsumer.Core.UnitTests.Consumers.Ignition.SystemDelivery
{
    [TestClass]
    public class IgnitionSystemDeliveryProcessorTest : MapCallKafkaConsumerInMemoryDatabaseTest<SystemDeliveryIgnitionEntry, IRepository<SystemDeliveryIgnitionEntry>>
    {
        #region Private Members

        private IIgnitionSystemDeliveryProcessor _target;

        #endregion

        #region Init / Cleanup
        
        [TestInitialize]
        public void InitializeTest()
        {
            _target = Container.GetInstance<IgnitionSystemDeliveryProcessor>();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            e.For<ILog>().Use(Mock.Of<ILog>());
            e.For<IUnitOfWorkFactory>().Use<TestUnitOfWorkFactory>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IDateTimeProvider>().Use<DateTimeProvider>();
        }

        #endregion

        [TestMethod]
        [DeploymentItem(@"TestData\systemdelivery.json")]
        public void Process_InstantiatesAListOfSystemDeliveryIgnitionEntriesAndSavesThem()
        {
            var systemDeliveryJson = File.ReadAllText("systemdelivery.json");
            var systemDelivery = _target.HydrateMessage(systemDeliveryJson);
            
            _target.Process(JsonConvert.SerializeObject(systemDelivery));
            var entries = Repository.GetAll().ToList();

            Assert.IsInstanceOfType(systemDelivery, typeof(Model.SystemDelivery));
            Assert.AreEqual(systemDelivery.SystemDeliveryEntry.FacilityEntries.Length, entries.Count);
        }
    }
}
