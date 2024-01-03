using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class SecondaryContainmentExcelRecordTest : EquipmentExcelRecordTestBase<SecondaryContainmentExcelRecord>
    {
        #region Private Methods

        protected override SecondaryContainmentExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.Application = "ALUM (ACIDIC)";
            ret.ContainmentType = "CONTAIN-CONC";
            ret.OwnedBy = "AW";
            ret.TnkStateInspection = "Y";
            ret.Underground = "Y";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-SCTG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<SecondaryContainmentExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_CONTAIN");
            test.DropDown(x => x.ContainmentType, "CONTAIN_TYP");
            test.DropDown(x => x.IndoorOutdoor, "LOCATION");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.TnkStateInspection, "TNK_STATE_INSPECTION_REQ");
            test.String(x => x.TnkVolumegal, "TNK_VOLUME (GAL)");
            test.DropDown(x => x.Underground, "UNDERGROUND");
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