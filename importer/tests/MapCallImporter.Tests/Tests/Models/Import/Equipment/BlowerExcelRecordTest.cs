using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class BlowerExcelRecordTest : EquipmentExcelRecordTestBase<BlowerExcelRecord>
    {
        #region Private Methods

        protected override BlowerExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.Application = "WATER PROCESSING";
            ret.BlowerType = "CENTRIFUGAL";
            ret.BHPRating = "100.00 meh";
            ret.CapacityRating = "100.00 meh";
            ret.MaxPressure = "100.00 meh";
            ret.OwnedBy = "AW";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-BLOG-1";

        #endregion

        #region Mapping

        protected override void
            TestCharacteristicMappings(EquipmentCharacteristicMappingTester<BlowerExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_BLWR");
            test.Numerical(x => x.BHPRating, "BHP_RATING");
            test.DropDown(x => x.BlowerType, "BLWR_TYP");
            test.Numerical(x => x.CapacityRating, "CAPACITY_RATING");
            test.String(x => x.CapacityUOM, "CAPACITY_UOM_BLWR");
            test.DropDown(x => x.DriveType, "DRIVE_TP");
            test.Numerical(x => x.MaxPressure, "MAX_PRESSURE");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.Numerical(x => x.RPMOperating, "RPM_OPERATING");
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