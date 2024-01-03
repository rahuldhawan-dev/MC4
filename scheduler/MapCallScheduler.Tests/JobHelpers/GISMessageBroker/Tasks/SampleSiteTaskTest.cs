using System;
using System.Collections.Generic;
using System.Linq;
using MapCallScheduler.JobHelpers.GISMessageBroker.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using MapCallSampleSite = MapCall.Common.Model.Entities.SampleSite;

namespace MapCallScheduler.Tests.JobHelpers.GISMessageBroker.Tasks
{
    [TestClass]
    public class SampleSiteTaskTest : GISMessageBrokerTaskTestBase<MapCallSampleSite, SampleSiteTask>
    {
        #region Tests
        
        [TestMethod]
        public void TestRunDoesNotChangeNeedsToSyncOrLastSyncedAtWhenSyncingFails()
        {
            var sites = new Dictionary<string, MapCallSampleSite> {
                {
                    "one", new MapCallSampleSite {
                        Id = 1,
                        NeedsToSync = true
                    }
                }
            };

            _repository.Setup(x => x.Linq).Returns(sites.Values.AsQueryable());
            foreach (var site in sites)
            {
                _serializer.Setup(x => x.Serialize(It.Is<MapCallSampleSite>(ss => ss.Id == sites["one"].Id), Formatting.None)).Returns(site.Key);
                _kafkaProducer.Setup(x => x.SendMessage(SampleSiteTask.KAFKA_TOPIC, site.Key)).Throws<Exception>();
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
            var sites = new Dictionary<string, MapCallSampleSite> {
                {
                    "one", new MapCallSampleSite {
                        NeedsToSync = false
                    }
                }
            };

            _repository.Setup(x => x.Linq).Returns(sites.Values.AsQueryable());

            _target.Run();
        }

        [TestMethod]
        public void TestRunSyncsRecordsWhichNeedSyncingAndUpdatesSyncFieldsOnEntity()
        {
            var sites = new Dictionary<string, MapCallSampleSite> {
                {
                    "one", new MapCallSampleSite {
                        Id = 1,
                        NeedsToSync = true
                    }
                }
            };

            _repository.Setup(x => x.Linq).Returns(sites.Values.AsQueryable());
            foreach (var site in sites)
            {
                _serializer
                   .Setup(x => x.Serialize(It.Is<MapCallSampleSite>(ss => ss.Id == sites["one"].Id), Formatting.None))
                   .Returns(site.Key);

                _kafkaProducer
                   .Setup(x => x.SendMessage(SampleSiteTask.KAFKA_TOPIC, site.Key));
            }

            _repository
               .Setup(x => x.Save(It.Is<List<MapCallSampleSite>>(ss => ss.All(xx => xx.NeedsToSync == false && xx.LastSyncedAt == _now))));

            _target.Run();
        }

        #endregion
    }
}
