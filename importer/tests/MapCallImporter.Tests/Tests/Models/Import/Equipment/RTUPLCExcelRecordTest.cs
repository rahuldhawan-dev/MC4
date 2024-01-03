using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class RTUPLCExcelRecordTest : EquipmentExcelRecordTestBase<RTUPLCExcelRecord>
    {
        #region Private Methods

        protected override RTUPLCExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.Application = "DATA COLLECTOR";
            ret.OwnedBy = "AW";
            ret.RTUType = "RTU-PLC*";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-RTUG-1";

        #endregion

        #region Mapping

        protected override void
            TestCharacteristicMappings(EquipmentCharacteristicMappingTester<RTUPLCExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_RTU-PLC");
            test.DropDown(x => x.COMM1Device, "COMM1_DEVICE");
            test.DropDown(x => x.COMM1Type, "COMM1_TP");
            test.DropDown(x => x.COMM2Device, "COMM2_DEVICE");
            test.DropDown(x => x.COMM2Type, "COMM2_TP");
            test.DropDown(x => x.COMM3Device, "COMM3_DEVICE");
            test.DropDown(x => x.COMM3Type, "COMM3_TP");
            test.DropDown(x => x.COMM4Device, "COMM4_DEVICE");
            test.DropDown(x => x.COMM4Type, "COMM4_TP");
            test.DropDown(x => x.COMM5Device, "COMM5_DEVICE");
            test.DropDown(x => x.COMM5Type, "COMM5_TP");
            test.DropDown(x => x.COMM6Device, "COMM6_DEVICE");
            test.DropDown(x => x.COMM6Type, "COMM6_TP");
            test.String(x => x.CPUBatteryorNo, "CPU_BATTERY");
            test.DropDown(x => x.EndNode, "END_NODE");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.RTUType, "RTU-PLC_TYP");
            test.DropDown(x => x.RemoteIO, "REMOTE_IO");
            test.DropDown(x => x.VoltRating, "VOLT_RATING_RTU-PLC");
            test.DropDown(x => x.StandbyPowerType, "STANDBY_POWER_TP");
            test.DropDown(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES_DIST");
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
