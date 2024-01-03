using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class EngineExcelRecordTest : EquipmentExcelRecordTestBase<EngineExcelRecord>
    {
        #region Private Methods

        protected override EngineExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.Application = "PUMPING";
            ret.EngineType = "DIESEL";
            ret.HPRating = "100.00 meh";
            ret.OwnedBy = "AW";
            ret.SelfStarting = "Y";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-ENGG-1";

        #endregion

        #region Mapping

        protected override void
            TestCharacteristicMappings(EquipmentCharacteristicMappingTester<EngineExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_ENG");
            test.DropDown(x => x.EngineType, "ENG_TYP");
            test.DropDown(x => x.FuelUOM, "ENG_FUEL_UOM");
            test.Numerical(x => x.HPRating, "HP_RATING");
            test.Numerical(x => x.MaxFuelConsumption, "ENG_MAX_FUEL");
            test.Numerical(x => x.NumberofCylinders, "ENG_CYLINDERS");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.SelfStarting, "SELF_STARTING");
            test.Numerical(x => x.TnkVolumegal, "TNK_VOLUME");
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