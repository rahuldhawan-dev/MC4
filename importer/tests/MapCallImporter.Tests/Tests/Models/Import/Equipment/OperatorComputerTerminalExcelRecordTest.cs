using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class OperatorComputerTerminalExcelRecordTest : EquipmentExcelRecordTestBase<OperatorComputerTerminalExcelRecord>
    {
        #region Private Methods

        protected override OperatorComputerTerminalExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-OCTG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<OperatorComputerTerminalExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_OIT");
            test.DropDown(x => x.HMISoftware, "HMI_MANUFACTURER");
            test.DropDown(x => x.NetworkScheme, "NETWORK_SCHEME");
            test.DropDown(x => x.OITType, "OIT_TYP");
            test.DropDown(x => x.OperatingSystem, "OPERATING_SYSTEMS");
            test.DropDown(x => x.RAID, "RAID");
            test.DropDown(x => x.StandbyPowerType, "STANDBY_POWER_TP");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.OwnedBy);
            test.NotMapped(x => x.RAMMemory);
            test.NotMapped(x => x.SoftwareLicense);
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
