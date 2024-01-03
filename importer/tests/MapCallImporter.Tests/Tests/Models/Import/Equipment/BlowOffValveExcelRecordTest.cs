using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class BlowOffValveExcelRecordTest : EquipmentExcelRecordTestBase<BlowOffValveExcelRecord>
    {
        #region Private Methods

        protected override BlowOffValveExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-BOVG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<BlowOffValveExcelRecord> test)
        {
            test.DropDown(x => x.ActuatorType, "ACTUATOR_TP");
            test.String(x => x.Application, "APPLICATION_SVLV-BO");
            test.DropDown(x => x.BlowoffType, "SVLV-BO_TYP");
            test.String(x => x.BookPage, "BOOK_PAGE");
            test.DropDown(x => x.BypassValve, "BYPASS_VALVE");
            test.String(x => x.DependencyDriver1, "DEPENDENCY_DRIVER_1");
            test.String(x => x.DependencyDriver2, "DEPENDENCY_DRIVER_2");
            test.DropDown(x => x.GearType, "GEAR_TP");
            test.DropDown(x => x.JointType, "JOINT_TP");
            test.String(x => x.MapPage, "MAP_PAGE");
            test.DropDown(x => x.NormalPosition, "NORMAL_POSITION");
            test.Numerical(x => x.NormalSysPressure, "NORMAL_SYS_PRESSURE");
            test.Numerical(x => x.NumberofTurns, "NUMBER_OF_TURNS");
            test.DropDown(x => x.OnSCADA, "ON_SCADA");
            test.DropDown(x => x.OpenDirection, "OPEN_DIRECTION");
            test.DropDown(x => x.OperatingNutType, "OPERATING_NUT_TP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.PipeMaterial, "PIPE_MATERIAL");
            test.DropDown(x => x.PressureClass, "PRESSURE_CLASS");
            test.String(x => x.PressureZone, "PRESSURE_ZONE");
            test.String(x => x.PressureZoneHGL, "PRESSURE_ZONE_HGL");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTE");
            test.String(x => x.SpecialMtnNoteDet, "SPECIAL_MAINT_NOTE_DETAILS");
            test.String(x => x.Subdivision, "SUBDIVISION");
            test.DropDown(x => x.SurfaceCover, "SURFACE_COVER");
            test.DropDown(x => x.SurfaceCoverLocTy, "SURFACE_COVER_LOC_TP");
            test.String(x => x.TorqueLimit, "TORQUE_LIMIT");
            test.DropDown(x => x.VLVAccessType, "ACCESS_TP");
            test.DropDown(x => x.VlvOperNutSize, "VLV_OPER_NUT_SIZE");
            test.DropDown(x => x.VlvSeatType, "VLV_SEAT_TP");
            test.DropDown(x => x.VlvSpecialVBoxMa, "VLV_SPECIAL_V_BOX_MARKING");
            test.DropDown(x => x.VlvValveSize, "VLV_VALVE_SIZE");
            test.DropDown(x => x.VlvValveType, "VLV_VALVE_TP");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.GEOACCURACYGISDATA);
            test.NotMapped(x => x.HistoricalID);
            test.NotMapped(x => x.InstallationWO);
            test.NotMapped(x => x.PipeChannelSize);
            test.NotMapped(x => x.Sketch);
            test.NotMapped(x => x.VlvDepthtoTopof);
            test.NotMapped(x => x.VlvTopValveNutDe);
            test.NotMapped(x => x.VlvValveBoxMarkin);
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
