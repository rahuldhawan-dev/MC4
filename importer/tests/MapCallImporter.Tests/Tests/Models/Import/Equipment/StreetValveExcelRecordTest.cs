using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class StreetValveExcelRecordTest : EquipmentExcelRecordTestBase<StreetValveExcelRecord>
    {
        #region Private Methods

        protected override StreetValveExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-STVG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<StreetValveExcelRecord> test)
        {
            test.DropDown(x => x.ActuatorType, "ACTUATOR_TP");
            test.DropDown(x => x.Application, "APPLICATION_SVLV");
            test.String(x => x.BookPage, "BOOK_PAGE");
            test.DropDown(x => x.BypassValve, "BYPASS_VALVE");
            test.String(x => x.DependencyDriver1, "DEPENDENCY_DRIVER_1");
            test.String(x => x.DependencyDriver2, "DEPENDENCY_DRIVER_2");
            test.DropDown(x => x.GearType, "GEAR_TP");
            test.String(x => x.GEOACCURACYGISDATA, "GEOACCURACY_GIS_DATASOURCETYPE");
            test.String(x => x.HistoricalID, "HISTORICAL_ID");
            test.String(x => x.InstallationWO, "INSTALLATION_ WO#");
            test.DropDown(x => x.JointType, "JOINT_TP");
            test.String(x => x.MapPage, "MAP_PAGE");
            test.DropDown(x => x.NormalPosition, "NORMAL_POSITION");
            test.Numerical(x => x.NormalSysPressure, "NORMAL_SYS_PRESSURE");
            test.Numerical(x => x.NumberofTurns, "NUMBER_OF_TURNS");
            test.DropDown(x => x.OnSCADA, "ON_SCADA");
            test.DropDown(x => x.OpenDirection, "OPEN_DIRECTION");
            test.DropDown(x => x.OperatingNutType, "OPERATING_NUT_TP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.PipeChannelSize, "PIPE_CHANNEL_SIZE");
            test.DropDown(x => x.PipeMaterial, "PIPE_MATERIAL");
            test.DropDown(x => x.PressureClass, "PRESSURE_CLASS");
            test.String(x => x.PressureZone, "PRESSURE_ZONE");
            test.String(x => x.PressureZoneHGL, "PRESSURE_ZONE_HGL");
            test.String(x => x.Sketch, "SKETCH_#");
            test.DropDown(x => x.StreetValveType, "SVLV_TYP");
            test.String(x => x.Subdivision, "SUBDIVISION");
            test.DropDown(x => x.SurfaceCover, "SURFACE_COVER");
            test.DropDown(x => x.SurfaceCoverLocTy, "SURFACE_COVER_LOC_TP");
            test.String(x => x.TorqueLimit, "TORQUE_LIMIT");
            test.DropDown(x => x.VLVAccessType, "ACCESS_TP");
            test.String(x => x.VlvDepthtoTopof, "VLV_DEPTH_TOP_OF_MAIN");
            test.DropDown(x => x.VlvOperNutSize, "VLV_OPER_NUT_SIZE");
            test.DropDown(x => x.VlvSeatType, "VLV_SEAT_TP");
            test.String(x => x.VlvTopValveNutDe, "VLV_TOP_VALVE_NUT_DEPTH");
            test.DropDown(x => x.VlvSpecialVBoxMa, "VLV_SPECIAL_V_BOX_MARKING");
            test.String(x => x.VlvValveBoxMarkin, "VALVE_BOX_MARKING");
            test.DropDown(x => x.VlvValveSize, "VLV_VALVE_SIZE");
            test.DropDown(x => x.VlvValveType, "VLV_VALVE_TP");
            test.DropDown(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES_DIST");
            test.String(x => x.SpecialMtnNoteDet, "SPECIAL_MAINT_NOTES_DETAILS");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");
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
