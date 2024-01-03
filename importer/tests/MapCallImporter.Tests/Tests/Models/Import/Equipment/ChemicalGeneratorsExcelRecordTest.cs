using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class ChemicalGeneratorsExcelRecordTest : EquipmentExcelRecordTestBase<ChemicalGeneratorsExcelRecord>
    {
        #region Private Methods

        protected override ChemicalGeneratorsExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.ChemicalGeneratorT = "CHLORINE DIOXIDE";
            ret.ChmFeedRate = "foo";
            ret.OwnedBy = "AW";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-CGNG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<ChemicalGeneratorsExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_CHEM-GEN");
            test.DropDown(x => x.ChemicalGeneratorT, "CHEM-GEN_TYP");
            test.DropDown(x => x.ChmDosingControl, "CHM_DOSING_CONTROL");
            test.String(x => x.ChmFeedRate, "CHEM_FEED_RATE");
            test.DropDown(x => x.ChmFeedRateUOM, "CHM_FEED_RATE_UOM");
            test.DropDown(x => x.ChmMaterial, "CHM_MATERIAL");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.SpecialMtnNote);
            test.NotMapped(x => x.SpecialMtnNoteDet);
        }

        [TestMethod]
        public override void TestMappings()
        {
            base.TestMappings();
        }

        [TestMethod]
        public void TestLockoutRequiredPrerequisiteIsAdded()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, _mappingHelper);

                MyAssert.Contains(result.ProductionPrerequisites,
                    pp => pp.Id == ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT);
            });
        }

        #endregion
    }
}