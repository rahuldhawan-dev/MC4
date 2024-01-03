using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class ChemicalDryFeederExcelRecordTest : EquipmentExcelRecordTestBase<ChemicalDryFeederExcelRecord>
    {
        #region Private Methods

        protected override ChemicalDryFeederExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-CDFG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<ChemicalDryFeederExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_CHMF-DRY");
            test.DropDown(x => x.ChmDosingControl, "CHM_DOSING_CONTROL");
            test.String(x => x.ChmFeedRate, "CHEM_FEED_RATE");
            test.DropDown(x => x.ChmFeedRateUOM, "CHM_FEED_RATE_UOM");
            test.DropDown(x => x.ChmMaterial, "CHM_MATERIAL");
            test.DropDown(x => x.DryFeederType, "CHMF-DRY_TYP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
            test.String(x => x.NARUCMaintenanceAccount, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAccount, "NARUC_OPERATIONS_ACCOUNT");

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
