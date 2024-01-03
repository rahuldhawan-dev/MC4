using log4net;
using MapCallKafkaConsumer.Consumers.Lims.SequenceNumber;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using Moq;
using System.IO;
using MapCall.Common.Model.Entities;
using MapCall.LIMS.Model.Entities;
using MapCallKafkaConsumer.Core.UnitTests.Testing;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using StructureMap;
using Newtonsoft.Json;
using MapCall.Common.Model.Repositories;
using MMSINC.Utilities;

namespace MapCallKafkaConsumer.Core.UnitTests.Consumers.Lims.SequenceNumber
{
    [TestClass]
    public class LimsSequenceNumberProcessorTest : MapCallKafkaConsumerInMemoryDatabaseTest<SampleSite, IRepository<SampleSite>>
    {
        #region Private Members

        private LimsSequenceNumberProcessor _target;

        #endregion

        #region Init / Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = Container.GetInstance<LimsSequenceNumberProcessor>();
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

        #region Tests

        [TestMethod]
        public void RetrieveEntity_ReturnsNull_WhenLocationsSampleSiteIdIsNull()
        {
            GetEntityFactory<SampleSite>().CreateList(3);

            using (var unitOfWork = Container.GetInstance<TestUnitOfWorkFactory>().Build())
            {
                var location = new Location {
                    LocationSequenceNumber = 321
                };

                var sampleSite = _target.RetrieveEntity(unitOfWork, location);

                Assert.IsNull(sampleSite);
            }
        }

        [TestMethod]
        public void RetrieveEntity_ReturnsNull_WhenLocationsSampleSiteIdIsNotFound()
        {
            GetEntityFactory<SampleSite>().CreateList();

            using (var unitOfWork = Container.GetInstance<TestUnitOfWorkFactory>().Build())
            {
                var location = new Location {
                    SampleSiteId = "4",
                    LocationSequenceNumber = 321
                };

                var sampleSite = _target.RetrieveEntity(unitOfWork, location);

                Assert.IsNull(sampleSite);
            }
        }

        [TestMethod]
        public void RetrieveEntity_ReturnsSampleSite_WhenLocationsSampleSiteIdIsFound()
        {
            var sampleSite2 = GetEntityFactory<SampleSite>().CreateList(3)[1];

            using (var unitOfWork = Container.GetInstance<TestUnitOfWorkFactory>().Build())
            {
                var location = new Location {
                    SampleSiteId = sampleSite2.Id.ToString(),
                    LocationSequenceNumber = 321
                };

                var sampleSite = _target.RetrieveEntity(unitOfWork, location);

                Assert.IsNotNull(sampleSite);
                Assert.AreEqual(sampleSite2.Id, sampleSite.Id);
            }
        }

        [TestMethod]
        [DeploymentItem(@"TestData\location.json")]
        public void Hydrate_InstantiatesALocationWithCorrectSequenceNumber()
        {
            var locationJson = File.ReadAllText("location.json");

            var location = _target.HydrateMessage(locationJson);

            Assert.IsInstanceOfType(location, typeof(Location));
            Assert.AreEqual(62721, location.LocationSequenceNumber);
        }

        [TestMethod]
        public void MapMessageToEntity_MapsSequenceNumber_WhenSourceIsNotNullAndIsDifferentThanSampleSite()
        {
            var location = new Location {
                LocationSequenceNumber = 1
            };

            var sampleSite = new SampleSite {
                LimsSequenceNumber = null
            };

            _target.MapMessageToEntity(location, sampleSite);

            Assert.AreEqual(sampleSite.LimsSequenceNumber, location.LocationSequenceNumber);
        }

        [TestMethod]
        public void MapMessageToEntity_DoesNotMapsSequenceNumber_WhenSourceIsNull()
        {
            var location = new Location {
                LocationSequenceNumber = null
            };

            var sampleSite = new SampleSite {
                LimsSequenceNumber = 1
            };

            _target.MapMessageToEntity(location, sampleSite);

            Assert.AreEqual(1, sampleSite.LimsSequenceNumber);
            Assert.AreNotEqual(sampleSite.LimsSequenceNumber, location.LocationSequenceNumber);
        }

        [TestMethod]
        public void Process_UpdatesSampleSite_WithSyncedLimsSequenceNumber()
        {
            var sampleSiteId = 1;

            var location = new Location {
                SampleSiteId = sampleSiteId.ToString(),
                LocationSequenceNumber = 321
            };

            GetEntityFactory<SampleSite>().Create();

            _target.Process(JsonConvert.SerializeObject(location));

            Session.Clear();
            Session.Flush();

            var sampleSite = Repository.Find(sampleSiteId);

            Assert.AreEqual(sampleSite.LimsSequenceNumber, location.LocationSequenceNumber);
        }

        [TestMethod]
        public void Process_SkipsUpdate_WhenMatchingEntityCannotBeFound()
        {
            var location = new Location {
                SampleSiteId = "10",
                LocationSequenceNumber = 321
            };

            GetEntityFactory<SampleSite>().Create();

            _target.Process(JsonConvert.SerializeObject(location));

            Session.Clear();
            Session.Flush();

            var sampleSite = Repository.Find(1);

            Assert.IsNull(sampleSite.LimsSequenceNumber);
        }

        #endregion
    }
}
