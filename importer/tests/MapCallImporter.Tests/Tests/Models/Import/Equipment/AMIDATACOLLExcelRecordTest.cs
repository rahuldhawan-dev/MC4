using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class AMIDATACOLLExcelRecordTest : EquipmentExcelRecordTestBase<AMIDATACOLLExcelRecord>
    {
        #region Private Methods

        protected override AMIDATACOLLExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-ADCG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<AMIDATACOLLExcelRecord> test)
        {
            test.String(x => x.AMIDC_LOC, "AMIDC_LOC");
            test.String(x => x.AMIDC_LOC_ACCESS, "AMIDC_LOC_ACCESS");
            test.String(x => x.AMIDC_POWR, "AMIDC_POWR");
            test.String(x => x.AMIDC_BATT_SIZE, "AMIDC_BATT_SIZE");
            test.String(x => x.AMIDC_FREQ, "ADMIDC_FREQ");
            test.String(x => x.AMIDC_BACKUP_SOURCE, "AMIDC_BACKUP_SOURCE");
            test.String(x => x.AMIDC_ANTENNATOP_ELEV, "ADMIDC_ANTENNATOP_ELEV");
            test.String(x => x.AMIDC_ANTENNABOT_ELEV, "ADMIDC_ANTENNABOT_ELEV");
            test.String(x => x.INSTALLATION_WO, "INSTALLATION_WO");
            test.String(x => x.OWNED_BY, "OWNED_BY");

            test.NotMapped(x => x.NARUCMaintenanceAccount);
            test.NotMapped(x => x.NARUCOperationsAccount);
            test.NotMapped(x => x.SpecialMtnNote);
            test.NotMapped(x => x.SpecialMtnNoteDet);
        }

        [TestMethod]
        public override void TestMappings()
        {
            base.TestMappings();
        }

        [TestMethod]
        public void TestLockoutRequiredPrerequisiteIsNotAdded()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, _mappingHelper);

                MyAssert.DoesNotContain(result.ProductionPrerequisites,
                    pp => pp.Id == ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT);
            });
        }

        #endregion
    }
}
