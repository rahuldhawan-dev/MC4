using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class FireSuppressionExcelRecordTest : EquipmentExcelRecordTestBase<FireSuppressionExcelRecord>
    {
        #region Private Methods

        protected override FireSuppressionExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.FireSuppressionTyp = "GAS SUP";
            ret.OwnedBy = "AW";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-FSPG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<FireSuppressionExcelRecord> test)
        {
            test.DropDown(x => x.FireSuppressionTyp, "FIRE-SUP_TYP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.FireClassRating, "FIRE_CLASS_RATING");
            test.DropDown(x => x.RetestRequired, "RETEST_REQUIRED");
            test.DropDown(x => x.ActionTakenUponAl, "ACTION_TAKEN_UPON_ALARM");
            test.String(x => x.NARUCMaintenanceAccount, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAccount, "NARUC_OPERATIONS_ACCOUNT");

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
