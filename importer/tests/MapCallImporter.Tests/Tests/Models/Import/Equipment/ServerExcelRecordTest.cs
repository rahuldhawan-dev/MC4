using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class ServerExcelRecordTest : EquipmentExcelRecordTestBase<ServerExcelRecord>
    {
        #region Private Methods

        protected override ServerExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-SERG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<ServerExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_SERVR");
            test.String(x => x.HMISoftware, "HMI_SOFTWARE");
            test.DropDown(x => x.NetworkScheme, "NETWORK_SCHEME");
            test.DropDown(x => x.OperatingSystem, "OPERATING_SYSTEMS");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.RAID, "RAID");
            test.String(x => x.RAMMemory, "RAM_MEMORY");
            test.DropDown(x => x.ServerType, "SERVR_TYP");
            test.String(x => x.SoftwareLicense, "SOFTWARE_LICENSE #");
            test.DropDown(x => x.StandbyPowerType, "STANDBY_POWER_TP");
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
