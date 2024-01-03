using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class AirCompressorExcelRecordTest : EquipmentExcelRecordTestBase<AirCompressorExcelRecord>
    {
        #region Private Methods

        protected override AirCompressorExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.CompressorType = "CENTRIFUGAL COMP";
            ret.OwnedBy = "AW";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-ACPG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<AirCompressorExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_COMP");
            test.Numerical(x => x.BHPRating, "BHP_RATING");
            test.String(x => x.CapacityRating, "CAPACITY_RATING");
            test.String(x => x.CapacityUOM, "CAPACITY_UOM_COMP");
            test.DropDown(x => x.CompressorType, "COMP_TYP");
            test.DropDown(x => x.DriveType, "DRIVE_TP");
            test.String(x => x.MaxPressure, "MAX_PRESSURE");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.RPMOperating, "RPM_OPERATING");
            test.DropDown(x => x.Stages, "STAGES");
            test.String(x => x.TnkVolumegal, "TNK_VOL_(GAL)");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

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