using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Update;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallImporter.Tests.Models.Update
{
    [TestClass]
    public class EquipmentRiskCharacteristicsExcelRecordTest : ExcelRecordTestBase<Equipment, MyEditEquipment, EquipmentRiskCharacteristicsExcelRecord>
    {
        public const int SAP_EQUIPMENT_ID = 5017540;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateSomeAdjustableSpeedDrivesInAberdeenNJ(_container);
        }

        #endregion

        protected override EquipmentRiskCharacteristicsExcelRecord CreateTarget()
        {
            return new EquipmentRiskCharacteristicsExcelRecord {Equipment = SAP_EQUIPMENT_ID};
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<Equipment, MyEditEquipment, EquipmentRiskCharacteristicsExcelRecord> test)
        {
            test.EntityLookup(x => x.EquipmentCondition, x => x.Condition, "Poor");
            test.EntityLookup(x => x.EquipmentPerformance, x => x.Performance, "Poor");
            test.EntityLookup(x => x.EquipmentConsequenceofFailure, x => x.ConsequenceOfFailure, "Low");
            test.EntityLookup(x => x.EquipmentStaticDynamicType, x => x.StaticDynamicType, "Dynamic");

            test.NotMapped(x => x.Description);
            test.NotMapped(x => x.Planningplant);

            test.TestedElsewhere(x => x.Equipment);
        }

        [TestMethod]
        public void TestEquipmentIsRequired()
        {
            WithUnitOfWork(uow => {
                _target.Equipment = 0;

                ExpectMappingFailure(() => _target.MapToEntity(uow, 2, MappingHelper));
            });
        }

        [TestMethod]
        public void TestRecordIsLookedUpByEquipment()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(SAP_EQUIPMENT_ID, _target.MapToEntity(uow, 2, MappingHelper).SAPEquipmentId);
            });
        }

        [TestMethod]
        public void TestDoesRetryThingy()
        {
            WithUnitOfWork(uow => Assert.AreEqual("RETRY::",
                _target.MapToEntity(uow, 2, MappingHelper).SAPErrorCode));
        }
    }
}
