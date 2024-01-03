using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class BacterialWaterSampleRepositoryTest : MapCallMvcSecuredRepositoryTestBase<BacterialWaterSample,
        BacterialWaterSampleRepository, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULES = RoleModules.HumanResourcesSampleSites;

        #endregion

        #region Fields

        private Mock<IDateTimeProvider> _dateProvider;

        private PublicWaterSupply _publicWaterSupply1, _publicWaterSupply2, _publicWaterSupply3, _publicWaterSupply4;

        private SampleSite _sampleSite1,
                           _sampleSite2,
                           _sampleSite3,
                           _sampleSite4,
                           _sampleSite5,
                           _sampleSite6,
                           _sampleSite7;

        private List<BacterialSampleType> _bacterialSampleTypes;

        #region Fields for SearchBactiSamplesChlorineHighLowReport

        private BacterialWaterSample BacterialWaterSampleAberdeen,
                                     BacterialWaterSampleAberdeenCl2FreeMax,
                                     BacterialWaterSampleAberdeenCl2FreeMin,
                                     BacterialWaterSampleAberdeenCl2TotalMax,
                                     BacterialWaterSampleAberdeenCl2TotalMin,
                                     BacterialWaterSampleNeptuneCl2FreeMinTotalMax,
                                     BacterialWaterSampleNeptuneCl2FreeMaxTotalMin;

        private Town _townAberdeen, _townNeptune;
        private PublicWaterSupply _publicWaterSupply;

        #endregion

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IPublicWaterSupplyRepository>().Use<PublicWaterSupplyRepository>();
            e.For<IDateTimeProvider>().Use((_dateProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        private void CreateBacterialWaterSample(SampleSite sampleSite, decimal cl2Free, decimal cl2Total,
            DateTime sampleDate, BacterialSampleType bacterialSampleType)
        {
            GetEntityFactory<BacterialWaterSample>().Create(new {
                SampleSite = sampleSite,
                Cl2Free = cl2Free,
                Cl2Total = cl2Total,
                SampleCollectionDTM = sampleDate,
                BacterialSampleType = bacterialSampleType
            });
        }

        private void SetupSampleData()
        {
            var type1 = GetFactory<RoutineBacterialSampleTypeFactory>().Create();
            var type2 = GetFactory<ProcessControlBacterialSampleTypeFactory>().Create();
            var type3 = GetFactory<NewMainBacterialSampleTypeFactory>().Create();

            _bacterialSampleTypes = new[] {type1, type2, type3}.ToList();
            _publicWaterSupply1 = GetEntityFactory<PublicWaterSupply>().Create(new { Identifier = "1111", OperatingArea = "North", JanuaryRequiredBacterialWaterSamples = 10, MarchRequiredBacterialWaterSamples = 15, SeptemberRequiredBacterialWaterSamples = 20 });
            _publicWaterSupply2 = GetEntityFactory<PublicWaterSupply>().Create(new { Identifier = "1112", OperatingArea = "North", JanuaryRequiredBacterialWaterSamples = 15, AprilRequiredBacterialWaterSamples = 20, OctoberRequiredBacterialWaterSamples = 10 });
            _publicWaterSupply3 = GetEntityFactory<PublicWaterSupply>().Create(new { Identifier = "1113", OperatingArea = "North", JanuaryRequiredBacterialWaterSamples = 20, MayRequiredBacterialWaterSamples = 10, NovemberRequiredBacterialWaterSamples = 15 });
            _publicWaterSupply4 = GetEntityFactory<PublicWaterSupply>().Create(new { Identifier = "1221", OperatingArea = "South", JanuaryRequiredBacterialWaterSamples = 2, MayRequiredBacterialWaterSamples = 1, NovemberRequiredBacterialWaterSamples = 3 });
            _sampleSite1 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Little Silver BH", PublicWaterSupply = _publicWaterSupply1 });
            _sampleSite2 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Krauszer's", PublicWaterSupply = _publicWaterSupply2 });
            _sampleSite3 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Rumson BH", PublicWaterSupply = _publicWaterSupply3 });
            _sampleSite4 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "King James Nursery", PublicWaterSupply = _publicWaterSupply1 });
            _sampleSite5 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Amour Florist", PublicWaterSupply = _publicWaterSupply2 });
            _sampleSite6 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Dunkin Donuts", PublicWaterSupply = _publicWaterSupply3 });
            _sampleSite7 = GetEntityFactory<SampleSite>().Create(new { CommonSiteName = "Cone Zone", PublicWaterSupply = _publicWaterSupply1 });
            var sampleSites = new[]{_sampleSite1, _sampleSite2, _sampleSite3, _sampleSite4, _sampleSite5, _sampleSite6};

            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 1, 1), _bacterialSampleTypes[0]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 1, 2), _bacterialSampleTypes[0]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 1, 3), _bacterialSampleTypes[0]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 2, 1), _bacterialSampleTypes[0]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 1, 1), _bacterialSampleTypes[1]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 1, 2), _bacterialSampleTypes[1]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 1, 3), _bacterialSampleTypes[2]);
            CreateBacterialWaterSample(_sampleSite1, 0.01m, 0.02m, new DateTime(2014, 2, 1), _bacterialSampleTypes[1]);

            CreateBacterialWaterSample(_sampleSite3, 0.01m, 0.02m, new DateTime(2014, 1, 3), _bacterialSampleTypes[2]);
            CreateBacterialWaterSample(_sampleSite5, 0.01m, 0.02m, new DateTime(2014, 2, 1), _bacterialSampleTypes[1]);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetBySampleSiteIdWithBracketSitesReturnsAllSamplesForASampleSiteAndItsBracketSites()
        {
            var sampleSite = GetEntityFactory<SampleSite>().Create();
            var bracketSite = GetEntityFactory<SampleSite>().Create();

            sampleSite.BracketSites.Add(new SampleSiteBracketSite {
                BracketSiteLocationType = GetEntityFactory<SampleSiteBracketSiteLocationType>().Create(),
                SampleSite = sampleSite,
                BracketSampleSite = bracketSite
            });

            Session.Save(sampleSite.BracketSites.First());
            Session.Clear();

            var sample1 = GetEntityFactory<BacterialWaterSample>().Create(new {SampleSite = sampleSite});
            var sample2 = GetEntityFactory<BacterialWaterSample>().Create(new {SampleSite = bracketSite});
            var sample3 = GetEntityFactory<BacterialWaterSample>().Create();

            var result = Repository.GetBySampleSiteIdWithBracketSites(sampleSite.Id).ToList();
            Assert.IsTrue(result.Contains(sample1));
            Assert.IsTrue(result.Contains(sample2));
            Assert.IsFalse(result.Contains(sample3));
        }

        #region BacterialWaterSampleRequirementReport / BacterialWaterSampleRequirement

        [TestMethod]
        public void TestGetBacterialWaterSampleRequirementWithCriteriaReturnsData()
        {
            SetupSampleData();
            var results =
                Repository.GetBacterialWaterSampleRequirements(
                    new TestBacterialWaterSampleRequirementViewModelSearch());
            Assert.AreEqual(11, results.Count());
        }

        [TestMethod]
        public void TestGetBacterialWaterSampleRequirementReportWithCriteriaReturnsSummarizedData()
        {
            SetupSampleData();
            var results =
                Repository.GetBacterialWaterSampleRequirementsReport(
                    new TestBacterialWaterSampleRequirementViewModelSearch());
            Assert.AreEqual(13, results.Count());
        }

        #endregion

        #region SearchBactiSamplesChlorineHighLowReport / GetBactiSamplesChlorineHighLow

        private void SetupSampleDataForSearchBactiSamplesChlorineHighLowReport()
        {
            _publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new { Identifier = "1111", OperatingArea = "North" });
            _townAberdeen = GetEntityFactory<Town>().Create(new { ShortName = "Aberdeen" });
            _townNeptune = GetEntityFactory<Town>().Create(new { ShortName = "Neptune" });
            _sampleSite1 = GetEntityFactory<SampleSite>().Create(new { Town = _townAberdeen, CommonSiteName = "Little Silver BH", PublicWaterSupply = _publicWaterSupply });
            _sampleSite2 = GetEntityFactory<SampleSite>().Create(new { Town = _townAberdeen, CommonSiteName = "Krauszer's", PublicWaterSupply = _publicWaterSupply });
            _sampleSite3 = GetEntityFactory<SampleSite>().Create(new { Town = _townAberdeen, CommonSiteName = "Rumson BH", PublicWaterSupply = _publicWaterSupply });
            _sampleSite4 = GetEntityFactory<SampleSite>().Create(new { Town = _townAberdeen, CommonSiteName = "King James Nursery", PublicWaterSupply = _publicWaterSupply });
            _sampleSite5 = GetEntityFactory<SampleSite>().Create(new { Town = _townAberdeen, CommonSiteName = "Amour Florist", PublicWaterSupply = _publicWaterSupply });
            _sampleSite6 = GetEntityFactory<SampleSite>().Create(new { Town = _townNeptune, CommonSiteName = "Dunkin Donuts", PublicWaterSupply = _publicWaterSupply });
            _sampleSite7 = GetEntityFactory<SampleSite>().Create(new { Town = _townNeptune, CommonSiteName = "Cone Zone", PublicWaterSupply = _publicWaterSupply });

            BacterialWaterSampleAberdeen = GetEntityFactory<BacterialWaterSample>().Create(new {
                SampleSite = _sampleSite1,
                Cl2Free = 0.16m,
                Cl2Total = 0.48m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 1)
            });
            BacterialWaterSampleAberdeenCl2FreeMax = GetEntityFactory<BacterialWaterSample>().Create(new {
                SampleSite = _sampleSite2,
                Cl2Free = 0.27m,
                Cl2Total = 1.26m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 1)
            });
            BacterialWaterSampleAberdeenCl2FreeMin = GetEntityFactory<BacterialWaterSample>().Create(new {
                SampleSite = _sampleSite3,
                Cl2Free = 0.14m,
                Cl2Total = 0.9m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 2)
            });
            BacterialWaterSampleAberdeenCl2TotalMax = GetEntityFactory<BacterialWaterSample>().Create(new {
                SampleSite = _sampleSite4,
                Cl2Free = 0.14m,
                Cl2Total = 1.33m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 3)
            });
            BacterialWaterSampleAberdeenCl2TotalMin = GetEntityFactory<BacterialWaterSample>().Create(new {
                SampleSite = _sampleSite5,
                Cl2Free = 0.21m,
                Cl2Total = 0.27m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 5)
            });
            BacterialWaterSampleNeptuneCl2FreeMinTotalMax = GetEntityFactory<BacterialWaterSample>().Create(new {
                SampleSite = _sampleSite6,
                Cl2Free = 0.01m,
                Cl2Total = 0.88m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 8)
            });
            BacterialWaterSampleNeptuneCl2FreeMaxTotalMin = GetEntityFactory<BacterialWaterSample>().Create(new {
                SampleSite = _sampleSite7,
                Cl2Free = 0.99m,
                Cl2Total = 0.02m,
                SampleCollectionDTM = new System.DateTime(2014, 1, 13)
            });
        }

        /*
       * | Town     | WQ_Sample_Site_ID | Sample_Site_Name   | Cl2_Free | Cl2_Total | Sample_Date |
       * | ABERDEEN | SS-135            | Little Silver BH   |     0.16 |      0.48 | 1/2/2014    |
       * | ABERDEEN | SS-146            | Krauszer's         |     0.27 |      1.26 | 1/5/2014    |
       * | ABERDEEN | SS-145            | Rumson BH          |     0.14 |       0.9 | 1/4/2014    |
       * | ABERDEEN | SS-141            | King James Nursery |     0.14 |      1.33 | 1/4/2014    |
       * | ABERDEEN | SS-138            | Amour Florist      |     0.21 |      0.27 | 1/2/2014    |
       * 
       * NJ7 - Aberdeen - Free-Highest: 0.27
       * NJ7 - Aberdeen - Total-Highest: 1.33
       * NJ7 - Aberdeen - Free-Lowest: 0.14
       * NJ7 - Aberdeen - Total-Lowest: 0.27
       */

        [TestMethod]
        public void TestSearchBactiSamplesChlorineHighLowReportReturnsCorrectModelAndValues()
        {
            SetupSampleDataForSearchBactiSamplesChlorineHighLowReport();
            var search = new TestSearchBactiSamplesChlorineHighLow {
                SampleCollectionDTM = new DateRange
                    {Start = new DateTime(2014, 1, 1), Operator = RangeOperator.GreaterThanOrEqualTo}
            };

            var febSample = GetEntityFactory<BacterialWaterSample>().Create(new {
                SampleSite = _sampleSite2,
                Cl2Free = 0.01m,
                Cl2Total = 9.99m,
                SampleCollectionDTM = new System.DateTime(2014, 2, 1)
            });

            var target = Repository.SearchBactiSamplesChlorineHighLowReport(search).ToArray();

            Assert.AreEqual(8, target.Count());
            Assert.AreEqual(target[0].Type, "Cl2 Free Min");
            Assert.AreEqual(BacterialWaterSampleAberdeenCl2FreeMin.Cl2Free, target[0].Jan);
            Assert.AreEqual(target[7].Type, "Cl2 Total Max");
            Assert.AreEqual(BacterialWaterSampleNeptuneCl2FreeMinTotalMax.Cl2Total, target[7].Jan);
        }

        [TestMethod]
        public void TestGetBacterialWaterSamplesForReportReturnsAllGroupedAppropriatelyWithCorrectMinsAndMaxes()
        {
            // NOTE: This is testing an implementation detail of SearchBactiSamplesChlorineHighLowReport. 
            // There is no reason for this method to be exposed. Also, I have no real clue what it's supposed to be testing.
            SetupSampleDataForSearchBactiSamplesChlorineHighLowReport();
            var search = new TestSearchBactiSamplesChlorineHighLow();

            var target = Repository.GetBactiSamplesChlorineHighLow(search);

            Assert.AreEqual(2, target.Count());

            var first = target.First();
            Assert.AreEqual(BacterialWaterSampleAberdeenCl2FreeMax.Cl2Free, first.Cl2FreeMax);
            Assert.AreEqual(BacterialWaterSampleAberdeenCl2FreeMin.Cl2Free, first.Cl2FreeMin);
            Assert.AreEqual(BacterialWaterSampleAberdeenCl2TotalMax.Cl2Total, first.Cl2TotalMax);
            Assert.AreEqual(BacterialWaterSampleAberdeenCl2TotalMin.Cl2Total, first.Cl2TotalMin);
            var last = target.Last();
            Assert.AreEqual(BacterialWaterSampleNeptuneCl2FreeMaxTotalMin.Cl2Free, last.Cl2FreeMax);
            Assert.AreEqual(BacterialWaterSampleNeptuneCl2FreeMinTotalMax.Cl2Free, last.Cl2FreeMin);
            Assert.AreEqual(BacterialWaterSampleNeptuneCl2FreeMinTotalMax.Cl2Total, last.Cl2TotalMax);
            Assert.AreEqual(BacterialWaterSampleNeptuneCl2FreeMaxTotalMin.Cl2Total, last.Cl2TotalMin);
        }

        [TestMethod]
        public void TestSearchBactiSamplesChlorineHighLowReportSearchingByYearOnlyReturnsSamplesForYear()
        {
            SetupSampleDataForSearchBactiSamplesChlorineHighLowReport();
            var search = new TestSearchBactiSamplesChlorineHighLow {
                SampleCollectionDTM = new DateRange {
                    Start = new DateTime(2014, 1, 1),
                    End = new DateTime(2014, 12, 31),
                    Operator = RangeOperator.Between
                }
            };

            var expectedCount = 8;

            var newSample = GetEntityFactory<BacterialWaterSample>().Create(new {
                SampleSite = _sampleSite1,
                Cl2Free = 0.16m,
                Cl2Total = 0.48m,
                SampleCollectionDTM = new DateTime(2013, 1, 1)
            });

            var target = Repository.SearchBactiSamplesChlorineHighLowReport(search);
            Assert.AreEqual(expectedCount, target.Count());

            search = new TestSearchBactiSamplesChlorineHighLow {
                SampleCollectionDTM = new DateRange {
                    Start = new DateTime(2013, 1, 1),
                    End = new DateTime(2013, 12, 31),
                    Operator = RangeOperator.Between
                }
            };

            target = Repository.SearchBactiSamplesChlorineHighLowReport(search);
            Assert.AreEqual(4, target.Count());
        }

        [TestMethod]
        public void TestSearchBactiSamplesChlorineHighLowReportDisablesPaging()
        {
            //SetupSampleDataForSearchBactiSamplesChlorineHighLowReport();
            var search = new TestSearchBactiSamplesChlorineHighLow();
            search.EnablePaging = true;

            Repository.SearchBactiSamplesChlorineHighLowReport(search);
            Assert.IsFalse(search.EnablePaging);
        }

        #endregion

        #region GetRecentByPWSIDOfBacterialWaterSample

        [TestMethod]
        public void
            TestGetRecentByPWSIDOfBacterialWaterSampleReturnsMatchingSamplesBasedOnTheInitialSamplesPublicWaterSupply()
        {
            var expectedSearchDate = DateTime.Now;
            _dateProvider.Setup(x => x.GetCurrentDate()).Returns(expectedSearchDate);

            var pwsid1 = GetEntityFactory<PublicWaterSupply>().Create();
            var pwsid2 = GetEntityFactory<PublicWaterSupply>().Create();
            var site1 = GetEntityFactory<SampleSite>().Create(new {PublicWaterSupply = pwsid1});
            var site2 = GetEntityFactory<SampleSite>().Create(new {PublicWaterSupply = pwsid1});
            var site3 = GetEntityFactory<SampleSite>().Create(new {PublicWaterSupply = pwsid2});
            var sample1 = GetEntityFactory<BacterialWaterSample>()
               .Create(new {SampleSite = site1, SampleCollectionDTM = expectedSearchDate});
            var sample2 = GetEntityFactory<BacterialWaterSample>()
               .Create(new {SampleSite = site2, SampleCollectionDTM = expectedSearchDate});
            var sample3 = GetEntityFactory<BacterialWaterSample>()
               .Create(new {SampleSite = site3, SampleCollectionDTM = expectedSearchDate});

            MyAssert.AreClose(expectedSearchDate, sample1.SampleCollectionDTM.Value);
            Assert.AreSame(sample1.SampleSite.PublicWaterSupply, sample2.SampleSite.PublicWaterSupply);

            var result = Repository.GetRecentByPWSIDOfBacterialWaterSample(sample1.Id);
            Assert.IsFalse(result.Contains(sample1),
                "The sample being used for the search should not be returned as part of the results.");
            Assert.IsTrue(result.Contains(sample2));
            Assert.IsFalse(result.Contains(sample3));
        }

        [TestMethod]
        public void TestGetRecentByPWSIDReturnsEmptyCollectionIfInitialSamplesPublicWaterSupplyIsNull()
        {
            var sample = GetEntityFactory<BacterialWaterSample>().Create();
            sample.SampleSite = null;
            Session.Save(sample);
            Session.Flush();
            Session.Evict(sample);

            sample = Session.Query<BacterialWaterSample>().Single(x => x.Id == sample.Id);

            Assert.IsNull(sample.SampleSite, "Sanity. Make sure SampleSite's been nulled out in the db.");

            var result = Repository.GetRecentByPWSIDOfBacterialWaterSample(sample.Id);
            Assert.IsFalse(result.Any());
        }

        #endregion

        #region GetSamplesWaitingForLIMSSubmission

        [TestMethod]
        public void TestGetSamplesWaitingForLIMSSubmissionReturnsExpectedResults()
        {
            // This method should only return samples with the "Ready to Send" LIMS Status. 
            // All other statuses should be ignored.
            var expected = GetFactory<BacterialWaterSampleFactory>()
               .Create(new {LIMSStatus = typeof(ReadyToSendLIMSStatusFactory)});
            var unexpected1 = GetFactory<BacterialWaterSampleFactory>()
               .Create(new {LIMSStatus = typeof(NotReadyLIMSStatusFactory)});
            var unexpected2 = GetFactory<BacterialWaterSampleFactory>()
               .Create(new {LIMSStatus = typeof(SentSuccessfullyLIMSStatusFactory)});
            var unexpected3 = GetFactory<BacterialWaterSampleFactory>()
               .Create(new {LIMSStatus = typeof(SendFailedLIMSStatusFactory)});

            // NOTE: This is an extension method rather than a direct repo method because scheduler.
            var result = Repository.GetSamplesWaitingForLIMSSubmission().Single();

            Assert.AreSame(expected, result);
        }

        #endregion

        #endregion

        #region Test classes

        private class TestBacterialWaterSampleRequirementViewModelSearch :
            SearchSet<BacterialWaterSampleRequirementViewModel>, ISearchBacterialWaterSampleRequirementViewModel
        {
            public bool? OnlyWithSamples { get; set; }
            public int[] Year { get; set; }
            public int[] BacterialSampleType { get; set; }
        }

        private class TestSearchBactiSamplesChlorineHighLow : SearchSet<BacterialWaterSample>,
            ISearchBactiSamplesChlorineHighLow
        {
            public int? PublicWaterSupply { get; set; }
            public DateRange SampleCollectionDTM { get; set; }
            public int[] Town { get; set; }
        }

        #endregion
    }
}
