using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class LeakMonitorExcelRecordTest : EquipmentExcelRecordTestBase<LeakMonitorExcelRecord>
    {
        #region Private Methods

        protected override LeakMonitorExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-LKMG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<LeakMonitorExcelRecord> test)
        {
            test.DropDown(x => x.LeakMonitorType, "LK-MON_TYP");
            test.DropDown(x => x.LeakMonitorCorrelationType, "LK-MON_TYPE");
            test.DropDown(x => x.LocationType, "LK-MON_LOC_TYPE");
            test.DropDown(x => x.DataRetrievalMethod, "LK-MON_DATA_RETRIEVE_METHOD");
            test.DropDown(x => x.DeploymentType, "LK-MON_DEPLOYMENT_TYPE");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.LocationReference);
            test.NotMapped(x => x.OwnedBy);
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
