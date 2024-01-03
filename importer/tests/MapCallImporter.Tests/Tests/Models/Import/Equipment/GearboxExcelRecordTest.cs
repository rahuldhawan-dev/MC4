using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class GearboxExcelRecordTest : EquipmentExcelRecordTestBase<GearboxExcelRecord>
    {
        #region Private Methods

        protected override GearboxExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-GRBG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<GearboxExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_GEARBOX");
            test.String(x => x.GearRatio, "GEAR_RATIO");
            test.DropDown(x => x.GearboxType, "GEARBOX_TYP");
            test.String(x => x.OilCapacitygal, "OIL_CAPACITY (GAL)");
            test.String(x => x.OilType, "OIL_TYPE");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.RPMOperating, "RPM_OPERATING");
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
