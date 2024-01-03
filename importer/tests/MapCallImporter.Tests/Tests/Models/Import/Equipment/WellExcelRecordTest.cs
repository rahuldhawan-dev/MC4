using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class WellExcelRecordTest : EquipmentExcelRecordTestBase<WellExcelRecord>
    {
        #region Private Methods

        protected override WellExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.OwnedBy = "AW";
            ret.WellType = "HORIZONTAL BORE";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-WELG-1";

        #endregion

        #region Mapping

        protected override void
            TestCharacteristicMappings(EquipmentCharacteristicMappingTester<WellExcelRecord> test)
        {
            test.Numerical(x => x.CapacityGPM, "WELL_CAPACITY_RATING");
            test.String(x => x.DepthinFT, "DEPTH_IN (FT)");
            test.String(x => x.DiameterBottomin, "DIAMETER BOTTOM (IN)");
            test.String(x => x.DiameterTopin, "DIAMETER_TOP (IN)");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.PermitDurationYrs, "PERMIT_DURATION (YRS)");
            test.String(x => x.Permit, "PERMIT#");
            test.String(x => x.StaticWaterLevel, "STATIC_WATER_LEVEL");
            test.DropDown(x => x.WellType, "WELL_TYP");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.SpecialMtnNoteDet);
            test.NotMapped(x => x.PermitLastRenewal);
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