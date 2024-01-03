using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class PumpPositiveDisplacementExcelRecordTest : EquipmentExcelRecordTestBase<PumpPositiveDisplacementExcelRecord>
    {
        #region Private Methods

        protected override PumpPositiveDisplacementExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.Orientation = "HORIZONTAL";
            ret.OwnedBy = "AW";
            ret.PmpDischargeSize = "1";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-PPDG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<PumpPositiveDisplacementExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_PMP-PD");
            test.Numerical(x => x.BHPRating, "BHP_RATING");
            test.Numerical(x => x.FlowMaximum, "FLOW_MAXIMUM");
            test.Numerical(x => x.FlowRating, "FLOW_RATING");
            test.DropDown(x => x.FlowUOM, "FLOW_UOM");
            test.DropDown(x => x.LubeType1, "LUBE_TP");
            test.DropDown(x => x.LubeType2, "LUBE_TP_2");
            test.DropDown(x => x.Orientation, "ORIENTATION");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.PmpBearinglowero, "PMP-BEARING#(LOWER/OUTER)");
            test.String(x => x.PmpBearingupperi, "PMP-BEARING#(UPPER/INNER)");
            test.String(x => x.PmpBearingTPlower, "PMP-BEARINGTP-LOWER/OUTER");
            test.String(x => x.PmpBearingTPupper, "PMP-BEARINGTP-UPPER/INNER");
            test.DropDown(x => x.PmpDischargeSize, "PMP_DISCHARGE_SIZE");
            test.Numerical(x => x.PmpEfficiencyfact, "PMP_EFICIENCY");
            test.DropDown(x => x.PmpImpellerMatl, "PMP_IMPELLER_MATL");
            test.Numerical(x => x.PmpImpellerSize, "PMP_IMPELLER_SIZE");
            test.DropDown(x => x.PmpInletSize, "PMP_INLET_SIZE");
            test.DropDown(x => x.PmpMaterial, "PMP_MATERIAL");
            test.String(x => x.PmpNPSHRating, "PMP_NPSH_RATING");
            test.DropDown(x => x.PmpSealType, "PMP_SEAL_TP");
            test.String(x => x.PmpShutoffHead, "PMP_SHUT_OFF_HEAD");
            test.DropDown(x => x.PmpStages, "PMP_STAGES");
            test.String(x => x.PmpTDHRating, "PMP_TDH_RATING");
            test.DropDown(x => x.RotationDirection, "ROTATION_DIRECTION");
            test.DropDown(x => x.RPMRating, "RPM_RATING");
            test.DropDown(x => x.PositiveDisplacemen, "PMP-PD_TYP");
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
