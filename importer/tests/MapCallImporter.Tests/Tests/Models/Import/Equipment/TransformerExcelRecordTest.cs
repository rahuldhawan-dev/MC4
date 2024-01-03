using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class TransformerExcelRecordTest : EquipmentExcelRecordTestBase<TransformerExcelRecord>
    {
        #region Private Methods

        protected override TransformerExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.Application = "CONTROLS";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-TRNG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<TransformerExcelRecord> test)
        {
            test.Numerical(x => x.AmpRating, "AMP_RATING");
            test.String(x => x.AmpsSecondary, "AMPS_SECONDARY");
            test.DropDown(x => x.Application, "APPLICATION_XFMR");
            test.String(x => x.BILRating, "BIL_RATING");
            test.String(x => x.ContainsPCBs, "CONTAINS_PCB'S");
            test.String(x => x.Impedance, "IMPEDANCE_%");
            test.DropDown(x => x.InsulationClass, "INSULATION_CLASS");
            test.String(x => x.KVARated, "KVA_RATED");
            test.DropDown(x => x.Mounting, "MOUNTING_XFMR");
            test.DropDown(x => x.NEMAEnclosure, "NEMA_ENCLOSURE");
            test.String(x => x.OilCapacitygal, "OIL_CAPACITY_GAL");
            test.String(x => x.OilType, "OIL_TYPE");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.Phases, "PHASES");
            test.String(x => x.TapRangeandTap, "TAP_RANGE_TAP");
            test.String(x => x.TemperatureRise, "TEMPERATURE_RISE");
            test.DropDown(x => x.TransformerType, "XFMR_TYP");
            test.DropDown(x => x.VoltRating, "VOLT_RATING");
            test.DropDown(x => x.VoltRatingSeconda, "VOLT_RATING_SECONDARY");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
            test.String(x => x.Weightlbs, "WEIGHT_LBS");
            test.DropDown(x => x.WindingConfigPrim, "XFMR_WINDING_PRI");
            test.DropDown(x => x.WindingConfigSecon, "XFMR_WINDING_SEC");
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
