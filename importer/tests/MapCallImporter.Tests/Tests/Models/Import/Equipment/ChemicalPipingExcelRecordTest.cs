using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class ChemicalPipingExcelRecordTest : EquipmentExcelRecordTestBase<ChemicalPipingExcelRecord>
    {
        #region Private Methods

        protected override ChemicalPipingExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.ChemicalPipingType = "FLEX WHIP";
            ret.OwnedBy = "AW";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-CPPG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<ChemicalPipingExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_CHEM-PIP");
            test.DropDown(x => x.ChemicalPipingType, "CHEM-PIP_TYP");
            test.DropDown(x => x.ChmDosingControl, "CHM_DOSING_CONTROL");
            test.String(x => x.ChmFeedRate, "CHEM_FEED_RATE");
            test.DropDown(x => x.ChmFeedRateUOM, "CHM_FEED_RATE_UOM");
            test.DropDown(x => x.ChmMaterial, "CHM_MATERIAL");
            test.String(x => x.OwnedBy, "OWNED_BY");
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