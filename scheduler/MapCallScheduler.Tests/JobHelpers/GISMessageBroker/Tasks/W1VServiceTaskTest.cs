using System;
using System.Collections.Generic;
using System.Linq;
using MapCallScheduler.JobHelpers.GISMessageBroker.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using MapCallService = MapCall.Common.Model.Entities.Service;

namespace MapCallScheduler.Tests.JobHelpers.GISMessageBroker.Tasks
{
    [TestClass]
    public class W1VServiceTaskTest : GISMessageBrokerTaskTestBase<MapCallService, W1VServiceTask>
    {
        #region Tests
        
        [TestMethod]
        public void TestRunDoesNotChangeNeedsToSyncOrLastSyncedAtWhenSyncingFails()
        {
            var services = new Dictionary<string, MapCallService> {
                {
                    "one", new MapCallService {
                        Id = 1,
                        NeedsToSync = true
                    }
                }
            };

            _repository.Setup(x => x.Linq).Returns(services.Values.AsQueryable());
            foreach (var service in services)
            {
                _serializer.Setup(x => x.Serialize(It.Is<MapCallService>(ss => ss.Id == services["one"].Id), Formatting.None)).Returns(service.Key);
                _kafkaProducer.Setup(x => x.SendMessage(_target.KafkaTopic, service.Key)).Throws<Exception>();
            }

            try
            {
                _target.Run();
            }
            catch {}
        }

        [TestMethod]
        public void TestRunDoesNotSyncRecordsWhichDoNotNeedSyncing()
        {
            var services = new Dictionary<string, MapCallService> {
                {
                    "one", new MapCallService {
                        NeedsToSync = false
                    }
                }
            };

            _repository.Setup(x => x.Linq).Returns(services.Values.AsQueryable());

            _target.Run();
        }

        [TestMethod]
        public void TestRunSyncsRecordsWhichNeedSyncingAndUpdatesSyncFieldsOnEntity()
        {
            var services = new Dictionary<string, MapCallService> {
                {
                    "one", new MapCallService {
                        Id = 1,
                        NeedsToSync = true
                    }
                }
            };

            _repository.Setup(x => x.Linq).Returns(services.Values.AsQueryable());
            foreach (var service in services)
            {
                _serializer
                   .Setup(x => x.Serialize(It.Is<MapCallService>(ss => ss.Id == services["one"].Id), Formatting.None))
                   .Returns(service.Key);

                _kafkaProducer
                   .Setup(x => x.SendMessage(_target.KafkaTopic, service.Key));
            }

            _repository
               .Setup(x => x.Save(It.Is<List<MapCallService>>(ss => ss.All(xx => xx.NeedsToSync == false && xx.LastSyncedAt == _now))));

            _target.Run();
        }

        [TestMethod]
        [DataRow(true, false, "prod.mc.premise.outbound")]
        [DataRow(false, true, "qa.mc.premise.outbound")]
        [DataRow(false, false, "dev.mc.premise.outbound")]
        public void TestKafkaTopicReturnsCorrectTopicNameBasedOnFlags(bool isProduction, bool isStaging, string topic)
        {
            _schedulerConfiguration.Setup(s => s.IsProduction).Returns(isProduction);
            _schedulerConfiguration.Setup(s => s.IsStaging).Returns(isStaging);
            Assert.AreEqual(topic, _target.KafkaTopic);
        }
        
        #endregion
    }
}
