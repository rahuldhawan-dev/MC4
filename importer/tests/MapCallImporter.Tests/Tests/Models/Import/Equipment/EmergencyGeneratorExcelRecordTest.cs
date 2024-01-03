using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class EmergencyGeneratorExcelRecordTest : EquipmentExcelRecordTestBase<EmergencyGeneratorExcelRecord>
    {
        #region Private Methods

        protected override EmergencyGeneratorExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.Application = "BASE LOAD";
            ret.AvailableforEMGLo = "Y";
            ret.GeneratorType = "ENGINE DRIVEN";
            ret.OwnedBy = "AW";
            ret.Portable = "Y";
            ret.RPMRating = "1200";
            ret.SelfStarting = "Y";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-EMGG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<EmergencyGeneratorExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_GEN");
            test.DropDown(x => x.AvailableforEMGLo, "GEN_LOAN");
            test.DropDown(x => x.FuelType, "FUEL_TYPE");
            test.DropDown(x => x.FuelTank, "FUEL_TNK");
            test.DropDown(x => x.GeneratorType, "GEN_TYP");
            test.Numerical(x => x.KWRating, "GEN_KW");
            test.Numerical(x => x.MaxOutputCurrent, "GEN_CURRENT");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.Phases, "PHASES");
            test.DropDown(x => x.Portable, "GEN_PORTABLE");
            test.DropDown(x => x.RPMRating, "RPM_RATING");
            test.DropDown(x => x.SelfStarting, "SELF_STARTING");
            test.DropDown(x => x.VoltRating, "VOLT_RATING");
            test.DropDown(x => x.VoltageACDC, "GEN_VOLTAGE_TP");
            test.DropDown(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES_DIST");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.FUEL_TYPE);
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