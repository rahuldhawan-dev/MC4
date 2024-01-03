using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class MotorStarterExcelRecordTest : EquipmentExcelRecordTestBase<MotorStarterExcelRecord>
    {
        #region Private Methods

        protected override MotorStarterExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.HPRating = "100.00 meh";
            ret.MotorStarterType = "MOTSTR*";
            ret.OwnedBy = "AW";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-MTSG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<MotorStarterExcelRecord> test)
        {
            test.Numerical(x => x.AmpRating, "AMP_RATING");
            test.String(x => x.FuseSize, "FUSE_SIZE");
            test.Numerical(x => x.HPRating, "HP_RATING");
            test.DropDown(x => x.MotorStarterType, "MOTSTR_TYP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.StrNEMASize, "STR_NEMA_SIZE");
            test.DropDown(x => x.StrOverloadType, "STR_OVERLOAD_TP");
            test.DropDown(x => x.StrType, "STR_TP");
            test.DropDown(x => x.VoltRating, "VOLT_RATING");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
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