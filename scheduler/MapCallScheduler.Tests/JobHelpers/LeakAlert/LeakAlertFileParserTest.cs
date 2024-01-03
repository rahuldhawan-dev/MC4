using System.Collections.Generic;
using MapCallScheduler.JobHelpers.LeakAlert;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallScheduler.Tests.JobHelpers.LeakAlert
{
    [TestClass]
    public class LeakAlertFileParserTest : SapFileParserTestBase<LeakAlertFileParser, LeakAlertFileRecord>
    {
        #region Properties

        protected override string SampleFile => "TestData/pcnExport.csv";

        protected override IEnumerable<LeakAlertFileRecord> SampleRecords => new[] {
            new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "2",
                DatePCNCreated = "2018-10-01T08:36:27Z",
                POIStatusId = "3",
                POIStatus = "Field Investigation Recommended",
                Latitude = "40.7001734",
                Longitude = "-74.21924929",
                SocketID1 = "HHSD-93",
                SocketID2 = "HHSD-94",
                DistanceFrom1 = "287.785153",
                DistanceFrom2 = "28.18123639",
                Note = "Reported on Oct 5, FC@5am Mar 26",
                SiteName = "hillside",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z"
            },
            new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "6",
                DatePCNCreated = "2018-10-02T08:43:53Z",
                POIStatusId = "3",
                POIStatus = "Field Investigation Recommended",
                Latitude = "40.70657306",
                Longitude = "-74.22746134",
                SocketID1 = "HHSD-262",
                SocketID2 = "HHSD-370",
                DistanceFrom1 = "0",
                DistanceFrom2 = "230.7681578",
                Note = "2019-02-07 Reported Oct 5 PPOI 10 updating group",
                SiteName = "hillside",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z"
            },
            new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "72",
                DatePCNCreated = "2018-10-07T08:52:38Z",
                POIStatusId = "3",
                POIStatus = "Field Investigation Recommended",
                Latitude = "40.68878834",
                Longitude = "-74.22246475",
                SocketID1 = "HHSD-149",
                SocketID2 = "HHSD-147",
                DistanceFrom1 = "408.0297541",
                DistanceFrom2 = "397.1717703",
                Note = "Group: PPOI 108. Reported 2018-11-29",
                SiteName = "hillside",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z"
            },
            new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "81",
                DatePCNCreated = "2018-10-09T08:45:58Z",
                POIStatusId = "3",
                POIStatus = "Field Investigation Recommended",
                Latitude = "40.70264505",
                Longitude = "-74.24442295",
                SocketID1 = "HHSD-245",
                SocketID2 = "HHSD-294",
                DistanceFrom1 = "469.6892003",
                DistanceFrom2 = "646.8346613",
                Note = "Group: PPOI 57. Reported 2018-11-30",
                SiteName = "hillside",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z"
            },
            new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "93",
                DatePCNCreated = "2018-10-12T08:56:57Z",
                POIStatusId = "3",
                POIStatus = "Field Investigation Recommended",
                Latitude = "40.70168201",
                Longitude = "-74.22372759",
                SocketID1 = "HHSD-492",
                SocketID2 = "HHSD-417",
                DistanceFrom1 = "0",
                DistanceFrom2 = "383.1721618",
                Note = "[AG 2020-03-28: +, EXEC, PPOI 2, Coef. 0.63, Dist. 1371ft, N: 22] keep tracking",
                SiteName = "hillside",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z"
            }
        };

        #endregion

        #region Private Methods

        protected override void CompareResult(LeakAlertFileRecord sampleRecord, LeakAlertFileRecord resultRecord)
        {
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.PersistedCorrelatedNoiseId);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.DatePCNCreated);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.POIStatusId);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.POIStatus);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.Latitude);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.Longitude);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.SocketID1);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.SocketID2);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.DistanceFrom1);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.DistanceFrom2);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.Note);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.SiteName);
            MyAssert.AreEqual(sampleRecord, resultRecord, x => x.FieldInvestigationRecommendedOn);
        }

        #endregion
    }
}
