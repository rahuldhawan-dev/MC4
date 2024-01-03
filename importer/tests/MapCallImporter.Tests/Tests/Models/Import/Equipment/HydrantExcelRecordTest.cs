using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class HydrantExcelRecordTest : EquipmentExcelRecordTestBase<HydrantExcelRecord>
    {
        #region Private Methods

        protected override HydrantExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-HYDG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<HydrantExcelRecord> test)
        {
            test.String(x => x.BookPage, "BOOK_PAGE");
            test.String(x => x.DependencyDriver1, "DEPENDENCY_DRIVER_1");
            test.String(x => x.DependencyDriver2, "DEPENDENCY_DRIVER_2");
            test.String(x => x.ECISAccount, "ECIS ACCOUNT #");
            test.String(x => x.GEOACCURACYGISDATA, "GEOACCURACY GIS-DATASOURCETYPE");
            test.String(x => x.HistoricalID, "HISTORICAL_ID");
            test.String(x => x.HydAuxValve, "HYD-AUX VALVE #");
            test.DropDown(x => x.HydAuxValveBranch, "HYD_AUX_VALVE_BRANCH_SIZE");
            test.DropDown(x => x.HydAuxillaryValve, "HYD_AUXILLARY_VALVE");
            test.DropDown(x => x.HydBarrelSize, "HYD_BARREL_SIZE");
            test.DropDown(x => x.HydBarrelType, "HYD_BARREL_TP");
            test.DropDown(x => x.HydBillingType, "HYD_BILLING_TP");
            test.String(x => x.HydBranchLength, "HYD-BRANCH_LENGTH");
            test.String(x => x.HydBuryDepth, "HYD_BURY_DEPTH");
            test.DropDown(x => x.HydColorColorCod, "HYD_COLORCODE");
            test.DropDown(x => x.HydColorCodeMetho, "HYD_COLOR_CODE_METHOD");
            test.DropDown(x => x.HydColorCodeType, "HYD_COLOR_CODE_TP");
            test.DropDown(x => x.HydDeadEndMain, "HYD_DEAD_END_MAIN");
            test.String(x => x.HydExtensionsSiz, "HYD-EXTENSIONS AND SIZES");
            test.String(x => x.HydFireDistrict, "HYD-FIRE_DISTRICT");
            test.DropDown(x => x.HydLockDeviceType, "HYD_LOCK_DEVICE_TP");
            test.DropDown(x => x.HydOutletConfig, "HYD_OUTLET_CONFIG");
            test.DropDown(x => x.HydSideNozzleSize, "HYD_SIDE_NOZZLE_SIZE");
            test.String(x => x.HydSidePortThread, "HYD-SIDE_PORT_THREAD_TYPE");
            test.DropDown(x => x.HydSteamerSize, "HYD_STEAMER_SIZE");
            test.DropDown(x => x.HydSteamerThreadT, "HYD_STEAMER_THREAD_TP");
            test.DropDown(x => x.HydStemLube, "HYD_STEM_LUBE");
            test.DropDown(x => x.HydrantType, "HYD_TYP");
            test.String(x => x.InstallationWO, "INSTALLATION_WO #");
            test.DropDown(x => x.JointType, "JOINT_TP");
            test.String(x => x.MapPage, "MAP_PAGE");
            test.Numerical(x => x.NormalSysPressure, "NORMAL_SYS_PRESSURE");
            test.DropDown(x => x.OpenDirection, "OPEN_DIRECTION");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.PipeChannelSize, "PIPE_CHANNEL_SIZE");
            test.DropDown(x => x.PipeMaterial, "PIPE_MATERIAL");
            test.DropDown(x => x.PressureClass, "PRESSURE_CLASS");
            test.String(x => x.PressureZone, "PRESSURE ZONE");
            test.String(x => x.PressureZoneHGL, "PRESSURE_ZONE_HGL");
            test.String(x => x.RepairKit, "REPAIR_KIT #");
            test.String(x => x.Sketch, "SKETCH #");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTE");
            test.String(x => x.Subdivision, "SUBDIVISION");
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
