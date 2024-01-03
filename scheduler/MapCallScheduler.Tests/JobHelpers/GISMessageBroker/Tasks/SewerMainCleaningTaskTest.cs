using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.GISMessageBroker.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapCallScheduler.Tests.JobHelpers.GISMessageBroker.Tasks
{
    [TestClass]
    public class SewerMainCleaningTaskTest : GISMessageBrokerTaskTestBase<SewerMainCleaning, SewerMainCleaningTask>
    {
        #region Tests
        
        [TestMethod]
        public void TestRunSyncsRecordsWhichNeedSyncingAndUpdatesSyncFieldsOnEntity()
        {
            var smc = new SewerMainCleaning {
                NeedsToSync = true,
                Date = DateTime.Now,
                Opening1 = new SewerOpening { OpeningNumber = "OpeningNumber1" },
                Opening2 = new SewerOpening { OpeningNumber = "OpeningNumber2" }
            };
            var soc = new SewerOpeningConnection {
                UpstreamOpening = smc.Opening1,
                DownstreamOpening = smc.Opening2,
                InspectionFrequency = 5,
                InspectionFrequencyUnit = new RecurringFrequencyUnit {
                    Id = RecurringFrequencyUnit.Indices.MONTH,
                    Description = RecurringFrequencyUnit.MONTH
                }
            };
            smc.Opening1.UpstreamSewerOpeningConnections.Add(soc);
            smc.InspectionType = new SewerMainInspectionType {
                Id = SewerMainInspectionType.Indices.MAIN_CLEANING_PM,
                Description = "Main Cleaning Pm"
            };
            var sewerMainCleaningRecords = new List<SewerMainCleaning> { smc };

            _repository.Setup(x => x.Linq).Returns(sewerMainCleaningRecords.AsQueryable());
            _target.Run();
            
            Assert.IsFalse(smc.NeedsToSync);
            Assert.IsNotNull(smc.LastSyncedAt);
        }

        [TestMethod]
        public void TestRunDoesNotSyncsSewerMainCleaningRecordsWithSmokeTestInspectionType()
        {
            var smc = new SewerMainCleaning {
                NeedsToSync = true,
                Date = DateTime.Now,
                Opening1 = new SewerOpening { OpeningNumber = "OpeningNumber1" },
                Opening2 = new SewerOpening { OpeningNumber = "OpeningNumber2" }
            };
            var soc = new SewerOpeningConnection {
                UpstreamOpening = smc.Opening1,
                DownstreamOpening = smc.Opening2,
                InspectionFrequency = 5,
                InspectionFrequencyUnit = new RecurringFrequencyUnit {
                    Id = RecurringFrequencyUnit.Indices.MONTH,
                    Description = RecurringFrequencyUnit.MONTH
                }
            };

            smc.Opening1.UpstreamSewerOpeningConnections.Add(soc);
            smc.InspectionType = new SewerMainInspectionType {
                Id = SewerMainInspectionType.Indices.SMOKE_TEST,
                Description = "Smoke Test"
            };

            var sewerMainCleaningRecords = new List<SewerMainCleaning> { smc };

            _repository.Setup(x => x.Linq).Returns(sewerMainCleaningRecords.AsQueryable());

            _target.Run();

            Assert.IsTrue(smc.NeedsToSync);
            Assert.IsNull(smc.LastSyncedAt);
        }

        [TestMethod]
        public void TestRunDoesNotSyncsSewerMainCleaningRecordsWithNullInspectionType()
        {
            var smc = new SewerMainCleaning {
                NeedsToSync = true,
                Date = DateTime.Now,
                Opening1 = new SewerOpening { OpeningNumber = "OpeningNumber1" },
                Opening2 = new SewerOpening { OpeningNumber = "OpeningNumber2" }
            };
            var soc = new SewerOpeningConnection {
                UpstreamOpening = smc.Opening1,
                DownstreamOpening = smc.Opening2,
                InspectionFrequency = 5,
                InspectionFrequencyUnit = new RecurringFrequencyUnit {
                    Id = RecurringFrequencyUnit.Indices.MONTH,
                    Description = RecurringFrequencyUnit.MONTH
                }
            };
            smc.Opening1.UpstreamSewerOpeningConnections.Add(soc);
            var sewerMainCleaningRecords = new List<SewerMainCleaning> { smc };

            _repository.Setup(x => x.Linq).Returns(sewerMainCleaningRecords.AsQueryable());
            _target.Run();

            Assert.IsTrue(smc.NeedsToSync);
            Assert.IsNull(smc.LastSyncedAt);
        }

        [TestMethod]
        public void TestRunDoesNotSyncRecordsWhichDoNotNeedSyncing()
        {
            var sites = new Dictionary<string, SewerMainCleaning> {
                { "one", new SewerMainCleaning { NeedsToSync = false } }
            };

            _repository.Setup(x => x.Linq).Returns(sites.Values.AsQueryable());

            _target.Run();
        }

        #endregion
    }
}
