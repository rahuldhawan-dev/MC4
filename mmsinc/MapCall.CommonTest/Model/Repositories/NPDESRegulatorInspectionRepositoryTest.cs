using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using StructureMap;
using System;
using System.Linq;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class NpdesRegulatorInspectionRepositoryTest : MapCallMvcSecuredRepositoryTestBase<NpdesRegulatorInspection,
        NpdesRegulatorInspectionRepository, User>
    {
        #region Nested Type: TestSearchNpdesRegulatorInspectionReportItem

        private class TestSearchNpdesRegulatorInspectionReportItem : SearchSet<NpdesRegulatorInspectionReportItem>,
            ISearchNpdesRegulatorInspectionReport
        {
            public int? OperatingCenter { get; set; }
            public int? Town { get; set; }
            public int? Year { get; set; }
            public DateRange DepartureDateTime { get; set; }
        }

        #endregion

        #region Private Methods

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
        }

        private void SetupInspectionDataForTestSearchNpdesRegulatorInspectionReport()
        {
            var _operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { OperatingCenterName = "Edison Water Company", OperatingCenterCode = "EW4" });
            var _town = GetEntityFactory<Town>().Create(new { ShortName = "Addison" });
            var _sewerOpening = GetEntityFactory<SewerOpening>().Create(new { Town = _town, OperatingCenter = _operatingCenter });

            GetEntityFactory<NpdesRegulatorInspection>().Create(new {
                SewerOpening = _sewerOpening,
                DepartureDateTime = new System.DateTime(2014, 1, 1)
            });
            GetEntityFactory<NpdesRegulatorInspection>().Create(new {
                SewerOpening = _sewerOpening,
                DepartureDateTime = new System.DateTime(2014, 1, 2)
            });
            GetEntityFactory<NpdesRegulatorInspection>().Create(new {
                SewerOpening = _sewerOpening,
                DepartureDateTime = new System.DateTime(2014, 1, 3)
            });
            GetEntityFactory<NpdesRegulatorInspection>().Create(new {
                SewerOpening = _sewerOpening,
                DepartureDateTime = new System.DateTime(2014, 1, 4)
            });
            GetEntityFactory<NpdesRegulatorInspection>().Create(new {
                SewerOpening = _sewerOpening,
                DepartureDateTime = new System.DateTime(2014, 1, 5)
            });
            GetEntityFactory<NpdesRegulatorInspection>().Create(new {
                SewerOpening = _sewerOpening,
                DepartureDateTime = new System.DateTime(2014, 1, 6)
            });
            GetEntityFactory<NpdesRegulatorInspection>().Create(new {
                SewerOpening = _sewerOpening,
                DepartureDateTime = new System.DateTime(2014, 1, 7)
            });
            GetEntityFactory<NpdesRegulatorInspection>().Create(new {
                SewerOpening = _sewerOpening,
                DepartureDateTime = new System.DateTime(2014, 1, 8)
            });
        }

        #endregion

        #region Fields

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestNpdesRegulatorInspectionReportCorrectlyMapsToViewModel()
        {
            var now = DateTime.Now;
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().CreateList(2);
            var towns = GetFactory<TownFactory>().CreateList(3);
            var inspection = GetFactory<NpdesRegulatorInspectionFactory>().Create(new {
                SewerOpening = GetFactory<SewerOpeningFactory>().Create(new {
                    LocationDescription = "description of location",
                    OutfallNumber = "003"
                }),
                ArrivalDateTime = DateTime.Today,
                DepartureDateTime = DateTime.Today.AddHours(3)
            });

            var search = new TestSearchNpdesRegulatorInspectionReportItem();
            Repository.SearchNpdesRegulatorInspectionReport(search);

            var result = search.Results.Single();
            Assert.AreEqual(inspection.Id, result.InspectionId);
            Assert.AreEqual(inspection.SewerOpening.LocationDescription, result.LocationDescription);
            Assert.AreEqual(inspection.SewerOpening.OutfallNumber, result.OutfallNumber);
            Assert.AreEqual(inspection.BlockCondition, result.BlockCondition);
            Assert.AreEqual(inspection.DischargeCause, result.DischargeCause);
        }

        [TestMethod]
        public void TestSearchNpdesRegulatorInspectionReportSearchingByYearOnlyReturnsItemsForYear()
        {
            SetupInspectionDataForTestSearchNpdesRegulatorInspectionReport();
            var search = new TestSearchNpdesRegulatorInspectionReportItem {
                DepartureDateTime = new DateRange {
                    Start = new DateTime(2014, 1, 1),
                    End = new DateTime(2014, 12, 31),
                    Operator = RangeOperator.Between
                }
            };

            var expectedCount = 8;
            GetFactory<NpdesRegulatorInspectionFactory>().Create(new {
                SewerOpening = GetFactory<SewerOpeningFactory>().Create(new {
                    LocationDescription = "description of location",
                    OutfallNumber = "003"
                }),
                ArrivalDateTime = new DateTime(2013, 1, 1),
                DepartureDateTime = new DateTime(2013, 1, 1)
            });
            GetFactory<NpdesRegulatorInspectionFactory>().Create(new {
                SewerOpening = GetFactory<SewerOpeningFactory>().Create(new {
                    LocationDescription = "description of location",
                    OutfallNumber = "003"
                }),
                ArrivalDateTime = new DateTime(2013, 1, 2),
                DepartureDateTime = new DateTime(2013, 1, 2)
            });

            var target = Repository.SearchNpdesRegulatorInspectionReport(search);
            Assert.AreEqual(expectedCount, target.Count());

            search = new TestSearchNpdesRegulatorInspectionReportItem {
                DepartureDateTime = new DateRange {
                    Start = new DateTime(2013, 1, 1),
                    End = new DateTime(2013, 12, 31),
                    Operator = RangeOperator.Between
                }
            };

            target = Repository.SearchNpdesRegulatorInspectionReport(search);
            Assert.AreEqual(2, target.Count());
        }

        #endregion
    }
}
