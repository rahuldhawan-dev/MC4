using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class PowerMonitorExcelRecordTest : EquipmentExcelRecordTestBase<PowerMonitorExcelRecord>
    {
        #region Private Methods

        protected override PowerMonitorExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.AmpRating = "100.00 meh";
            ret.OwnedBy = "AW";
            ret.PowerMonitorType = "PWRMON*";
            ret.VoltRating = "120";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-PMTG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<PowerMonitorExcelRecord> test)
        {
            test.DropDown(x => x.Adjustable, "ADJUSTABLE");
            test.Numerical(x => x.AmpRating, "AMP_RATING");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.PowerMonitorType, "PWRMON_TYP");
            test.String(x => x.SystemMonitored, "SYSTEM_MONITORED");
            test.DropDown(x => x.VoltRating, "VOLT_RATING");
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