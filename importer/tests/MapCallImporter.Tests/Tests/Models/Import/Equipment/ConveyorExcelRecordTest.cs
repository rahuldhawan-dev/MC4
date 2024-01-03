using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class ConveyorExcelRecordTest : EquipmentExcelRecordTestBase<ConveyorExcelRecord>
    {
        #region Private Methods

        protected override ConveyorExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-CVRG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<ConveyorExcelRecord> test)
        {
            test.DropDown(x => x.ConveyorType, "CONVEYOR_TYP");
            test.String(x => x.LengthFT, "LENGTH (FT)");
            test.String(x => x.LiftFT, "LIFT (FT)");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.RPMOperating, "RPM_OPERATING");
            test.String(x => x.SpeedFPS, "SPEED (FPS)");
            test.String(x => x.WidthFT, "WIDTH (FT)");
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
