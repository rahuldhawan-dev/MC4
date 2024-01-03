using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Update;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallImporter.Tests.Models.Update
{
    [TestClass]
    public class FacilityRiskCharacteristicsExcelRecordTest : ExcelRecordTestBase<Facility, MyEditFacility, FacilityRiskCharacteristicsExcelRecord>
    {
        public const int FACILITY_ID = 11;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateSomeFacilitiesInAberdeenNJ(_container);
        }

        #endregion

        protected override FacilityRiskCharacteristicsExcelRecord CreateTarget()
        {
            return new FacilityRiskCharacteristicsExcelRecord {Id = FACILITY_ID};
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<Facility, MyEditFacility, FacilityRiskCharacteristicsExcelRecord> test)
        {
            test.EntityLookup(x => x.FacilityConditions, x => x.Condition, "Poor");
            test.EntityLookup(x => x.FacilityPerformance, x => x.Performance, "Poor");
            test.EntityLookup(x => x.FacilityConsequencesOfFailure, x => x.ConsequenceOfFailure, "Low");

            test.TestedElsewhere(x => x.Id);

            test.NotMapped(x => x.OperatingCenter);
            test.NotMapped(x => x.FacilityName);
        }

        [TestMethod]
        public void TestRecordIsLookedUpById()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(FACILITY_ID, _target.MapToEntity(uow, 2, MappingHelper).Id);
            });
        }
    }
}